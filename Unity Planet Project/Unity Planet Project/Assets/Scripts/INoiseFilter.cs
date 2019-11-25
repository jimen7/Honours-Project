//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and adapted to fit this project's criteria

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter 
{
    float Evaluate(Vector3 point);

}
