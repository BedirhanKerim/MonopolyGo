using UnityEngine;

namespace MonopolyGo
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private DiceRoller m_DiceRoller;
        [SerializeField] private DiceInputUI m_DiceInputUI;
        [SerializeField] private Transform m_LandingAnchor;

        private void Awake()
        {
            m_DiceRoller.Init(m_LandingAnchor);
            m_DiceInputUI.Init(m_DiceRoller);
        }
    }
}
