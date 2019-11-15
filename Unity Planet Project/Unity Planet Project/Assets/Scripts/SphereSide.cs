using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSide
{

    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA; //Axis between normal and ground axis
    Vector3 axisB; //Axis perpendicular to normal along the ground

    public SphereSide(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution]; //Number of vertices along a single edge of a face(resolution squared)
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0; //Keeps a track of whicch point of the square is where
        //we want to make sure that uvs are thye right size in case resolution is changed, aso we add an ifd statement
        Vector2[] uv = (mesh.uv.Length == vertices.Length)? mesh.uv : new Vector2[vertices.Length]; //This is added so when we rebuild the mesh we don't want to lose the uv data.

        //This method uses the same technique we used in Physics to crerate the Physics Flag
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //vertices[i] = pointOnUnitCube;    //Displays a cube instead of a sphere
                //vertices[i] = pointOnUnitSphere;  //Just to infllate cube to Shhere
                //vertices[i] = shapeGenerator.CalclulatePointOnPlanet(pointOnUnitSphere);  //Before altering CalculatePointOnPlanet to add ocean depth
                float unscaledElevation = shapeGenerator.CalculateUncsaledElevation(pointOnUnitSphere); 
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;    //X axis is used for biomes 

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        //If we update the mesh with a lower resolution version we will be referencing indeices that don't ewxist, so we clear out the mesh and reassign it.
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals= vertices;     //Fixes the seams temporarily instead of recalculating the nmormals. also saves computiung power
        //mesh.RecalculateNormals();  //works but seams are visible
        if (mesh.uv.Length == uv.Length )
        {
            mesh.uv = uv;
        }
        
    }



    //The reasion this is not being put in the for loop on all the vertices is that if we did, every time the color was changed mesh would need to be rebuild which would take more time
    public void UpdateUVs(ColorGenerator colorGenerator)    //For each vertex in the planet we want to calculate in which biome it's in using BiomePercentFromPoint method from ColorGenerator class
    {
       // Vector2[] uv = new Vector2[resolution * resolution];    //number of vertices

        Vector2[] uv = mesh.uv; //Making sure it's not being overwritten, set them to be the existing UVs.

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere); //making sure x of uvs is used for biomes to not overwritethe y axis which is used for the ocean
            }
        }
        mesh.uv = uv;
    }
}