using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviourPun
    {
        [Header("Camera")] 
        public Camera playerCamera;
        public CinemachineVirtualCamera cameraCinemachine;

        
        
        [SerializeField]
        private Transform followTarget;
        [SerializeField]
        private GameObject playerModel;
        
        private void Start()
        {
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
    }
}