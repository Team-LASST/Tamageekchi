using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private Sprite unselectSprite, selectSprite;

    public Button button { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpdateSelection(bool select)
    {
        GetComponent<Image>().sprite = select ? selectSprite : unselectSprite;
        GetComponent<Image>().color = new Color(1, 1, 1, select ? 1f : 0.5f);
        GetComponent<Button>().interactable = !select;
    }
}
