using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public int energyCost;
    public int damage;
    public int block;
    public StatusEffectData friendlyStatusEffect;
    public StatusEffectData targetingStatusEffect;
}
