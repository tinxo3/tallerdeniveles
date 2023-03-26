using EZCameraShake;
using UnityEngine;

namespace EZCameraController
{
    public class CameraController : MonoBehaviour
    {
        #region EXPOSED_FIELD
        [Header("Camera Movement")]
        [SerializeField] private Transform parentHolder = null;
        [SerializeField] private Transform player = null;

        [Range(0f, 1f)]
        [SerializeField] private float smoothSpeed = 0.125f;

        [SerializeField] private Vector3 positionOffset = Vector3.zero;
        [SerializeField] private Vector3 rotationOffset = Vector3.zero;
        [SerializeField] private bool lookAt = false;

        [Header("Camera Shake")]
        [SerializeField] private CameraShaker cameraShaker = null;

        [SerializeField] private float magnitude = 2f;
        [SerializeField] private float roughness = 2f;
        [SerializeField] private float fadeInTime = 0.1f;
        [SerializeField] private float fadeOutTime = 1f;
        #endregion

        #region STATIC_FIELD
        public static CameraController Instance;
        #endregion

        #region PRIVATE_METHODS
        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            Vector3 desiredPosition = player.position + positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(parentHolder.position, desiredPosition, smoothSpeed);

            parentHolder.position = smoothedPosition;

            if (lookAt)
            {
                parentHolder.LookAt(player);
            }
            else
            {
                //parentHolder.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void ShakeOnce()
        {
            cameraShaker.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        }
        #endregion
    }
}