using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class TitleScene : MonoBehaviour
    {
        public void BtnSingleplayer_OnClick()
        {
            PhotonNetwork.OfflineMode = true;
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