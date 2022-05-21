using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void BtnSingleplayer_OnClick()
    {
        //SceneManager.LoadScene("LevelSelectScene");
    }
    
    public void BtnMultiplayer_OnClick()
    {
        SceneManager.LoadScene("ConnectToServerScene");
    }
    
    public void BtnQuit_OnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
