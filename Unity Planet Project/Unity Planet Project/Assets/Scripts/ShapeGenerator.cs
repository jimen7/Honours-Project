//This Script was developed from scratch using online sources (https://www.youtube.com/playlist?list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8) and adapted to fit this project's criteria

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i =0; i<noiseFilters.Length;i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }



    public float CalculateUncsaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0; //before layerrs it was noiseFilter.Evaluate(pointOnUnitSphere);

        if (noiseFilters.Length > 0)    //Store the firstLayerValue so it can be used as a refernece for where the noise map can pop out
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i=1; i<noiseFilters.Length;i++)    //Starts at 1 causer we've already evaluated for the first layer just above
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask)?firstLayerValue:1;       //If current loise layer is using first layer as a mask it's equal to firstlayerMask and it is used,. if not set it equal to 1 which basically means not usiong the mask
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
            
        }
         elevationMinMax.AddValue(elevation);   //his way we can keep track of the minimum andf maximum vertices of the planet
         return  elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = settings.planetRadius* (1+elevation);
        return elevation;
    }






    // Before altering mnethod to add ocean depth
    /* 
    public Vector3 CalclulatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0; //before layerrs it was noiseFilter.Evaluate(pointOnUnitSphere);

        if (noiseFilters.Length > 0)    //Store the firstLayerValue so it can be used as a refernece for where the noise map can pop out
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i=1; i<noiseFilters.Length;i++)    //Starts at 1 causer we've already evaluated for the first layer just above
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask)?firstLayerValue:1;       //If current loise layer is using first layer as a mask it's equal to firstlayerMask and it is used,. if not set it equal to 1 which basically means not usiong the mask
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
            
        }
        elevation = settings.planetRadius * (1+elevation);
        //return pointOnUnitSphere * settings.planetRadius * (1+elevation); //Alters the hsape of the planet, before we added elevation for minmax. Altered so that we can use elavationMinMax0
         elevationMinMax.AddValue(elevation);   //his way we can keep track of the minimum andf maximum vertices of the planet
         return pointOnUnitSphere * elevation;
    }


*/
    
}
