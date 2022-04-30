using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// [ExecuteAlways]
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class SphereGenerator_V1 : MonoBehaviour
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
        hightdivide = Mathf.Max(1, hightdivide);
        radius = Mathf.Max(0, radius);

        var rad_normalize = radius / radius;
        
        var vertices = new List<Vector3>(){ };

        ////////////////////////////
        // 頂点作成
        ////////////////////////////
        // 天井
        for (int i = 0; i < polycount; i++)
        {
            var hight_1 = Mathf.Lerp(radius, -radius, /* t */ 0 / (hightdivide + 1f));
            var hight_2 = Mathf.Lerp(radius, -radius, /*(t + 1f)*/ 1 / (hightdivide + 1f));
        
            var elem_arccos_1 = Mathf.Lerp(rad_normalize, -rad_normalize, 0 / (hightdivide + 1f));
            var elem_arccos_2 = Mathf.Lerp(rad_normalize, -rad_normalize, 1 / (hightdivide + 1f));

            var rad_circle_1 = Mathf.Sin(Mathf.Acos(elem_arccos_1));
            var rad_circle_2 = Mathf.Sin(Mathf.Acos(elem_arccos_2));
            
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1,
                     hight_1,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1
                )
            );
            vertices.Add(
                new Vector3(
                     Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2,
                     hight_2,
                     Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2
                )
            );
            vertices.Add(
                new Vector3(
                     Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_2,
                     hight_2,
                     Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_2
                )
            );
        }
        
        // 柱
        for(int t = 1; t < hightdivide ; t++)
        {
            var hight_1 = Mathf.Lerp(radius, -radius, t / (hightdivide + 1f));
            var hight_2 = Mathf.Lerp(radius, -radius, (t + 1f) / (hightdivide + 1f));
            
            var elem_arccos_1 = Mathf.Lerp(rad_normalize, -rad_normalize, t / (hightdivide + 1f));
            var elem_arccos_2 = Mathf.Lerp(rad_normalize, -rad_normalize, (t + 1f) / (hightdivide + 1f));

            var rad_circle_1 = Mathf.Sin(Mathf.Acos(elem_arccos_1));
            var rad_circle_2 = Mathf.Sin(Mathf.Acos(elem_arccos_2));

            
            for (int i = 0; i < polycount; i++)
            {
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1,
                        hight_1,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_1,
                        hight_1,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_1
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2,
                        hight_2,
                        Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2
                    )
                );
                vertices.Add(
                    new Vector3(
                        Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_2,
                        hight_2,
                        Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_2
                    )
                );
            }
        }
        
        // 床
        
        for (int i = 0; i < polycount; i++)
        {
            var hight_1 = Mathf.Lerp(radius, -radius, hightdivide / (hightdivide + 1f));
            var hight_2 = Mathf.Lerp(radius, -radius, (hightdivide + 1f) / (hightdivide + 1f));

            var elem_arccos_1 = Mathf.Lerp(rad_normalize, -rad_normalize, hightdivide / (hightdivide + 1f));
            var elem_arccos_2 = Mathf.Lerp(rad_normalize, -rad_normalize, (hightdivide + 1f) / (hightdivide + 1f));

            var rad_circle_1 = Mathf.Sin(Mathf.Acos(elem_arccos_1));
            var rad_circle_2 = Mathf.Sin(Mathf.Acos(elem_arccos_2));

            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2,
                    hight_2,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_2
                )
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1,
                    hight_1,
                    Mathf.Sin(i * Mathf.PI * 2f / polycount) * radius * rad_circle_1
                )
            );
            vertices.Add(
                new Vector3(
                    Mathf.Cos((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_1,
                    hight_1,
                    Mathf.Sin((i + 1) * Mathf.PI * 2f / polycount) * radius * rad_circle_1
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
        for(int tmp_2 = 0; tmp_2 < hightdivide - 1; tmp_2++){
            for (int i = 0; i < polycount; i++)
            {
                int j = polycount * 3 + 4 * i + (tmp_2 * polycount * 4);
        
                triangles.Add(j);
                triangles.Add(j + 1);
                triangles.Add(j + 2);
                triangles.Add(j + 1);
                triangles.Add(j + 3);
                triangles.Add(j + 2);
            }
        }

        // 床
        int tmp_1 = polycount * 3 + polycount * 4 *  (hightdivide - 1);
        for (int i = 0; i < polycount; i++)
        {
            int j = i + tmp_1;
            var x_1 = tmp_1 + i * 3 + 0;
            var x_2 = tmp_1 + i * 3 + 1;
            var x_3 = tmp_1 + i * 3 + 2;
            
            triangles.Add(x_1);
            triangles.Add(x_2);
            triangles.Add(x_3);
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
