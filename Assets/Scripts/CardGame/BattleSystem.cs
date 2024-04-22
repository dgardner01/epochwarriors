using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : StateMachine
{
    public BattleUI ui;
    public BattleVFX vfx;
    public Player player;
    public Enemy enemy;
    public Hand hand;
    public Discard discard;
    public PlayArea playArea;
    public EnemyPlayArea enemyPlayArea;
    public PlayerCombo playerCombo;
    public EnemyCombo enemyCombo;
    public float anticipationMinDamage;
    [Header("Card Events")]
    public UnityEvent OnCardDraw;
    public UnityEvent OnCardHover;
    public UnityEvent OnCardPickup;
    public UnityEvent OnCardSwap;
    public UnityEvent OnCardPutdown;
    public UnityEvent OnCardPlay;
    public UnityEvent OnCardDiscard;
    public UnityEvent OnCardShuffle;
    [Header("Combat Events")]
    public UnityEvent OnGainBlock;
    public UnityEvent OnAttackBlocked;
    public UnityEvent OnAnticipation;
    [Header("Game Events")]
    public UnityEvent OnStatusEffectUp;
    public UnityEvent OnStatusEffectDown;
    public UnityEvent OnSpiritGain;
    public UnityEvent OnSpiritReduce;
    public UnityEvent OnNotEnoughSpirit;
    [Header("Misc Events")]
    public UnityEvent OnChainBegin;
    public UnityEvent OnChainExtended;
    public UnityEvent OnChargeBegin;
    public UnityEvent OnChargeExtended;
    private void Start()
    {
        SetState(new Begin(this));
    }

    public void PlayCard(Card card)
    {
        OnSpiritReduce.Invoke();
        hand.cards.Remove(card);
        playArea.cards.Add(card);
        player.spirit -= card.spiritCost;
    }
    public void ReturnCard(Card card)
    {
        OnSpiritGain.Invoke();
        playArea.cards.Remove(card);
        hand.cards.Add(card);
        player.spirit += card.spiritCost;
    }
    public void AssignEnemyIntent()
    {
        //StartCoroutine(enemy.AssignIntent());
    }
    public void EndTurn()
    {
        SetState(new Battle(this));
    }
    public void ResolveInstantCard(Card card)
    {
        if (card.block > 0)
        {
            player.block += card.block;
        }
        if (card.statusEffect != null)
        {
            player.ApplyStatusEffect(card.statusEffect.CreateStatusEffect());
        }
        playArea.cards.Remove(card);
        discard.cards.Add(card);
    }
    public IEnumerator InitializeCombos()
    {
        player.spirit = 0;
        int cards = playArea.cards.Count;
        int chains = playArea.chain;
        for (int i = 0; i < cards; i++)
        {
            OnCardDiscard.Invoke();
            CardDisplay display = playArea.transform.GetChild(0).gameObject.GetComponent<CardDisplay>();
            if (display.card.name != "Scale Shield")
            {
                if (chains > 0)
                {
                    player.chain++;
                    chains--;
                    display.chained = false;
                }
                playerCombo.cards.Add(playArea.cards[0]);
                ui.ReparentCard(playArea.transform.GetChild(0).gameObject, ui.playedZone.transform);
            }
            else
            {
                OnGainBlock.Invoke();
                player.animator.PlayAnimationClipByName("guard");
                player.block += display.card.block;
                ui.ReparentCard(playArea.transform.GetChild(0).gameObject, ui.discard.transform);
            }
            discard.cards.Add(playArea.cards[0]);
            playArea.cards.Remove(playArea.cards[0]);
            yield return new WaitForSeconds(0.1f);
        }
        cards = hand.cards.Count;
        for (int i = 0; i < cards; i++)
        {
            OnCardDiscard.Invoke();
            ui.ReparentCard(hand.transform.GetChild(0).gameObject, ui.discard.transform);
            discard.cards.Add(hand.cards[0]);
            hand.cards.Remove(hand.cards[0]);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
