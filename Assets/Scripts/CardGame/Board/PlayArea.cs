using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public List<Card> cards;
    public bool chain;
    private void Update()
    {
        CheckForChain();
    }
    void CheckForChain()
    {
        bool comboStarted = false;
        int startIndex = 0;
        bool comboLinked = false;
        bool comboEnded = false;
        int endIndex = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cardObject = transform.GetChild(i).gameObject;
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            cardDisplay.chained = false;
            Card card = cardDisplay.card;
            if (card.comboPosition == 0)
            {
                //starter card
                comboStarted = true;
                startIndex = i;
            }
            if (card.comboPosition == 1)
            {
                //linker cardd
                if (comboStarted)
                {
                    comboLinked = true;
                }
            }
            if (card.comboPosition == 2)
            {
                //ender card
                if (comboLinked || comboStarted)
                {
                    comboEnded = true;
                    endIndex = i;
                }
            }
        }
        chain = comboEnded;
        if (chain)
        {
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                GameObject cardObject = transform.GetChild(i).gameObject;
                CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
                cardDisplay.chained = true;
            }
        }
    }
}
