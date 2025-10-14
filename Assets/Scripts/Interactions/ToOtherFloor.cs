using UnityEngine;
using UnityEngine.SceneManagement;

public class ToOtherFloor : MonoBehaviour
{
    public GameObject player;
    public GameObject myText;

    //Privates
    private bool _playerIsClose;

    void Start()
    {
        _playerIsClose = false;
        if(!myText) myText = transform.Find("Text").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerIsClose)
        {
            PlayerPrefs.SetString("pastScene", SceneManager.GetActiveScene().name);

            if (SceneManager.GetActiveScene().name.Equals("Terreo"))
            {
                if (this.gameObject.transform.localPosition.y < 0) { StartCoroutine(GameObject.FindAnyObjectByType<GameManager>().FrontTransition(1f, "Floor2")); }
            }
            else if (SceneManager.GetActiveScene().name.Equals("Floor2"))
            {
                if (this.gameObject.transform.localPosition.x > 0) { StartCoroutine(GameObject.FindAnyObjectByType<GameManager>().BackTransition(1f, "Terreo")); }
                else if (this.gameObject.transform.localPosition.x < 0) { StartCoroutine(GameObject.FindAnyObjectByType<GameManager>().FrontTransition(1f, "Terreo")); }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player))
        {
            _playerIsClose = true;
            if (myText) myText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player))
        {
            _playerIsClose = false;
            if (myText) myText.SetActive(false);
        }
    }
}
