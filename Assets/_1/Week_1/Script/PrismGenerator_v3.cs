using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class PrismGenerator_v3 : MonoBehaviour
{
    [SerializeField] private int polycount = 3;

    [SerializeField] private float hight = 0.5f;
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
        
        var vertices = new List<Vector3>(){ };

        ////////////////////////////
        // 頂点作成
        ////////////////////////////
        // 天井
        vertices.Add(new Vector3 (0f, hight, 0f));
        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                     Mathf.Cos(i * Mathf.PI * 2f / polycount),
                     hight,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount))
            );
        }
        
        // 柱
        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount),
                    hight,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount))
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount),
                    hight,
                    Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount))
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount),
                    -hight,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount))
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount),
                    -hight,
                    Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount))
            );
            
        }

        // 床
        vertices.Add(new Vector3 (0f, -hight, 0f));
        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount),
                    -hight,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount))
            );
        }

        
        ////////////////////////////
        // 三角形登録
        ////////////////////////////
        var triangles = new List<int>() { };
        
        // 天井
        for (int i = polycount; i > 0; i--)
        {
            triangles.Add(0);
            triangles.Add(i % polycount + 1);
            triangles.Add((i - 1) % polycount + 1);
        }

        // 柱
        for (int i = 0; i < polycount; i++)
        {
            var j = (polycount + 1) + 4 * i;

            triangles.Add(j);
            triangles.Add(j + 1);
            triangles.Add(j + 2);
            triangles.Add(j + 1);
            triangles.Add(j + 3);
            triangles.Add(j + 2);
        }
        
        // 床
        int t = polycount + polycount * 4 + 1;
        for (int i = 0; i < polycount; i++)
        {
            triangles.Add(t);
            triangles.Add(t + i % polycount + 1);
            triangles.Add(t + (i + 1) % polycount + 1);
        }
        
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.name = "PolygonalPrism";

        GetComponent<MeshFilter>().mesh = mesh;
        
        /*
        var filter = GetComponent<MeshFilter> ();
        // filter.mesh = mesh;
        filter.sharedMesh = mesh;  
        */
    }
 
}
