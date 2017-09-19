using UnityEngine;
using UnityEditor;
using System;

public class BiologyListWindow : EditorWindow
{
    bool FrameSelected = true;
    private string biologyName;
    private BiologysMenu BiologysMenu;

    private Vector2 DrawBiologysListScrollPos, DrawSelectedBiologyScrollPos;
    bool BiologyNUMisRight = false;

    public string DrawSelectedBiologyLayout_Input { get; private set; }

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
        DrawSelectedBiologyLayout_Input = Selection.activeGameObject.GetComponent<Biology>().BiologyNum;
        DrawSelectedBiologyLayout_Input = GUILayout.TextField(DrawSelectedBiologyLayout_Input, 5);
        if (GUILayout.Button("上一項"))
        {
            if (DrawSelectedBiologyLayout_Input == "99999") { DrawSelectedBiologyLayout_Input = "10001"; }
            DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) + 1).ToString();
            while (GameDB.Instance.biologyDB.ContainsKey(DrawSelectedBiologyLayout_Input) == false)
            {
                DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) + 1).ToString();
                if (DrawSelectedBiologyLayout_Input == "99999") break;
            }
        }
        if (GUILayout.Button("下一項"))
        {
            if (DrawSelectedBiologyLayout_Input == "10001") { DrawSelectedBiologyLayout_Input = "99999"; }
            DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) - 1).ToString();
            while (GameDB.Instance.biologyDB.ContainsKey(DrawSelectedBiologyLayout_Input) == false)
            {
                DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) - 1).ToString();
                if (DrawSelectedBiologyLayout_Input == "10001") break;
            }
        }
        Selection.activeGameObject.GetComponent<Biology>().BiologyNum = DrawSelectedBiologyLayout_Input;
        Selection.activeGameObject.GetComponent<Biology>().LoadDB();



        GUILayout.EndHorizontal();
        DrawSelectedBiologyScrollPos = EditorGUILayout.BeginScrollView(DrawSelectedBiologyScrollPos);

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("移除生物")) Selection.activeGameObject.GetComponent<Biology>().DestroyGameObject();
        if (GUILayout.Button("複製生物"))
        {
            var dupBio = Instantiate(Selection.activeGameObject).transform;
            dupBio.SetParent(Selection.activeGameObject.transform.parent);
            dupBio.transform.localPosition = Vector3.zero + Vector3.up * 0.5f;
            Selection.activeGameObject = dupBio.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }
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



        EditorGUI.BeginDisabledGroup(BiologyNUMisRight == false);
        if (GUILayout.Button("新增空白生物"))
        {
            newBio = Instantiate(Resources.Load("Prefab/Biology", typeof(GameObject))) as GameObject;
            newBio.transform.SetParent(GameObject.Find("生物清單").transform);
            newBio.transform.position = Vector3.up * 0.5f;
            newBio.GetComponent<Biology>().BiologyNum = "99999";
            newBio.GetComponent<Biology>().LoadDB();
            Selection.activeGameObject = newBio.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();

        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

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
                DrawSelectedBiologyLayout_Input = i.GetComponent<Biology>().BiologyNum;
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