using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDrawManager : MonoBehaviour
{
    public List<GameObject> rows = new List<GameObject>();
    public GameObject player;
    public SpriteRenderer playerSR;

    void Awake()
    {
        rows.AddRange(GameObject.FindGameObjectsWithTag("Row"));
        player = GameObject.FindGameObjectWithTag("Player");
        playerSR = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        foreach(GameObject row in rows)
        {
            if(player.transform.position.y < row.transform.position.y)
            {
                playerSR.sortingOrder = row.GetComponent<SortingGroup>().sortingOrder + 1; 
            }else
            {
                playerSR.sortingOrder = row.GetComponent<SortingGroup>().sortingOrder - 1; 
            }
        }
    }
}
