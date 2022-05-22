using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class Eraser : MonoBehaviour
    {
        public float speed;
        public bool IsRunning = true;

        private void Update()
        {
            if (IsRunning)
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player died");
                PlayerObject playerObject = other.GetComponent<PlayerObject>();
                if (!playerObject.Dead)
                    playerObject.TriggerDeath();
            }
            else if (other.CompareTag("DeathZone"))
            {
            }
            else if (other.CompareTag("Finish"))
            {
                IsRunning = false;
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"GameObject Erased - {other.name}");
                Destroy(other.gameObject);
            }
        }
    }
}