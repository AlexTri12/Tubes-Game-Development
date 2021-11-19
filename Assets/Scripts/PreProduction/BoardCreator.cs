using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject tileSelectionPrefab;
    [SerializeField] int maxWidthBoard = 10;
    [SerializeField] int maxDepthBoard = 10;
    [SerializeField] int maxHeight = 10;
    [SerializeField] Point pos;
    [SerializeField] int widthRect;
    [SerializeField] int depthRect;
    [SerializeField] LevelData loadLevelData;
    [SerializeField] string saveName;

    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    public void GrowAreaRandom()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    public void ShrinkAreaRandom()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    public void GrowAreaAll()
    {
        Rect r = new Rect(0, 0, maxWidthBoard, maxDepthBoard);
        GrowRect(r);
    }

    public void ShrinkAreaAll()
    {
        Rect r = new Rect(0, 0, maxWidthBoard, maxDepthBoard);
        ShrinkRect(r);
    }

    public void GrowAreaRect()
    {
        Rect r = new Rect(pos.x, pos.y, widthRect, depthRect);
        GrowRect(r);
    }

    public void ShrinkAreaRect()
    {
        Rect r = new Rect(pos.x, pos.y, widthRect, depthRect);
        ShrinkRect(r);
    }

    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, maxWidthBoard);
        int y = UnityEngine.Random.Range(0, maxDepthBoard);
        int w = UnityEngine.Random.Range(1, maxWidthBoard - x + 1);
        int h = UnityEngine.Random.Range(1, maxDepthBoard - y + 1);
        return new Rect(x, y, w, h);
    }

    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    Tile Create()
    {
        GameObject instance = Instantiate(tilePrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < maxHeight)
            t.Grow();
    }

    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    public void Grow()
    {
        GrowSingle(pos);
    }

    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
            board.tiles.Add( new Vector3(t.pos.x, t.height, t.pos.y) );

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, saveName);
        AssetDatabase.CreateAsset(board, fileName);
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        Clear();
        if (loadLevelData == null)
            return;

        foreach (Vector3 v in loadLevelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
}
