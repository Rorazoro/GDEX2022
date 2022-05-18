using System.Linq;
using _Project.Scripts;
using _Project.Scripts.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviourPun
{
    [SerializeField] private PlayerObject playerObject;
    [SerializeField] private PlayerInputHandler InputHandler;
    [SerializeField] private GameObject InGameUICanvas;
    [SerializeField] private GameObject txtPressWhenReady;
    [SerializeField] private GameObject InGameMenu;
    [SerializeField] private GameObject PlayerList;
    [SerializeField] private GameObject PlayerListItemPrefab;
    [SerializeField] private Color[] PlayerColors;

    private void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        if (SceneManager.GetActiveScene().name == "RoomScene")
        {
            SetupRoomUI();
        }
        else
        {
            SetupGameUI();
        }
        InGameUICanvas.SetActive(true);
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        if (SceneManager.GetActiveScene().name == "RoomScene")
        {
            if (InputHandler.playerActionMap.Ready)
            {
                
            }
            if (InputHandler.playerActionMap.Start)
            {
                
            }
        }
        
        if (InputHandler.playerActionMap.InGameMenu)
        {
            txtPressWhenReady.SetActive(false);
            InGameMenu.SetActive(true);

            playerObject.cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = false;
            InputHandler.SwitchActionMap("ui");
        }

        if (InputHandler.uiActionMap.Cancel)
        {
            InGameMenu.SetActive(false);
            txtPressWhenReady.SetActive(true);
            
            playerObject.cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = true;
            InputHandler.SwitchActionMap("player");
        }
    }

    public void BtnReturn_OnClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
    
    public void BtnQuit_OnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    public void AddPlayerToPlayerList(Player newPlayer)
    {
        GameObject playerListItem = Instantiate(PlayerListItemPrefab, PlayerList.transform);
        PlayerListItem listItem = playerListItem.GetComponent<PlayerListItem>();
        listItem.SetPlayerName(newPlayer.NickName);
        if (newPlayer.ActorNumber - 1 < PlayerColors.Length)
            listItem.SetPlayerColor(PlayerColors[playerObject.PlayerIndex]);
    }

    public void RemovePlayerFromPlayerList(Player otherPlayer)
    {
        PlayerListItem[] players = PlayerList.GetComponentsInChildren<PlayerListItem>();
        PlayerListItem player = players.FirstOrDefault(x => x.PlayerName == otherPlayer.NickName);
        Destroy(player.gameObject);
    }
    
    private void SetupRoomUI()
    {
        txtPressWhenReady.GetComponent<TMP_Text>().text = PhotonNetwork.IsMasterClient ? "Press Enter to Start Game" : "Press Tab When Ready";
        txtPressWhenReady.SetActive(true);
        GetPlayerList();
    }

    private void SetupGameUI()
    {
        
    }

    private void GetPlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            AddPlayerToPlayerList(player);
        }
    }
}
