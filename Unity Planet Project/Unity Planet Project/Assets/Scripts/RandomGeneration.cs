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
    public Vector2 simple_Strength = new Vector2(0.01f, 0.1f);
    public Vector2 simple_BaseRoughness = new Vector2(1f, 2f);
    public Vector2 simple_Roughness = new Vector2(2.2f, 3.2f);
    public Vector2 simple_Centre = new Vector2(0f, 20f);
    public Vector2 simple_MinimalValue = new Vector2(0.2f, 1f);


    [Header("Rigid Noise Attributes")]
    public Vector2 rigid_Strength = new Vector2(0.6f, 1f);
    public Vector2 rigid_BaseRoughness = new Vector2(1f, 4.5f);
    public Vector2 rigid_Roughness = new Vector2(1f, 2f);
    public Vector2 rigid_Persistence = new Vector2(.3f, .8f);
    public Vector2 rigid_Centre = new Vector2(0f, 20f);
    public Vector2 rigid_MinimalValue = new Vector2(0f, 1f);
    public Vector2 rigid_WeightMultiplier = new Vector2(0.3f, 2.5f);

    [Header("Biome Noise Attributes")]
    public Vector2 biome_BiomeNumber = new Vector2(1f, 4f);
    public Vector2 biome_BlendAmount = new Vector2(0.7f, 1.0f);
    public Vector2 biome_MinValue = new Vector2(0.7f,0.9f);
    public Vector2 biome_BiomeNoiseStrength = new Vector2(1.0f, 1.3f);   //MIMINIMU WAS 1 and i changed it
    public Vector2 biome_NoiseOffset_2Biomes = new Vector2(0.8f, 1.09f);
    public Vector2 biome_NoiseOffset_Biomes = new Vector2(.1f, .5f);

     public Vector2 biome_NoiseOffset_3Biomes = new Vector2(6.5f, 8.5f);

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

        //Simple noise randomised settings
        simpleNoiseSettings.simpleNoiseSettings.strength = Random.Range(simple_Strength.x, simple_Strength.y);
        simpleNoiseSettings.simpleNoiseSettings.baseRoughness = Random.Range(simple_BaseRoughness.x, simple_BaseRoughness.y);
        simpleNoiseSettings.simpleNoiseSettings.roughness = Random.Range(simple_Roughness.x, simple_Roughness.y);
        simpleNoiseSettings.simpleNoiseSettings.centre = new Vector3(Random.Range(simple_Centre.x, simple_Centre.y), Random.Range(simple_Centre.x, simple_Centre.y), Random.Range(simple_Centre.x, simple_Centre.y));


        //Ridgid noise randomised settings
        rigidNoiseSettings.rigidNoiseSettings.strength = Random.Range(rigid_Strength.x, rigid_Strength.y);
        rigidNoiseSettings.rigidNoiseSettings.baseRoughness = Random.Range(rigid_BaseRoughness.x, rigid_BaseRoughness.y);
        rigidNoiseSettings.rigidNoiseSettings.roughness = Random.Range(rigid_Roughness.x, rigid_Roughness.y);
        rigidNoiseSettings.rigidNoiseSettings.persistence = Random.Range(rigid_Persistence.x, rigid_Persistence.y);
        rigidNoiseSettings.rigidNoiseSettings.centre = new Vector3(Random.Range(rigid_Centre.x, rigid_Centre.y), Random.Range(rigid_Centre.x, rigid_Centre.y), Random.Range(rigid_Centre.x, rigid_Centre.y));
        rigidNoiseSettings.rigidNoiseSettings.weightMultiplier = Random.Range(rigid_WeightMultiplier.x, rigid_WeightMultiplier.y);
        rigidNoiseSettings.rigidNoiseSettings.minValue = Random.Range(rigid_MinimalValue.x, rigid_MinimalValue.y);

        //Biome Settings(Noise and Biome Numbers)
        newBiomeList = new ColorSettings.BiomeColorSettings.Biome[Mathf.FloorToInt(Random.Range(biome_BiomeNumber.x, biome_BiomeNumber.y))];

        if (newBiomeList.Length == colorSettings.biomeColorSettings.biomes.Length)
        {
           //DO nothing as the biomes will be the dame length and randomized later on
        }
        else if (newBiomeList.Length > colorSettings.biomeColorSettings.biomes.Length)
        {
            previousArraySize = colorSettings.biomeColorSettings.biomes.Length;
            //System.Array.Resize(ref colorSettings.biomeColorSettings.biomes, newBiomeList.Length);
            
            int j = 0;
            for (int i = 0; i < previousArraySize; i++)
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

            colorSettings.biomeColorSettings.biomes = newBiomeList;
        }
        else
        {
            for (int i = 0; i < newBiomeList.Length; i++)
            {
                newBiomeList[i] = colorSettings.biomeColorSettings.biomes[i];
            }
            colorSettings.biomeColorSettings.biomes  = newBiomeList;
        }

        colorSettings.biomeColorSettings.noise.simpleNoiseSettings.minValue = Random.Range(biome_MinValue.x, biome_MinValue.y);
        colorSettings.biomeColorSettings.noise.rigidNoiseSettings.minValue = Random.Range(biome_MinValue.x, biome_MinValue.y);
        colorSettings.biomeColorSettings.blendAmount = Random.Range(biome_BlendAmount.x, biome_BlendAmount.y);
        colorSettings.biomeColorSettings.noiseStrength = Random.Range(biome_BiomeNoiseStrength.x, biome_BiomeNoiseStrength.y);


        if (colorSettings.biomeColorSettings.biomes.Length == 2)
        {
            colorSettings.biomeColorSettings.noiseOffset = Random.Range(biome_NoiseOffset_2Biomes.x, biome_NoiseOffset_2Biomes.y);
        }
        else if (colorSettings.biomeColorSettings.biomes.Length == 1) //If it's 1 noise is not applied in the texture/Colors so we won't do any unessesary calculations
        {
            colorSettings.biomeColorSettings.noiseOffset = Random.Range(biome_NoiseOffset_Biomes.x, biome_NoiseOffset_Biomes.y);
        }
        else if (colorSettings.biomeColorSettings.biomes.Length == 3)
        {
            colorSettings.biomeColorSettings.noiseOffset = Random.Range(biome_NoiseOffset_3Biomes.x, biome_NoiseOffset_3Biomes.y);
            colorSettings.biomeColorSettings.noise.simpleNoiseSettings.minValue = 0f;
            colorSettings.biomeColorSettings.noise.simpleNoiseSettings.strength = Random.Range(0.1f,0.2f);
        }

        planet.resolution = 256;
        RandomiseColors();
        planet.StartCoroutine("MeshCoper"); //Generates the planets after 2 seconds so that the camera is already in front of the hyperdrive and particles have been initiated
        checkIfPlanetsAreDone = true;
        }

    }

    public void RandomiseColors()
    {
        currentOcean = ocean.list[Random.Range(0, ocean.list.Length)];
        colorSettings.colorOfOcean = currentOcean;
        for (int i = 0; i < colorSettings.biomeColorSettings.biomes.Length; i++)    //CHoose terrain colors for each biome of the planet randomly from the terrain pool
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
}
