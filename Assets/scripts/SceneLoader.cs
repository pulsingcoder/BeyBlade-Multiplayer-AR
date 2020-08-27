using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNametoBeLoaded;
    public void LoadScene(string _sceneName)
    {
        sceneNametoBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
        // first load the loading scene

        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        // load actual scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNametoBeLoaded);
        // this stops the scene from displaying while scene is loading
        asyncSceneLoading.allowSceneActivation = false;
        while(!asyncSceneLoading.isDone)
        {
            Debug.Log(asyncSceneLoading.progress);
            if (asyncSceneLoading.progress >= 0.9f)
            {
                asyncSceneLoading.allowSceneActivation = true;
            }
            yield return null;
        }
    }
   
}
