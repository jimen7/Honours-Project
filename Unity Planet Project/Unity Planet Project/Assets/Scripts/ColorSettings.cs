//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and adapted to fit this project's criteria

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    //public Color planetColour;  //Before altering shader

    //public Gradient gradient; //Used before Gradient was added to the Biome class
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient colorOfOcean;

    [System.Serializable]
    public class BiomeColorSettings
    {
        
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0,1)]
        public float blendAmount;



        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0, 1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
