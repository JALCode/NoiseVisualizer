using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshGenerator {

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, Gradient _colorGradient, int levelOfDetail,bool flatshading) {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        Gradient colorGradient = new Gradient();
        colorGradient.SetKeys(_colorGradient.colorKeys, _colorGradient.alphaKeys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        if(levelOfDetail == 5) {
            meshSimplificationIncrement = (levelOfDetail - 1) * 2;
        }
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine,false);
        int vertexIndex = 0;

        for (int y = 0; y < height; y += meshSimplificationIncrement) {
            for (int x = 0; x < width; x += meshSimplificationIncrement) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, (heightCurve.Evaluate(heightMap[x, y])*2-1) * heightMultiplier, topLeftZ - y);
                meshData.colors[vertexIndex] = colorGradient.Evaluate(heightCurve.Evaluate(heightMap[x, y]));
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        if (flatshading) {
            meshData.FlatShading();
        }


        return meshData;

    }



    public static MeshData GenerateTerrainMeshStand(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, Gradient _colorGradient, int levelOfDetail, bool flatshading) {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        Gradient colorGradient = new Gradient();
        colorGradient.SetKeys(_colorGradient.colorKeys, _colorGradient.alphaKeys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        if (levelOfDetail == 5) {
            meshSimplificationIncrement = (levelOfDetail - 1) * 2;
        }
        int verticesPerLine = (width -1) / meshSimplificationIncrement + 1 +2;

        MeshData meshData = new MeshData(verticesPerLine,true);
        int vertexIndex = 0;


        for (int y = -meshSimplificationIncrement; y < height + meshSimplificationIncrement; y += meshSimplificationIncrement) {

            for (int x = -meshSimplificationIncrement; x < width + meshSimplificationIncrement; x += meshSimplificationIncrement) {

                int i = (x == -meshSimplificationIncrement) ? 0 : (x == width-1+ meshSimplificationIncrement) ? width - 1 : x;
                int j = (y == -meshSimplificationIncrement) ? 0 : (y == height-1+ meshSimplificationIncrement) ? height - 1 : y;


                if (x == -meshSimplificationIncrement || y == -meshSimplificationIncrement || x == width - 1 + meshSimplificationIncrement || y == height - 1 + meshSimplificationIncrement) {

                    meshData.vertices[vertexIndex] = new Vector3(topLeftX+i, (heightCurve.Evaluate(heightMap[i, j])*2-1) * heightMultiplier, topLeftZ-j);
                    meshData.colors[vertexIndex] = colorGradient.Evaluate(heightCurve.Evaluate(heightMap[i, j]));
                    meshData.uvs[vertexIndex] = new Vector2(i / (float)width, j / (float)height);
                
                } else{
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, -heightMultiplier*1.5f, topLeftZ - y);
                    meshData.colors[vertexIndex] = colorGradient.Evaluate(0);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                }


                if (x < width && y < height && !(x==-meshSimplificationIncrement && y == -meshSimplificationIncrement) 
                    && !(x == -meshSimplificationIncrement && y == height-1) 
                    && !(x == width- 1 && y == -meshSimplificationIncrement) && !(x == width - 1 && y == height - 1)) {
                    meshData.AddTriangle(vertexIndex + 1, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
                    meshData.AddTriangle(vertexIndex + verticesPerLine, vertexIndex + 1, vertexIndex);
                    //meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    //meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);

                }

                vertexIndex++;
            }
        }

        return meshData;

    }
}


