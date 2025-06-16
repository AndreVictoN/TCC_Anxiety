using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToOtherScene : MonoBehaviour
{
    public void StartLoadNewScene(string sceneName)
    {
        StartCoroutine(LoadNewScene(sceneName));
    }

    public IEnumerator LoadNewScene(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
