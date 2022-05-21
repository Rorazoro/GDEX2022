using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class PlayerSpawner : MonoBehaviour
    {
        public GameObject PlayerPrefab;

        public float MinX;
        public float MaxX;
        public float MinZ;
        public float MaxZ;

        private void Start()
        {
            Vector2 randomPosition = new Vector3(Random.Range(MinX, MaxX), 0f, Random.Range(MinZ, MaxZ));
            PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
        }
    }
}