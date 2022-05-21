using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public TMP_Text buttonText;
    private string gameVersion = "0.1.0";
    
    public void Connect_OnClick()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            Debug.Log("Connecting to Photon Server...");
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void BtnReturn_OnClick()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("TitleScene");
    }
    
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}