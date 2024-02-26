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
    public GameObject cardDisplayPrefab;
    public Hand hand => battleSystem.hand;
    public PlayArea playArea => battleSystem.playArea;
    public EnemyPlayArea enemyPlayArea => battleSystem.enemyPlayArea;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;

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
        CardsDisplay(hand.cards);
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
    public void CardsDisplay(List<Card> cards)
    {
        Transform _hand = hand.gameObject.transform;
        if (_hand.childCount < hand.cards.Count)
        {
            for (int i = 0; i < hand.cards.Count - _hand.childCount; i++)
            {
                Instantiate(cardDisplayPrefab, hand.gameObject.transform);
            }
        }
        for (int i = 0; i < _hand.childCount; i++)
        {
            Transform card = _hand.GetChild(i);
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
