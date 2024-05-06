using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public List<Card> cards;
    public int chain;
    public bool hasChained;
    private void Update()
    {
        CheckForChain();
        if (chain <= 0)
        {
            MusicManager.Instance.StopMusic("11");
            MusicManager.Instance.StopMusic("12");
            hasChained = false;
        }
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
                if (comboLinked || comboStarted && !comboEnded)
                {
                    comboEnded = true;
                    endIndex = i;
                }
            }
        }
        chain = endIndex-startIndex;
        if (chain > 0)
        {
            chain++;
            if (!hasChained)
            {
                if (chain > 2)
                {
                    MusicManager.Instance.PlayMusicOver("9", "12");
                }
                else
                {
                    MusicManager.Instance.PlayMusicOver("9", "11");
                }
                hasChained = true;
            }
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                GameObject cardObject = transform.GetChild(i).gameObject;
                CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
                cardDisplay.chained = true;
            }
        }
    }
}
