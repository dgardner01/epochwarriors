using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI title, spiritCost, description;
    public TextMeshProUGUI[] symbolMagnitudes;
    public Image bg;
    public Sprite[] bgs;
    public Image[] symbols;
    public Sprite[] symbolSprites;
    public Image chain;
    public Sprite[] comboChains;
    BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();

    public bool hover;
    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player player = battleSystem.player;
        title.text = card.name;
        spiritCost.text = ""+card.spiritCost;
        description.text = card.description;
        if (description.text == "")
        {
            description.gameObject.SetActive(false);
        }
        else
        {
            description.gameObject.SetActive(true);
        }
        chain.sprite = comboChains[card.comboPosition];
        switch (card.cardType)
        {
            case CardType.Attack:
                bg.sprite = bgs[0];
                symbols[0].sprite = symbolSprites[0];
                if (player.strength > 0)
                {
                    symbolMagnitudes[0].text = "" + (card.damage+player.strength);
                }
                else
                {
                    symbolMagnitudes[0].text = "" + card.damage;
                }
                if (card.statusEffect != null)
                {
                    symbols[1].sprite = card.statusEffect.symbol;
                    symbolMagnitudes[1].text = ""+Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[1].sprite = null;
                }
                break;
            case CardType.Block:
                bg.sprite = bgs[1];
                symbols[0].sprite = symbolSprites[1];
                if (card.statusEffect != null)
                {
                    symbols[1].sprite = card.statusEffect.symbol;
                    if (card.statusEffect.id == "Parry")
                    {
                        Enemy enemy = battleSystem.enemy;
                        int halvedDamage = 0;
                        if (enemy.FirstAttackInTurn() != null)
                        {
                            halvedDamage = (enemy.FirstAttackInTurn().damage + enemy.strength) / 2;
                        }
                        card.statusEffect.magnitude = halvedDamage;
                        card.block = halvedDamage;
                    }
                    symbolMagnitudes[1].text = "" + Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[1].sprite = null;
                }
                symbolMagnitudes[0].text = ""+card.block;
                if (card.damage > 0)
                {
                    symbols[1].sprite = symbolSprites[0];
                    symbolMagnitudes[1].text = card.damage + "";
                    if (player.strength > 0)
                    {
                        symbolMagnitudes[1].text = "" + (card.damage + player.strength);
                    }
                    else
                    {
                        symbolMagnitudes[1].text = "" + card.damage;
                    }
                }
                break;
            case CardType.Skill:
                bg.sprite = bgs[2];
                if (card.statusEffect != null)
                {
                    symbols[0].sprite = card.statusEffect.symbol;
                    symbolMagnitudes[0].text = "" + Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[0].sprite = null;
                }
                symbols[1].sprite = null;
                break;
        }
        for (int i = 0; i < symbols.Length; i++)
        {
            if (symbols[i].sprite == null)
            {
                symbols[i].gameObject.SetActive(false);
            }
            else
            {
                symbols[i].gameObject.SetActive(true);
            }
        }
    }
    public void PlayCard()
    {
        if (card == null)
        {
            return;
        }
        if (transform.parent.GetComponent<Hand>())
        {
            battleSystem.PlayCard(card);
            return;
        }
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
