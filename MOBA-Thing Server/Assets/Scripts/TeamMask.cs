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

    public TeamMask(bool _allowBlue, bool _allowNeutral, bool _allowRed)
    {
        mask = 0b_000;

        if (_allowBlue)
            mask += blue;
        if (_allowNeutral)
            mask += neutral;
        if (_allowRed)
            mask += red;

        Debug.Log($"CurrentMask: {Convert.ToString(mask, 2)}");
    }
}
