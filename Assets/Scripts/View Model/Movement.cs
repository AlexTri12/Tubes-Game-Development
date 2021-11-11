using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range
    {
        get { return stats[StatsTypes.MOV]; }
    }
    public int jumpHeight
    {
        get { return stats[StatsTypes.JMP]; }
    }
    protected Unit unit;
    protected Transform jumper;
    protected Stats stats;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
    }

    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
    }

    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count; ++i)
        {
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
        }
    }

    public abstract IEnumerator Traverse(Tile tile);

    protected virtual IEnumerator Turn(Directions dir)
     {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        // When rotating between North and Weat, we must make an exception so it look like the unit rotates the most efficient way
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
        {
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        }
        else if (Mathf.Approximately(t.startValue.y, 270f) && Mathf.Approximately(t.endValue.y, 0))
        {
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        }

        unit.dir = dir;

        while (t != null)
            yield return null;
    }
}
