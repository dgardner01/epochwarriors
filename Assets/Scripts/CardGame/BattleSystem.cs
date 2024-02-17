using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : StateMachine
{
    public BattleUI ui;
    public Player player;
    public Enemy enemy;
    public Hand hand;
    public Discard discard;
    public PlayArea playArea;
    public EnemyPlayArea enemyPlayArea;
    public PlayerCombo playerCombo;
    public EnemyCombo enemyCombo;
    private void Start()
    {
        SetState(new Begin(this));
    }

    public void PlayCard(Card card)
    {
        if (player.energy >= card.energyCost)
        {
            hand.cards.Remove(card);
            playArea.cards.Add(card);
            player.energy -= card.energyCost;
        }
    }
    public void ReturnCard(Card card)
    {
        player.energy += card.energyCost;
        playArea.cards.Remove(card);
        hand.cards.Add(card);
    }
    public void AssignEnemyIntent()
    {
        StartCoroutine(enemy.AssignIntent());
    }
    public void EndTurn()
    {
        SetState(new Battle(this));
    }
    public void InitializeCombos()
    {
        int cards = playArea.cards.Count;
        for(int i = 0; i < cards; i++)
        {
            playerCombo.cards.Add(playArea.cards[0]);
            discard.cards.Add(playArea.cards[0]);
            playArea.cards.Remove(playArea.cards[0]);
        }
        cards = hand.cards.Count;
        for (int i = 0; i < cards; i++)
        {
            discard.cards.Add(hand.cards[0]);
            hand.cards.Remove(hand.cards[0]);
        }
        int intents = enemyPlayArea.intents.Count;
        for (int i = 0; i < intents; i++)
        {
            enemyCombo.intents.Add(enemyPlayArea.intents[0]);
            enemyPlayArea.intents.Remove(enemyPlayArea.intents[0]);
        }
    }
}
