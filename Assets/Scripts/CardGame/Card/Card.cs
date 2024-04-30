using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Attack,
    Block,
    Skill
}
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public int spiritCost;
    public int damage;
    public int block;
    //0 = starter, 1 = linker, 2 = ender
    public int comboPosition;
    public CardType cardType;
    public StatusEffectData statusEffect;
    public AnimationClip animation;
    public AudioClip SFXClip;
}
