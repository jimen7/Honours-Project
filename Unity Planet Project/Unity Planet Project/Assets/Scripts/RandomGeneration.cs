using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{

    [HideInInspector()]
    public Planet planet;

    ShapeSettings shapeSettings;
    ColorSettings colorSettings;

    [HideInInspector]
    Preset currentPreset;        
    





    NoiseSettings simpleNoiseSettings;
    NoiseSettings rigidNoiseSettings;


    [Header("Global Graphics Settings")]
    public bool maxResolution;
    public bool LOD = false;


    [Range(1, 8)]
    public int LODValue = 1;

    Material currentMaterial;   //Need to get the current material to apply it to the 

    [Header("X is minimum, Y is max")]
    Vector2 planetRadius = new Vector2(1.0f, 4.0f);

    [Header("Simple Noise Attributes")]
    public Vector2 s_Strength = new Vector2(0.01f, 0.1f);
    public Vector2 s_BaseRoughness = new Vector2(0.7f, 2f);
    public Vector2 s_Roughness = new Vector2(2.2f, 3.2f);
    public Vector2 s_Centre = new Vector2(0f, 20f);
    public Vector2 s_MinimalValue = new Vector2(0.2f, 1f);


    [Header("Ridgid Noise Attributes")]
    public Vector2 r_Strength = new Vector2(0.6f, 1f);
    public Vector2 r_BaseRoughness = new Vector2(0.1f, 4.5f);
    public Vector2 r_Roughness = new Vector2(0.1f, 2f);
    public Vector2 r_Persistence = new Vector2(.3f, .8f);
    public Vector2 r_Centre = new Vector2(0f, 20f);
    public Vector2 r_MinimalValue = new Vector2(0f, 1f);
    public Vector2 r_WeightMultiplier = new Vector2(0.3f, 2.5f);


    public
    // Start is called before the first frame update
    void Start()
    {
        planet = GetComponent<Planet>();
        shapeSettings = planet.shapeSettings;
        colorSettings = planet.colorSettings;
        currentMaterial = planet.colorSettings.planetMaterial;
        simpleNoiseSettings = shapeSettings.noiseLayers[0].noiseSettings;
        rigidNoiseSettings = shapeSettings.noiseLayers[1].noiseSettings;

    }

    public void RandomisePlanet()
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

        

        planet.resolution = 256;
        RandomiseColors();
        planet.colorSettings.planetMaterial = currentMaterial;
        planet.GeneratePlanet();



    }

    [SerializeField]
    Preset[] colorPresets;

    public void RandomiseColors()
    {
        currentPreset = colorPresets[Random.Range(0,colorPresets.Length)];

        currentPreset.ApplyTo(colorSettings);


        // if (currentPreset!=colorSettings)
        // {
        //    currentPreset.ApplyTo(colorSettings);
        // }
        // else
        // {
        //     RandomiseColors();
        // }
    }

    // Update is called once per frame
    void Update()
    {


    }
}
