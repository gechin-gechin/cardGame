public enum CardKind
{
    FOLLOWER,
    SPELL,
    TRAP,
}

public enum CardCol
{
    NONE,
    WHITE,
    BLUE,
    BLACK,
    RED,
    GREEN
}

public enum AbilityTiming
{
    Common,
    TurnStart,
    CIP,
    Attack,
    Battle,
    PIG,
    TurnEnd,
    Always,
}

public enum AbilityTarget
{
    PLAYER,
    ENEMY,
    BOTH
}
