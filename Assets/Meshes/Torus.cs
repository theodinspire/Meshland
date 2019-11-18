using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : MonoBehaviour
{
    // Adjustables
    public float curveRadius;
    public float torusRadius;
    public int curveSegmentCount;
    public int torusSegmentCount;

    // Mesh Render
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.name = "Torus";
        SetVertices();
        SetTriangles();
    }

    // Map torus
    private Vector3 GetPointOnTorus(float u, float v)
    {
        var radius = curveRadius + torusRadius * Mathf.Cos(v);

        return new Vector3
        {
            x = radius * Mathf.Sin(u),
            y = radius * Mathf.Cos(u),
            z = torusRadius * Mathf.Sin(v)
        };
    }
    
    // Draw torus
    private void OnDrawGizmos()
    {
        var uStep = 2 * Mathf.PI / curveSegmentCount;
        var vStep = 2 * Mathf.PI / torusSegmentCount;

        for (var u = 0; u < curveSegmentCount; ++u)
        for (var v = 0; v < torusSegmentCount; ++v)
        {
            var point = GetPointOnTorus(u * uStep, v * vStep);
            Gizmos.color = new Color(1, (float) v / torusSegmentCount, (float) u / curveSegmentCount);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    private void SetVertices()
    {
        vertices = new Vector3[torusSegmentCount * curveSegmentCount * 4];

        var step = 2f * Mathf.PI / curveSegmentCount;
        
        CreateFirstQuadRing(step);

        var delta = torusSegmentCount * 4;

        for (int u = 2, i = delta; u <= curveSegmentCount; ++u, i += delta)
        {
            CreateQuadRing(u * step, i);
        }

        mesh.vertices = vertices;
    }

    private void SetTriangles()
    {
        var triangleCount = torusSegmentCount * curveSegmentCount * 6;
        triangles = new int[triangleCount];

        for (int t = 0, i = 0; t < triangleCount; t += 6, i += 4)
        {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }

        mesh.triangles = triangles;
    }

    private void CreateFirstQuadRing(float u)
    {
        var step = 2 * Mathf.PI / torusSegmentCount;

        var a = GetPointOnTorus(0, 0);
        var b = GetPointOnTorus(u, 0);

        for (int v = 1, i = 0; v <= torusSegmentCount; ++v, i += 4)
        {
            vertices[i] = a;
            vertices[i + 2] = b;

            a = GetPointOnTorus(0, v * step);
            b = GetPointOnTorus(u, v * step);

            vertices[i + 1] = a;
            vertices[i + 3] = b;
        }
    }

    private void CreateQuadRing(float u, int i)
    {
        var step = 2 * Mathf.PI / torusSegmentCount;
        var offset = torusSegmentCount * 4;

        var vertex = GetPointOnTorus(u, 0);

        for (var v = 1; v <= torusSegmentCount; ++v, i += 4)
        {
            vertices[i] = vertices[i - offset + 2];
            vertices[i + 1] = vertices[i - offset + 3];
            vertices[i + 2] = vertex;

            vertex = GetPointOnTorus(u, v * step);

            vertices[i + 3] = vertex;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
