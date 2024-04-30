using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> tooltips = new List<GameObject>();
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < tooltips.Count; i++)
        {
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
