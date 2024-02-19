using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public RectTransform backgroundRectTransform;
    public RectTransform canvasRectTransform;
    RectTransform rect => transform.GetComponent<RectTransform>();
    public TextMeshProUGUI text;
    private void Awake()
    {

    }
    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        //if (anchoredPosition.x - backgroundRectTransform.rect.width < 0)
        //{
        //    anchoredPosition.x = 0;
        //}
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        //if (anchoredPosition.y - backgroundRectTransform.rect.height < 0)
        //{
        //    anchoredPosition.y = 0;
        //}
        rect.anchoredPosition = anchoredPosition;
    }
    public void ShowTooltip(string tooltipText)
    {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }
    public void HideTooltip()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    void SetText(string toolTipText)
    {
        text.SetText(toolTipText);
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }
}
