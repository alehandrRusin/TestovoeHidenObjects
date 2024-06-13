using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class CameraController
    {
        [Inject] private Camera _camera;
        
        public void SetBounds(Bounds bounds)
        {
            var orthographicSize = bounds.size.x / _camera.aspect / 2f;
            _camera.orthographicSize = orthographicSize;
        }
    }
}