using UnityEngine;

public class Items : Subject
{
    public bool canGet;
    public InventoryManager ivManager;

    private void Start()
    {
        canGet = false;

        Subscribe(ivManager);
    }

    void Update()
    {
        if(canGet && Input.GetKeyDown(KeyCode.E))
        {
            Notify(EventsEnum.NewItem);
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
