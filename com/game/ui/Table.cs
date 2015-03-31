﻿using UnityEngine;
using System;

namespace com.game.ui
{
    public class Table : UITable
    {

        static public int SortByName(Transform a, Transform b)
        {
            int x = Convert.ToInt32(a.name);
            int y = Convert.ToInt32(b.name);
            return Convert.ToInt32(x > y);
        }

    }

}