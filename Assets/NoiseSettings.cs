using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseSettings : ScriptableObject
{
    public int seed;
    public Vector3 centre;
    [Range(1, 8)]
    public int numLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float persistence = .5f;
    public float minValue;
    public float strength = 1;
    
}
