using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;

    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;


    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;   //Using this booleand in planet editor so the inspectir for shape nand color settings can be folded
    [HideInInspector]
    public bool colorSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    SphereSide[] sphereSides;

    // Start is called before the first frame update, added to fix black planet issue, because the texture that we created while in edit mode is lost
    void Start()
    {
        GeneratePlanet();
    }

    void Initialize()    
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        sphereSides = new SphereSide[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                //meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));    //Before using l8ghteweight RP material
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                //meshFilters[i].sharedMesh = new Mesh();
                meshFilters[i].mesh = new Mesh();   
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial; //Making sure planet material gets assigned to then planet mesh  

            // ORIGINAL terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            sphereSides[i] = new SphereSide(shapeGenerator, meshFilters[i].mesh, resolution, directions[i]);  //Editor throws a suggestion fix because we are instantiating the mesh but we want to instantiate it
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i; //So we can render one face at a time
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnColorSettingsUpdated()    //If only shape settings are changed use this method to update
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void OnShapeSettingsUpdated() //If only shape settings are changed use this method to update
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                sphereSides[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    public void GenerateColors()
    {
        colorGenerator.UpdateColors();

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                sphereSides[i].UpdateUVs(colorGenerator);
            }
        }
    }

    public IEnumerator MeshCoper()
    {
        yield return new WaitForSeconds(3f);
        GeneratePlanet();
        
    }

}