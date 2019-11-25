//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and changed to match the way maximum and minumum vectors are evaluated

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax 
{
    public float Min {get; private set;}
    public float Max {get; private set;}

    public MinMax()
    {
        Min = float.MaxValue; 
        Max = float.MinValue; 
    }

    public void AddValue(float vector)
    {
        if (vector > Max)
        {
            Max = vector;
        }
        if (vector < Min)
        {
            Min = vector;
        }
    }
}
