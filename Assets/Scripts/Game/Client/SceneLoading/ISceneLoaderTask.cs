namespace Game.Client.SceneLoading
{
    public interface ISceneLoaderTask
    {
        /// <summary>
        /// Starts up the loader task. Make sure that you provide required variables through the constructor
        ///
        /// If you can, hook events here and have those set a boolean for test.
        /// </summary>
        void Start();
        
        
        /// <summary>
        /// Is finished or not
        /// </summary>
        /// <returns></returns>
        bool IsFinished();
        
        /// <summary>
        /// Called when all tasks are finished.
        /// </summary>
        void AllFinished();

        /// <summary>
        /// Get loading text to display
        /// </summary>
        /// <returns></returns>
        string GetLoadingText();


        /// <summary>
        /// Gets the progress of loading
        /// </summary>
        /// <returns>0.0f to 1.0f</returns>
        float GetProgressPercentage();

        /// <summary>
        /// The weight this bar holds on the total progress bar
        /// </summary>
        /// <returns></returns>
        int GetProgressWeight();

        /// <summary>
        /// Clean up the scene loader task in case scene loading got canceled
        /// Remove any events made.
        /// </summary>
        void CleanUp();
    }
}
