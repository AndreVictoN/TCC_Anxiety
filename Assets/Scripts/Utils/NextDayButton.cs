using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDayButton : MonoBehaviour
{
    public void LoadNextDay()
    {
        if (PlayerPrefs.GetString("currentState").Equals("FirstLeaving"))
        {
            PlayerPrefs.SetString("currentState", "StartDayTwo");
            SceneManager.LoadScene("Terreo");
        }
    }
}
