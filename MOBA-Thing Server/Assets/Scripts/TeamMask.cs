using System;
using System.Collections.Generic;
using UnityEngine;

public struct TeamMask
{
    private byte mask;

    private static byte blue = 0b_100;
    private static byte neutral = 0b_010;
    private static byte red = 0b_001;

    public byte GetByte()
    {
        return mask;
    }
    public KeyValuePair<Team_Type, bool>[] Get()
    {
        return new KeyValuePair<Team_Type, bool>[3]
        {
            new KeyValuePair<Team_Type, bool>(Team_Type.Blue, (mask & (1 << 0)) != 0),
            new KeyValuePair<Team_Type, bool>(Team_Type.Neutral, (mask & (1 << 1)) != 0),
            new KeyValuePair<Team_Type, bool>(Team_Type.Red, (mask & (1 << 2)) != 0),
        };
    }

    public bool Allows(Team_Type _team)
    {
        return (mask & (1 << 0)) != 0 || (mask & (1 << 1)) != 0 || (mask & (1 << 2)) != 0;
    }
    public TeamMask Flip()
    {
        byte maskXOR = 0b_111;
        byte newMask = (byte)(maskXOR ^ mask);
        mask = newMask;
        return this;
    }

    public static TeamMask MaskToIgnoreAllies(Team_Type _allyTeam)
    {
        byte mask = 0b_000;

        if (!(_allyTeam == Team_Type.Blue))
            mask += blue;
        if (!(_allyTeam == Team_Type.Neutral))
            mask += neutral;
        if (!(_allyTeam == Team_Type.Red))
            mask += red;
        return new TeamMask((mask & (1 << 0)) != 0, (mask & (1 << 1)) != 0, (mask & (1 << 2)) != 0);
    }
    public static TeamMask MaskToAlliesOnly(Team_Type _allyTeam)
    {
        byte mask = 0b_000;

        if (_allyTeam == Team_Type.Blue)
            mask += blue;
        if (_allyTeam == Team_Type.Neutral)
            mask += neutral;
        if (_allyTeam == Team_Type.Red)
            mask += red;
        return new TeamMask((mask & (1 << 0)) != 0, (mask & (1 << 1)) != 0, (mask & (1 << 2)) != 0);
    }
    public static TeamMask MaskToHitAll()
    {
        return new TeamMask(true, true, true);
    }

    public TeamMask(bool _allowBlue, bool _allowNeutral, bool _allowRed)
    {
        mask = 0b_000;

        if (_allowBlue)
            mask += blue;
        if (_allowNeutral)
            mask += neutral;
        if (_allowRed)
            mask += red;
    }

    public override string ToString()
    {
        return $"{(mask & (1 << 0)) != 0}, {(mask & (1 << 1)) != 0}, {(mask & (1 << 2)) != 0}";
    }
}
