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
        SceneManager.LoadScene("Level1");
    }

    public void OnLevelPressed(string Level)
    {
        SceneManager.LoadScene(Level);
    }

    public void OnMainPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
