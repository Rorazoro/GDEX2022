using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text txtRoomName;
    [SerializeField] private TMP_Text txtPlayerCount;

    private KeyValuePair<string, RoomInfo> RoomPanelInfo;
    
    public void SetRoomInfo(KeyValuePair<string, RoomInfo> info)
    {
        RoomPanelInfo = info;
        txtRoomName.text = info.Value.Name;
        txtPlayerCount.text = $"{info.Value.PlayerCount} / {info.Value.MaxPlayers} Players";
    }

    public void RoomPanel_OnClick()
    {
        LobbyManager.Instance.SetSelectedRoomName(RoomPanelInfo.Key);
    }
}
