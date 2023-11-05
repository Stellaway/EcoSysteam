using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerBehaviour : Synchronizable
{

    protected Vector2 direction = Vector2.zero;
    protected float distanceFromTarget = 0f;

    //Az érzékenység, hogy mennyire kell közel menni
    public float viggleRoom = 0.5f;

    private System.DateTime startTime= System.DateTime.UtcNow;

    protected BaseInteraction CurrentInteraction = null;


    public float health = 100;
    public float hunger = 0;
    private bool alive = true;

    private double viewAngle = 120;

    // This method will be called every frame on the server side
    protected override void ServerUpdate()
    {
        if (health <= 0)
        {
            alive = false;
            return;
        }
        else
        {
            System.TimeSpan ts = System.DateTime.UtcNow - startTime;
            startTime = System.DateTime.UtcNow;
            hunger += ts.Milliseconds/1000f;
        }

        //éhezés
        if(alive && hunger>=health)
        {
            health--;
        }

        //Ha épp tud csinálni valamit
        if (CurrentInteraction != null && closeEnoughtToInteract())
        {
            CurrentInteraction.Perform(this, OnInteractionFinished);
        }
        else
        {
            if (CurrentInteraction == null)
            {
                PickRandomInteraction();
            }
        }


        //Beállítja magát a megfelelő irányba
        SetDirection();

        // így el lehet érni az éppen aktuális pozíciót
        // transform.position.z == 0.0f, sztem szerencsésebb Vector2-t használni
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 speed = direction; // pixel / sec
        // előző frissítés óta eltelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
        float delta = Time.deltaTime; // másodpercben
        // ez itt egy egyenes vonalú egyenletes mozgás
        Vector2 newPos = currentPos + speed * delta;

        //határon belül marad
        newPos.x = Mathf.Clamp(newPos.x, -7f, 7f);
        newPos.y = Mathf.Clamp(newPos.y, -3f, 3f);

        // elküldjük a hálózaton az új pozíciót (TODO ez lehet majd változik)
        UpdatePosition(newPos);
    }

    private void OnInteractionFinished(BaseInteraction interaction)
    {
        CurrentInteraction = null;
        Debug.Log($"Finished interaction {interaction}");
    }



    // This will be called when the prefab spawns, but we only want to randomize the position
    // from the "owner": only from the client that corresponds to the player
    // (at least if I understand it well xd)
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            Move();
    }

    // Ask the server to place the bogyesz on a random position
    // it is called from the GUI, and from OnNetworkSpawn
    // and it may be called from the server from the GUI
    public void Move()
    {
        if (NetworkManager.Singleton.IsServer) MoveServer();
        else SubmitRandomPositionRequestServerRpc();
    }

    // This can be called from the clients, and will be run on the server
    [ServerRpc]
    private void SubmitRandomPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        MoveServer();
    }

    private void MoveServer()
    {
        UpdatePosition(new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
    }


    private void SetDirection()
    {
        if (CurrentInteraction == null)
        {
            direction = Vector2.zero;
        }
        else
        {
            var dirVec3 = (CurrentInteraction.transform.position - transform.position).normalized;
            direction = new Vector2(dirVec3.x, dirVec3.y);
            distanceFromTarget = dirVec3.magnitude;
            if (closeEnoughtToInteract())
            {
                direction = Vector2.zero;
            }
        }
    }

    private bool closeEnoughtToInteract()
    {
        return distanceFromTarget <= viggleRoom;
    }

    void PickRandomInteraction()
    {
        Debug.Log($"Finding something new to do...");
        //Pick a random object
        int objIndex = Random.Range(0, SmartObjectManager.Instance.RegisteredObjects.Count);
        var ChosenObject = SmartObjectManager.Instance.RegisteredObjects[objIndex];

        //Pick a random interaction
        int interactionIndex = Random.Range(0, ChosenObject.Interactions.Count);
        var selectedInteraction = ChosenObject.Interactions[interactionIndex];

        if (selectedInteraction.CanPerform())
        {
            CurrentInteraction = selectedInteraction;
            SetDirection();
        }
        if (selectedInteraction == null)
        {
            Debug.Log($"No available interactions at all :((");
        }
    }


}
