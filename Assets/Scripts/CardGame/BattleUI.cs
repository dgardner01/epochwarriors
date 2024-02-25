using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public float cardSpaceMin;
    public float cardSpaceMax;
    public float cardCenterMin;
    public float cardCenterMax;

    public BattleSystem battleSystem;
    public Hand hand => battleSystem.hand;
    public List<GameObject> handObjects;
    public PlayArea playArea => battleSystem.playArea;
    public List<GameObject> playAreaObjects;
    public EnemyPlayArea enemyPlayArea => battleSystem.enemyPlayArea;
    public List<GameObject> enemyPlayAreaObjects;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public List<GameObject> playerComboObjects;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;
    public List<GameObject> enemyComboObjects;

    public List<GameObject> playerStatusEffectIcons;
    public List<GameObject> enemyStatusEffectIcons;

    public Image playerHealthBar;
    public Image playerBlockBar;
    public Image enemyHealthBar;
    public Image enemyBlockBar;

    public GameObject cardGamePanel;
    public GameObject fightPanel;

    public TextMeshProUGUI drawPileCount;
    public TextMeshProUGUI discardPileCount;
    public TextMeshProUGUI energyCount;

    private void Update()
    {
        CardsDisplay(hand.cards, handObjects, hand.gameObject, true);
        CardsDisplay(playArea.cards, playAreaObjects, playArea.gameObject, true);
        CardsDisplay(playerCombo.cards, playerComboObjects, playerCombo.gameObject, false);
        EnemyCardsDisplay(enemyPlayArea.intents, enemyPlayAreaObjects);
        EnemyCardsDisplay(enemyCombo.intents, enemyComboObjects);
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
    public void CardsDisplay(List<Card> cards, List<GameObject> objects, GameObject container, bool scaled)
    {
        for (int i = 0; i < objects.Count; i++)
        { 
            if (i < cards.Count)
            {
                objects[i].SetActive(true);
                if (cards[i] != null)
                {
                    objects[i].GetComponent<CardDisplay>().card = cards[i];
                }
                if (scaled)
                {
                    Vector3 _pos = new Vector3();
                    float scaledSpacing = Mathf.Lerp(cardSpaceMin, cardSpaceMax, cards.Count / 10);
                    _pos.x = scaledSpacing * i;
                    _pos.y = objects[i].transform.localPosition.y;
                    objects[i].transform.localPosition = _pos;
                }
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
        if (scaled)
        {
            Vector3 pos = new Vector3();
            pos.x = Mathf.Lerp(cardCenterMin, cardCenterMax, cards.Count / 10f);
            pos.y = container.transform.localPosition.y;
            container.transform.localPosition = pos;
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
        playerBlockBar.fillAmount = (float)battleSystem.player.block / (float)battleSystem.player.maxHealth;
        enemyHealthBar.fillAmount = (float)battleSystem.enemy.health / (float)battleSystem.enemy.maxHealth;
        enemyBlockBar.fillAmount = (float)battleSystem.enemy.block / (float)battleSystem.enemy.maxHealth;
    }
}
