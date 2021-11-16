using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour
{
    [SerializeField] List<Position> positionList;
    Dictionary<string, Position> positionMap;
    LayoutAnchor anchor;

    public Position CurrentPosition
    {
        get;
        private set;
    }
    public Tweener Transition
    {
        get;
        private set;
    }
    public bool InTransition
    {
        get { return Transition != null; }
    }

    public Position this[string name]
    {
        get
        {
            if (positionMap.ContainsKey(name))
                return positionMap[name];
            return null;
        }
    }

    void Awake()
    {
        anchor = GetComponent<LayoutAnchor>();
        positionMap = new Dictionary<string, Position>(positionList.Count);
        for (int i = positionList.Count - 1; i >= 0; --i)
            AddPosition(positionList[i]);
    }

    public void AddPosition(Position p)
    {
        positionMap[p.name] = p;
    }

    public void RemovePosition(Position p)
    {
        if (positionMap.ContainsKey(p.name))
            positionMap.Remove(p.name);
    }

    public Tweener SetPosition(string positionName, bool animated)
    {
        return SetPosition(this[positionName], animated);
    }

    public Tweener SetPosition(Position p, bool animated)
    {
        CurrentPosition = p;
        if (CurrentPosition == null)
            return null;

        if (InTransition)
            Transition.Stop();

        if (animated)
        {
            Transition = anchor.MoveToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return Transition;
        }
        else
        {
            anchor.SnapToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return null;
        }
    }

    void Start()
    {
        if (CurrentPosition == null && positionList.Count > 0)
            SetPosition(positionList[0], false);
    }
}
