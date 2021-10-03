using Game.Common.Instances;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.UI
{
    /// <summary>
    /// Controls all parts of persistant UI
    /// </summary>
    public class MasterUIController : MonoBehaviour, IGamePersistantInstance
    {

        public Button ReadyButton;
        public TMP_Text ReadyUpText;

        public Button selectCharacter1;
        public Button selectCharacter2;
        public Button selectCharacter3;
        public Button lockedInButton;
        
        void Start()
        {
            MainPersistantInstances.Add(this);
            DontDestroyOnLoad(this.gameObject);
        }
        
        // TODO: Replace current system with long term solution to create different types of UI
        // This solution should be managed all in this class, but other classes can edit different types of UI
        // It will return an ID that other classes will use to configure the id's UI

        public void ActivateReady()
        {
            ReadyButton.gameObject.SetActive(true);
        }

        public void StopReady()
        {
            ReadyButton.gameObject.SetActive(false);
        }

        public void ActivateLobby()
        {
            selectCharacter1.gameObject.SetActive(true);
            selectCharacter2.gameObject.SetActive(true);
            selectCharacter3.gameObject.SetActive(true);
            lockedInButton.gameObject.SetActive(true);
            lockedInButton.enabled = true;
        }

        public void LockLobby()
        {
            selectCharacter1.gameObject.SetActive(false);
            selectCharacter2.gameObject.SetActive(false);
            selectCharacter3.gameObject.SetActive(false);
            lockedInButton.enabled = false;
        }

        public void StopLobby()
        {
            selectCharacter1.gameObject.SetActive(false);
            selectCharacter2.gameObject.SetActive(false);
            selectCharacter3.gameObject.SetActive(false);
            lockedInButton.gameObject.SetActive(false);
            lockedInButton.enabled = false;
        }
        
    }
}
