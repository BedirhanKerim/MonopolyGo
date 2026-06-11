using System.Collections;
using UnityEngine;

namespace MonopolyGo
{
    public class VFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_HopPrefab;
        [SerializeField] private ParticleSystem m_RewardPrefab;
        [SerializeField] private int m_PoolSize = 4;

        private ParticlePool m_HopPool;
        private ParticlePool m_RewardPool;

        private void Awake()
        {
            m_HopPool = new ParticlePool(m_HopPrefab, transform, m_PoolSize);
            m_RewardPool = new ParticlePool(m_RewardPrefab, transform, m_PoolSize);
        }

        private void OnEnable()
        {
            GameEvents.PlayerLanded += OnPlayerLanded;
        }

        private void OnDisable()
        {
            GameEvents.PlayerLanded -= OnPlayerLanded;
        }

        private void OnPlayerLanded(Vector3 position, bool hasReward)
        {
            ParticlePool pool = hasReward ? m_RewardPool : m_HopPool;
            ParticleSystem burst = pool.Get();
            burst.transform.position = position;
            burst.Play();
            StartCoroutine(ReturnWhenFinished(burst, pool));
        }

        private IEnumerator ReturnWhenFinished(ParticleSystem burst, ParticlePool pool)
        {
            // Let it emit for a frame, then wait out the burst before pooling it back.
            yield return null;
            yield return new WaitWhile(() => burst.IsAlive(true));
            pool.Release(burst);
        }
    }
}
