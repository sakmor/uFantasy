using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BiologysMenu))]
public class BiologysMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BiologysMenu BiologysMenu = (BiologysMenu)target;
        BiologysMenu.UpdateBiologysList();

        if (BiologysMenu.Biologys.Length == 0) return;

        foreach (var i in BiologysMenu.Biologys)
        {
            if (GUILayout.Button(i.name))
            {
                Selection.activeGameObject = i.gameObject;
            }
        }

        if (GUI.changed)
        {
            // Biology.LoadDB();
        }

    }
}