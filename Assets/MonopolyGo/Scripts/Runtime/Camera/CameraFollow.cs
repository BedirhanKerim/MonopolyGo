using UnityEngine;

namespace MonopolyGo
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 m_Offset = new Vector3(0f, 6f, -6f);
        [SerializeField] private float m_SmoothTime = 0.2f;

        private Transform m_Target;
        private Vector3 m_Velocity;

        public void Init(Transform target)
        {
            m_Target = target;
            transform.position = target.position + m_Offset;
        }

        private void LateUpdate()
        {
            if (m_Target == null)
            {
                return;
            }

            Vector3 desired = m_Target.position + m_Offset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref m_Velocity, m_SmoothTime);
        }
    }
}
