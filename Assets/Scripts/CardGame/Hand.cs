using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public BattleSystem battleSystem;
    public Player player;
    public List<Card> cards;

    public IEnumerator DrawCard(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            if (player.drawPile.Count > 0)
            {
                int rand = Random.Range(0, player.drawPile.Count);
                Card drawnCard = player.drawPile[rand];
                cards.Add(drawnCard);
                player.drawPile.Remove(drawnCard);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                Shuffle();
                break;
            }
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < battleSystem.discard.cards.Count; i++)
        {
            Card card = battleSystem.discard.cards[0];
            battleSystem.player.drawPile.Add(card);
            battleSystem.discard.cards.Remove(card);
        }
        StartCoroutine(DrawCard(5-battleSystem.hand.cards.Count));
    }
}
