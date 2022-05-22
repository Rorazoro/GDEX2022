using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class SpinningBeam : MonoBehaviour
    {
        public float speed;
        private void Update()
        {
            transform.Rotate(Vector3.right * ( speed * Time.deltaTime ));
        }
    }
}