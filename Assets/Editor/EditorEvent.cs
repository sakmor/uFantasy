using UnityEditor;
using UnityEngine;

public static class EditorEvent
{

    [InitializeOnLoadMethod]
    private static void Example()
    {
        Selection.selectionChanged += () =>
        {
            // var n = GameObject.FindObjectsOfType(typeof(Biology));
            // if (Selection.activeObject == null) return;
            // if (Selection.transforms[0].GetComponent<SkinnedMeshRenderer>())
            // {
            //     Selection.objects = n;
            // }

            // if (Selection.gameObjects.Length > 0)
            // {
            //     foreach (var i in Selection.gameObjects)
            //     {

            //         i.transform.position = new Vector3(
            //             Mathf.Round(i.transform.position.x),
            //             Mathf.Round(i.transform.position.y),
            //             Mathf.Round(i.transform.position.z));

            //     }
            // }
            // Selection.activeObject = GameObject.FindObjectOfType(typeof(Biology));
        };
    }
}