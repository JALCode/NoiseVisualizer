using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NoiseGenerator))]
public class NoiseGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        NoiseGenerator mapGen = (NoiseGenerator)target;

        if (DrawDefaultInspector()) {
                mapGen.DrawNoise();
        }

        if (GUILayout.Button("Generate")) {
            mapGen.DrawNoise();
        }
    }
}