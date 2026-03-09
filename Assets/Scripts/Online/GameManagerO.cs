using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GameManagerO : NetworkBehaviour
{
    public static GameManagerO instance {  get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of game manager");
        }
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallbackRpc;
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void NetworkManager_OnClientConnectedCallbackRpc(ulong obj)
    {
       if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            SessionUI.SetActive(false);
            Starting = true;
        }
    }

    // [SerializeField] GameObject player;
    public Transform playerSpawn;
    public TMP_Text Timer;
    public TMP_Text EndTime;
    public Button EndButton;
    public Camera cam;
    [SerializeField] GameObject SessionUI;

    public bool Starting;
    public bool Running;
    public bool End;

   // private PlayerMovement Player;

    private float timer;
    private float countdown;

       private NetworkVariable<float> timeToBeat = new NetworkVariable<float>();
   // private float timeToBeat;

    void Start()
    {
        // player.transform.position = playerSpawn.position;
        //Starting = true;
        // Player = player.GetComponent<PlayerMovement>();
        countdown = 3;
        EndButton.gameObject.SetActive(false);
        EndTime.text = "Waiting...";
        timeToBeat.Value = 0;
            }

    // Update is called once per frame
    void Update()
    {
        if (Starting)
        {
            countdown -= Time.deltaTime;
            EndTime.text = Mathf.Floor(countdown).ToString();
            if (countdown <= 0)
            {
                EndTime.text = "";
                Starting=false;
                Running = true;
              //  Player.Wait = false;
            }
        }
        if (Running)
        {

            timer += Time.deltaTime;
            Timer.text = timer.ToString();
        }
    }
    
    public void OnFinishRpc()
    {
        Running = false;
        EndButton.gameObject.SetActive(true);
        Timer.text = "";
        if (!End)
        {
            if (timeToBeat.Value == 0)
            {
                timeToBeat.Value = timer;
                EndTime.text = "You Win!";
            }
            else
            {
                EndTime.text = "You Lose!";
            }
        }
        End = true;
    }
    //public bool CheckWon(float id)
    //{

    //    if (timeToBeat == 0)
    //    {
    //        timeToBeat = timer;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    public void OnClickEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
