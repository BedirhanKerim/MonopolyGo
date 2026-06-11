using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonopolyGo
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_NumberLabel;
        [SerializeField] private Image m_RewardIcon;
        [SerializeField] private TMP_Text m_AmountLabel;

        public int Number { get; private set; }
        public ItemType Item { get; private set; }
        public int Amount { get; private set; }

        public bool HasReward => Item != ItemType.None && Amount > 0;

        public void Configure(int number, ItemType item, int amount, ItemDatabase database)
        {
            Number = number;
            Item = item;
            Amount = amount;

            m_NumberLabel.text = number.ToString();

            if (HasReward)
            {
                m_RewardIcon.sprite = database.GetIcon(item);
                m_RewardIcon.enabled = true;
                m_AmountLabel.text = amount.ToString();
                m_AmountLabel.enabled = true;
            }
            else
            {
                m_RewardIcon.enabled = false;
                m_AmountLabel.enabled = false;
            }
        }
    }
}
