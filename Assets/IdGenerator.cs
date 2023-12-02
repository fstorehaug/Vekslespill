using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDgenerator
{
    private static int HEAD = 0;
    private int _id;

    public int Id => _id;
    public static int GetID()
    {
        return ++HEAD;
    }

    public IDgenerator()
    {
        _id = GetID();
    }

}