using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _horizontalInput = new Repeater("Horizontal");
    Repeater _verticalInput = new Repeater("Vertical");
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    void Update()
    {
        int x = _horizontalInput.Update();
        int y = _verticalInput.Update();

        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        for (int i = 0; i < _buttons.Length; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                {
                    fireEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }
    }

    class Repeater
    {
        // Amount of pause to wait between an initial press of the button and the point at which the input will begin repeating
        const float threshold = 0.5f; 
        // The speed that the input will repeat
        const float rate = 0.25f;
        // To mark a target point in time which must be passed before new events will be registered
        float _next;
        bool _hold;
        string _axis;

        public Repeater(string axisName)
        {
            _axis = axisName;
        }

        public int Update()
        {
            int retValue = 0;
            int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

            if (value != 0)
            {
                if (Time.time > _next)
                {
                    retValue = value;
                    _next = Time.time + (_hold ? rate : threshold);
                    _hold = true;
                }
            }
            else
            {
                _hold = false;
                _next = 0;
            }

            return retValue;
        }
    }
}
