using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    private Vector3[] directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

    public ColourSettings colourSettings;
    [HideInInspector]
    public bool showcolourSettings;

    public NoiseSettings NoiseSettings;
    [HideInInspector]
    public bool showNoiseSettings;

    public ShapeSettings shapeSettings;
    [HideInInspector]
    public bool showShapeSettings;

    public Noise noise;
    public int seed;

    public bool autoUpdate = true;


   
    //ADd custom editor
    private GameObject[] gameObjects = new GameObject[6];


    void Start()
    {
        gameObjects = new GameObject[6];
        initMesh();
    }

    public void initMesh()
    {
        noise = new Noise(seed);
        System.Type[] RequiredComponents = new System.Type[] {
        typeof(MeshRenderer),
        typeof(MeshFilter),
        typeof(MeshGenerator)
        };
        //6 Faces To The SPhere
        for (int i = 0; i < 6; i++)
        {
            MeshGenerator meshGenerator;
            if (gameObjects[i] == null)
            {   
                gameObjects[i] = new GameObject(i.ToString(), RequiredComponents);
                gameObjects[i].transform.parent = transform;
                meshGenerator = gameObjects[i].GetComponent<MeshGenerator>();
                meshGenerator.localUp = directions[i];
            }
            else
            {
                meshGenerator = gameObjects[i].GetComponent<MeshGenerator>();
            }

            meshGenerator.cellSize = shapeSettings.faceSize;
            meshGenerator.gridSize = shapeSettings.gridSize;
            meshGenerator.colorChart = colourSettings.Colors;
            meshGenerator.NoiseSettings = NoiseSettings;
            meshGenerator.updateColors();
            meshGenerator.startMesh();
            meshGenerator.updateMesh();
        }
    }

    public void onUpdateShapeSettings()
    {
        
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] == null) continue;
            MeshGenerator meshGenerator = gameObjects[i].GetComponent<MeshGenerator>();
            meshGenerator.cellSize = shapeSettings.faceSize;
            meshGenerator.gridSize = shapeSettings.gridSize;
        }
        
    }
    public void onColorChange()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            MeshGenerator meshGenerator = gameObjects[i].GetComponent<MeshGenerator>();
            meshGenerator.colorChart = colourSettings.Colors;
            meshGenerator.updateColors();
        }
    }

    public void onNoiseSettingsUpdate()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            MeshGenerator meshGenerator = gameObjects[i].GetComponent<MeshGenerator>();
            meshGenerator.NoiseSettings = NoiseSettings;
            meshGenerator.updateMesh();
        }
    }
    





}
