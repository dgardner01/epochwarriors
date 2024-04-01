using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public BattleSystem battleSystem;
    public Player player => battleSystem.player;
    public List<Card> cards;

    public IEnumerator DrawCard(int numCards)
    {
        int cardsDrawn = 0;
        for (int i = 0; i < numCards; i++)
        {
            if (player.drawPile.Count > 0)
            {
                int rand = Random.Range(0, player.drawPile.Count);
                Card drawnCard = player.drawPile[rand];
                cards.Add(drawnCard);
                print(drawnCard.name);
                battleSystem.ui.AddCardToHand(drawnCard);
                player.drawPile.Remove(drawnCard);
                yield return new WaitForSeconds(0.1f);
                cardsDrawn++;
            }
            else
            {
                Shuffle(numCards, cardsDrawn);
                break;
            }
        }
    }

    public void Shuffle(int numCards, int cardsDrawn)
    {
        for (int i = 0; i < battleSystem.discard.cards.Count; i++)
        {
            Card card = battleSystem.discard.cards[0];
            battleSystem.player.drawPile.Add(card);
            battleSystem.discard.cards.Remove(card);
        }
        int cardsLeft = numCards - cardsDrawn;
        print(cardsLeft + " cards left");
        StartCoroutine(DrawCard(cardsLeft));
    }
}
