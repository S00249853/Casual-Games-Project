using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
   public void OnTimeTrialPressed()
    {
        SceneManager.LoadScene("TimeTrials");
    }

    public void OnOnlinePressed()
    {
        SceneManager.LoadScene("OnlineLoading");
    }

    public void OnLevelPressed(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
