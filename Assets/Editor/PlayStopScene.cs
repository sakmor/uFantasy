using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class PlayStopScene
{
	[MenuItem("編輯工具/執行遊戲 %`")]
	private static void PlayFromPrelaunchScene()
	{
		if (EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = false;
			EditorWindow.GetWindow<OpenPreviousScene>();
		}
		else
		{
			string openScene = SceneUtility.GetScenePathByBuildIndex(0);
			Scene activeScene = EditorSceneManager.GetActiveScene();
			if (activeScene.IsValid() && (activeScene.path != openScene))
			{
				PlayerPrefs.SetString("PreviousScene", activeScene.path);
			}
			else
			{
				PlayerPrefs.SetString("PreviousScene", string.Empty);
			}
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene(openScene);
			EditorApplication.isPlaying = true;
		}
	}
	[ExecuteInEditMode]
	public class OpenPreviousScene : EditorWindow
	{
		private float _time;

		private void OnEnable()
		{
			_time = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			if (_time - Time.realtimeSinceStartup > 1.0f)
			{
				string openScene = PlayerPrefs.GetString("PreviousScene");
				if (!string.IsNullOrEmpty(openScene))
				{
					Scene activeScene = EditorSceneManager.GetActiveScene();
					if (activeScene.IsValid() && (activeScene.path != openScene))
					{
						EditorSceneManager.OpenScene(openScene);
					}
				}
				Close();
			}
		}
	}

}

