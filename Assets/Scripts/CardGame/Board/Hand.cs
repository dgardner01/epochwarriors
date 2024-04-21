using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public BattleSystem battleSystem;
    public BattleUI ui;
    public Player player => battleSystem.player;
    public List<Card> cards;

    public IEnumerator DrawCard(int numCards)
    {
        int numLeft = numCards;
        for (int i = 0; i < numCards; i++)
        {
            if (player.drawPile.Count > 0)
            {
                SFXManager.Instance.PlaySound("cardPickup");
                int rand = Random.Range(0, player.drawPile.Count);
                Card drawnCard = player.drawPile[rand];
                cards.Add(drawnCard);
                GameObject nextCard = ui.drawPile.transform.GetChild(0).gameObject;
                nextCard.GetComponent<CardDisplay>().card = drawnCard;
                ui.ReparentCard(ui.drawPile.transform.GetChild(0).gameObject, transform);
                player.drawPile.Remove(drawnCard);
                yield return new WaitForSeconds(0.1f);
                numLeft--;
            }
            else
            {
                StartCoroutine(Shuffle(numLeft));
                break;
            }
        }
    }

    public IEnumerator Shuffle(int numLeft)
    {
        SFXManager.Instance.PlaySound("cardShuffle");
        for (int i = 0; i < battleSystem.discard.cards.Count; i++)
        {
            Card card = battleSystem.discard.cards[0];
            battleSystem.player.drawPile.Add(card);
            ui.ReparentCard(ui.discard.transform.GetChild(0).gameObject, ui.drawPile.transform);
            battleSystem.discard.cards.Remove(card);
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(DrawCard(numLeft));
    }
}
