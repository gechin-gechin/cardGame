using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CreateEntity", menuName ="Create CardEntity")]

public class CardEntity : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private int hp;
    [SerializeField] private int at;
    [SerializeField] private int cost;
    [SerializeField] private Sprite icon;
    [SerializeField] private ABILITY ability;
    [SerializeField] private CIP cip;
    [SerializeField] private PIG pig;
    [SerializeField] private SP_ABILITY spAbility;
    [SerializeField] private SPELL spell;

    public string Name { get => name;private set => name = value; }
    public int HP { get => hp; private set => hp = value; }
    public int At { get => at; private set => at = value; }
    public int Cost { get => cost; private set => cost = value; }
    public Sprite Icon { get => icon; private set => icon = value; }
    public ABILITY Ability { get => ability; private set => ability = value; }
    public CIP Cip { get => cip; private set => cip = value; }
    public PIG Pig { get => pig; private set => pig = value; }
    public SP_ABILITY SpAbility { get => spAbility; private set => spAbility = value; }
    public SPELL Spell { get => spell; private set => spell = value; }
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
