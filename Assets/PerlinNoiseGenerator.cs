using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//No Longer using perlin
// Switched to a simplex noise 
public static class PerlinNoiseGenerator 
{
    public static float[,] generateNoiseMap(int mapwidth, int mapheight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        //Use pseudo rng to create seed allowing for recreation of certain maps
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            //Gain offset between two points  and store them to the octaves offset array
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //clamp scale above 0 to prevent a divison by zero
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = mapwidth / 2f;
        float halfHeight = mapheight / 2f;
        float[,] noiseMap = new float[mapwidth, mapheight];
        for (int y = 0; y < mapheight; y++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                float amplitude = 1;
                float frequncey = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float samplex = (x - halfWidth) / scale * frequncey + octaveOffsets[i].x;
                    float sampley = (y - halfHeight) / scale * frequncey + octaveOffsets[i].y;
                    float perlin = (Mathf.PerlinNoise(samplex, sampley) * 2) - 1;
                    noiseHeight += perlin * amplitude;
                    amplitude *= persistance;
                    frequncey *= lacunarity;
                }
                //Make note of min and max noise heights
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapheight; y++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                //Returns value from 0,1 in terms of percent of min and max noise height
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y])*.5f;
            }
        }
        return noiseMap;
    }
}
