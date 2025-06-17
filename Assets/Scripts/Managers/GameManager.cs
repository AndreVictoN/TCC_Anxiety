using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Singleton;
using UnityEngine.UI;
using Unity.Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public GameObject PlayerPFB;
    public Image transitionImage;
    public List<GameObject> doors;
    public CinemachineCamera cinemachineCamera;

    protected override void Awake()
    {
        cinemachineCamera = GameObject.FindFirstObjectByType<CinemachineCamera>();
        PlayerManagement();
    }

    public void PlayerManagement()
    {
        if (GameObject.FindFirstObjectByType<PlayerController>() == null)
        {
            CinemachineFollow(GameObject.Instantiate(PlayerPFB).GetComponent<Transform>());
        }
    }

    void CinemachineFollow(Transform transform)
    {
        if(cinemachineCamera != null)
        {
            cinemachineCamera.Follow = transform;
        }
    }

    public void StartLoadNewScene(string sceneName, GameObject player, GameObject toOtherScene)
    {
        StartCoroutine(LoadNewScene(sceneName, player, toOtherScene));
    }

    public IEnumerator LoadNewScene(string sceneName, GameObject player, GameObject toOtherScene)
    {
        transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>();
        AnimateTransition(0.5f, false);
        yield return new WaitForSeconds(0.5f);

        string identifier = toOtherScene.GetComponentInChildren<Door>().identifier;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return null;

        cinemachineCamera = GameObject.FindFirstObjectByType<CinemachineCamera>();
        CinemachineFollow(player.GetComponent<Transform>());

        Door door = null;

        if (sceneName == "Classroom")
        {
            door = GameObject.FindFirstObjectByType<Door>();
            player.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
            player.GetComponent<PlayerController>().SetSpeed(8f);
            door.SetIsClosed(true);
        }
        else if (sceneName == "PrototypeScene")
        {
            doors.Clear();
            doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
            foreach (GameObject d in doors)
            {
                if (d.GetComponent<Door>().identifier == identifier)
                {
                    door = d.GetComponent<Door>();
                    door.SetIsClosed(true);
                    break;
                }
            }
            player.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            player.GetComponent<PlayerController>().SetSpeed(5f);
        }

        if (door == null)
        {
            Debug.LogError("Door not found in the new scene. " + identifier);
            yield break;
        }
        player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 1f, door.transform.position.z);
        player.gameObject.GetComponent<PlayerController>().SetSpriteDown();

        door.ChangePlayer(player);
        door.ChangeSprite("open");

        transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>();
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1f);

        yield return new WaitForSeconds(0.1f);

        AnimateTransition(0.5f, true);
        yield return new WaitForSeconds(0.5f);

        door.ChangeSprite("close");

        door.IgnoreCollision(player, false);

        player.GetComponent<PlayerController>().SetCanMove(true);
        doors.Clear();
    }

    void AnimateTransition(float time, bool inRoom)
    {
        if (!inRoom)
        {
            Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1f);
            StartCoroutine(FadeTransition(transitionImage.color, newColor, time));
        }
        else
        {
            Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0f);
            StartCoroutine(FadeTransition(transitionImage.color, newColor, time));
        }
    }

    private IEnumerator FadeTransition(Color old, Color color, float time)
    {
        float elapsedTime = 0f;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float lerpAmount = Mathf.Clamp01(elapsedTime/time);
            transitionImage.color = Color.Lerp(old, color, lerpAmount);

            yield return null;
        }
    }
}
