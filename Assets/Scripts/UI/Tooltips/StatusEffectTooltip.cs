using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class StatusEffectTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Fighter fighter;
    public List<GameObject> tooltips = new List<GameObject>();
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < tooltips.Count; i++)
        {
            int siblingIndex = transform.GetSiblingIndex();
            StatusEffect status = fighter.activeStatusEffects[siblingIndex];
            tooltips[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = status.id;
            string description = status.description;
            print(Mathf.Max(status.magnitude, status.duration));
            description = description.Replace('x', (char)(Mathf.Max(status.magnitude,status.duration) + '0'));
            tooltips[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
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
