using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Color selectedTileColor = new Color(0, 1, 1, 1);
    [SerializeField] Color defaultTileColor = new Color(1, 1, 1, 1);
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    public Point min
    {
        get { return _min; }
    }
    public Point max
    {
        get { return _max; }
    }
    Point _min;
    Point _max;

    private Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0),
    };

    public void Load(LevelData data)
    {
        _min = new Point(int.MaxValue, int.MaxValue);
        _max = new Point(int.MinValue, int.MinValue);

        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance;
            if (i < data.tilePrefabsName.Count && data.tilePrefabsName[i] != "")
                instance = Instantiate(Resources.Load("Tiles/" + data.tilePrefabsName[i])) as GameObject;
            else
                instance = Instantiate(tilePrefab) as GameObject;

            instance.transform.SetParent(transform);

            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);

            _min.x = Mathf.Min(_min.x, t.pos.x);
            _min.y = Mathf.Min(_min.x, t.pos.y);
            _max.x = Mathf.Max(_max.x, t.pos.x);
            _max.y = Mathf.Max(_max.x, t.pos.y);
        }
    }

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        // Starting point is where the character is standing
        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue(); 
            
            // Get the next tile to be checked (+/ up, down, left, right of this tile)
            for (int i = 0; i < dirs.Length; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }

            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public void SelectTile(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count; ++i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    public void DeSelectTile(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count; ++i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }
}
