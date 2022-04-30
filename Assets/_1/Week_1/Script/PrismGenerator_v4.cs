using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// [ExecuteAlways]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class PrismGenerator_v4 : MonoBehaviour
{
    [SerializeField] private int polycount = 3;

    [SerializeField] private float hight = 0.5f;

    [SerializeField] private int hightdivide = 0;

    [SerializeField] private float radius = 1;
    
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
        hightdivide = Mathf.Max(0, hightdivide);
        radius = Mathf.Max(0, radius);

        var hight_max = hight / 2f;
        var hight_min = hight / -2f;

        var maxdividenum = hightdivide + 2;
        
        
        var vertices = new List<Vector3>(){ };

        ////////////////////////////
        // 頂点作成
        ////////////////////////////
        // 天井
        vertices.Add(new Vector3 (0f, hight_max, 0f));
        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                     Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius,
                     hight_max,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius
                     )
            );
        }
        
        // 柱
        for(int t = 0; t < maxdividenum - 1 ; t++)
        {
            var hight_1 = Mathf.Lerp(hight_max, hight_min, t / (hightdivide + 1f));
            var hight_2 = Mathf.Lerp(hight_max, hight_min, (t + 1f) / (hightdivide + 1f));
            
            for (int i = 0; i < polycount; i++)
            {
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius,
                        hight_1,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius,
                        hight_1,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius,
                        hight_2,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius,
                        hight_2,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius
                    )
                );
            }
        }

        // 床
        vertices.Add(new Vector3 (0f, hight_min, 0f));
        for (int i = 0; i < polycount; i++)
        {
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius,
                    hight_min,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius
                )
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
        for(int tmp_2 = 0; tmp_2 < hightdivide + 1; tmp_2++){
            for (int i = 0; i < polycount; i++)
            {
                int j = (polycount + 1) + 4 * i + (tmp_2 * polycount * 4);

                triangles.Add(j);
                triangles.Add(j + 1);
                triangles.Add(j + 2);
                triangles.Add(j + 1);
                triangles.Add(j + 3);
                triangles.Add(j + 2);
            }
        }
        
        // 床
        int tmp_1 = polycount + polycount * 4 *  (hightdivide + 1) + 1;
        for (int i = 0; i < polycount; i++)
        {
            triangles.Add(tmp_1);
            triangles.Add(tmp_1 + i % polycount + 1);
            triangles.Add(tmp_1 + (i + 1) % polycount + 1);
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
