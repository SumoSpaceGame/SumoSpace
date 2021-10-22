using System;
using Game.Common.ScriptableData;
using UnityEngine;

namespace Game.Common.UI
{
    [ExecuteInEditMode]
    public class LoadingBarUI : MonoBehaviour, ILoadingUI
    {

        public RectTransform ProgressBarTopPanel;
        public bool IsVertical = false;

        public float currentValue, maxValue = 10;

        public FloatScriptableData progressData;
        
        private bool _validate = false;


        private void Start()
        {
            if(progressData != null) progressData.OnChangeEvent += SetProgress;
        }

        public void OnValidate()
        {
            _validate = true;
        }

        public void Update()
        {
            if (_validate)
            {
                UpdateBar();
                _validate = false;
            }
        }

        private void OnDestroy()
        {
            if(progressData != null) progressData.OnChangeEvent -= SetProgress;
        }


        public void UpdateBar() {


            Vector2 anchor = ProgressBarTopPanel.anchorMax;
            if (IsVertical) {
                anchor.y = Mathf.Clamp((float)currentValue/ maxValue, 0.0f, 1.0f);
            } else {
                anchor.x = Mathf.Clamp((float)currentValue/ maxValue, 0.0f, 1.0f);
            }
            ProgressBarTopPanel.anchorMax = anchor;
            ProgressBarTopPanel.sizeDelta = Vector2.zero;
        }
        
        
        public void SetActive(bool active)
        {
            this.enabled = active;
        }
        
        public void SetProgress(float progress)
        {
            maxValue = 1.0f;
            currentValue = progress;
            UpdateBar();
        }
    }
}
