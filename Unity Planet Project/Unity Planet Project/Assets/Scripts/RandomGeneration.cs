using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{

    [HideInInspector()]
    public Planet planet;

    ShapeSettings shapeSettings;
    ColorSettings colorSettings;


    bool checkIfPlanetsAreDone = false;


    public GradientLibrary terrain;
    public GradientLibrary ocean;


    Gradient currentTerrain;
    Gradient currentOcean;



    NoiseSettings simpleNoiseSettings;
    NoiseSettings rigidNoiseSettings;



    [Header("Global Graphics Settings")]
    public bool maxResolution;
    public bool LOD = false;


    [Range(1, 8)]
    public int LODValue = 1;

    [Header("X is minimum, Y is max")]
    Vector2 planetRadius = new Vector2(1.0f, 4.0f);

    [Header("Simple Noise Attributes")]
    public Vector2 s_Strength = new Vector2(0.01f, 0.1f);
    public Vector2 s_BaseRoughness = new Vector2(1f, 2f);
    public Vector2 s_Roughness = new Vector2(2.2f, 3.2f);
    public Vector2 s_Centre = new Vector2(0f, 20f);
    public Vector2 s_MinimalValue = new Vector2(0.2f, 1f);


    [Header("Ridgid Noise Attributes")]
    public Vector2 r_Strength = new Vector2(0.6f, 1f);
    public Vector2 r_BaseRoughness = new Vector2(1f, 4.5f);
    public Vector2 r_Roughness = new Vector2(1f, 2f);
    public Vector2 r_Persistence = new Vector2(.3f, .8f);
    public Vector2 r_Centre = new Vector2(0f, 20f);
    public Vector2 r_MinimalValue = new Vector2(0f, 1f);
    public Vector2 r_WeightMultiplier = new Vector2(0.3f, 2.5f);

    [Header("Biome Noise Attributes")]
    public Vector2 b_BiomeNumber = new Vector2(1f, 4f);
    public Vector2 b_BlendAmount = new Vector2(0.5f, 1.0f);
    public Vector2 b_MinValue = new Vector2(0.7f,0.9f);
    public Vector2 b_BiomeNoiseStrength = new Vector2(1.0f, 1.3f);
    public Vector2 b_NoiseOffset_2Biomes = new Vector2(0.8f, 1.09f);
    public Vector2 b_NoiseOffset_Biomes = new Vector2(.1f, .5f);

    ColorSettings.BiomeColorSettings.Biome[] newBiomeList;


    int previousArraySize=0;





    public
    // Start is called before the first frame update
    void Start()
    {
        planet = GetComponent<Planet>();
        shapeSettings = planet.shapeSettings;
        colorSettings = planet.colorSettings;
        simpleNoiseSettings = shapeSettings.noiseLayers[0].noiseSettings;
        rigidNoiseSettings = shapeSettings.noiseLayers[1].noiseSettings;

    }

    public void RandomisePlanet()
    {

        if (Camera.main.GetComponent<FlyingCam>().GetHyperdriveStatus()==true)
        {
            planet.resolution = 10; //Set planet resolution really low so that app doesn;'t lag when making changes, will be set back to max at the end of the method

        shapeSettings.planetRadius = Random.Range(planetRadius.x, planetRadius.y);

        //Simple noise RNG (highly unoptimised)
        simpleNoiseSettings.simpleNoiseSettings.strength = Random.Range(s_Strength.x, s_Strength.y);
        simpleNoiseSettings.simpleNoiseSettings.baseRoughness = Random.Range(s_BaseRoughness.x, s_BaseRoughness.y);
        simpleNoiseSettings.simpleNoiseSettings.roughness = Random.Range(s_Roughness.x, s_Roughness.y);
        simpleNoiseSettings.simpleNoiseSettings.centre = new Vector3(Random.Range(s_Centre.x, s_Centre.y), Random.Range(s_Centre.x, s_Centre.y), Random.Range(s_Centre.x, s_Centre.y));


        //Ridgid noise RNG (highly unoptimised)
        rigidNoiseSettings.rigidNoiseSettings.strength = Random.Range(r_Strength.x, r_Strength.y);
        rigidNoiseSettings.rigidNoiseSettings.baseRoughness = Random.Range(r_BaseRoughness.x, r_BaseRoughness.y);
        rigidNoiseSettings.rigidNoiseSettings.roughness = Random.Range(r_Roughness.x, r_Roughness.y);
        rigidNoiseSettings.rigidNoiseSettings.persistence = Random.Range(r_Persistence.x, r_Persistence.y);
        rigidNoiseSettings.rigidNoiseSettings.centre = new Vector3(Random.Range(r_Centre.x, r_Centre.y), Random.Range(r_Centre.x, r_Centre.y), Random.Range(r_Centre.x, r_Centre.y));
        rigidNoiseSettings.rigidNoiseSettings.weightMultiplier = Random.Range(r_WeightMultiplier.x, r_WeightMultiplier.y);
        rigidNoiseSettings.rigidNoiseSettings.minValue = Random.Range(r_MinimalValue.x, r_MinimalValue.y);

        //Biome Settings(Noise and Biome Numbers)
        newBiomeList = new ColorSettings.BiomeColorSettings.Biome[Mathf.FloorToInt(Random.Range(b_BiomeNumber.x, b_BiomeNumber.y))];

        if (newBiomeList.Length == colorSettings.biomeColorSettings.biomes.Length)
        {
           // newBiomeList.CopyTo(colorSettings.biomeColorSettings.biomes,0);

            //colorSettings.biomeColorSettings.biomes = newBiomeList;
        }
        else if (newBiomeList.Length > colorSettings.biomeColorSettings.biomes.Length)
        {
            previousArraySize = colorSettings.biomeColorSettings.biomes.Length;
            System.Array.Resize(ref colorSettings.biomeColorSettings.biomes, newBiomeList.Length);
            
            int j = 0;
            for (int i = 0; i < previousArraySize; i++,j++)
            {
                newBiomeList[i] = colorSettings.biomeColorSettings.biomes[i];
                j++;
            }
            for (int i = j; i < newBiomeList.Length;i++)
            {
                newBiomeList[i] = new ColorSettings.BiomeColorSettings.Biome();
                newBiomeList[i].tint = Color.red;
                newBiomeList[i].tintPercent = 0.0f;
                newBiomeList[i].startHeight = 0.0f;
            }

            newBiomeList = colorSettings.biomeColorSettings.biomes;
           // colorSettings.biomeColorSettings.biomes  = newBiomeList;
           //colorSettings.biomeColorSettings.biomes.CopyTo(newBiomeList ,0);
        }
        else
        {
            for (int i = 0; i < newBiomeList.Length; i++)
            {
                newBiomeList[i] = colorSettings.biomeColorSettings.biomes[i];
            }
            //colorSettings.biomeColorSettings.biomes  = newBiomeList;
            newBiomeList.CopyTo(colorSettings.biomeColorSettings.biomes,0);
        }

        colorSettings.biomeColorSettings.noise.simpleNoiseSettings.minValue = Random.Range(b_MinValue.x, b_MinValue.y);
        colorSettings.biomeColorSettings.noise.rigidNoiseSettings.minValue = Random.Range(b_MinValue.x, b_MinValue.y);
        colorSettings.biomeColorSettings.blendAmount = Random.Range(b_BlendAmount.x, b_BlendAmount.y);
        colorSettings.biomeColorSettings.noiseStrength = Random.Range(b_BiomeNoiseStrength.x, b_BiomeNoiseStrength.y);


        if (colorSettings.biomeColorSettings.biomes.Length == 2)
        {
            colorSettings.biomeColorSettings.noiseOffset = Random.Range(b_NoiseOffset_2Biomes.x, b_NoiseOffset_2Biomes.y);
        }
        else if (colorSettings.biomeColorSettings.biomes.Length != 1) //If it's 1 noise is not applied in the texture/Colors so we won't do any unessesary calculations
        {
            colorSettings.biomeColorSettings.noiseOffset = Random.Range(b_NoiseOffset_Biomes.x, b_NoiseOffset_Biomes.y);
        }



        planet.resolution = 256;
        RandomiseColors();
        planet.StartCoroutine("MeshCoper");
        // planet.MeshCoper();
        //planet.GeneratePlanet();

        checkIfPlanetsAreDone = true;
        }

    }

    public void RandomiseColors()
    {
        currentOcean = ocean.list[Random.Range(0, ocean.list.Length)];
        colorSettings.colorOfOcean = currentOcean;


        for (int i = 0; i < colorSettings.biomeColorSettings.biomes.Length; i++)
        {
            currentTerrain = terrain.list[Random.Range(0, terrain.list.Length)];
            colorSettings.biomeColorSettings.biomes[i].gradient = currentTerrain;

        }


    }

    public IEnumerator RandomiseInBackground()
    {
        RandomisePlanet();

        yield return new WaitForSeconds(5f);



    }

    // Update is called once per frame
    void Update()
    {


    }
}
