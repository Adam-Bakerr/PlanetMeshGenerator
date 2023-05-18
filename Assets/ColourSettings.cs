using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{
    [System.Serializable]
    public struct terrainColours
    {
        public string name;
        public float height;
        public Color color;

    }
    [SerializeField]
    public terrainColours[] Colors;

}
