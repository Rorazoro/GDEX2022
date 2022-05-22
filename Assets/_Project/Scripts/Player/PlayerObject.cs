using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Player
{
    public class PlayerObject : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInputHandler InputHandler;
        [SerializeField] private Transform followTarget;

        [Header("Player")] 
        public bool Dead;
        
        [Header("Camera")] 
        public Camera playerCamera;
        public GameObject PlayerCMCamPrefab;
        public CinemachineVirtualCamera PlayerCMVCam;
        public GameObject SpecatorCMCamPrefab;
        public CinemachineFreeLook SpecatorCMVCam;

        [Header("Model")] public GameObject playerModel;

        public GameObject playerModelSurface;
        public Color[] PlayerModelColors;

        [Header("UI")] public GameObject Canvas;
        public GameObject InGameUIPanel;
        public GameObject DeathPanel;

        private void Start()
        {
            SetPlayerModelColor();

            if (!photonView.IsMine) return;

            playerModel.SetActive(false);
            LoadCamera();
            Canvas.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            
            List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
            Debug.Log($"Player Objects: {players.Count}");
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

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

            List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();

            SpecatorCMVCam.Follow = players.First().transform;
            SpecatorCMVCam.LookAt = players.First().transform;
        }
        
        public void TriggerDeath()
        {
            Dead = true;
            playerModel.SetActive(false);

            if (!photonView.IsMine)
                return;

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            
            Destroy(PlayerCMVCam.gameObject);
            LoadSpectatorCamera();
            DeathPanel.SetActive(true);
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
            if (Dead)
                DeathPanel.SetActive(!active);
            
            InGameUIPanel.SetActive(active);
            Cursor.visible = active;
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Confined;
        }
    }
}