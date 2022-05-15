using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class CubeGeneratorWithVertexColor : MonoBehaviour
{
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        var vertices = new List<Vector3>(){
            new Vector3 (-1f, 1f, -1f),  
            new Vector3 (1f, 1f, -1f),  
            new Vector3 (-1f, -1f, -1f),  
            new Vector3 (1f, -1f, -1f),

            new Vector3 (1f, 1f, -1f),  
            new Vector3 (1f, 1f, 1f),  
            new Vector3 (1f, -1f, -1f),  
            new Vector3 (1f, -1f, 1f),

            new Vector3 (1f, 1f, 1f),  
            new Vector3 (-1f, 1f, 1f),
            new Vector3 (1f, -1f, 1f),
            new Vector3 (-1f, -1f, 1f),

            new Vector3 (-1f, 1f, 1f),  
            new Vector3 (-1f, 1f, -1f),  
            new Vector3 (-1f, -1f, 1f),  
            new Vector3 (-1f, -1f, -1f),

            new Vector3 (-1f, 1f, -1f),  
            new Vector3 (-1f, 1f, 1f),  
            new Vector3 (1f, 1f, -1f),  
            new Vector3 (1f, 1f, 1f),

            new Vector3 (-1f, -1f, -1f),  
            new Vector3 (1f, -1f, -1f),  
            new Vector3 (-1f, -1f, 1f),  
            new Vector3 (1f, -1f, 1f),
        };
        
        var triangles = new List<int>()
        {
            0, 1, 2, 1, 3, 2,
            4, 5, 6, 5, 7, 6,
            8, 9, 10, 9, 11, 10,
            12, 13, 14, 13, 15, 14,
            
            16, 17, 18, 17, 19, 18,
            20, 21, 22, 21, 23, 22,
        };

        var colors = new List<Color>()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.cyan,

            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,

            Color.white,
            Color.yellow,
            Color.green,
            Color.blue,

            Color.red,
            Color.green,
            Color.blue,
            Color.cyan,

            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,

            Color.white,
            Color.yellow,
            Color.green,
            Color.blue,

        };
        
        var mesh = new Mesh ();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.SetColors(colors);
        mesh.RecalculateNormals();
        mesh.name = "Cube";

        GetComponent<MeshFilter>().mesh = mesh;
        
        /*
        var filter = GetComponent<MeshFilter> ();
        // filter.mesh = mesh;
        filter.sharedMesh = mesh;  
        */
    }
 
}
