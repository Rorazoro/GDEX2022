using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerObject : MonoBehaviourPun
    {
        [Header("Player")] 
        public int PlayerIndex;
        
        [Header("Camera")] 
        public Camera playerCamera;
        public CinemachineVirtualCamera cameraCinemachine;

        [Header("Model")]
        public GameObject playerModel;
        public GameObject playerModelSurface;
        public GameObject playerModelJoints;
        
        [SerializeField]
        private Transform followTarget;

        private void Start()
        {
            PlayerIndex = photonView.OwnerActorNr - 1;
            SetPlayerColor();
            
            if (!photonView.IsMine) return;

            playerModel.SetActive(false);
            LoadCamera();
        }
        
        private void LoadCamera()
        {
            playerCamera = Camera.main;
            cameraCinemachine = Instantiate(GameManager.Instance.playerCmCamPrefab, Vector3.zero, Quaternion.identity).GetComponent<CinemachineVirtualCamera>();
            cameraCinemachine.Follow = followTarget;
        }
        
        private void SetPlayerColor()
        {
            var renderer = playerModelSurface.GetComponent<Renderer>();
            if (PlayerIndex < GameManager.Instance.playerMaterials.Length)
            {
                renderer.material = GameManager.Instance.playerMaterials[PlayerIndex];
            }
        }
    }
}