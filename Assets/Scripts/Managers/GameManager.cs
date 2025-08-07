using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Singleton;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;
using UnityEditor.PackageManager;

public class GameManager : Singleton<GameManager>, IObserver
{
    public GameObject playerPFB;
    public GameObject animalPlayerPFB;
    public Image transitionImage;
    public List<GameObject> doors;
    public CinemachineCamera cinemachineCamera;
    public TextMeshProUGUI currentDay;
    public TextMeshProUGUI currentObjective;

    [Header("Prototype")]
    [SerializeField] private GameObject _prototypeTeacher;

    [Header("Texts")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public List<string> dialogue = new List<string>();
    public float wordSpeed = 0.6f;

    private PlayerController _playerController;
    private Ezequiel _ezequiel;
    private bool _isTyping;
    private bool _skipped;
    private bool _canSkip;
    private int _i;

    protected override void Awake()
    {
        cinemachineCamera = GameObject.FindFirstObjectByType<CinemachineCamera>();
        PlayerManagement();
        _canSkip = false;
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals("PrototypeScene"))
        {
            PrototypeConfig();
        }
    }

    private void PrototypeConfig()
    {
        if(currentDay == null){currentDay = GameObject.FindGameObjectWithTag("CurrentDay").GetComponent<TextMeshProUGUI>();}
        if(currentObjective == null){currentObjective = GameObject.FindGameObjectWithTag("Objective").GetComponent<TextMeshProUGUI>();}
        transitionImage.color = new Vector4(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1f);
        currentObjective.color = new Vector4(currentObjective.color.r, currentObjective.color.g, currentObjective.color.b, 0f);
        AnimateTransition(3f, true);
        AnimateText(currentDay, 3f, true);
        AnimateText(currentObjective, 6f, false);
        StartCoroutine(SetPlayerCanMove());
    }

    IEnumerator SetPlayerCanMove()
    {
        _playerController.SetCanMove(false);
        yield return new WaitForSeconds(3f);
        _playerController.SetCanMove(true);
    }

