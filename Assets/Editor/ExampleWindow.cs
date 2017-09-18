using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{
    bool FrameSelected = true;
    private string biologyName;
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

        BiologysMenu BiologysMenu = GameObject.Find("生物清單").GetComponent<BiologysMenu>();
        BiologysMenu.UpdateBiologysList();

        if (BiologysMenu.Biologys.Length == 0) return;
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
        //==========================================
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
            GameObject newBio = Instantiate(Resources.Load("Prefab/Biology", typeof(GameObject))) as GameObject;
            newBio.transform.SetParent(GameObject.Find("生物清單").transform);
            newBio.GetComponent<Biology>().BiologyNum = stringToEdit;
            newBio.GetComponent<Biology>().LoadDB();
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

    }
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

}