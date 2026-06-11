using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonopolyGo
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private ItemDatabase m_ItemDatabase;
        [SerializeField] private Counter[] m_Counters;

        private Inventory m_Inventory;

        public void Init(Inventory inventory)
        {
            m_Inventory = inventory;
            m_Inventory.Changed += OnInventoryChanged;

            foreach (Counter counter in m_Counters)
            {
                if (counter.Icon != null)
                {
                    counter.Icon.sprite = m_ItemDatabase.GetIcon(counter.Type);
                }

                counter.Label.text = m_Inventory.Get(counter.Type).ToString();
            }
        }

        private void OnDisable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.Changed -= OnInventoryChanged;
            }
        }

        private void OnInventoryChanged(ItemType item, int total)
        {
            foreach (Counter counter in m_Counters)
            {
                if (counter.Type == item)
                {
                    counter.Label.text = total.ToString();
                    return;
                }
            }
        }

        [Serializable]
        private struct Counter
        {
            public ItemType Type;
            public Image Icon;
            public TMP_Text Label;
        }
    }
}
