using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseException
{
    public bool toogle
    {
        get;
        private set;
    }
    private bool defaultToogle;

    public BaseException(bool defaultToogle)
    {
        this.defaultToogle = defaultToogle;
        toogle = defaultToogle;
    }

    public void FlipToogle()
    {
        toogle = !defaultToogle;
    }
}
