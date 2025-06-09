using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public List<GameObject> cardsList = new List<GameObject>();
    public Transform transformCard;
    public Vector3 defaultPosition;

    #region Privates
    private bool _isClicked;
    #endregion

    void Awake()
    {
        cardsList.AddRange(GameObject.FindGameObjectsWithTag("Card"));
        transformCard = this.gameObject.transform;
        defaultPosition = transformCard.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Animator>().enabled = false;

        if(!_isClicked)
        {
            transformCard.position = new Vector3(transformCard.position.x, 100, transformCard.position.z);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isClicked = true;

        Vector3 newPosition = new Vector3(defaultPosition.x, defaultPosition.y + 236.4f, defaultPosition.z);

        transformCard.position = newPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Animator>().enabled = true;
        _isClicked = false;
    }
}
