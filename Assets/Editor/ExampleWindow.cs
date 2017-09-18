using UnityEngine;
using UnityEditor;
using System;

public class ExampleWindow : EditorWindow
{
    bool FrameSelected = true;
    private string biologyName;
    private BiologysMenu BiologysMenu;
    public string stringToEdit = "";
    private Vector2 scrollPos;
    bool BiologyNUMisRight = false;

    [MenuItem("Window/自製編輯器/生物清單")]
    public static void ShowWindow()
    {
        ExampleWindow window = GetWindow<ExampleWindow>();
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Biologys.png");
        GUIContent titleContent = new GUIContent(" 生物清單", icon);
        window.titleContent = titleContent;

    }

    void OnGUI()
    {
        UpdateBiologysList();
        DrawBiologysList();
        DrawAddBiologyLayout();

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
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
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
        if (BiologysMenu.Biologys.Length == 0) return;
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

}