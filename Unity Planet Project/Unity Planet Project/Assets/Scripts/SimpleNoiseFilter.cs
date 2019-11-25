//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and adapted to fit this projhect's criteria

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)    //Takes in a vector 3 for a poiinmt for ehich we're evaluating the noise
    {
        //float noiseValue = (noise.Evaluate(point*settings.roughness +settings.centre)+1)*.5f; //We are squishing this to be from 0 to 1 instead f -1 to 1. We are also multiplying / roughness because the further part on the points we are sampling the rougher the terrauinb will bve
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue += (v + 1) * .5f * amplitude;  //Noise value gets increased by each loayer, also depending on the amplituded of the layer
            frequency *= settings.roughness;    //when roughness is greater than one the frequency will increase per layer
            amplitude *= settings.persistence;  //when persistence is a value less than 1 value will decrease per layer
        }



        //noiseValue= Mathf.Max(0, noiseValue - settings.minValue); //Before adding ocean
        noiseValue = noiseValue - settings.minValue; //We no longer clamp the values from 0 above in order to know how far the ocean goes
        return noiseValue * settings.strength;
    }
}
