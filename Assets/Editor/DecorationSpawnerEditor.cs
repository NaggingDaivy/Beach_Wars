using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects, CustomEditor(typeof(DecorationSpawner))]
public class DecorationSpawnerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Respawn decorations"))
        {
            ((DecorationSpawner)target).SpawnDecoration();
        }

        if (GUILayout.Button("Remove decorations"))
        {
            ((DecorationSpawner)target).RemoveDecoration();
        }
    }

	
}
