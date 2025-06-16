using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject player;
    public Sprite doorOpened;
    public Sprite doorClosed;

    //Privates
    private bool _playerIsClose;
    private bool _isClosed;

    void Start()
    {
        _playerIsClose = false;
        _isClosed = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerIsClose)
        {
            if(_isClosed)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = doorOpened;
                Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), this.gameObject.GetComponent<BoxCollider2D>(), true);

                _isClosed = false;
            }else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
                Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), this.gameObject.GetComponent<BoxCollider2D>(), false);

                _isClosed = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerIsClose = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerIsClose = false;
        }
    }
}
