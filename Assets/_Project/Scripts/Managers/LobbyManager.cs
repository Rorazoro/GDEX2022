using System.Collections.Generic;
using _Project.Scripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public GameObject[] UIPanels;

    [SerializeField] private GameObject HostListContent;
    [SerializeField] private GameObject RoomPanelPrefab;
    [SerializeField] private Button BtnJoinRoom;
    [SerializeField] private string SelectedRoomName;

    public void BtnNameEnter_OnClick(TMP_InputField txtName)
    {
        NetworkManager.Instance.SetPlayerName(txtName.text);
        NetworkManager.Instance.JoinDefaultLobby();
        SwitchUIPanels(1);
    }

    public void BtnReturn_OnClick()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void BtnHost_OnClick()
    {
        SwitchUIPanels(2);
    }
    
    public void BtnJoin_OnClick()
    {
        SwitchUIPanels(3);
        PopulateHostList();
    }
    
    public void BtnGoBack_OnClick(int uiPanelIndex)
    {
        SwitchUIPanels(uiPanelIndex);
    }

    public void BtnHostEnter_OnClick(TMP_InputField txtHostName)
    {
        NetworkManager.Instance.CreateHost(txtHostName.text, "");
    }
    
    public void BtnJoinRoom_OnClick()
    {
        NetworkManager.Instance.JoinHost(SelectedRoomName);
    }

    public void SetSelectedRoomName(string roomName)
    {
        SelectedRoomName = roomName;
        if (!BtnJoinRoom.interactable)
        {
            BtnJoinRoom.interactable = true;
        }
    }
    
    private void SwitchUIPanels(int uiPanelIndex)
    {
        foreach (var uiPanel in UIPanels)
        {
            uiPanel.SetActive(uiPanel == UIPanels[uiPanelIndex]);
        }
    }

    private void PopulateHostList()
    {
        Dictionary<string, RoomInfo> roomList = NetworkManager.Instance.GetCachedRoomList();
        foreach (var room in roomList)
        {
            GameObject roomPanel = Instantiate(RoomPanelPrefab, HostListContent.transform);
            roomPanel.GetComponent<RoomPanel>().SetRoomInfo(room);
        }
        
    }
}
