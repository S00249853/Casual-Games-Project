using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform playerSpawn;
    [SerializeField] TMP_Text Timer;
    [SerializeField] TMP_Text EndTime;
    [SerializeField] Button EndButton;

    private bool Starting;
    private bool Running;
    private bool End;

    private PlayerMovement Player;

    private float timer;
    private float countdown;

    void Start()
    {
        player.transform.position = playerSpawn.position;
        Starting = true;
        Player = player.GetComponent<PlayerMovement>();
        countdown = 3;
        EndButton.gameObject.SetActive(false);
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
                Player.Wait = false;
            }
        }
        if (Running)
        {

            timer += Time.deltaTime;
            Timer.text = timer.ToString();
        }
    }

    public void OnFinish()
    {
        Running = false;
        End = true;
        EndButton.gameObject.SetActive(true);
        EndTime.text = timer.ToString();
        Timer.text = "";
        Player.Wait = true;
    }

    public void OnClickEnd()
    {
        SceneManager.LoadScene("TimeTrials");
    }
}
