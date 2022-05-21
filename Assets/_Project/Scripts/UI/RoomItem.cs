using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text txtRoomName;
    [SerializeField] private TMP_Text txtPlayerCount;

    private LobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomInfo(string name, int playerCount, int maxPlayers)
    {
        txtRoomName.text = name;
        txtPlayerCount.text = $"{playerCount} / {maxPlayers} Players";
    }

    public void RoomItem_OnClick()
    {
        manager.JoinRoom(txtRoomName.text);
    }
}
