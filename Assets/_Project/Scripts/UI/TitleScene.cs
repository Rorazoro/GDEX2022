using _Project.Scripts.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class TitleScene : MonoBehaviour
    {
        public GameObject AudioManagerPrefab;

        private void Start()
        {
            if (FindObjectOfType<AudioManager>() == null)
            {
                Instantiate(AudioManagerPrefab);
            }
        }

        public void BtnSingleplayer_OnClick()
        {
            SceneManager.LoadScene("Level1Scene");
        }

        public void BtnMultiplayer_OnClick()
        {
            SceneManager.LoadScene("ConnectToServerScene");
        }

        public void BtnCredits_OnClick()
        {
            SceneManager.LoadScene("CreditsScene");
        }

        public void BtnQuit_OnClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}