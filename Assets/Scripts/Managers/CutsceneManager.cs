using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Core.Singleton;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : DialogueBox
{
    public List<Sprite> npcSprites = new();
    public List<Sprite> playerSprites = new();

    [SerializeField] private Image _transitionImage;
    private Color _transitionImageColor;
    private Color _newTransitionColor;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        Transition();
    }

    private void Transition()
    {
        if (!_transitionImage) { _transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>(); }
        _transitionImageColor = _transitionImage.color;
        _newTransitionColor = new Vector4(_transitionImageColor.r, _transitionImageColor.g, _transitionImageColor.b, 1f);

        _currentCoroutine = StartCoroutine(FadeTransition(_transitionImageColor, _newTransitionColor, 1f));
    }

    private IEnumerator StartAutomaticTalk()
    {
        GameObject skipText = null;

        if (!dialoguePanel.activeSelf)
        {
            dialogueText.text = "";
            dialogueText.alignment = TextAlignmentOptions.Center;
            dialoguePanel.SetActive(true);
            _i = 0;

            GameObject npcImage = GameObject.FindGameObjectWithTag("NPC_Image");
            npcImage.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);

            GameObject npcName = GameObject.FindGameObjectWithTag("NPC_Name");
            npcName.GetComponent<TextMeshProUGUI>().text = "";

            GameObject playerImage = GameObject.FindGameObjectWithTag("Player_Image");
            playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 1f);

            skipText = GameObject.FindGameObjectWithTag("SkipText");
            skipText.SetActive(false);
            StartCoroutine(Typing());
        }

        while (_i != dialogue.Count - 1)
        {
            yield return null;

            if (!_isTyping)
            {
                yield return new WaitForSeconds(1.5f);
                NextLine();
            }
        }
    }

    protected override void CheckCharacter(int i)
    {
        GameObject playerImage = GameObject.FindGameObjectWithTag("Player_Image");
        GameObject playerName = GameObject.FindGameObjectWithTag("PlayerName");
        GameObject npcImage = GameObject.FindGameObjectWithTag("NPC_Image");
        GameObject npcName = GameObject.FindGameObjectWithTag("NPC_Name");

        if (i == 3 || i == 17)
        {
            if (i == 3) { playerName.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1); }

            wordSpeed = 0.07f;
            dialogueText.fontStyle = FontStyles.Normal;
            dialogueText.alignment = TextAlignmentOptions.Right;
            playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 1);
            npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0.3f);
        }
        else if (i == 0 || (i >= 4 && i <= 15) || i == 18 || i == 19)
        {
            if (i == 0)
            {
                npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0f);
                playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0f);
                playerName.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
            }
            else
            {
                if(!(i >= 5 && i <= 13)) npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0.3f);

                if (i == 5) {
                    npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0f);
                    playerImage.GetComponent<Image>().sprite = playerSprites[0];
                    npcName.GetComponent<TextMeshProUGUI>().text = "";
                }
                else if (i == 7) { playerImage.GetComponent<Image>().sprite = playerSprites[1]; }
                else if (i == 11) { playerImage.GetComponent<Image>().sprite = playerSprites[0]; }
                else if (i == 12) { playerImage.GetComponent<Image>().sprite = playerSprites[2]; }
                else if (i == 14)
                {
                    playerImage.GetComponent<Image>().sprite = playerSprites[0];
                    npcImage.GetComponent<Image>().sprite = npcSprites[0];
                    npcName.GetComponent<TextMeshProUGUI>().text = "?";
                }

                playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0.3f);
            }

            wordSpeed = 0.07f;
            dialogueText.fontStyle = FontStyles.Italic;
            dialogueText.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            wordSpeed = 0.07f;
            playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0.3f);

            if (i == 1 || i == 2)
            {
                playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0f);
                if (i == 1)
                {
                    npcName.GetComponent<TextMeshProUGUI>().text = "MÃe";
                    wordSpeed = 0.1f;
                }else{ playerImage.GetComponent<Image>().sprite = playerSprites[2]; }
            }

            dialogueText.fontStyle = FontStyles.Normal;
            dialogueText.alignment = TextAlignmentOptions.Left;
            npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 1);
        }
    }

    private IEnumerator FadeTransition(Color old, Color color, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float lerpAmount = Mathf.Clamp01(elapsedTime / time);
            _transitionImage.color = Color.Lerp(old, color, lerpAmount);

            yield return null;
        }

        _currentCoroutine = StartCoroutine(StartAutomaticTalk());
    }
}
