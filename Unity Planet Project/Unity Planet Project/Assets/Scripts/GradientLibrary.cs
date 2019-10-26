using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
[Serializable]
public class GradientLibrary : ScriptableObject //This is used as two instances which are accessed by evry planet. It's the pool of gradients for the terrain, amd the ocean
{
    [SerializeField]
    public Gradient[] list = new Gradient[4];
}
