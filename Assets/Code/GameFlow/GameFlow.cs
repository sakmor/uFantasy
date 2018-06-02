using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameFlow : MonoBehaviour
{
    public GameObject LoadingMask;
    public Animator Animator;
    public static Animator Controller;

    private void Awake()
    {
        Object.DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Animator.enabled = true;
        Controller = Animator;
    }

    public void Loading(string sceneName)
    {
        StartCoroutine(LoadingSceneRealProgress(sceneName));
    }

    private void LoadingEnd()
    {
        Debug.Log(LoadingMask.GetComponent<Animator>().gameObject.name);
        LoadingMask.GetComponent<Animator>().SetTrigger("IsFinished");
    }

    IEnumerator LoadingSceneRealProgress(string sceneName)
    {
        yield return null;
        AsyncOperation sceneAO = SceneManager.LoadSceneAsync(sceneName);

        // disable scene activation while loading to prevent auto load
        sceneAO.allowSceneActivation = false;

        while (!sceneAO.isDone)
        {
            if (sceneAO.progress >= 0.9f)
            {
                sceneAO.allowSceneActivation = true;
                LoadingEnd();
            }
            Debug.Log(sceneAO.progress);
            yield return null;
        }
    }


}
