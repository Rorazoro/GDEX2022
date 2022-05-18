using System.Collections.Generic;
using _Project.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : NetworkSingletonBehaviour<NetworkManager>
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject playerCmCamPrefab;
    public Material[] playerMaterials;
    
    [Header("References")]
    public GameObject localPlayer;
    
    [SerializeField] private string gameVersion = "0.1.0";
    [SerializeField] private Vector3 spawnPosition;
    
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    
    private void Start()
    {
        if (PhotonNetwork.IsConnected) return;
        
        // Debug.LogWarning("Photon Not Connected. Creating Dev.");
        // PhotonNetwork.NickName = "Dev";
        Debug.Log("Connecting to Photon Server...");
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server.");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to Default Lobby.");
        cachedRoomList.Clear();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("RoomScene");
        PhotonView pv = PhotonView.Get(localPlayer);
        pv.RPC("AddPlayerToPlayerList", RpcTarget.AllBuffered, PhotonNetwork.NickName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnLeftRoom()
    {
        PhotonView pv = PhotonView.Get(localPlayer);
        pv.RPC("RemovePlayerFromPlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public void JoinDefaultLobby()
    {
        Debug.Log("Joining to Default Lobby...");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void CreateHost(string hostName, string password)
    {
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 4
        };

        PhotonNetwork.CreateRoom(hostName, options);
    }

    public void JoinHost(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    
    public void SetPlayerName(string playerName)
    {
        PhotonNetwork.NickName = playerName;
    }

    public Dictionary<string, RoomInfo> GetCachedRoomList()
    {
        return cachedRoomList;
    }
    
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for(int i=0; i<roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }
    
    private void InstantiateLocalPlayer()
    {
        if (localPlayer == null)
        {
            Debug.Log("Instantiating LocalPlayer");
            Quaternion spawnRotation = Quaternion.identity;

            localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

            PhotonNetwork.LocalPlayer.TagObject = localPlayer;
            foreach (var t in PhotonNetwork.PlayerList)
            {
                GameObject p = t.TagObject as GameObject;
                // if (p != null)
                //     p.BroadcastMessage("NetworkNotice", $"{PhotonNetwork.LocalPlayer.NickName} has joined the game!");
            }
        }
        else
        {
            Debug.LogWarning("Unable to instantiate LocalPlayer");
        }
    }
}
