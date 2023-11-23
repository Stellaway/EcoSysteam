// Source: https://docs-multiplayer.unity3d.com/netcode/current/tutorials/get-started-ngo/
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using System.Net;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{


    private static bool visible = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            visible = !visible;
    }
    void OnGUI()
    {
        if (!visible)
            return;
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    static string IP = "127.0.0.1";
    static string Port = "7777";
    
    static void StartButtons()
    {
        IP = GUILayout.TextField(IP);//(IP, 15) for max length
        Port = GUILayout.TextField(Port);
        //var hostBtn = GUILayout.Button("Host");
        //var clientBtn = GUILayout.Button("Client");
        //var serverBtn = GUILayout.Button("Server");


        //if (hostBtn) NetworkManager.Singleton.StartHost();
        //if (clientBtn) StartClient();
        //if (serverBtn) NetworkManager.Singleton.StartServer();
    }

    public void StartingHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartingServer()
    {
        StartClient();
    }

    public void StartingClient()
    {
        NetworkManager.Singleton.StartServer();
    }

    static void StartClient() {
        // https://stackoverflow.com/questions/6727187/converting-a-string-to-a-short
        ushort Port_sh;
        if (!ushort.TryParse(Port, out Port_sh)) {
            Port_sh = 7777;
        }
        // https://stackoverflow.com/questions/12756302/resolving-an-ip-address-from-dns-in-c-sharp
        string resolvIP = Dns.GetHostAddresses(IP)[0].ToString();
        Debug.Log($"Client connecting to {resolvIP}:{Port_sh}");
        // https://docs-multiplayer.unity3d.com/netcode/current/components/networkmanager/
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(resolvIP, Port_sh);
        NetworkManager.Singleton.StartClient();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
        GUILayout.Label("IP: " + NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
        GUILayout.Label("Port: " + NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port);

        /*var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerBehaviour>();
        GUILayout.Label("HP: " + player.health);
        GUILayout.Label("Hunger: " + player.hunger);*/
    }

    static void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient )
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerBehaviour>().Move();
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                if (playerObject != null) {
                    var player = playerObject.GetComponent<PlayerBehaviour>();
                    player.Move();
                }
            }
        }
    }
}