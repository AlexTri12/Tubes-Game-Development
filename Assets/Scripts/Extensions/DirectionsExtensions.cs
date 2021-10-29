using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    public static Directions GetDirections(this Tile t1, Tile t2)
    {
        if (t1.pos.y < t2.pos.y)
        {
            return Directions.North;
        }
        else if (t1.pos.x < t2.pos.x)
        {
            return Directions.East;
        }
        else if (t1.pos.y > t2.pos.y)
        {
            return Directions.South;
        }
        else
        {
            return Directions.West;
        }
    }

    public static Vector3 ToEuler(this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}