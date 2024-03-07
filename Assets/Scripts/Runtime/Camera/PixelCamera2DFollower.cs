using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace FS
{

    public class PixelCamera2DFollower : MonoBehaviour
    {
        [SerializeField]
        private Bounds _targetBound;

        private Bounds _cameraBound = new Bounds();
        [SerializeField]
        private float smoothTime;

        [SerializeField] private PixelPerfectCamera _pixelCamera;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 position;

        public Vector3 offset;
        private Vector3 _shakeCameraPosition;

        [SerializeField] private Camera _camera2D;

        private Vector3 velocityPosition;

        public void ResetPosition()
        {
            UpdateToTargetPosition(new Vector3(), true);
        }

        public void SetTargetBound(Bounds bound)
        {
            this._targetBound = bound;
        }

        public void FollowTarget(Transform target)
        {
            this.target = target;
        }

        private void Update()
        {
            if (target == null) return;

            //CalculateFrustum();
            //UpdateBound();

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (target != null)
                position = target.position;
            else
                position = transform.position;

            UpdateToTargetPosition(position);
        }

        public void UpdateToTargetPosition(Vector3 position, bool isForceSet = false)
        {
            //position.x = Mathf.Clamp(position.x, this._cameraBound.min.x * 0.5f, this._cameraBound.max.x * 0.5f);
            //position.z = Mathf.Clamp(position.z, this._cameraBound.min.z * 0.5f, this._cameraBound.max.z * 0.5f);
            //position += offset;

            Vector3 resultPos = isForceSet ? position : Vector3.Lerp(this.transform.position, position, smoothTime * Time.deltaTime);

            Vector3 calculatePos = this._pixelCamera.RoundToPixel(resultPos + _shakeCameraPosition);
            this.transform.position = calculatePos;

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(this._cameraBound.center, this._cameraBound.extents);
        }
#endif
    }
}