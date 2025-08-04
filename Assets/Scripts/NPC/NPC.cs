using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class NPC : MonoBehaviour
{
    [Header("GeneralSettings")]
    public Color defaultColor;
    public Vector3 defaultPosition;

    [Header("Dialogue")]
    public GameObject dialoguePanel;
    //public GameObject continueButton;
    //public TextMeshProUGUI continueButtonText;
    public TextMeshProUGUI dialogueText;
    public List<String> dialogue = new List<String>();
    public float wordSpeed;

    [Header("Battle")]
    public Animator battleAnimator;
    public BattleManager battleManager;
    public SpriteRenderer spriteRenderer;
    public GameObject enemy;
    public float attackTime = 0.3f;
    public float fadeTime = 0.5f;

    #region Privates
    protected Coroutine _currentCoroutine;
    protected Tween _currentTween;
    protected bool _playerIsClose;
    protected bool _isBattling;
    protected String _npcName;
    protected bool _isMoving;
    protected bool _isTyping;
    protected int _i;
    #endregion

    void Start()
    {
        if(SceneManager.GetActiveScene().name != "BattleScene")
        {
            ResetText();
        }else{
            defaultPosition = getPosition();
            this.gameObject.transform.position = defaultPosition;
        }
    }

    protected void BasicSettings()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        _npcName = this.gameObject.name;
    }

    protected virtual Vector3 getPosition()
    {
        /*return _npcName switch
        {
            "Estella" => new Vector3(-5.3f, 0.01f, 0),
            "Rebecca" => new Vector3(-6.9f, 3.7f, 0),
            "Ezequiel" => new Vector3(-5.3f, 2.5f, 0),
            "Yuri" => new Vector3(-6.9f, -1.1f, 0),
            _ => this.gameObject.transform.position,
        };*/
        return this.gameObject.transform.position;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "BattleScene")
        {
            UpdateNPC();
        }else
        {
            _isBattling = true;
            BattleManagement();
        }
    }

    public void BattleManagement()
    {
        if(!_isBattling) return;
        battleAnimator = this.gameObject.GetComponent<Animator>();

        _currentTween = battleManager.GoToDefaultPosition(this.gameObject, _isMoving, _currentTween, defaultPosition, attackTime);
    }

    void OnMouseOver()
    {
        if(SceneManager.GetActiveScene().name != "BattleScene") return;
        _currentCoroutine = battleManager.FadeToColor(CheckColorAspectByNPC(), _currentCoroutine, spriteRenderer, fadeTime);

        /*if(Input.GetMouseButtonDown(0))
        {
            Vector2 enemyPosition = enemy.transform.position;
            Movement(enemyPosition);
        }*/
    }

    protected virtual Color CheckColorAspectByNPC()
    {
        Color colorToFade = spriteRenderer.color;

        /*switch(_npcName)
        {
            case "Estella":
                colorToFade.r += 0.2f;
                break;
            case "Rebecca":
                colorToFade.g += 0.7f;
                break;
            case "Ezequiel":
                colorToFade.b -= 0.7f;
                break;
            case "Yuri":
                colorToFade.b -= 1f;
                break;
        }*/

        return colorToFade;
    }

    public void Movement()
    {
        Vector2 enemyPosition = enemy.transform.position;
        _isMoving = true;
        _currentTween?.Kill();
        _currentTween = transform.DOLocalMove(enemyPosition, attackTime).SetEase(Ease.InQuad).OnComplete(() => _isMoving = false);
    }

    void OnMouseExit()
    {
        if(SceneManager.GetActiveScene().name != "BattleScene") return;
        _currentCoroutine = battleManager.FadeToColor(defaultColor, _currentCoroutine, spriteRenderer, fadeTime);
    }

    public virtual void UpdateNPC()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerIsClose && !_isTyping)
        {
            if(!dialoguePanel.activeSelf)
            {
                dialogueText.text = "";
                _i = 0;

                SetDialoguePanel();
                StartCoroutine(Typing());
            }
            else
            {
                dialoguePanel.SetActive(false);
                StopCoroutine(Typing());
                ResetText();
            }
        }else if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && _playerIsClose && _isTyping)
        {
            StopCoroutine(Typing());
            dialogueText.text = dialogue[_i];
            //continueButton.SetActive(true);
        }else if(Input.GetKeyDown(KeyCode.Return) && !_isTyping && _playerIsClose && dialoguePanel.activeSelf)
        {
            NextLine();
        }

        if(dialogueText.text == dialogue[_i])
        {
            //continueButton.SetActive(true);
        }
    }

    public void SetDialoguePanel()
    {
        dialoguePanel.SetActive(true);

        GameObject npcImage = GameObject.FindGameObjectWithTag("NPC_Image");
        npcImage.GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        npcImage.GetComponent<Image>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
        npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 1);

        GameObject npcName = GameObject.FindGameObjectWithTag("NPC_Name");
        npcName.GetComponent<TextMeshProUGUI>().text = this.gameObject.name.ToString();

        GameObject playerImage = GameObject.FindGameObjectWithTag("Player_Image");
        playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0.75f);

    }

    public virtual void ResetText()
    {
        if(_isBattling) return;

        dialogueText.text = "";
        dialogueText.alignment = TextAlignmentOptions.Left;
        _i = 0;

        if(dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
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

    public virtual void NextLine()
    {
        //continueButton.SetActive(false);

        if(_i < dialogue.Count - 1)
        {
            _i++;
            CheckCharacter(_i);
            dialogueText.text = "";
            StartCoroutine(Typing());
        }else
        {
            ResetText();
        }
    }
    
    protected virtual void CheckCharacter(int i){}

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerIsClose = true;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerIsClose = false;
            ResetText();
        }
    }

    public void AnimateAttack()
    {
        if (!_isBattling) return;

        battleAnimator.SetTrigger("Attack");
    }
}
