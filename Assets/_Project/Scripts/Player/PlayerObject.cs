using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Gameplay;
using _Project.Scripts.Managers;
using _Project.Scripts.UI;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Player
{
    public class PlayerObject : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInputHandler InputHandler;
        [SerializeField] private Transform followTarget;
        private Hashtable playerProperties = new Hashtable();

        [Header("Game")] public int totalPlayers;
        public int playersLeft;
        public bool GameOver;
        public List<GameOverPlayerItem> GameOverPlayerItems = new List<GameOverPlayerItem>();
        public GameOverPlayerItem GameOverPlayerItemPrefab;
        public Transform GameOverPlayerItemsParent;
        public float startTime; 

        [Header("Player")] 
        public bool Dead;
        public bool Finished;

        [Header("Camera")] public Camera playerCamera;
        public GameObject PlayerCMCamPrefab;
        public CinemachineVirtualCamera PlayerCMVCam;
        public GameObject SpecatorCMCamPrefab;
        public CinemachineFreeLook SpecatorCMVCam;

        [Header("Model")] public GameObject playerModel;

        public GameObject playerModelSurface;
        public Color[] PlayerModelColors;

        [Header("UI")] public GameObject Canvas;
        public GameObject InGameUIPanel;
        public GameObject InGameMenuPanel;
        public GameObject DeathPanel;
        public GameObject GameOverPanel;
        public TMP_Text txtTimer;
        public GameObject BtnPlayAgain;
        public GameObject BtnLeaveRoom;

        private void Start()
        {
            SetPlayerModelColor();
            playerProperties["PlayerDead"] = Dead;
            playerProperties["PlayerFinished"] = Finished;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            
            if (!photonView.IsMine) return;

            playerModel.SetActive(false);
            LoadCamera();
            Canvas.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            startTime = Time.time;
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            if (GameOver) return;
                HandleInput();
                UpdateTimer();
                CheckPlayerCount();
        }

        public void BtnPlayerAgain_OnClick()
        {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        }
        
        public void BtnLeaveRoom_OnClick()
        {
            if (PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("TitleScene");
            }
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        
        public void BtnReturnToTitle_OnClick()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("TitleScene");
        }

        public void BtnQuit_OnClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void UpdateTimer()
        {
            if (Dead) return;
            
            var guiTime = Time.time - startTime;

            string timer = FormatTimer(guiTime);
            
            txtTimer.text = timer;
            playerProperties["PlayerTimer"] = guiTime;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        private string FormatTimer(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int fraction = Mathf.FloorToInt(time * 100 % 100);

            return $"{minutes:00}:{seconds:00}:{fraction:00}";
        }

        private void HandleInput()
        {
            if (InputHandler.playerActionMap.InGameMenu)
            {
                if (!Dead)
                {
                    PlayerCMVCam.GetComponent<CinemachineInputProvider>().enabled = false;
                }
                else
                {
                    SpecatorCMVCam.GetComponent<CinemachineInputProvider>().enabled = false;
                }

                InputHandler.SwitchActionMap("ui");
                ToggleInGameMenu(true);
            }

            if (InputHandler.uiActionMap.Cancel)
            {
                if (!Dead)
                {
                    PlayerCMVCam.GetComponent<CinemachineInputProvider>().enabled = true;
                }
                else
                {
                    SpecatorCMVCam.GetComponent<CinemachineInputProvider>().enabled = true;
                }

                InputHandler.SwitchActionMap("player");
                ToggleInGameMenu(false);
            }
        }
        
        private void CheckPlayerCount()
        {
            List<PlayerObject> playerObjectsAlive = FindObjectsOfType<PlayerObject>().Where(x => !x.Dead && !x.Finished).ToList();
            if (playerObjectsAlive.Count <= 0)
            {
                photonView.RPC("GameOverRPC", RpcTarget.All);
            }
        }
        
        private void LoadCamera()
        {
            playerCamera = Camera.main;
            PlayerCMVCam = Instantiate(PlayerCMCamPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<CinemachineVirtualCamera>();
            PlayerCMVCam.Follow = followTarget;
        }

        private void LoadSpectatorCamera()
        {
            SpecatorCMVCam = Instantiate(SpecatorCMCamPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<CinemachineFreeLook>();

            List<GameObject> players = GameObject.FindGameObjectsWithTag("Player")
                .Where(x => !x.gameObject.GetComponent<PlayerObject>().Dead).ToList();

            if (players.Count <= 0) return;
            
            SpecatorCMVCam.Follow = players.First().transform;
            SpecatorCMVCam.LookAt = players.First().transform;
        }

        public void TriggerDeath()
        {
            Dead = true;
            playerProperties["PlayerDead"] = Dead;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            playerModel.SetActive(false);

            if (!photonView.IsMine)
                return;

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<CharacterController>().enabled = false;

            Destroy(PlayerCMVCam.gameObject);
            LoadSpectatorCamera();
            DeathPanel.SetActive(true);
        }

        [PunRPC]
        public void GameOverRPC()
        {
            GameOver = true;
            FindObjectOfType<Eraser>().IsRunning = false;
            
            Photon.Realtime.Player winner = CheckWinner();
            CreateGameOverList(winner);
            
            if (!photonView.IsMine)
                return;

            DeathPanel.SetActive(false);
            InGameUIPanel.SetActive(false);
            GameOverPanel.SetActive(true);
            
            if (!Dead)
            {
                PlayerCMVCam.GetComponent<CinemachineInputProvider>().enabled = false;
            }
            else
            {
                SpecatorCMVCam.GetComponent<CinemachineInputProvider>().enabled = false;
            }
            
            InputHandler.SwitchActionMap("ui");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            AudioManager.Instance.SwitchAudioSource(2);
            
            if (PhotonNetwork.IsMasterClient)
                BtnPlayAgain.SetActive(true);

            if (PhotonNetwork.OfflineMode)
                BtnLeaveRoom.GetComponentInChildren<TMP_Text>().text = "Return to Title";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                Finished = true;
                playerProperties["PlayerFinished"] = Finished;
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            }
        }

        private Photon.Realtime.Player CheckWinner()
        {
            float lowestTimer = 0f;
            Photon.Realtime.Player Winner = null;
            foreach (var player in PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key))
            {
                float timer = (float)player.Value.CustomProperties["PlayerTimer"];
                bool dead = (bool)player.Value.CustomProperties["PlayerDead"];
                bool finished = (bool)player.Value.CustomProperties["PlayerFinished"];

                if (!dead && finished && (timer < lowestTimer || lowestTimer == 0f))
                {
                    lowestTimer = timer;
                    Winner = player.Value;
                }
            }

            return Winner;
        }
        
        private void CreateGameOverList(Photon.Realtime.Player winner)
        {
            foreach (var item in GameOverPlayerItems) Destroy(item.gameObject);
            GameOverPlayerItems.Clear();
            
            if (PhotonNetwork.CurrentRoom == null) return;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key))
            {
                var index = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList().IndexOf(player);
                Debug.Log($"{index} - {player.Key} - {player.Value}");
                var newPlayerItem = Instantiate(GameOverPlayerItemPrefab, GameOverPlayerItemsParent);
                newPlayerItem.SetPlayerInfo(index, player.Value);
                if (winner == player.Value)
                    newPlayerItem.SetWinner();
                
                GameOverPlayerItems.Add(newPlayerItem);
            }
        }
        
        private void SetPlayerModelColor()
        {
            var index = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList()
                .FindIndex(x => x.Value.ActorNumber == photonView.Owner.ActorNumber);

            Debug.Log($"{index} - {photonView.Owner.NickName}");

            var playerModelColor = PlayerModelColors[index];
            var renderer = playerModelSurface.GetComponent<Renderer>();
            renderer.material.color = playerModelColor;
        }

        private void ToggleInGameMenu(bool active)
        {
            // if (Dead && !GameOver)
            // {
            //     DeathPanel.SetActive(!active);
            // }
            // else if (!Dead && !GameOver)
            // {
            //     InGameUIPanel.SetActive(!active);
            // }
            
            InGameMenuPanel.SetActive(active);
            Cursor.visible = active;
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}