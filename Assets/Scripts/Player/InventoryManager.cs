using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IObserver
{
    public List<Image> itemsImages = new();
    public Image currentMask;
    public Sprite fan;
    private Animator _animator;
    private int _selectedSlot;
    [SerializeField] private Color _defaultSlotColor;

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#EF776F", out _defaultSlotColor);
        _selectedSlot = 1;

        int i = 0;
        foreach (Image image in itemsImages)
        {
            if (image.gameObject.transform.parent != null && i != 0)
            {
                _animator = image.gameObject.transform.parent.gameObject.GetComponent<Animator>();

                _animator.enabled = false;
            }

            i++;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_selectedSlot == 1)
            {
                _selectedSlot = 2;
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 3)
            {
                _selectedSlot = 4;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 5)
            {
                _selectedSlot = 6;
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_selectedSlot == 2)
            {
                _selectedSlot = 1;
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 4)
            {
                _selectedSlot = 3;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 6)
            {
                _selectedSlot = 5;
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_selectedSlot == 1)
            {
                _selectedSlot = 3;
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 2)
            {
                _selectedSlot = 4;
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 3)
            {
                _selectedSlot = 5;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 4)
            {
                _selectedSlot = 6;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_selectedSlot == 3)
            {
                _selectedSlot = 1;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 4)
            {
                _selectedSlot = 2;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 5)
            {
                _selectedSlot = 3;
                itemsImages[4].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[2].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
            else if (_selectedSlot == 6)
            {
                _selectedSlot = 4;
                itemsImages[5].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                itemsImages[3].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
            }
        }
    }

    private void CheckItems()
    {
        foreach (Image image in itemsImages)
        {
            if (image.gameObject.activeSelf == false)
            {
                image.gameObject.SetActive(true);
                image.sprite = fan;
                break;
            }
        }
    }

    public void OnNotify(EventsEnum evt)
    {
        if(evt == EventsEnum.NewItem)
        {
            CheckItems();
        }
    }
}
