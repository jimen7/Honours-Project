using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
[Serializable]
public class GradientLibrary : ScriptableObject
{
    [SerializeField]
    public Gradient[] list = new Gradient[4];
}
