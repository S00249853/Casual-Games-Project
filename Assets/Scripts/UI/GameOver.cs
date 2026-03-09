using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Result;

    private void Start()
    {
        GameManagerO.instance.OnGameWin += GameManager_OnGameWin;
    }

    private void GameManager_OnGameWin(object sender, GameManagerO.OnGameWinEventArgs e)
    {
       if (e.winPlayer == GameManagerO.instance.GetLocalPlayer())
        {
            Result.text = "You Win!";
        }
       else
        {
            Result.text = "You Lose";
        }

    }
}
