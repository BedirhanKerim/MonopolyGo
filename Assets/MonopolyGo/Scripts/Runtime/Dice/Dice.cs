using System.Collections;
using UnityEngine;

namespace MonopolyGo
{
    public class Dice : MonoBehaviour
    {
        private const int k_FaceCount = 6;
        private const float k_LandedAngle = 45f;

        [SerializeField] private Vector3[] m_FaceUpEulers = new Vector3[k_FaceCount];
        [SerializeField] private float m_CubeSize = 1f;
        [SerializeField] private float m_DropHeight = 2f;
        [SerializeField] private float m_DropDuration = 0.25f;
        [SerializeField] private float m_PivotDuration = 0.18f;
        [SerializeField] private float m_ApproachAngle = 60f;

        private void Awake()
        {
            if (m_FaceUpEulers.Length != k_FaceCount)
            {
                Debug.LogError($"{name}: FaceUpEulers must have {k_FaceCount} entries, one per face value.", this);
            }
        }

        public IEnumerator RollTo(int faceValue, Vector3 restCenter, Vector3 travelDir, int quarterRolls)
        {
            Vector3 rollAxis = Vector3.Cross(Vector3.up, travelDir).normalized;

            // Yaw the die so its faces line up with the travel direction; otherwise a
            // diagonal roll tips over a corner instead of a clean face edge.
            Quaternion align = Quaternion.LookRotation(travelDir, Vector3.up);
            Quaternion targetRot = align * FaceUpRotation(faceValue);

            // Rewind the target by the rolls we are about to make, so finishing those
            // rolls leaves the die exactly on the chosen face.
            Quaternion startRot = Quaternion.AngleAxis(-90f * quarterRolls, rollAxis) * targetRot;
            Vector3 rollStart = restCenter - travelDir * (quarterRolls * m_CubeSize);

            yield return Approach(rollStart, startRot, travelDir, rollAxis);
            yield return Roll(rollStart, startRot, travelDir, rollAxis, quarterRolls);

            transform.SetPositionAndRotation(restCenter, targetRot);
        }

        public void ResetForPool()
        {
            transform.rotation = Quaternion.identity;
        }

        private IEnumerator Approach(Vector3 rollStart, Quaternion startRot, Vector3 travelDir, Vector3 rollAxis)
        {
            // The die comes down already half-way through its first quarter-turn (45),
            // landing balanced on its edge instead of flat.
            Quaternion landRot;
            Vector3 landPos = PivotPose(rollStart, startRot, travelDir, rollAxis, 0, k_LandedAngle, out landRot);

            // Fly in along a slanted descent from behind the roll direction, so the die
            // carries forward momentum into the roll rather than dropping straight down.
            float rad = m_ApproachAngle * Mathf.Deg2Rad;
            Vector3 descentDir = (travelDir * Mathf.Cos(rad) - Vector3.up * Mathf.Sin(rad)).normalized;
            Vector3 approachStart = landPos - descentDir * (m_DropHeight / Mathf.Sin(rad));

            float elapsed = 0f;
            while (elapsed < m_DropDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / m_DropDuration);
                float eased = t * t;
                transform.SetPositionAndRotation(
                    Vector3.Lerp(approachStart, landPos, eased),
                    Quaternion.Slerp(startRot, landRot, eased));
                yield return null;
            }

            transform.SetPositionAndRotation(landPos, landRot);
        }

        private IEnumerator Roll(Vector3 rollStart, Quaternion startRot, Vector3 travelDir, Vector3 rollAxis, int quarterRolls)
        {
            // Continue from the half-turn the approach already completed, then decelerate
            // into the final face.
            float startProgress = k_LandedAngle / 90f;
            float remaining = quarterRolls - startProgress;
            float total = m_PivotDuration * remaining;

            float elapsed = 0f;
            while (elapsed < total)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / total);
                float easedOut = 1f - (1f - t) * (1f - t);
                float progress = startProgress + remaining * easedOut;
                ApplyRollPose(rollStart, startRot, travelDir, rollAxis, progress, quarterRolls);
                yield return null;
            }
        }

        private void ApplyRollPose(Vector3 rollStart, Quaternion startRot, Vector3 travelDir, Vector3 rollAxis, float progress, int quarterRolls)
        {
            int completed = Mathf.Min(Mathf.FloorToInt(progress), quarterRolls);
            float localAngle = (progress - completed) * 90f;
            Vector3 pos = PivotPose(rollStart, startRot, travelDir, rollAxis, completed, localAngle, out Quaternion rot);
            transform.SetPositionAndRotation(pos, rot);
        }

        private Vector3 PivotPose(Vector3 rollStart, Quaternion startRot, Vector3 travelDir, Vector3 rollAxis, int completed, float localAngle, out Quaternion rotation)
        {
            float half = m_CubeSize * 0.5f;
            Vector3 pivotCenter = rollStart + travelDir * (completed * m_CubeSize);
            Quaternion pivotRot = Quaternion.AngleAxis(90f * completed, rollAxis) * startRot;
            Vector3 edge = pivotCenter + travelDir * half - Vector3.up * half;

            Quaternion delta = Quaternion.AngleAxis(localAngle, rollAxis);
            rotation = delta * pivotRot;
            return edge + delta * (pivotCenter - edge);
        }

        private Quaternion FaceUpRotation(int faceValue)
        {
            return Quaternion.Euler(m_FaceUpEulers[faceValue - 1]);
        }
    }
}
