using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*!Handles changing and loading scenes with transition effects. Called by Levelmanager on coroutine.
 */
public static class SceneLoader
{
    private static LoadingMB loader = null;
    public static bool IsLoading => loader != null;
    /*!A nested class that allows for accessing of MonoBehavior functions in its outer, static class. Single-case use in the field "loader."
    */
    private class LoadingMB : MonoBehaviour { }
    /*!Static method called from LevelManager.cs that begins the loading event*/
    public static void Load(SceneTransition transition)
    {
        Load(transition.sceneField, transition.style);
    }
    public static void Load(string sceneName, SceneTransitionStyleSO style)
    {
        if (IsLoading) return;

        loader = new GameObject("Loader").AddComponent<LoadingMB>();
        Object.DontDestroyOnLoad(loader.gameObject);

        loader.StartCoroutine(Loading(sceneName, style));
    }
    /*!Loading(SceneTransition transition)
     * Event for handling the loading logic
     */
    private static IEnumerator Loading(string sceneName, SceneTransitionStyleSO style)
    {
        SceneTransitionEffect transitionEffect = null;

        if (style)
        {
            //Instantiates the transition object
            transitionEffect = Object.Instantiate(style.transitionPrefab).GetComponent<SceneTransitionEffect>();
            yield return new WaitForSecondsRealtime(transitionEffect.OutDuration);
        }
        AsyncOperation async;
        async = SceneManager.LoadSceneAsync(sceneName);

        //async.allowSceneActivation = false; 
        //yield return new WaitForSecondsRealtime(0.5f);
        //Debug.Log("allowing scene activation...");
        async.allowSceneActivation = true; //Allows the scene to be activated as soon as it is ready
        //Debug.Log("waiting for scene activation...");
        //Waits until the scene is loaded before starting the transition
        while (!async.isDone)
        {
            yield return null;
        }

        //Begins the transition effect
        //Debug.Log("transitionEffect: " + transitionEffect);
        if (transitionEffect)
        {
            transitionEffect.In();
        }
        Object.Destroy(loader.gameObject);
    }
}
