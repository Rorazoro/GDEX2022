using UnityEngine;

namespace _Project.Scripts
{
    public class DDOL : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(this);
        }
    }
}