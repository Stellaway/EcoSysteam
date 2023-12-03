// Source: https://docs-multiplayer.unity3d.com/netcode/current/tutorials/get-started-ngo/
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GUI_lobby_temporary : NetworkBehaviour
{
    [SerializeField]
    public GameObject hostBtn;

    [SerializeField]
    public GameObject serverBtn;

    [SerializeField]
    public GameObject clientBtn;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 20, 300, 300));
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
    public Texture2D hostTex;
    public Texture2D hostTexD;
    public Texture2D serverTex;
    public Texture2D serverTexD;
    public Texture2D startTex;
    public Texture2D startTexD;
    public Texture2D clientTex;
    public Texture2D clientTexD;

    static GUIStyle createButtonStyle(Texture2D tex, Texture2D tex2)
    {
        //string filename = "Assets/Textures/"+name+".png";
        //var rawData = System.IO.File.ReadAllBytes(filename);
        //Texture2D tex = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
        //tex.LoadImage(rawData);

        //filename = "Assets/Textures/" + name + "Darker.png";
        //rawData = System.IO.File.ReadAllBytes(filename);
        //Texture2D tex2 = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
        //tex2.LoadImage(rawData);

        return new GUIStyle()
        {
            normal = new GUIStyleState() { background = tex },
            hover = new GUIStyleState() { background = tex2 },
            active = new GUIStyleState() { background = tex }
        };
    }

    static int w = 150;
    static int h = 45;

    private void StartButtons()
    {
        IP = GUILayout.TextField(IP);//(IP, 15) for max length
        Port = GUILayout.TextField(Port);

        var hostBtn = GUILayout.Button("", createButtonStyle(hostTex,hostTexD), GUILayout.Width(w), GUILayout.Height(h));
        var clientBtn = GUILayout.Button("", createButtonStyle(clientTex,clientTexD), GUILayout.Width(w), GUILayout.Height(h));
        var serverBtn = GUILayout.Button("", createButtonStyle(serverTex,serverTexD), GUILayout.Width(w), GUILayout.Height(h));


        if (hostBtn) NetworkManager.Singleton.StartHost();
        if (clientBtn) StartClient();
        if (serverBtn) NetworkManager.Singleton.StartServer();
    }

    //kiszervez�s a gombokhoz, hogyha a guin k�v�l csin�ln�nk
    public static void StartingHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public static void StartingServer()
    {
        StartClient();
    }

    public static void StartingClient()
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

    void SubmitNewPosition()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "" : "Nemvagyokitt, nekattintside", createButtonStyle(startTex,startTexD), GUILayout.Width(w), GUILayout.Height(h)))
            {
                Debug.Log("Starting game!");



                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) {
                    PlayerBehaviour player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerBehaviour>();
                    player.StartGame();
                }


                NetworkManager.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // quit
            NetworkManager.Singleton.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}