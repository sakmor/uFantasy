using UnityEngine;
using UnityEditor;
using System;

public class BiologyListWindow : EditorWindow
{
    bool FrameSelected = true;
    private string biologyName;
    private BiologysMenu BiologysMenu;
    public string stringToEdit = "";
    private Vector2 DrawBiologysListScrollPos, DrawSelectedBiologyScrollPos;
    bool BiologyNUMisRight = false;

    [MenuItem("Window/自製編輯器/生物清單")]

    public static void ShowWindow()
    {
        BiologyListWindow window = GetWindow<BiologyListWindow>();
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Biologys.png");
        GUIContent titleContent = new GUIContent(" 生物清單", icon);
        window.titleContent = titleContent;

    }

    void OnGUI()
    {
        UpdateBiologysList();
        DrawBiologysList();
        DrawSelectedBiologyLayout();
        DrawAddBiologyLayout();
    }

    private void DrawSelectedBiologyLayout()
    {

        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Biology>() == null) return;
        GUILayout.Label(new GUIContent(" 選取生物 :" + Selection.activeGameObject.GetComponent<Biology>().name, AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Target.png")));
        GUILayout.BeginHorizontal("box");
        stringToEdit = GUILayout.TextField(stringToEdit, 5);
        if (GUILayout.Button("恢復編號"))
        {

        }
        if (GUILayout.Button("替換編號"))
        {

        }
        GUILayout.EndHorizontal();
        DrawSelectedBiologyScrollPos = EditorGUILayout.BeginScrollView(DrawSelectedBiologyScrollPos);

        GUILayout.BeginHorizontal("box");
        Texture texture = AssetPreview.GetAssetPreview(Resources.Load("Biology/" + Selection.activeGameObject.GetComponent<Biology>().ModelName, typeof(GameObject))); //fixme:應該顯示生物編號轉圖號結果
        GUILayout.Label(new GUIContent("", texture));
        if (GUILayout.Button("移除生物")) Selection.activeGameObject.GetComponent<Biology>().DestroyGameObject();
        if (GUILayout.Button("複製生物")) Instantiate(Selection.activeGameObject).transform.SetParent(Selection.activeGameObject.transform.parent);
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();


    }

    private bool JumpOutIfNotScene(string v)
    { //fixme:現在這個視窗在StartScene會有問題
        return true;
    }


    private void DrawAddBiologyLayout()
    {
        GameObject newBio = null;
        GUILayout.Label(new GUIContent(" 輸入生物圖號 :" + biologyName, AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Dice.png")));
        GUILayout.BeginHorizontal("box");
        stringToEdit = GUILayout.TextField(stringToEdit, 5);
        if (GUI.changed)
        {
            BiologyNUMisRight = GameDB.Instance.biologyDB.ContainsKey(stringToEdit) && stringToEdit != "";
            if (BiologyNUMisRight == false) { biologyName = ""; return; }
            biologyName = new BiologyBuilder(stringToEdit).Name;
        }

        if (GUILayout.Button("清除"))
        {
            stringToEdit = "";
            BiologyNUMisRight = false;

        }
        EditorGUI.BeginDisabledGroup(BiologyNUMisRight == false);
        if (GUILayout.Button("新增生物"))
        {
            newBio = Instantiate(Resources.Load("Prefab/Biology", typeof(GameObject))) as GameObject;
            newBio.transform.SetParent(GameObject.Find("生物清單").transform);
            newBio.transform.position = Vector3.up * 0.5f;
            newBio.GetComponent<Biology>().BiologyNum = stringToEdit;
            newBio.GetComponent<Biology>().LoadDB();
            Selection.activeGameObject = newBio.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();

        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        if (Selection.gameObjects.Length < 1) return;
        Texture texture = AssetPreview.GetAssetPreview(Resources.Load("Biology/b101", typeof(GameObject))); //fixme:應該顯示生物編號轉圖號結果
        GUILayout.Label(new GUIContent("", texture));
    }

    private void DrawBiologysList()
    {
        DrawBiologysListScrollPos = EditorGUILayout.BeginScrollView(DrawBiologysListScrollPos);
        GUILayout.Label(new GUIContent(" 生物清單", AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/List.png")));
        FrameSelected = EditorGUI.Toggle(new Rect(100, 3, position.width, 10), "追蹤選取", FrameSelected);
        foreach (var i in BiologysMenu.Biologys)
        {
            if (GUILayout.Button(i.name))
            {
                Selection.activeGameObject = i.gameObject;
                stringToEdit = i.GetComponent<Biology>().BiologyNum;
                if (FrameSelected) SceneView.lastActiveSceneView.FrameSelected();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void UpdateBiologysList()
    {
        BiologysMenu = GameObject.Find("生物清單").GetComponent<BiologysMenu>();
        BiologysMenu.UpdateBiologysList();
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

}