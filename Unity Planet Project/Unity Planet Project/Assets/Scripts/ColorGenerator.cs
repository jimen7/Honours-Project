using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;  //Could create
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            //Uing first half of texture for ocean and second half for solid ground
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);

    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;   //Should go from 0 to 1, south pole being 0 and north ole being 1
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;  //the ,inus onward is so that we can comntrol noise strength and further settings
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;

        float blendRange = settings.biomeColorSettings.blendAmount / 2f + .001f;    //Adding so its not 0

        for (int i = 0; i < numBiomes; i++)
        {

            float distance = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        return biomeIndex / Mathf.Max(1, numBiomes - 1);    //Return biome index but squeeze it from 0 to 1
    }



    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height]; //Takes into account jo of texture
        int colorIndex = 0;
        foreach (var biome in settings.biomeColorSettings.biomes)       //loop througgh all the biomes
        {
            for (int i = 0; i < textureResolution * 2; i++)   //*2 because we added the ocxean texture in the first half */
            {
                Color gradientCol;
                if (i < textureResolution)  //Then sample from the ocean gradient
                {
                    gradientCol = settings.colorOfOcean.Evaluate(i / (textureResolution - 1f));  //Evaluate same way as single gradient
                }
                else    //Evaluate from biome Gradient
                {
                    gradientCol = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));    //Evaluate same way as single gradient, evaluate from biome. this should be between 0 and 1 which is whjy nwe are substarcting texture resolution from i
                } 
                Color tintCol = biome.tint;
                colors[colorIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
