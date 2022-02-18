using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client.SceneLoading.SceneLoaderTasks
{
    public class LoadSceneTask : ISceneLoaderTask
    {
        private string loadSceneName;
        private AsyncOperation sceneloadingOperation;
        
        public LoadSceneTask(string sceneName)
        {
            loadSceneName = sceneName;
        }
        

        public void Start()
        {
            sceneloadingOperation = SceneManager.LoadSceneAsync(loadSceneName);
            sceneloadingOperation.allowSceneActivation = false;
        }

        public bool Test()
        {
            return sceneloadingOperation.isDone;
        }

        public void AllFinished()
        {
            sceneloadingOperation.allowSceneActivation = true;
        }

        public string GetLoadingText()
        {
            return "Loading map..";
        }

        public float GetProgressPercentage()
        {
            if (sceneloadingOperation != null)
            {
                return sceneloadingOperation.progress / 0.9f;
            }

            return 0.0f;
        }

        public int GetProgressWeight()
        {
            return 100;
        }

        public void CleanUp()
        {
            // TODO: Scene loading may be canceled, check if there is garbage to collect, if so you have to unload the scene async.
            sceneloadingOperation = null;
            loadSceneName = null;
        }
    }
}