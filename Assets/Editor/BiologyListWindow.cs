using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;

public class BiologyListWindow : EditorWindow
{
    bool FrameSelected = true;
    private string biologyName;
    private Biology[] BiologyList;

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

    void Awake()
    {
        UpdateBiologysList();
    }

    void OnHierarchyChange()
    {
        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<Biology>()) Selection.activeGameObject.GetComponent<Biology>().LoadDB();
        UpdateBiologysList();
    }
    void OnGUI()
    {
        // DrawMapEditor();
        DrawBiologysList();
        DrawSelectedBiologyLayout();
        DrawAddBiologyLayout();
    }
    bool MapMode = true, bioMode = false;
    int selGridInt = 0;
    String[] selStrings = { "生物模式", "地圖模式" };
    private void DrawMapEditor()
    {
        GUILayout.Label(new GUIContent(" 地圖編輯", AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/List.png")));
        if (GUILayout.Button("鎖圖層")) { EditorExpand.SetSortingLayerLocked(8, true); }
        GUILayout.BeginVertical("box");
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 2);
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var i = tagManager.targetObjects[0].name;
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        GUILayout.EndVertical();
    }

    private void DrawSelectedBiologyLayout()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Biology>() == null) return;

        GUILayout.Label(new GUIContent(" 選取生物 :" + Selection.activeGameObject.GetComponent<Biology>().name, AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Target.png")));
        GUILayout.BeginHorizontal("box");
        DrawSelectedBiologyLayout_Input = Selection.activeGameObject.GetComponent<Biology>().BiologyNum;
        DrawSelectedBiologyLayout_Input = GUILayout.TextField(DrawSelectedBiologyLayout_Input, 5);
        DrawSelectedBiologyLayout_Input = Regex.Replace(DrawSelectedBiologyLayout_Input, "[^0-9]", "");

        if (GUILayout.Button("上一項"))
        {
            if (DrawSelectedBiologyLayout_Input == "99999") { DrawSelectedBiologyLayout_Input = "10001"; }
            DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) + 1).ToString();
            while (GameDB.Instance.biologyDB.ContainsKey(DrawSelectedBiologyLayout_Input) == false)
            {
                DrawSelectedBiologyLayout_Input = (int.Parse(DrawSelectedBiologyLayout_Input) + 1).ToString();
                if (DrawSelectedBiologyLayout_Input == "99999") { DrawSelectedBiologyLayout_Input = "10001"; break; }
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
        if (GUI.changed)
        {
            Selection.activeGameObject.GetComponent<Biology>().BiologyNum = DrawSelectedBiologyLayout_Input;
            Selection.activeGameObject.GetComponent<Biology>().LoadDB();
        }

        GUILayout.EndHorizontal();
        DrawSelectedBiologyScrollPos = EditorGUILayout.BeginScrollView(DrawSelectedBiologyScrollPos);

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("移除生物"))
        {
            Selection.activeGameObject.GetComponent<Biology>().DestroyGameObject();
        }
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

        GUILayout.EndHorizontal();

    }

    private void DrawBiologysList()
    {
        FrameSelected = EditorGUI.Toggle(new Rect(100, 3, position.width, 10), "追蹤選取", FrameSelected);
        GUILayout.Label(new GUIContent(" 生物清單", AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/List.png")));
        GUILayout.BeginVertical("box");
        DrawBiologysListScrollPos = EditorGUILayout.BeginScrollView(DrawBiologysListScrollPos);
        UpdateBiologysList();
        foreach (var i in BiologyList)
        {
            if (GUILayout.Button(i.name))
            {
                Selection.activeGameObject = i.gameObject;
                DrawSelectedBiologyLayout_Input = i.GetComponent<Biology>().BiologyNum;
                if (FrameSelected) SceneView.lastActiveSceneView.FrameSelected();
            }
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void UpdateBiologysList()
    {
        BiologyList = GameObject.Find("生物清單").GetComponentsInChildren<Biology>();
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

}