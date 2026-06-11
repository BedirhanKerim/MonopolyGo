using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonopolyGo
{
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField] private GameObject m_DicePrefab;
        [SerializeField] private int m_PoolSize = 2;
        [SerializeField] private float m_MinRadius = 0.6f;
        [SerializeField] private float m_ScatterRadius = 1.5f;
        [SerializeField] private float m_AngleJitter = 20f;
        [SerializeField] private int m_MinRolls = 2;
        [SerializeField] private int m_MaxRolls = 4;

        private readonly List<Dice> m_ActiveDice = new List<Dice>();
        private DicePool m_Pool;
        private Transform m_LandingAnchor;

        public bool IsRolling { get; private set; }

        public void Init(Transform landingAnchor)
        {
            m_LandingAnchor = landingAnchor;
            m_Pool = new DicePool(m_DicePrefab.GetComponent<Dice>(), transform, m_PoolSize);
        }

        public void Roll(IReadOnlyList<int> values)
        {
            if (IsRolling)
            {
                return;
            }

            StartCoroutine(RollRoutine(values));
        }

        private IEnumerator RollRoutine(IReadOnlyList<int> values)
        {
            IsRolling = true;
            ReleaseActiveDice();

            int count = values.Count;
            float baseAngle = Random.Range(0f, 360f);
            var routines = new List<Coroutine>(count);
            for (int i = 0; i < count; i++)
            {
                Dice dice = m_Pool.Get();
                m_ActiveDice.Add(dice);

                // Give each die its own angular sector around the anchor and roll it
                // inward. Paths stay in separate sectors, so dice never cross or
                // overlap, yet each arrives from a different direction.
                float angle = baseAngle + i * (360f / count) + Random.Range(-m_AngleJitter, m_AngleJitter);
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
                Vector3 restCenter = m_LandingAnchor.position + dir * Random.Range(m_MinRadius, m_ScatterRadius);
                Vector3 travelDir = -dir;

                int rolls = Random.Range(m_MinRolls, m_MaxRolls + 1);
                routines.Add(StartCoroutine(dice.RollTo(values[i], restCenter, travelDir, rolls)));
            }

            foreach (Coroutine routine in routines)
            {
                yield return routine;
            }

            int sum = 0;
            foreach (int value in values)
            {
                sum += value;
            }

            GameEvents.RaiseDiceRollCompleted(sum);

            IsRolling = false;
        }

        private void ReleaseActiveDice()
        {
            foreach (Dice dice in m_ActiveDice)
            {
                m_Pool.Release(dice);
            }

            m_ActiveDice.Clear();
        }
    }
}
