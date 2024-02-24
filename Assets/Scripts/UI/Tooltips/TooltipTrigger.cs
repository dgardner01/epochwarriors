using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [Multiline()]
    public string content;
    public TooltipUI tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip("<b>"+header+"</b>\n"+content);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
