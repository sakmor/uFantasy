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
        if (GUILayout.Button("移除生物")) Selection.activeGameObject.GetComponent<Biology>().DestroyGameObject();
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            Selection.activeGameObject.GetComponent<Biology>().BiologyNum = Biology.BiologyNum;
            Biology.LoadDB();
        }
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
            while (GameDB.Instance.BiologyDB.ContainsKey(Biology.BiologyNum) == false)
            {
                Biology.BiologyNum = (int.Parse(Biology.BiologyNum) + 1).ToString();
                if (Biology.BiologyNum == "99999") { Biology.BiologyNum = "10001"; break; }
            }
        }
        if (GUILayout.Button("下一項"))
        {
            if (Biology.BiologyNum == "10001") { Biology.BiologyNum = "99999"; }
            Biology.BiologyNum = (int.Parse(Biology.BiologyNum) - 1).ToString();
            while (GameDB.Instance.BiologyDB.ContainsKey(Biology.BiologyNum) == false)
            {
                Biology.BiologyNum = (int.Parse(Biology.BiologyNum) - 1).ToString();
                if (Biology.BiologyNum == "10001") break;
            }
        }
        if (GUILayout.Button("複製生物"))
        {
            var dupBio = Instantiate(Selection.activeGameObject).transform;
            dupBio.SetParent(Selection.activeGameObject.transform.parent);
            dupBio.transform.localPosition = Vector3.zero + Vector3.up * 0.5f;
            Selection.activeGameObject = dupBio.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }



        GUILayout.EndHorizontal();





        if (GUILayout.Button("載入資訊"))
        {
            Biology.LoadDB();
        }



    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}