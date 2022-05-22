using _Project.Scripts.Util;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public AudioSource[] audioSources;

        public void SwitchAudioSource(int index)
        {
            foreach (AudioSource a in audioSources)
            {
                a.enabled = false;
            }
            audioSources[index].enabled = true;
        }

        public void DestroyManager()
        {
            Destroy(this);
        }
    }
}
