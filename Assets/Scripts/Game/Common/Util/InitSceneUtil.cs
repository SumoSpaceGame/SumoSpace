using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Common.Util
{
    public class InitSceneUtil : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}