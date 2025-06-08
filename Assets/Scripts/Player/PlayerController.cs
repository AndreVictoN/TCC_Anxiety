using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public InputActionReference move;

    #region tags to compare
    [Header("ScenesToLoad")]
    public string prototypeScene = "PrototypeScene";
    public string firstFloor = "FirstFloorPrototype";
    public string battleScene = "BattleScene";

    private string npcTag = "NPC";
    private string doorTag = "Door";
    private string stairsTag = "Stairs";
    #endregion

    [Header("HoverSettings")]
    public SpriteRenderer spriteRenderer;
    public Color defaultColor;
    public float fadeTime = 1f;

    [Header("BattleSettings")]
    public BattleManager battleManager;
    public GameObject enemy;
    public float attackTime = 0.5f;
    public Vector3 defaultPosition;

    //Privates
    private Vector2 _moveDirection;
    private Vector3 _positionBeforeFloor;
    private bool _canMove = true;
    private bool _isBattleScene = false;
    private bool _isMoving = false;
    private Coroutine _currentCoroutine;
    private Tween _currentTween;

    void Awake()
    {
        if(SceneManager.GetActiveScene().name == battleScene)
        {
            defaultPosition = new Vector3(0, -3.11f, 0);
            this.gameObject.transform.position = defaultPosition;
        }else{
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }
        
        if(rb == null && this.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            rb = this.gameObject.GetComponent<Rigidbody2D>();
        }

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == battleScene) 
        {
            _isBattleScene = true;

            _currentTween = battleManager.GoToDefaultPosition(this.gameObject, _isMoving, _currentTween, defaultPosition, attackTime);
        }

        _canMove = !_isBattleScene;

        if(_canMove) _moveDirection = move.action.ReadValue<Vector2>();
    }

    void OnMouseOver()
    {
        Color colorToFade = spriteRenderer.color;
        colorToFade.b += 1f;
        _currentCoroutine = battleManager.FadeToColor(colorToFade, _currentCoroutine, spriteRenderer, fadeTime);

        if(Input.GetMouseButtonDown(0) && this.gameObject.transform.position == defaultPosition)
        {
            Vector2 position = enemy.transform.position;
            position.y -= 0.8f;
            PlayerMovement(position);
        }
    }

    public void PlayerMovement(Vector2 position)
    {
        _isMoving = true;
        _currentTween?.Kill();
        _currentTween = transform.DOLocalMove(position, attackTime).SetEase(Ease.InQuad).OnComplete(() => _isMoving = false);
    }

    void OnMouseExit()
    {
        _currentCoroutine = battleManager.FadeToColor(defaultColor, _currentCoroutine, spriteRenderer, fadeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(npcTag) || collision.gameObject.CompareTag(doorTag))
        {
            rb.linearVelocity = Vector2.zero;
        }else if(collision.gameObject.CompareTag(stairsTag))
        {
            if(SceneManager.GetActiveScene().name == prototypeScene) StartCoroutine(LoadScene(firstFloor));
            else if(SceneManager.GetActiveScene().name == firstFloor) StartCoroutine(LoadScene(prototypeScene));

            _canMove = false;
        }
    }

    IEnumerator LoadScene(string sceneName)
    {
        transform.DOMoveY(-2, 1f).SetRelative();

        yield return new WaitForSeconds(1f);

        _positionBeforeFloor = this.gameObject.transform.position;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        transform.DOMoveY(2.5f, 1f).SetRelative();

        yield return new WaitForSeconds(1f);

        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        _canMove = true;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        rb.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }
}
