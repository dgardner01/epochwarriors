using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitTooltip : TooltipTrigger
{
    public Fighter fighter;
    private void Update()
    {
        header = fighter.name;
        content = "HP: " + fighter.health + "/" + fighter.maxHealth;
    }
}
