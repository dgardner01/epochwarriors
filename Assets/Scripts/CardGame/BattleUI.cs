using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public float idealCardSpacing;
    float cardSpacing;
    public float cardSpeed;
    public BattleSystem battleSystem;
    public GameObject cardDisplayPrefab;
    public Hand hand => battleSystem.hand;
    public PlayArea playArea => battleSystem.playArea;
    public EnemyPlayArea enemyPlayArea => battleSystem.enemyPlayArea;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;

    public List<GameObject> playerStatusEffectIcons;
    public List<GameObject> enemyStatusEffectIcons;

    public Image playerHealthBar;
    public Image enemyHealthBar;

    public GameObject cardGamePanel;
    public GameObject fightPanel;

    public TextMeshProUGUI drawPileCount;
    public TextMeshProUGUI discardPileCount;
    public TextMeshProUGUI energyCount;

    private void Update()
    {
        CardsDisplay(hand.cards, hand.gameObject.GetComponent<RectTransform>());
        CardsDisplay(playArea.cards, playArea.gameObject.GetComponent<RectTransform>());
        //CardsDisplay(playArea.cards, playAreaObjects, playArea.gameObject, true);
        //CardsDisplay(playerCombo.cards, playerComboObjects, playerCombo.gameObject, false);
        //EnemyCardsDisplay(enemyPlayArea.intents, enemyPlayAreaObjects);
        //EnemyCardsDisplay(enemyCombo.intents, enemyComboObjects);
        CardCountDisplay();
        EnergyDisplay();
        HealthBarDisplay();
    }
    public void PrintLog(string log)
    {
        print(log);
    }

    public void EndTurnButtonPressed()
    {
        battleSystem.EndTurn();
    }
    public void CardsDisplay(List<Card> cards, RectTransform container)
    {
        //if there are not enough card display objects for the number of cards, instantiate new ones
        if (container.childCount < cards.Count)
        {
            for (int i = 0; i < cards.Count - container.childCount; i++)
            {
                Instantiate(cardDisplayPrefab, container.gameObject.transform);
            }
        }
        if (cards.Count > 0)
        {
            float containerWidth = container.sizeDelta.x;
            float idealHandWidth = cards.Count * idealCardSpacing;
            float handWidth = cards.Count * cardSpacing;
            float middle = (handWidth / 2)-cardSpacing/2;
            cardSpacing = idealCardSpacing;
            //if the total width of the cards with ideal spacing is greater than the container, scale the spacing
            if (idealHandWidth > containerWidth)
            {
                cardSpacing = containerWidth / cards.Count;
            }
            for (int i = 0; i < container.childCount; i++)
            {
                //hoverIndex is the list index of the card currently being hovered over
                //by default the middle card is considered hovered over
                int hoverIndex = Mathf.RoundToInt(cards.Count / 2);
                for (int j = 0; j < container.childCount; j++)
                {
                    Transform _card = container.GetChild(j);
                    CardDisplay _display = _card.gameObject.GetComponent<CardDisplay>();
                    if (_display.hover)
                    {
                        hoverIndex = j;
                    }
                }
                Transform card = container.GetChild(i);
                CardDisplay display = card.GetComponent<CardDisplay>();
                //by default, displays are inactive
                card.gameObject.SetActive(false);
                int distanceFromHover = i - hoverIndex;
                //only use as many displays as there are cards
                if (cards.Count > 0 && i < cards.Count)
                {
                    card.gameObject.SetActive(true);
                    display.card = cards[i];
                    float hoverDist = Mathf.Abs(distanceFromHover);
                    float maxDist = 2;
                    float dist = distanceFromHover * Mathf.Lerp(cardSpacing / 4, 0, hoverDist / maxDist);
                    float offset = (i * cardSpacing) + dist;
                    Vector3 currentPos = card.transform.localPosition;
                    Vector3 targetPos = new Vector2(offset-middle, 0);
                    Vector3 lerpedPos = Vector3.Lerp(currentPos, targetPos, cardSpeed);
                    card.transform.localPosition = lerpedPos;
                }
            }
        }
    }
    public void EnemyCardsDisplay(List<Enemy.Intents> intents, List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (i < intents.Count)
            {
                objects[i].SetActive(true);
                objects[i].GetComponent<EnemyCardDisplay>().intent = intents[i];
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
    }
    public void CardCountDisplay()
    {
        drawPileCount.text = battleSystem.player.drawPile.Count + "";
        discardPileCount.text = battleSystem.discard.cards.Count + "";
    }
    public void EnergyDisplay()
    {
        energyCount.text = battleSystem.player.energy + "";
    }
    public void HealthBarDisplay()
    {
        playerHealthBar.fillAmount = (float)battleSystem.player.health / (float)battleSystem.player.maxHealth;
        enemyHealthBar.fillAmount = (float)battleSystem.enemy.health / (float)battleSystem.enemy.maxHealth;
    }
}
