using UnityEngine;

namespace MonopolyGo
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource m_Source;
        [SerializeField] private AudioClip m_HopClip;
        [SerializeField] private AudioClip m_RewardClip;

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
            AudioClip clip = hasReward ? m_RewardClip : m_HopClip;
            if (clip != null)
            {
                m_Source.PlayOneShot(clip);
            }
        }
    }
}
