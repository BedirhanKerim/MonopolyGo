using UnityEngine;

namespace MonopolyGo
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private Board m_Board;
        [SerializeField] private DiceRoller m_DiceRoller;
        [SerializeField] private DiceInputUI m_DiceInputUI;
        [SerializeField] private Transform m_LandingAnchor;
        [SerializeField] private PlayerMover m_PlayerMover;
        [SerializeField] private CameraFollow m_CameraFollow;
        [SerializeField] private InventoryController m_InventoryController;
        [SerializeField] private InventoryUIController m_InventoryUI;

        private void Awake()
        {
            m_Board.Generate();

            m_DiceRoller.Init(m_LandingAnchor);
            m_DiceInputUI.Init(m_DiceRoller);

            m_PlayerMover.Init(m_Board);
            m_CameraFollow.Init(m_PlayerMover.transform);

            m_InventoryController.Init();
            m_InventoryUI.Init(m_InventoryController.Inventory);
        }
    }
}
