using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : Fighter
{ 
    public BattleSystem battleSystem;
    public BattleUI ui => battleSystem.ui;
    public int damage;
    public int turnIndex;
    public List<TurnPattern> turnPattern = new List<TurnPattern>();
    public List<GameObject> intentObjects;
    public List<Card> currentTurn;
    private void Start()
    {
        
    }
    private void Update()
    {
        DisplayIntents();
    }
    public void UpdateTurnIndex()
    {
        turnIndex++;
        turnIndex %= turnPattern.Count;
        currentTurn.Clear();
        for (int i = 0; i < turnPattern[turnIndex].turn.Count; i++)
        {
            currentTurn.Add(turnPattern[turnIndex].turn[i]);
        }
    }
    void DisplayIntents()
    {
        for (int i = 0; i < intentObjects.Count; i++)
        {
            if (i < currentTurn.Count)
            {
                intentObjects[i].SetActive(true);
                CardDisplay displayTemplate = ui.cardDisplayPrefab.GetComponent<CardDisplay>();
                Image symbol = intentObjects[i].GetComponent<Image>();
                TextMeshProUGUI number = intentObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                Card card = currentTurn[i];
                switch (card.cardType)
                {
                    case CardType.Attack:
                        symbol.sprite = displayTemplate.symbolSprites[0];
                        number.text = "" + (card.damage+strength);
                        break;
                    case CardType.Block:
                        symbol.sprite = displayTemplate.symbolSprites[1];
                        number.text = "" + card.block;
                        break;
                    case CardType.Skill:
                        symbol.sprite = currentTurn[i].statusEffect.symbol;
                        number.text = "" + card.statusEffect.magnitude;
                        break;
                }
            }
            else
            {
                intentObjects[i].SetActive(false);
            }
        }
    }
    public Card FirstAttackInTurn()
    {
        for (int i = 0; i < currentTurn.Count; i++)
        {
            if (currentTurn[i].damage > 0)
            {
                return currentTurn[0];
            }
        }
        return null;
    }
}

[System.Serializable]
public class TurnPattern
{
    public List<Card> turn = new List<Card>();
}
