using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardTooltip : TooltipTrigger
{
    CardDisplay display => GetComponent<CardDisplay>();
    private void Update()
    {
        header = display.card.name;
        content = display.card.description;
    }
}
