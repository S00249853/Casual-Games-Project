using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;

public class GameManagerO : NetworkBehaviour
{
    public static GameManagerO instance {  get; private set; }

    public event EventHandler<OnGameWinEventArgs> OnGameWin;
    public class OnGameWinEventArgs : EventArgs
    {
        public Player winPlayer;
    }

    public enum Player
    {
      
        Player1,
        Player2
    }

    private Player LocalPlayer;
    private Player WinningPlayer;

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
        Debug.Log(NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            LocalPlayer = Player.Player1;
        }
        else
        {
            LocalPlayer = Player.Player2;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallbackRpc;
        }
    }

    public Player GetLocalPlayer()
    {
        return LocalPlayer;
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

     //  private NetworkVariable<float> timeToBeat = new NetworkVariable<float>();
   // private float timeToBeat;

    void Start()
    {
        // player.transform.position = playerSpawn.position;
        //Starting = true;
        // Player = player.GetComponent<PlayerMovement>();
        countdown = 3;
        EndButton.gameObject.SetActive(false);
        EndTime.text = "Waiting...";
      //  timeToBeat.Value = 0;
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

    public void OnFinish(Player p)
    {
        Running = false;
        EndButton.gameObject.SetActive(true);
        Timer.text = timer.ToString();
        Timer.text = "";
        if (!End)
        {
            TriggerOnGameWinRpc(p);
        }
        End = true;

    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerOnGameWinRpc(Player p)
    {
        OnGameWin?.Invoke(this, new OnGameWinEventArgs
        {
            winPlayer = p
        });
    }

    private void ShowVictory()
    {
        if (LocalPlayer == WinningPlayer)
        {
            EndTime.text = "You Win!";
        }
        else
        {
            EndTime.text = "You Lose!";
        }
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
