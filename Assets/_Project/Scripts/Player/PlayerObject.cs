using System.Linq;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class PlayerObject : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInputHandler InputHandler;
        [SerializeField] private Transform followTarget;

        [Header("Camera")] 
        public Camera playerCamera;
        public GameObject PlayerCMCamPrefab;
        public CinemachineVirtualCamera cameraCinemachine;

        [Header("Model")] 
        public GameObject playerModel;
        public GameObject playerModelSurface;
        public Color[] PlayerModelColors;

        [Header("UI")] 
        public GameObject Canvas;
        public GameObject InGameUIPanel;

        private void Start()
        {
            SetPlayerModelColor();

            if (!photonView.IsMine) return;
            
            playerModel.SetActive(false);
            LoadCamera();
            Canvas.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            if (InputHandler.playerActionMap.InGameMenu)
            {
                cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = false;
                InputHandler.SwitchActionMap("ui");
                ToggleInGameMenu(true);
            }

            if (InputHandler.uiActionMap.Cancel)
            {
                cameraCinemachine.GetComponent<CinemachineInputProvider>().enabled = true;
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
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        
        private void LoadCamera()
        {
            playerCamera = Camera.main;
            cameraCinemachine = Instantiate(PlayerCMCamPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<CinemachineVirtualCamera>();
            cameraCinemachine.Follow = followTarget;
        }

        private void SetPlayerModelColor()
        {
            int index = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList()
                .FindIndex(x => x.Value.ActorNumber == photonView.Owner.ActorNumber);

            Debug.Log($"{index} - {photonView.Owner.NickName}");

            Color playerModelColor = PlayerModelColors[index];
            var renderer = playerModelSurface.GetComponent<Renderer>();
            renderer.material.color = playerModelColor;
        }

        private void ToggleInGameMenu(bool active)
        {
            InGameUIPanel.SetActive(active);
            Cursor.visible = active;
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Confined;
        }
    }
}