public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    public Color[] colors;

    bool stand;

    int triangleIndex;


    public MeshData(int verticesPerLine,bool stand) {
        this.stand = stand;

        if (!stand) {
            vertices = new Vector3[verticesPerLine * verticesPerLine];
            uvs = new Vector2[vertices.Length];
            colors = new Color[vertices.Length];
            triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
        } else {
            vertices = new Vector3[(verticesPerLine) * (verticesPerLine)];
            uvs = new Vector2[vertices.Length];
            colors = new Color[vertices.Length];
            triangles = new int[(verticesPerLine +1) * (verticesPerLine +1) * 6 - (8*3)];
        }
    }


    public void AddTriangle(int a, int b, int c) {

        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;

    }

    public void FlatShading() {
        if (!stand) {
            Vector3[] flatShadedVertices = new Vector3[triangles.Length];
            Vector2[] flatShadedUvs = new Vector2[triangles.Length];
            Color[] flatShadedColors = new Color[triangles.Length];

            for (int i = 0; i < triangles.Length; i++) {
                flatShadedVertices[i] = vertices[triangles[i]];
                flatShadedUvs[i] = uvs[triangles[i]];
                flatShadedColors[i] = colors[triangles[i]];
                triangles[i] = i;
            }

            vertices = flatShadedVertices;
            uvs = flatShadedUvs;
            colors = flatShadedColors;
        }
    }

    public void StandRecalculation(int vertexPerRow) {

        Vector3[] standVertices = new Vector3[vertices.Length-8];
        Vector2[] standUvs = new Vector2[standVertices.Length];
        Color[] standColors = new Color[standVertices.Length];

        int indexcount = 0;

        for (int i = 0; i < vertices.Length; i++) {

            if(i != 1 && i != vertexPerRow - 2 && i != vertexPerRow && i != 2 * vertexPerRow - 1 
                && i != vertices.Length-2 && i != vertices.Length - vertexPerRow + 1 && i != vertices.Length - vertexPerRow - 1 && i != vertices.Length - 2 * vertexPerRow) {

                standVertices[indexcount] = vertices[i];
                standUvs[indexcount] = uvs[i];
                standColors[indexcount] = colors[i];

                indexcount++;
            }
            
        }

        for (int i = 0; i < triangles.Length; i++) {
            if (triangles[i] == 1) {
                triangles[i] = 0;
            }else if (triangles[i] == vertexPerRow - 2) {
                triangles[i] = vertexPerRow - 1;
            } else if (triangles[i] == vertexPerRow) {
                triangles[i] = 0;
            } else if (triangles[i] == 2 * vertexPerRow - 1) {
                triangles[i] = vertexPerRow - 1;
            } else if (triangles[i] == vertexPerRow * vertexPerRow - 2) {
                triangles[i] = vertexPerRow * vertexPerRow;
            } else if (triangles[i] == vertexPerRow * (vertexPerRow - 1) + 1) {
                triangles[i] = vertexPerRow * (vertexPerRow - 1);
            } else if (triangles[i] == vertexPerRow * (vertexPerRow - 1) - 1) {
                triangles[i] = vertexPerRow * vertexPerRow;
            } else if (triangles[i] == vertexPerRow * (vertexPerRow - 2)) {
                triangles[i] = vertexPerRow * (vertexPerRow - 1);
            } else {
                if (triangles[i] < 1) {
                    triangles[i] = triangles[i];
                } else if (triangles[i] < vertexPerRow - 2) {
                    triangles[i] = triangles[i] - 1;
                } else if (triangles[i] < vertexPerRow) {
                    triangles[i] = triangles[i] - 2;
                } else if (triangles[i] < 2 * vertexPerRow - 1) {
                    triangles[i] = triangles[i] - 3;
                } else if (triangles[i] < vertexPerRow * vertexPerRow - 2) {
                    triangles[i] = triangles[i] - 4;
                } else if (triangles[i] < vertexPerRow * (vertexPerRow - 1) + 1) {
                    triangles[i] = triangles[i] - 5;
                } else if (triangles[i] < vertexPerRow * (vertexPerRow - 1) - 1) {
                    triangles[i] = triangles[i] - 6;
                } else if (triangles[i] < vertexPerRow * (vertexPerRow - 2)) {
                    triangles[i] = triangles[i] - 7;
                }

            }
        }



        vertices = standVertices;
        uvs = standUvs;
        colors = standColors;

    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;


        mesh.RecalculateNormals();

        return mesh;
    }


    public void MakeQuad() {
        Vector3[] quadVertices = new Vector3[triangles.Length];
        Vector2[] quadUvs = new Vector2[triangles.Length];
        Color[] quadColors = new Color[triangles.Length];
        int[] quadTriangles = new int[triangles.Length];
    }
}