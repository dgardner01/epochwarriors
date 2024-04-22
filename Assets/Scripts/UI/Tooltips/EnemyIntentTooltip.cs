using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class EnemyIntentTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Enemy enemy;
    public List<GameObject> tooltips = new List<GameObject>();
    string BruttiaIntent()
    {
        string intentDescription = "This turn, Bruttia will ";
        for (int i = 0; i < enemy.currentTurn.Count; i++)
        {
            switch (enemy.currentTurn[i].cardType)
            {
                case CardType.Attack:
                    intentDescription += "attack";
                    break;
                case CardType.Block:
                    intentDescription += "block";
                    break;
                case CardType.Skill:
                    intentDescription += "grow stronger";
                    break;
            }
            if (i-1 > -1 && enemy.currentTurn[i].cardType == enemy.currentTurn[i-1].cardType)
            {
                intentDescription += " again";
            }
            if (i < enemy.currentTurn.Count-1)
            {
                intentDescription += ", then ";
            }
            else
            {
                intentDescription += ".";
            }
        }
        return intentDescription;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < tooltips.Count; i++)
        {
            tooltips[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bruttia's Turn";
            tooltips[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = BruttiaIntent();
            tooltips[i].SetActive(true);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < tooltips.Count; i++)
        {
            tooltips[i].SetActive(false);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < tooltips.Count; i++)
        {
            tooltips[i].SetActive(false);
        }
    }
}
