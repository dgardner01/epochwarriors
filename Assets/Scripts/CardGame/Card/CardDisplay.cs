using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>().GetComponent<BattleSystem>();
    public Sprite[] bgs;
    public Sprite[] symbols;
    public Image bg;
    public Image symbol;
    public Card card;
    public int energyCost;
    public string name;
    public string description;
    public bool hover;

    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private void Update()
    {
        if (card != null)
        {
            energyCostText.text = card.energyCost + "";
            nameText.text = card.name;
            descriptionText.text = card.description;
            if (card.damage > 0)
            {
                bg.sprite = bgs[0];
                symbol.sprite = symbols[0];
                descriptionText.text = card.damage + "";
            }
            else if (card.block > 0)
            {
                bg.sprite = bgs[1];
                symbol.sprite = symbols[1];
                descriptionText.text = card.block + "";
            }
            else
            {
                bg.sprite = bgs[2];
                symbol.sprite = symbols[2];
                descriptionText.text = "2";
            }
        }
    }
    public void PlayCard()
    {
        battleSystem.PlayCard(card);
    }
    public void ReturnCard()
    { 
        battleSystem.ReturnCard(card);
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hover = false;
    }
}
