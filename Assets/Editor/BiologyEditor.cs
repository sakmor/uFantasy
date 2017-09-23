using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;

[CanEditMultipleObjects]
[CustomEditor(typeof(Biology))]
public class BiologyEditor : Editor
{
    private Biology Biology;
    private Vector2 DrawBiologysListScrollPos, DrawSelectedBiologyScrollPos;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Biology = (Biology)target;

        GUILayout.BeginVertical("box");
        DrawSelectedBiologyLayout();
        if (GUI.changed)
        {
            Biology.LoadDB();
        }
        if (GUILayout.Button("移除生物"))
        {
            Selection.activeGameObject.GetComponent<Biology>().DestroyGameObject();
        }
        GUILayout.EndVertical();
        /*
                if (GUI.changed)
                {
                    Biology.LoadDB();
                }
                if (GUILayout.Button("移除生物"))
                {
                    Selection.activeGameObject = GameObject.Find("生物清單").gameObject;
                    Biology.DestroyGameObject(); //fixme:在unity 5.6 這樣的操作會造成錯誤訊息，如果2017不會的話就把這行備註更正吧
                }

                if (GUILayout.Button("返回清單"))
                {
                    Selection.activeGameObject = GameObject.Find("生物清單").gameObject;
                }
        */

    }


    private void DrawSelectedBiologyLayout()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Biology>() == null) return;

        GUILayout.BeginHorizontal("box");
        Biology.BiologyNum = Selection.activeGameObject.GetComponent<Biology>().BiologyNum;

        if (GUILayout.Button("上一項"))
        {
            if (Biology.BiologyNum == "99999") { Biology.BiologyNum = "10001"; }
            Biology.BiologyNum = (int.Parse(Biology.BiologyNum) + 1).ToString();
            while (GameDB.Instance.biologyDB.ContainsKey(Biology.BiologyNum) == false)
            {
                Biology.BiologyNum = (int.Parse(Biology.BiologyNum) + 1).ToString();
                if (Biology.BiologyNum == "99999") { Biology.BiologyNum = "10001"; break; }
            }
        }
        if (GUILayout.Button("下一項"))
        {
            if (Biology.BiologyNum == "10001") { Biology.BiologyNum = "99999"; }
            Biology.BiologyNum = (int.Parse(Biology.BiologyNum) - 1).ToString();
            while (GameDB.Instance.biologyDB.ContainsKey(Biology.BiologyNum) == false)
            {
                Biology.BiologyNum = (int.Parse(Biology.BiologyNum) - 1).ToString();
                if (Biology.BiologyNum == "10001") break;
            }
        }
        if (GUI.changed)
        {
            Selection.activeGameObject.GetComponent<Biology>().BiologyNum = Biology.BiologyNum;
            Selection.activeGameObject.GetComponent<Biology>().LoadDB();
        }

        GUILayout.EndHorizontal();
        DrawSelectedBiologyScrollPos = EditorGUILayout.BeginScrollView(DrawSelectedBiologyScrollPos);



        if (GUILayout.Button("複製生物"))
        {
            var dupBio = Instantiate(Selection.activeGameObject).transform;
            dupBio.SetParent(Selection.activeGameObject.transform.parent);
            dupBio.transform.localPosition = Vector3.zero + Vector3.up * 0.5f;
            Selection.activeGameObject = dupBio.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }
        if (GUILayout.Button("載入資訊"))
        {
            Biology.LoadDB();
        }
        EditorGUILayout.EndScrollView();

    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}