using System.Collections.Generic;
using UnityEngine;

namespace MonopolyGo
{
    public class ParticlePool
    {
        private readonly ParticleSystem m_Prefab;
        private readonly Transform m_Parent;
        private readonly Stack<ParticleSystem> m_Free = new Stack<ParticleSystem>();

        public ParticlePool(ParticleSystem prefab, Transform parent, int initialSize)
        {
            m_Prefab = prefab;
            m_Parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                m_Free.Push(Create());
            }
        }

        public ParticleSystem Get()
        {
            ParticleSystem particles = m_Free.Count > 0 ? m_Free.Pop() : Create();
            particles.gameObject.SetActive(true);
            return particles;
        }

        public void Release(ParticleSystem particles)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particles.gameObject.SetActive(false);
            m_Free.Push(particles);
        }

        private ParticleSystem Create()
        {
            ParticleSystem particles = Object.Instantiate(m_Prefab, m_Parent);
            particles.gameObject.SetActive(false);
            return particles;
        }
    }
}
