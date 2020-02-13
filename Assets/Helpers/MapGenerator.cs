using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [Range(0.5f, 10f)]
    public float perlinScale = 1.65f;
    public bool useRandomOffsets = true;
    [Range(0f, 5000f)]
    public float offsetX = 100f;
    [Range(0f, 5000f)]
    public float offsetY = 100f;

    public bool useRadialFalloff = true;
    [Range(0f, 10f)]
    public float radialScale = 1f;
    [Range(-50f, 50f)]
    public float radialOffsetX = 0f;
    [Range(-50f, 50f)]
    public float radialOffsetY = 0f;

    // GenerateMap() function is in Map.cs which is called in MapController.cs

    // Generates a heightmap for the tile array using Perlin Noise and an optional Radial Falloff to look like an island.
    public float[,] GenerateHeights(int width, int height) {

        if (useRandomOffsets) {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);
        }

        float[,] heights = new float[width, height];


        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                heights[x, y] = calculateHeight(x, y, width, height);
            }
        }

        if (useRadialFalloff) {

            float halfwidth = width * 0.5f;
            float halfheight = height * 0.5f;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    float xx = x - halfwidth + radialOffsetX;
                    float yy = y - halfheight + radialOffsetY;
                    float dx = xx / halfwidth;
                    float dy = yy / halfheight;
                    float d = 1.0f - Mathf.Sqrt(dx * dx + dy * dy);

                    if (d < 0) d = 0.001f;


                    heights[x, y] *= d * radialScale;
                }
            }
        }


        return heights;
    }

    // Calculates the Perlin Noise value for the array.
    public float calculateHeight(int x, int y, int width, int height) {

        float xCoord = (float)x / width * perlinScale + offsetX;
        float yCoord = (float)y / height * perlinScale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }


}
