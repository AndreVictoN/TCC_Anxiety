using UnityEngine;

public class Items : MonoBehaviour
{
    public bool canGet;

    private void Awake()
    {
        canGet = false;
    }

    void Update()
    {
        if(canGet && Input.GetKeyDown(KeyCode.E))
        {
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
