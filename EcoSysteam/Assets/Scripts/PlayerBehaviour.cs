using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;


public class PlayerBehaviour : Synchronizable
{

    protected Vector2 direction = Vector2.zero;
    protected float distanceFromTarget = 0f;

    //Az érzékenység, hogy mennyire kell közel menni
    public float viggleRoom = 0.5f;

    protected BaseInteraction CurrentInteraction = null;


    private float health = 100;
    private float[] healthmultipliers = {0,0,-1,1}; //fruit, meat, danger, safety

    private float hunger = 60;
    private float[] hungermultipliers = {1,1,0,0};


    private bool alive = true;
    private float speed = 1.0f;

    private float viewRadius = 4;


    [SerializeField] protected float DefaultInteractionScore = 0f;

    // This method will be called every frame on the server side
    protected override void ServerUpdate()
    {
        if (health <= 0) alive = false;

        //Ha épp tud csinálni valamit
        if (CurrentInteraction != null && closeEnoughtToInteract())
        {
            CurrentInteraction.Perform(this, OnInteractionFinished);
        }
        else
        {
            if (CurrentInteraction == null)
            {
                PickBestInteraction();
            }
        }


        //Beállítja magát a megfelelő irányba
        SetDirection();

        // így el lehet érni az éppen aktuális pozíciót
        // transform.position.z == 0.0f, sztem szerencsésebb Vector2-t használni
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        float multiplier = GetComponent<PlayerSkillTree>().GetSpeed() ? 3 : 1; // fejlesztés
        Vector2 velocity = direction * this.speed * multiplier; // pixel / sec
        // előző frissítés óta eltelt idő (HASZNÁLJÁTOK PLS, HOGY FPS-FÜGGETLEN LEGYEN):
        float delta = Time.deltaTime; // másodpercben
        // ez itt egy egyenes vonalú egyenletes mozgás
        Vector2 newPos = currentPos + velocity * delta;

        //határon belül marad
        newPos.x = Mathf.Clamp(newPos.x, -7f, 7f);
        newPos.y = Mathf.Clamp(newPos.y, -3f, 3f);

        // elküldjük a hálózaton az új pozíciót (TODO ez lehet majd változik)
        UpdatePosition(newPos);
    }
    
    class ScoredInteraction
    {
        public SmartObject TargetObject;
        public BaseInteraction Interaction;
        public float Score;

    }


    private void PickBestInteraction()
    {
        var scoredInteractions = new List<ScoredInteraction>();
        //loop through all available objects
        var availableObjects = SmartObjectManager.Instance.getSmartObjectsInRange(this.viewRadius, this.transform.position);
        foreach (var availableObject in availableObjects)
        {
            //loop through all the interactions
            foreach(var interaction in availableObject.Interactions)
            {
                if (!interaction.CanPerform())
                {
                    continue;
                }

                float score = ScoreInteraction(interaction);

                scoredInteractions.Add(new ScoredInteraction()  {   Interaction = interaction,
                                                                    Score = score,
                                                                    TargetObject = availableObject
                                                                });

            }
        }
        if(scoredInteractions.Count == 0)
        {
            return;
        }

        //Ha többet akarnánk és onnan random választani, azt itt kéne, a mérete alapján indexelni, mondjuk a top3ból választani
        var bestScoredInteraction = scoredInteractions.OrderByDescending(i => i.Score).ToList();
        
        //actually beállunk felé
        CurrentInteraction = bestScoredInteraction[0].Interaction;
        SetDirection();
    }


    private float ScoreInteraction(BaseInteraction interaction)
    {
        if(interaction.StatChanges.Length == 0)
        {
            return DefaultInteractionScore;
        }

        float score = 0f;
        foreach(var change in interaction.StatChanges)
        {
            score += ScoreChange(change.Type, change.Value);
        }


        return score;

    }

    private float ScoreChange(EInteractionType type, float amount)
    {
        float currentHealth = this.health;
        float currentHunger = this.hunger;

        float healthchange = (this.health +amount * healthmultipliers[(int)type]) - currentHealth;
        float hungerchange = (-1f)*((this.hunger - amount * hungermultipliers[(int)type]) - currentHunger);
        
        return healthchange+hungerchange;
    }

    private void OnInteractionFinished(BaseInteraction interaction)
    {
        CurrentInteraction.ApplyStatChanges(this);
        CurrentInteraction = null;
        Debug.Log($"Finished interaction {interaction}, current hunger: {this.hunger}");
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

    public void UpdateIndividualStat(EInteractionType Interactiontype, float amount)
    {
        Debug.Log($"Stats are updating by {amount}");
        this.health += amount * healthmultipliers[(int)Interactiontype];
        this.hunger -= amount * hungermultipliers[(int)Interactiontype];
        if (hunger < 0)
        {
            hunger = 0;
        }
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
