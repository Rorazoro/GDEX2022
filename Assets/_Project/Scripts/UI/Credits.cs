using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class Credits : MonoBehaviour
    {
        public void BtnGoBack_OnClick()
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}