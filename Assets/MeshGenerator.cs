using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    int[] triangles;
    public Vector3[] verticies;
    public Vector3 localUp;
    public int gridSize;
    public int cellSize;
    public Material defaultMatierial;
    public ColourSettings.terrainColours[] colorChart;
    public Noise noise;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Vector3 axisA;
    Vector3 axisB;
    [HideInInspector]
    public NoiseSettings NoiseSettings;


    public void startMesh()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        defaultMatierial = Resources.Load<Material>("Material/Default");
        meshRenderer.material = defaultMatierial;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }


    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    private void Update()
    {

    }


    public void createGrid()
    {
        
        verticies = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[gridSize * gridSize * 6];
        
        for (int y = 0; y < (gridSize + 1); y++)
        {
            for (int x = 0; x <(gridSize+1 ); x++)
            {
                int i = x + y * (gridSize+1);
                Vector2 percent = new Vector2(x, y) / (gridSize);
                Vector3 PointOnCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 PointOnSphere = PointOnCube.normalized;
                float elevation = evalNoise(PointOnSphere);
                PointOnSphere *= cellSize * (1 + elevation);
                verticies[i] = PointOnSphere;
                
            }
        }
        
        triangles = new int[gridSize * gridSize * 6];
        int tris = 0;
        int vert = 0;
        for(int z = 0; z < gridSize; z++) { 
        for (int x = 0 ; x < gridSize; x ++)
        { 
                triangles[tris + 2] = vert + 0;
                triangles[tris + 1] = vert + gridSize + 1;
                triangles[tris + 0] = vert + 1;
                triangles[tris + 5] = vert + 1;
                triangles[tris + 4] = vert + gridSize + 1;
                triangles[tris + 3] = vert + gridSize + 2;
                vert++;
                tris +=6;
        }
        vert++;
        }
    }

    public float evalNoise(Vector3 PointOnSphere)
    {
        Noise noise = new Noise(NoiseSettings.seed);
        float noiseValue = 0;
        float frequency = NoiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < NoiseSettings.numLayers; i++)
        {
            float v = noise.Evaluate(PointOnSphere * frequency + NoiseSettings.centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= NoiseSettings.roughness;
            amplitude *= NoiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - NoiseSettings.minValue);
        return (noiseValue * NoiseSettings.strength);
    }

    public void updateMesh()
    {
        mesh.Clear();
        createGrid();
        mesh.vertices = verticies;
        updateColors();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void updateColors()
    {
        if (mesh == null) return;
        mesh.colors = getColours();
    }
    
    public Color[] getColours()
    {
        Color[] vertexColours = new Color[verticies.Length];
        for (int vertexIndex = 0; vertexIndex < verticies.Length-1; vertexIndex++)
        {
            //vertexColours[vertexIndex] = gradient.Evaluate(verticies[vertexIndex].y);
            float vertexMagnitude = (Vector3.Magnitude(verticies[vertexIndex] - transform.position))/cellSize;
            for (int ColourIndex = 0; ColourIndex < colorChart.Length; ColourIndex++)
            {
                if (vertexMagnitude > colorChart[ColourIndex].height)
                {
                    vertexColours[vertexIndex] = colorChart[ColourIndex].color;
                }
            }
        }
        return vertexColours;

    }
    
}

