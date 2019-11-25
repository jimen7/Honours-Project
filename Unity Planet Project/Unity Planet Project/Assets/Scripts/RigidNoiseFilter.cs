//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and adapted to fit this project's criteria

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    NoiseSettings.RigidNoiseSettings settings;
    Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)    //Takes in a vector 3 for a poiinmt for ehich we're evaluating the noise
    {
        //float noiseValue = (noise.Evaluate(point*settings.roughness +settings.centre)+1)*.5f; //We are squishing this to be from 0 to 1 instead f -1 to 1. We are also multiplying / roughness because the further part on the points we are sampling the rougher the terrauinb will bve
        float noiseValue = 0;
        float frequency = settings.baseRoughness;   
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point*frequency + settings.centre)) ;    //Taking absolute value
            v *= v;
            v*= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v*amplitude;  //Noise value gets increased by each loayer, also depending on the amplituded of the layer
            frequency *= settings.roughness;    //when roughness is greater than one the frequency will increase per layer
            amplitude *= settings.persistence;  //when persistence is a value less than 1 value will decrease per layer
        }

        //noiseValue= Mathf.Max(0, noiseValue - settings.minValue);   //Before adding ocean
        noiseValue=  noiseValue - settings.minValue;
        return noiseValue* settings.strength;
    }
}
