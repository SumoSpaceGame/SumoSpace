using UnityEngine;

namespace Game.Common.UI
{
    public interface ILoadingUI
    {
        /// <summary>
        /// Disables or enables the loading ui
        /// </summary>
        /// <param name="active"></param>
        void SetActive(bool active);
        
        
        /// <summary>
        /// Value from 0.0 to 1.0
        /// </summary>
        /// <param name="progress"></param>
        void SetProgress(float progress);
    }
}
