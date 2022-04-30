using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// [ExecuteAlways]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class SphereGenerator_V2 : MonoBehaviour
{
    [SerializeField] private int polycount = 3;

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
        hightdivide = Mathf.Max(2, hightdivide);
        radius = Mathf.Max(0, radius);
        var vertices = new List<Vector3>(){ };

        ////////////////////////////
        // 頂点作成
        ////////////////////////////
        // 天井
        for (int i = 0; i < polycount; i++)
        {
            var hight_1 = Mathf.Cos(Mathf.PI * 0 / hightdivide) * radius;
            var hight_2 = Mathf.Cos(Mathf.PI * 1 / hightdivide) * radius;
            var rad_circle_1 = Mathf.Sin(Mathf.PI * 0 / hightdivide) * radius;
            var rad_circle_2 = Mathf.Sin(Mathf.PI * 1 / hightdivide) * radius;

            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_1,
                     hight_1,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount) * rad_circle_1
                )
            );
            vertices.Add(
                new Vector3(
                     Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_2,
                     hight_2,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount) * rad_circle_2
                )
            );
            vertices.Add(
                new Vector3(
                     Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_2,
                     hight_2,
                     Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_2
                )
            );
        }
        
        // 柱
        for(int t = 1; t < hightdivide ; t++)
        {
            var hight_1 = Mathf.Cos(Mathf.PI * t / hightdivide) * radius;
            var hight_2 = Mathf.Cos(Mathf.PI * (t + 1) / hightdivide) * radius;
            var rad_circle_1 = Mathf.Sin(Mathf.PI * t / hightdivide) * radius;
            var rad_circle_2 = Mathf.Sin(Mathf.PI * (t + 1) / hightdivide) * radius;
            
            for (int i = 0; i < polycount; i++)
            {
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_1,
                        hight_1,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount)* rad_circle_1
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_1,
                        hight_1,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount)  * rad_circle_1
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_2,
                        hight_2,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount) * rad_circle_2
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_2,
                        hight_2,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_2
                    )
                );
            }
        }
        
        // 床
        for (int i = 0; i < polycount; i++)
        {

            var hight_1 = Mathf.Cos(Mathf.PI * (hightdivide - 1) / hightdivide) * radius;
            var hight_2 = Mathf.Cos(Mathf.PI * hightdivide / hightdivide) * radius;
            var rad_circle_1 = Mathf.Sin(Mathf.PI * (hightdivide - 1) / hightdivide) * radius;
            var rad_circle_2 = Mathf.Sin(Mathf.PI * hightdivide / hightdivide) * radius;

            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_2,
                    hight_2,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount) * rad_circle_2
                )
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * rad_circle_1,
                    hight_1,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount) * rad_circle_1
                )
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_1,
                    hight_1,
                    Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * rad_circle_1
                )
            );
        }
        
        ////////////////////////////
        // 三角形登録
        ////////////////////////////
        var triangles = new List<int>() { };
        
        // 天井
        for (int i = 0; i < polycount; i++)
        {
            triangles.Add(i * 3 + 0);
            triangles.Add(i * 3 + 2);
            triangles.Add(i * 3 + 1);
        }

        // 柱
        for(int k = 0; k < hightdivide - 1; k++){
            for (int i = 0; i < polycount; i++)
            {
                int j = polycount * 3 + 4 * i + (k * polycount * 4);
        
                triangles.Add(j);
                triangles.Add(j + 1);
                triangles.Add(j + 2);
                triangles.Add(j + 1);
                triangles.Add(j + 3);
                triangles.Add(j + 2);
            }
        }

        // 床
        int numberOfVerticesSoFar = polycount * 3 + polycount * 4 *  (hightdivide - 1);
        for (int i = 0; i < polycount; i++)
        {
            triangles.Add(numberOfVerticesSoFar + i * 3 + 0);
            triangles.Add(numberOfVerticesSoFar + i * 3 + 1);
            triangles.Add(numberOfVerticesSoFar + i * 3 + 2);
        }
        
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.name = "Sphere";

        GetComponent<MeshFilter>().mesh = mesh;
        
        /*
        var filter = GetComponent<MeshFilter> ();
        // filter.mesh = mesh;
        filter.sharedMesh = mesh;  
        */
    }
 
}
