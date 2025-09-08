using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IObserver
{
    public List<Image> itemsImages = new();
    public Image currentMask;
    public List<Sprite> itemsSprites = new();
    public Image alexImage;
    private Animator _animator;
    private int _selectedSlot;
    [SerializeField] private Color _defaultSlotColor;
    private GameObject _currentItemImage;
    private GameObject _player;
    private int _lastItemSpriteIndex;

    #region Masks Images
    [SerializeField] private Sprite _alexNoMask;
    [SerializeField] private List<Sprite> _alexMasks;
    #endregion

    void Awake()
    {
        alexImage.sprite = _alexNoMask;
        if (_player == null) { _player = GameObject.FindGameObjectWithTag("Player"); }
        BasicSettings();
    }

    void OnEnable()
    {
        BasicSettings();
    }

    private void BasicSettings()
    {
        ColorUtility.TryParseHtmlString("#EF776F", out _defaultSlotColor);
        _selectedSlot = 1;

        int i = 0;
        foreach (Image image in itemsImages)
        {
            if (image.gameObject.transform.parent != null && i != 0)
            {
                _animator = image.gameObject.transform.parent.gameObject.GetComponent<Animator>();
                _animator.SetBool("DEACTIVATE", true);
                //_animator.enabled = false;
            }

            i++;
        }
    }

    void Update()
    {
        Navigate();
        Select();
    }

    private void SelectNewSlot(int selectedSlot, int deactivated, int activated)
    {
        _selectedSlot = selectedSlot;
        itemsImages[deactivated].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", true);
        itemsImages[activated].gameObject.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
        itemsImages[activated].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("DEACTIVATE", false);
    }

    private void Navigate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_selectedSlot == 1) { SelectNewSlot(2, 0, 1); }
            else if (_selectedSlot == 3) { SelectNewSlot(4, 2, 3); }
            else if (_selectedSlot == 5) { SelectNewSlot(6, 4, 5); }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_selectedSlot == 2) { SelectNewSlot(1, 1, 0); }
            else if (_selectedSlot == 4) { SelectNewSlot(3, 3, 2); }
            else if (_selectedSlot == 6) { SelectNewSlot(5, 5, 4); }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_selectedSlot == 1) { SelectNewSlot(3, 0, 2); }
            else if (_selectedSlot == 2) { SelectNewSlot(4, 1, 3); }
            else if (_selectedSlot == 3) { SelectNewSlot(5, 2, 4); }
            else if (_selectedSlot == 4) { SelectNewSlot(6, 3, 5); }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_selectedSlot == 3) { SelectNewSlot(1, 2, 0); }
            else if (_selectedSlot == 4) { SelectNewSlot(2, 3, 1); }
            else if (_selectedSlot == 5) { SelectNewSlot(3, 4, 2); }
            else if (_selectedSlot == 6) { SelectNewSlot(4, 5, 3); }
        }
    }

    private void Select()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _currentItemImage = itemsImages[_selectedSlot - 1].gameObject.transform.parent?.Find("ItemImage").gameObject;

            if (_currentItemImage.activeSelf)
            {
                if (!currentMask.gameObject.activeSelf)
                {
                    currentMask.gameObject.SetActive(true);
                    currentMask.sprite = _currentItemImage.GetComponent<Image>().sprite;

                    _currentItemImage.SetActive(false);
                }
                else
                {
                    (_currentItemImage.GetComponent<Image>().sprite, currentMask.sprite) = (currentMask.sprite, _currentItemImage.GetComponent<Image>().sprite);
                }

                for (int i = 0; i < itemsSprites.Count; i++)
                {
                    if (currentMask.sprite == itemsSprites[i])
                    {
                        alexImage.sprite = _alexMasks[i];
                    }
                }

                if (_player == null) { _player = GameObject.FindGameObjectWithTag("Player"); }
                _player?.GetComponent<HumanPlayer>().SetIsMasked(true);
            }
            else
            {
                if (currentMask.gameObject.activeSelf)
                {
                    if (itemsSprites.Contains(currentMask.sprite))
                    {
                        _currentItemImage.GetComponent<Image>().sprite = currentMask.sprite;
                        if (!_currentItemImage.activeSelf) _currentItemImage.SetActive(true);

                        alexImage.sprite = _alexNoMask;

                        if (_player == null) { _player = GameObject.FindGameObjectWithTag("Player"); }
                        _player?.GetComponent<HumanPlayer>().SetIsMasked(false);
                    }

                    currentMask.gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckItems()
    {
        foreach (Image image in itemsImages)
        {
            if (!image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(true);
                image.sprite = itemsSprites[_lastItemSpriteIndex];
                break;
            }
        }
    }

    public void OnNotify(EventsEnum evt)
    {
        if (evt == EventsEnum.NewItem)
        {
            CheckItems();
        }
    }

    public void LastItemRecieved(Sprite itemSprite)
    {
        if (itemsSprites.Contains(itemSprite))
        {
            _lastItemSpriteIndex = itemsSprites.IndexOf(itemSprite);
        }
    }
}
