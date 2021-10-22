using System.Collections.Generic;
using Game.Common.Instances;
using Game.Common.ScriptableData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client.SceneLoader
{
    public class SceneLoader : MonoBehaviour, IGamePersistantInstance
    {
        public bool IsLoadingScene { get; private set; }

        public delegate void FinishLoadingSceneEventHandler();
        public event FinishLoadingSceneEventHandler FinishLoadingSceneEvent;

        [SerializeField] private FloatScriptableData floatScriptableData;
        [SerializeField] private StringScriptableData stringScriptableData;

        private List<ISceneLoaderTask> _loadingTasks;
        private int currentTaskIndex = 0;
        private int totalWeight;
        
        public SceneLoader()
        {
            IsLoadingScene = false;
        }

        public void Awake()
        {
            MainPersistantInstances.Add(this);
            DontDestroyOnLoad(this);
        }

        
        /// <summary>
        /// Updates text and progress bar. In fixed update to reduce the amount of times a task is getting called.
        /// </summary>
        private void FixedUpdate()
        {
            var currentTask = _loadingTasks[currentTaskIndex];

            stringScriptableData.value = currentTask.GetLoadingText();
            if (currentTask.Test())
            {
                currentTaskIndex++;

                if (currentTaskIndex >= _loadingTasks.Count)
                {
                    FinishedLoading();
                    return;
                }
                else
                {
                    _loadingTasks[currentTaskIndex].Start();
                }
                
                floatScriptableData.value = 1.0f;
            }
            else
            {
                floatScriptableData.value = currentTask.GetProgressPercentage();
            }
            
        }

        /// <summary>
        /// Load loading scene and activate loading tasks. This starts the loading
        /// </summary>
        /// <param name="loadingTasks"></param>
        /// <param name="loadingSceneName"></param>
        public void Load(List<ISceneLoaderTask> loadingTasks, string loadingSceneName = "MapLoadingScene")
        {
            if (IsLoadingScene)
            {
                Debug.LogError("Tried loading a scene while already loading another scene");
                return;
            }

            if (loadingTasks.Count == 0)
            {
                Debug.LogError("Can not load without any scene loader tasks!");
                return;
            }
            
            IsLoadingScene = true;
            
            _loadingTasks = loadingTasks;
            
            
            SceneManager.sceneLoaded += StartTaskLoading;
            SceneManager.LoadScene(loadingSceneName);
            
        }


        /// <summary>
        /// After the loading scene is loaded, this gets called to start the task loading
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void StartTaskLoading(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= StartTaskLoading;

            foreach (var task in _loadingTasks)
            {
                totalWeight += task.GetProgressWeight();
            }
            
            _loadingTasks[0].Start();
            
        }

        /// <summary>
        /// Called after all scene loading tasks are finished
        /// </summary>
        private void FinishedLoading()
        {
            foreach (var task in _loadingTasks)
            {
                task.AllFinished();
            }
            
            Reset();
        }
        
        /// <summary>
        /// Cancel the current scene loading. If an error occurs or host lost connection, use this.
        /// </summary>
        /// <param name="loadBackScene"></param>
        public void CancelSceneLoading(string loadBackScene)
        {
            Reset();
            SceneManager.LoadScene(loadBackScene);
        }

        /// <summary>
        /// Reset the data stored.
        /// </summary>
        private void Reset()
        {

            foreach (var task in _loadingTasks)
            {
                task?.CleanUp();
            }
            
            currentTaskIndex = 0;
            totalWeight = 0;
            _loadingTasks.Clear();
            SceneManager.sceneLoaded -= StartTaskLoading;
            FinishLoadingSceneEvent = null;
        }
    }
}
