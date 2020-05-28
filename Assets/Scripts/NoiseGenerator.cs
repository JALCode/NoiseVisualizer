using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public enum DrawMode { Line, Plane, Mesh };
    public DrawMode drawMode;

    #region noise
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    #endregion

    #region terrain
    public float uniformScale = 2.5f;

    public bool useFalloff;
    float[,] falloffMap;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    #endregion

    #region visual
    public Gradient colorGradient;

    public bool useFlatShading;

    public int mapSize = 97;

    [Range(0, 6)]
    public int LOD;
    #endregion

    public void DrawNoise() {
        float[,] heightMap = GenerateHeightMap();

        NoiseDisplay display = FindObjectOfType<NoiseDisplay>();

        if (drawMode == DrawMode.Line) {
            display.DrawLine(TextureGenerator.LinePositionsFromHeightMap(heightMap, meshHeightMultiplier), colorGradient);
        } else if (drawMode == DrawMode.Plane) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap, colorGradient));
        } else {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, meshHeightMultiplier, meshHeightCurve, colorGradient, LOD, useFlatShading));
            display.DrawStand(MeshGenerator.GenerateTerrainMeshStand(heightMap, meshHeightMultiplier, meshHeightCurve, colorGradient, LOD, useFlatShading));
        }
    }

    private void Update() {

    }

    public float[,] GenerateHeightMap() {



        float[,] noiseMap = new float[mapSize, mapSize];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (noiseScale <= 0) {
            noiseScale = 0.0001f;
        }

        float halfSize = mapSize / 2f;



        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {

                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfSize + octaveOffsets[i].x) / noiseScale * frequency;
                    float sampleY = (y - halfSize + octaveOffsets[i].y) / noiseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }



        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }
        }


        if (useFalloff) {
            if (falloffMap == null) {
                falloffMap = FalloffGenerator.GenerateFalloffMap(mapSize);
            }

            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) {
                    noiseMap[j, i] = Mathf.Clamp01(noiseMap[j, i] - falloffMap[j, i]);
                }

            }
        }

        return noiseMap;
    }


    public void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
        if (persistance < 0) {
            persistance = 0;
        }
    }
}
