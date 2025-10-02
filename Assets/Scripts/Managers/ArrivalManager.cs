using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrivalManager : DialogueBox
{
    public List<string> arrivalDialogue = new();
    public GameManager gameManager;
    private PlayerController _playercontroller;

    [SerializeField] private Image playerImage;
    [SerializeField] private GameObject playerName;
    [SerializeField] private GameObject npcName;
    [SerializeField] private Image npcImage;

    public void SetGameManager(GameManager gm)
    {
        gameManager = gm;
        _playercontroller = gm.GetPlayerController();
    }

    public IEnumerator FirstLines()
    {
        _playercontroller.SetCanMove(false);
        _canSkip = true;
        yield return new WaitForSeconds(3f);

        dialogue.Clear();
        for (int iterator = 0; iterator < 3; iterator++) { dialogue.Add(arrivalDialogue[iterator]); }

        StartDialogue();

        while (!_isClosed) { yield return null; }
        _playercontroller.SetCanMove(true);

        gameManager.instruction.text = "Pressione TAB para abrir o inventÁrio e conferir seu objetivo.";
        gameManager.instruction.gameObject.SetActive(true);
        gameManager.currentObjective.text = "OBJETIVO: Encontre alguma forma de descobrir sua sala de aula.";
        yield return new WaitForSeconds(5f);
        gameManager.instruction.gameObject.SetActive(false);
    }

    protected void StartDialogue()
    {
        if (!dialoguePanel.activeSelf)
        {
            dialogueText.text = "";
            _i = 0;

            SetDialoguePanel();
            StartCoroutine(Typing());
        }
    }

    void Update()
    {
        if (_canSkip && Input.GetKeyDown(KeyCode.Return) && _isTyping && !_isClosed)
        {
            StopCoroutine(Typing());
            dialogueText.text = dialogue[_i];
        }
        else if (_canSkip && Input.GetKeyDown(KeyCode.Return) && !_isTyping && !_isClosed)
        {
            NextLine();
        }
    }

    protected override void CheckCharacter(int i)
    {
        npcName = GameObject.FindGameObjectWithTag("NPC_Name");
        playerName = GameObject.FindGameObjectWithTag("PlayerName");

        if (i < 3)
        {
            npcName.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
            npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0f);
            playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0.5f);
            dialogueText.alignment = TextAlignmentOptions.Center;
        }
    }
}
