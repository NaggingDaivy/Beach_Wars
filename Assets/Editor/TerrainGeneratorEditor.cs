using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(TerrainGenerator))]  // pour faire le lien avec le script TerrainGenerator
public class TerrainGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector(); // dessine l'inspecteur par défaut

        if (GUILayout.Button("Rebuild Terrain")) // si on a appuyé sur le bouton
        {
            ((TerrainGenerator)target).RegenerateMesh(); 

        }

        if (GUILayout.Button("Rebuild Texture")) // si on a appuyé sur le bouton
        {
            ((TerrainGenerator)target).RegenerateTexture();

        }


    }
}
