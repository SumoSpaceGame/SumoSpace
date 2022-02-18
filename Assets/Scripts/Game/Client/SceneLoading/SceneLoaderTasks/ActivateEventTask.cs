namespace Game.Client.SceneLoading.SceneLoaderTasks
{
    public class ActivateEventTask : ISceneLoaderTask
    {

        public string loadingTaskText = "";
        
        public bool finished = false;

        public delegate void OnActivateEventHandler();
        public OnActivateEventHandler OnActivateEvent;

        public ActivateEventTask(string loadingText = "Waiting for event..")
        {
            loadingTaskText = loadingText;
        }
        
        public void FinishActivateEvent()
        {
            finished = true;
        }
        
        public void Start()
        {
            OnActivateEvent?.Invoke();   
        }

        public bool Test()
        {
            return finished;
        }

        public void AllFinished()
        {
            throw new System.NotImplementedException();
        }

        public string GetLoadingText()
        {
            return loadingTaskText;
        }

        public float GetProgressPercentage()
        {
            return finished ? 0 : 1;
        }

        public int GetProgressWeight()
        {
            return 10;
        }

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }
    }
}