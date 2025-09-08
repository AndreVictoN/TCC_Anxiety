using UnityEngine;

public class Items : MonoBehaviour
{
    public bool canGet;
    public InventoryManager ivManager;
    private PlayerController _player;

    private void Start()
    {
        canGet = false;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _player.RegisterToSubscribers(ivManager);
    }

    void Update()
    {
        if(canGet && Input.GetKeyDown(KeyCode.E))
        {
            ivManager.LastItemRecieved(this.gameObject.GetComponent<SpriteRenderer>().sprite);
            _player.NotifyFromItem(EventsEnum.NewItem);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canGet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canGet = false;
        }
    }
}
