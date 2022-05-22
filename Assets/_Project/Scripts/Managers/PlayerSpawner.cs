using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerPrefab;

        public float MinX;
        public float MaxX;
        public float MinZ;
        public float MaxZ;

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                InstantiatePlayerPrefab();
            }
            else
            {
                // PhotonNetwork.NickName = "Dev";
                // Debug.Log("Connecting to Photon Server...");
                // PhotonNetwork.GameVersion = "0.1.0";
                // PhotonNetwork.AutomaticallySyncScene = true;
                // PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.OfflineMode = true;
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Server.");
            // Debug.Log("Joining Lobby...");
            // PhotonNetwork.JoinLobby();
            Debug.Log("Joining Room...");
            PhotonNetwork.CreateRoom("Singleplayer");
        }

        // public override void OnJoinedLobby()
        // {
        //     Debug.Log("Lobby Joined.");
        //     Debug.Log("Joining Room...");
        //     PhotonNetwork.JoinOrCreateRoom("DEVROOM", new RoomOptions
        //     {
        //         MaxPlayers = 4
        //     }, TypedLobby.Default);
        // }

        public override void OnJoinedRoom()
        {
            Debug.Log("Room Joined.");
            InstantiatePlayerPrefab();
        }

        private void InstantiatePlayerPrefab()
        {
            Vector2 randomPosition = new Vector3(Random.Range(MinX, MaxX), 0f, Random.Range(MinZ, MaxZ));
            PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
        }
    }
}