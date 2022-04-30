using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class PrismGenerator_v2 : MonoBehaviour
{
    [SerializeField] private int polycount = 3; 
    
    void Start()
    {
        Setup();
    }

    void OnValidate()
    {
        Setup();
    }

    void Setup()
    {
        polycount = Mathf.Max(3, polycount);
        
        var vertices = new List<Vector3>(){
            new Vector3 (0f, 0f, 0f),
        };

        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                     Mathf.Cos(i * Mathf.PI * 2f / polycount),
                    0f,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount))
                );
        }
        
        var triangles = new List<int>()
        {
            // 0, 2, 1 
        };

        for (int i = polycount; i > 0; i--)
        {
            triangles.Add(0);
            triangles.Add(i % polycount + 1);
            triangles.Add((i - 1) % polycount + 1);
        }
        
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.name = "Prism";

        GetComponent<MeshFilter>().mesh = mesh;
        
        /*
        var filter = GetComponent<MeshFilter> ();
        // filter.mesh = mesh;
        filter.sharedMesh = mesh;  
        */
    }
 
}
