using UnityEngine;

namespace ithappy
{
    public class RotationScript : MonoBehaviour
    {
        private Rigidbody rb;
        public enum RotationAxis
        {
            X,
            Y,
            Z
        }

        public RotationAxis rotationAxis = RotationAxis.Y;
        public float rotationSpeed = 50.0f;

        private void Awake()
        {
            TryGetComponent<Rigidbody>(out rb);
        }

        void Update()
        {
            float rotationValue = rotationSpeed * Time.deltaTime;

            Vector3 axis = Vector3.zero;
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    axis = Vector3.right;
                    break;
                case RotationAxis.Y:
                    axis = Vector3.up;
                    break;
                case RotationAxis.Z:
                    axis = Vector3.forward;
                    break;
            }

            if (rb)
                rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(rotationValue, axis));
            else
                transform.Rotate(axis, rotationValue);
        }
    }
}
