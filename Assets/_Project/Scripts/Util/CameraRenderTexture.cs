using UnityEngine;

namespace _Project.Scripts.Util
{
    [ExecuteInEditMode]
    public class CameraRenderTexture : MonoBehaviour
    {
        public Material Mat;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, Mat);
        }
    }
}