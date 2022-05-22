using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Managers
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public TMP_InputField RoomInputField;
        public GameObject LobbyPanel;
        public GameObject RoomPanel;
        public TMP_Text RoomName;

        public RoomItem RoomItemPrefab;
        public Transform RoomItemListParent;

        public List<PlayerItem> PlayerItemsList = new();
        public PlayerItem PlayerItemPrefab;
        public Transform PlayerItemsListParent;
        public GameObject PlayButton;

        public float timeBetweenUpdates = 1.5f;
        private float nextUpdateTime;
        private readonly List<RoomItem> roomItemsList = new();

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinLobby();
        }

        private void Update()
        {
            PlayButton.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2);
        }

        public void BtnCreateRoom_OnClick()
        {
            if (RoomInputField.text.Length >= 1)
                PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions
                {
                    MaxPlayers = 4
                });
        }

        public void BtnReturn_OnClick()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("TitleScene");
        }

        public void BtnLeaveRoom_OnClick()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void BtnPlay_OnClick()
        {
            PhotonNetwork.LoadLevel("Level1Scene");
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedRoom()
        {
            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);
            RoomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }

        public override void OnLeftRoom()
        {
            LobbyPanel.SetActive(true);
            RoomPanel.SetActive(false);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            UpdatePlayerList();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            UpdatePlayerList();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (Time.time >= nextUpdateTime)
            {
                UpdateRoomList(roomList);
                nextUpdateTime = Time.time + timeBetweenUpdates;
            }
        }

        private void UpdateRoomList(List<RoomInfo> roomList)
        {
            foreach (var item in roomItemsList) Destroy(item.gameObject);
            roomItemsList.Clear();

            foreach (var room in roomList)
            {
                if (room.PlayerCount <= 0) continue;
                var newRoom = Instantiate(RoomItemPrefab, RoomItemListParent);
                newRoom.SetRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);
                roomItemsList.Add(newRoom);
            }
        }

        private void UpdatePlayerList()
        {
            foreach (var item in PlayerItemsList) Destroy(item.gameObject);
            PlayerItemsList.Clear();

            if (PhotonNetwork.CurrentRoom == null) return;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key))
            {
                var index = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList().IndexOf(player);
                Debug.Log($"{index} - {player.Key} - {player.Value}");
                var newPlayerItem = Instantiate(PlayerItemPrefab, PlayerItemsListParent);
                newPlayerItem.SetPlayerInfo(index, player.Value);
                PlayerItemsList.Add(newPlayerItem);
            }
        }
    }
}