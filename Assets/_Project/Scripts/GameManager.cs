using _Project.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject playerCmCamPrefab;
    
    [Header("References")]
    public GameObject localPlayer;

    [SerializeField] Vector3 spawnPosition;
    
    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "0.1.0";
    
    private void Start()
    {
        if (PhotonNetwork.IsConnected) return;
        
        Debug.LogWarning("Photon Not Connected. Creating Dev.");
        PhotonNetwork.NickName = "Dev";
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby.");
        
        PhotonNetwork.JoinOrCreateRoom("Dev_Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: Dev_Room.");
    }

    public override void OnJoinedRoom()
    {
        InstantiateLocalPlayer();
    }
    
    public void InstantiateLocalPlayer()
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

    // public override void OnPlayerEnteredRoom(Player other)
    // {
    //     //Notification.Instance.Display($"{other.NickName} has joined the game!");
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
    //             PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
    //     }
    // }
    //
    // public override void OnPlayerLeftRoom(Player other)
    // {
    //     Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
    //
    //     localPlayer.BroadcastMessage("NetworkNotice", $"{other.NickName} has left the game!");
    //
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
    //             PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
    //     }
    // }
}
