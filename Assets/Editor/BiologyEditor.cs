using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Biology))]
public class BiologyEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Biology Biology = (Biology)target;

        if (GUI.changed)
        {
            Biology.LoadDB();
        }
        if (GUILayout.Button("移除生物"))
        {
            Selection.activeGameObject = GameObject.Find("生物清單").gameObject;
            Biology.DestroyGameObject(); //fixme:在unity 5.6 這樣的操作會造成錯誤訊息，如果2017不會的話就把這行備註更正吧
        }
        if (GUILayout.Button("載入資訊"))
        {
            Biology.LoadDB();
        }
        if (GUILayout.Button("返回清單"))
        {
            Selection.activeGameObject = GameObject.Find("生物清單").gameObject;
        }
    }


}