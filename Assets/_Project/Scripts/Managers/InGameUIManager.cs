using System.Linq;
using _Project.Scripts;
using _Project.Scripts.UI;
using Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviourPun
{
    [SerializeField] private PlayerObject player;
    [SerializeField] private PlayerInputHandler InputHandler;
    [SerializeField] private GameObject txtPressWhenReady;
    [SerializeField] private GameObject InGameMenu;
    [SerializeField] private GameObject PlayerList;
    [SerializeField] private GameObject PlayerListItemPrefab;
    [SerializeField] private Color[] PlayerColors;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "RoomScene")
        {
            SetupRoomUI();
        }
        else
        {
            SetupGameUI();
        }
    }

    private void Update()
    {
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

            player.cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = false;
            InputHandler.SwitchActionMap("ui");
        }

        if (InputHandler.uiActionMap.Cancel)
        {
            InGameMenu.SetActive(false);
            txtPressWhenReady.SetActive(true);
            
            player.cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = true;
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

    [PunRPC]
    public void AddPlayerToPlayerList(string playerName)
    {
        GameObject playerListItem = Instantiate(PlayerListItemPrefab, PlayerList.transform);
        PlayerListItem player = playerListItem.GetComponent<PlayerListItem>();
        player.SetPlayerName(playerName);
        player.SetPlayerColor(PlayerColors[photonView.OwnerActorNr - 1]);
    }

    [PunRPC]
    public void RemovePlayerFromPlayerList(string playerName)
    {
        PlayerListItem[] players = PlayerList.GetComponentsInChildren<PlayerListItem>();
        PlayerListItem player = players.FirstOrDefault(x => x.PlayerName == playerName);
        Destroy(player.gameObject);
    }
    
    private void SetupRoomUI()
    {
        txtPressWhenReady.GetComponent<TMP_Text>().text = PhotonNetwork.IsMasterClient ? "Press Enter to Start Game" : "Press Tab When Ready";
        txtPressWhenReady.SetActive(true);
    }

    private void SetupGameUI()
    {
        
    }
}
