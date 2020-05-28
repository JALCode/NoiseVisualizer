using UnityEngine;
using System.Collections;

public class NoiseDisplay : MonoBehaviour {

    public LineRenderer lineRenderer;

    public MeshFilter planeFilter;
    public MeshRenderer planeRenderer;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public MeshFilter standFilter;
    public MeshRenderer standRenderer;

    public Material lineMaterial;
    public Material textureMaterial;
    public Material meshMaterial;


    public void DrawTexture(Texture2D texture) {
        textureMaterial.SetTexture("_BaseMap", texture);
        planeRenderer.sharedMaterial = textureMaterial;
        planeRenderer.transform.localScale = new Vector3(-10, 1, 10);
    }

    public void DrawMesh(MeshData meshData) {

        meshRenderer.sharedMaterial = meshMaterial;

        meshFilter.sharedMesh = meshData.CreateMesh();

        meshFilter.transform.localScale = Vector3.one; 
    }

    public void DrawStand(MeshData meshData) {

        standRenderer.sharedMaterial = meshMaterial;

        standFilter.sharedMesh = meshData.CreateMesh();

        standFilter.transform.localScale = Vector3.one; 
    }

    public void DrawLine(Vector3 [] positions, Gradient gradient) {

        lineRenderer.positionCount = positions.Length;
        lineRenderer.sharedMaterial.SetColor("Color0", gradient.Evaluate(0));
        lineRenderer.sharedMaterial.SetColor("Color1", gradient.Evaluate(1));
        lineRenderer.SetPositions(positions);

        meshRenderer.transform.localScale = new Vector3(1, 1, 1);

    }

}