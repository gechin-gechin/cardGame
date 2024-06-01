using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CreateEntity", menuName ="Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public new string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public ABILITY ability;
    public CIP cip;
    public PIG pig;
    public SP_ABILITY spAbility;
    public SPELL spell;
}

public enum ABILITY
{
    NONE,
    INIT_ATTACKABLE,
    SHIELD,
}
public enum CIP
{
    NONE,
    ALL_DELETE,
}
public enum PIG
{
    NONE,
    DRAW1,
}

public enum SP_ABILITY
{
    NONE,
}


public enum SPELL
{
    NONE,
    DAMAGE_ENEMY_CARD,
    DAMAGE_ENEMY_CARDS,
    DAMAGE_ENEMY_HERO,
    HEAL_FRIEND_CARD,
    HEAL_FRIEND_CARDS,
    HEAL_FRIEND_HERO,
    DRAW_CARD,
    DRAW_2CARD,
    DESTROY_ENEMY_CARD,
}