    public void PlayerManagement()
    {
        if (GameObject.FindFirstObjectByType<PlayerController>() == null)
        {
            CinemachineFollow(GameObject.Instantiate(playerPFB).GetComponent<Transform>());
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        else if (sceneName == "TestScene")
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

    void AnimateTransition(float time, bool toTransparent)
    {
        if (!toTransparent)
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

    void AnimateText(TextMeshProUGUI textToFade, float time, bool toTransparent)
    {
        if (!toTransparent)
        {
            Color newColor = new Color(1f, 1f, 1f, 1f);
            StartCoroutine(FadeText(textToFade, textToFade.color, newColor, time));
        }
        else
        {
            Color newColor = new Color(1f, 1f, 1f, 0f);
            StartCoroutine(FadeText(textToFade, textToFade.color, newColor, time));
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI textToFade, Color old, Color color, float time)
    {
        float elapsedTime = 0f;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float lerpAmount = Mathf.Clamp01(elapsedTime/time);
            textToFade.color = Color.Lerp(old, color, lerpAmount);

            yield return null;
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

    public void CallEzequiel(string trigger)
    {
        if(trigger.Equals("PrototypeEzequielTrigger1"))
        {
            _ezequiel = GameObject.FindGameObjectWithTag("Ezequiel").GetComponent<Ezequiel>();
            _ezequiel.RecieveTrigger(_playerController.gameObject, "PrototypeEzequielTrigger1");
            Destroy(GameObject.FindGameObjectWithTag("PrototypeEzequielTrigger1"));
        }
    }

    public void OnNotify(EventsEnum evt)
    {
        if(evt == EventsEnum.CallPrototypeEzequiel)
        {
            CallEzequiel("PrototypeEzequielTrigger1");
        }else if(evt == EventsEnum.PrototypeBattle)
        {
            StartCoroutine(LoadBattleScene("PrototypeScene"));
        }else if(evt == EventsEnum.PrototypeFirstInteraction)
        {
            ManagePrototypeInteraction();
        }
    }

    private void ManagePrototypeInteraction()
    {
        if(dialogue.Count < 2)
        {
            dialogue.Add("Que lugar enorme...");
            dialogue.Add("acho que vou ter que pedir informaÇÃo para alguÉm... de preferÊncia um professor.");
        }

        Destroy(GameObject.FindGameObjectWithTag("PrototypeFirstInteractionTrigger"));
        StartCoroutine(StartAutomaticTalk());
    }

    protected IEnumerator Typing()
    {
        _isTyping = true;

        if(dialogueText.text != "")
        {
            dialogueText.text = "";
        }

        foreach(char letter in dialogue[_i].ToCharArray())
        {
            if(dialogueText.text != dialogue[_i])
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }

        _isTyping = false;
    }

    protected IEnumerator StartAutomaticTalk()
    {
        GameObject skipText = null;

        if(!dialoguePanel.activeSelf)
        {
            dialogueText.text = "";
            dialogueText.alignment = TextAlignmentOptions.Center;
            dialogueText.fontStyle = FontStyles.Italic;
            dialoguePanel.SetActive(true);
            _i = 0;

            GameObject npcImage = GameObject.FindGameObjectWithTag("NPC_Image");
            npcImage.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

            GameObject npcName = GameObject.FindGameObjectWithTag("NPC_Name");
            npcName.GetComponent<TextMeshProUGUI>().text = "";

            GameObject playerImage = GameObject.FindGameObjectWithTag("Player_Image");
            playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 1f);
            
            skipText = GameObject.FindGameObjectWithTag("SkipText");
            skipText.SetActive(false);
            StartCoroutine(Typing());
        }

        while(_i != dialogue.Count - 1)
        {
            yield return null;

            if(!_isTyping)
            {
                yield return new WaitForSeconds(1f);
                NextLine();
            }
        }

        if(skipText != null) skipText.SetActive(true);
        _canSkip = true;
        _skipped = false;

        StartCoroutine(TeacherTalk());
    }

    private IEnumerator TeacherTalk()
    {
        while(!_skipped) yield return null;

        if(_playerController.gameObject.transform.localPosition.x > 18.44f) _playerController.SetAnimation("H_WalkingLeft");
        else if(_playerController.gameObject.transform.localPosition.x < 18.44f) _playerController.SetAnimation("H_WalkingRight");

        float seconds = _playerController.ToX(18.44f);
        yield return new WaitForSeconds(seconds);

        if(_playerController.gameObject.transform.localPosition.y > 65.22f) _playerController.SetAnimation("H_WalkingDown");
        else if(_playerController.gameObject.transform.localPosition.y < 65.22f) _playerController.SetAnimation("H_WalkingUp");

        seconds = _playerController.ToY(65.22f);
        yield return new WaitForSeconds(seconds);

        _playerController.SetAnimation("H_IdleR");

        _prototypeTeacher.SetActive(true);

        _prototypeTeacher.GetComponent<Teacher>().Move(4f, new Vector2(this.gameObject.transform.position.x, 65.22f), 'y');
        _prototypeTeacher.GetComponent<Teacher>().SetAnimation("WalkingDown");
        yield return new WaitForSeconds(1f);
        StartCoroutine(_prototypeTeacher.GetComponent<Teacher>().StartAutomaticTalk());
        yield return new WaitForSeconds(4f);
        _prototypeTeacher.GetComponent<Teacher>().Move(2f, new Vector2(this.gameObject.transform.position.x, 51.33f), 'y');
        yield return new WaitForSeconds(3f);

        while(!_prototypeTeacher.GetComponent<Teacher>().GetIsClosed()) yield return null;
        
        _prototypeTeacher.GetComponent<Teacher>().SetIsAutomatic(false);
        _playerController.SetCanMove(true);
        _playerController.SetAnimation("Moving");

        Destroy(_prototypeTeacher);
    }

    public virtual void NextLine()
    {

        if(_i < dialogue.Count - 1)
        {
            _i++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }else
        {
            _i = 0;
            if(dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(_canSkip && Input.GetKeyDown(KeyCode.Return) && _isTyping)
        {
            StopCoroutine(Typing());
            dialogueText.text = dialogue[_i];
        }else if(_canSkip && Input.GetKeyDown(KeyCode.Return) && !_isTyping)
        {
            NextLine();
            _canSkip = false;
            _skipped = true;
            //_playerController.SetCanMove(true);
        }
    }

    public IEnumerator LoadBattleScene(string pastScene)
    {
        transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>();
        _playerController.SetCanMove(false);
        AnimateTransition(1f, false);
        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetString("pastScene", pastScene);
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
    }
}
