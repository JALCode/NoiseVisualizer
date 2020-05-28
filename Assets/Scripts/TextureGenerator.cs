using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextureGenerator {

    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }


    public static Texture2D TextureFromHeightMap(float[,] heightMap,Gradient gradient) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap[y * width + x] = gradient.Evaluate(heightMap[x, y]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }

    public static Texture2D LineFromHeightMap(float[,] heightMap, Gradient gradient) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colourMap = new Color[width * height];

        int halfHeight = height / 2;
        int quarterHeight = height / 4;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap[y * width + x] = Color.white;
                if (Mathf.RoundToInt(heightMap[x, halfHeight] *quarterHeight)+halfHeight == y) {
                    colourMap[y * width + x] = gradient.Evaluate(heightMap[x, halfHeight]);
                }

            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }

    public static Vector3[] LinePositionsFromHeightMap(float[,] heightMap, float heightmultiplier) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        List<Vector3> positions = new List<Vector3>();



        for (int x = 0; x < width-2; x++) {

            positions.Add(new Vector3(x - width / 2, (heightMap[x, height / 2] * 2 - 1) * heightmultiplier, 0));

        }

        return positions.ToArray();
    }

}
