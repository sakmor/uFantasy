using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;

[CanEditMultipleObjects]
[CustomEditor(typeof(CUBE))]
public class CubeEditor : Editor
{
    private CUBE CUBE;
    private Vector2 DrawBiologysListScrollPos, DrawSelectedBiologyScrollPos;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CUBE = (CUBE)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("^"))
        {
            foreach (var i in Selection.gameObjects)
            {
                i.transform.eulerAngles += new Vector3(0, 0, -90);
            }
        }

        if (GUILayout.Button("<"))
        {
            foreach (var i in Selection.gameObjects)
            {
                i.transform.eulerAngles += new Vector3(0, 0, 90);
            }
        }
        GUILayout.EndHorizontal();

    }


    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}