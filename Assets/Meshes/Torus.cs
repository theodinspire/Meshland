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
        this.mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = this.mesh;

        this.mesh.name = "Torus";
        this.SetVertices();
        this.SetTriangles();
    }

    // Map torus
    private Vector3 GetPointOnTorus(float u, float v)
    {
        var radius = this.curveRadius + this.torusRadius * Mathf.Cos(v);

        return new Vector3
        {
            x = radius * Mathf.Sin(u),
            y = radius * Mathf.Cos(u),
            z = this.torusRadius * Mathf.Sin(v)
        };
    }
    
    // Draw torus
    private void OnDrawGizmos()
    {
        var uStep = 2 * Mathf.PI / this.curveSegmentCount;
        var vStep = 2 * Mathf.PI / this.torusSegmentCount;

        for (var u = 0; u < this.curveSegmentCount; ++u)
        for (var v = 0; v < this.torusSegmentCount; ++v)
        {
            var point = GetPointOnTorus(u * uStep, v * vStep);
            Gizmos.color = new Color(1, (float) v / this.torusSegmentCount, (float) u / this.curveSegmentCount);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    private void SetVertices()
    {
        this.vertices = new Vector3[this.torusSegmentCount * this.curveSegmentCount * 4];

        var step = 2f * Mathf.PI / curveSegmentCount;
        
        CreateFirstQuadRing(step);

        var delta = this.torusSegmentCount * 4;

        for (int u = 2, i = delta; u <= this.curveSegmentCount; ++u, i += delta)
        {
            CreateQuadRing(u * step, i);
        }

        mesh.vertices = vertices;
    }

    private void SetTriangles()
    {
        triangles = new int[torusSegmentCount * curveSegmentCount * 6];

        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4)
        {
            triangles[t] = i;
            triangles[i + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }

        mesh.triangles = triangles;
    }

    private void CreateFirstQuadRing(float u)
    {
        var step = 2 * Mathf.PI / this.torusSegmentCount;

        var a = this.GetPointOnTorus(0, 0);
        var b = this.GetPointOnTorus(u, 0);

        for (int v = 1, i = 0; v <= this.torusSegmentCount; ++v, i += 4)
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
        var step = 2 * Mathf.PI / this.torusSegmentCount;
        var offset = this.torusSegmentCount * 4;

        var vertex = this.GetPointOnTorus(u, 0);

        for (var v = 1; v <= this.torusSegmentCount; ++v, i += 4)
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
