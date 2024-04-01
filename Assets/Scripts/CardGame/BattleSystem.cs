using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void PlayCard(Card card)
    {
        hand.cards.Remove(card);
        playArea.cards.Add(card);
        player.spirit -= card.spiritCost;
    }
    public void ReturnCard(Card card)
    {
        player.spirit += card.spiritCost;
        playArea.cards.Remove(card);
        hand.cards.Add(card);
    }
    public void ResolveInstantCard(Card card)
    {
        switch (card.cardType)
        {
            case CardType.Block:
                player.block += card.block;
                break;
        }
        if (card.statusEffect != null)
        {
            player.ApplyStatusEffect(card.statusEffect.CreateStatusEffect());
        }
        player.spirit -= card.spiritCost;
        discard.cards.Add(card);
        hand.cards.Remove(card);
    }
    public void AssignEnemyIntent()
    {
        //StartCoroutine(enemy.AssignIntent());
    }
    public void EndTurn()
    {
        SetState(new Battle(this));
    }
    public void InitializeCombos()
    {
        player.spirit = 0;
        int cards = playArea.cards.Count;
        for (int i = 0; i < cards; i++)
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
    }
}
