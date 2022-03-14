using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client.SceneLoading.SceneLoaderTasks
{
    public class LoadSceneTask : ISceneLoaderTask
    {
        private string loadSceneName;
        private AsyncOperation sceneloadingOperation;
        
        private bool _finished = false;
        private Coroutine loadSceneCor = null;

        public delegate Coroutine StartCoroutineEvent(IEnumerator ienumerator);

        public StartCoroutineEvent StartCoroutine;
        
        public LoadSceneTask(string sceneName, StartCoroutineEvent startCoroutine)
        {
            loadSceneName = sceneName;
            StartCoroutine = startCoroutine;
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(1.0f);
            sceneloadingOperation = SceneManager.LoadSceneAsync(loadSceneName);
            sceneloadingOperation.allowSceneActivation = false;
            _finished = false;

            while (!sceneloadingOperation.isDone)
            {
                if (sceneloadingOperation.progress >= 0.9f)
                {
                    _finished = true;
                    break;
                }
                yield return null;
            }
            
        }

        public void Start()
        {
            loadSceneCor = StartCoroutine(LoadScene());
        }

        public bool IsFinished()
        {
            return _finished;
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