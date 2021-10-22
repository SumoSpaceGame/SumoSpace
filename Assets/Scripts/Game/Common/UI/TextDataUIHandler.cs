using System;
using Game.Common.ScriptableData;
using TMPro;
using UnityEngine;

namespace Game.Common.UI
{
    public class TextDataUIHandler : MonoBehaviour
    {
        public StringScriptableData textData;
        public TextMeshProUGUI textUI;
        private void Start()
        {
            textData.OnChangeEvent += UpdateText;
        }

        public void UpdateText(string newText)
        {
            textUI.text = newText;
        }

        private void OnDestroy()
        {
            textData.OnChangeEvent -= UpdateText;
        }
    }
}