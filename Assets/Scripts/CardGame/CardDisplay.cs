using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>().GetComponent<BattleSystem>();
    public Card card;
    public int energyCost;
    public string name;
    public string description;

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
}
