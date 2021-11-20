using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuEntry : MonoBehaviour
{
    [SerializeField] Image bullet;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite disabledSprite;
    [SerializeField] Text labelName;
    [SerializeField] Text labelMP;
    Outline outlineName;
    Outline outlineMP;

    public string Title
    {
        get { return labelName.text; }
        set { labelName.text = value; }
    }
    public string ManaCost
    {
        get { return labelMP.text; }
        set { labelMP.text = value; }
    }

    States State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;
            state = value;

            if (IsLocked)
            {
                bullet.sprite = disabledSprite;
                labelName.color = Color.gray;
                labelMP.color = Color.gray;
                outlineName.effectColor = new Color32(20, 36, 44, 255);
                outlineMP.effectColor = new Color32(20, 36, 44, 255);
            }
            else if (IsSelected)
            {
                bullet.sprite = selectedSprite;
                labelName.color = new Color32(249, 210, 118, 255);
                labelMP.color = new Color32(249, 210, 118, 255);
                outlineName.effectColor = new Color32(255, 160, 72, 255);
                outlineMP.effectColor = new Color32(255, 160, 72, 255);
            }
            else
            {
                bullet.sprite = normalSprite;
                labelMP.color = Color.white;
                labelName.color = Color.white;
                outlineName.effectColor = new Color32(8, 0, 255, 128);
                outlineMP.effectColor = new Color32(8, 0, 255, 128);
            }
        }
    }
    States state;

    void Awake()
    {
        outlineName = labelName.GetComponent<Outline>();
        outlineMP = labelMP.GetComponent<Outline>();
    }

    public bool IsLocked
    {
        get { return (State & States.Locked) != States.None; }
        set
        {
            if (value)
                State |= States.Locked;
            else
                State &= ~States.Locked;
        }
    }

    public bool IsSelected
    {
        get { return (State & States.Selected) != States.None; }
        set
        {
            if (value)
                State |= States.Selected;
            else
                State &= ~States.Selected;
        }
    }

    public void Reset()
    {
        State = States.None;
    }
}
