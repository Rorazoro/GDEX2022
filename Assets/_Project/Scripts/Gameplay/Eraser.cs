using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class Eraser : MonoBehaviour
    {
        public float speed;
        
        private void Update()
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"GameObject Erased - {collision.gameObject.name}");
            Destroy(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"GameObject Erased - {other.name}");
            Destroy(other.gameObject);
        }
    }
}