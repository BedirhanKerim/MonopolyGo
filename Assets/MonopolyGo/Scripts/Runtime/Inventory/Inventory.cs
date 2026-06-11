using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonopolyGo
{
    public class Inventory
    {
        private readonly Dictionary<ItemType, int> m_Counts = new Dictionary<ItemType, int>();
        private readonly ISaveStorage m_Storage;

        public event Action<ItemType, int> Changed;

        public Inventory(ISaveStorage storage)
        {
            m_Storage = storage;
        }

        public int Get(ItemType item)
        {
            return m_Counts.TryGetValue(item, out int amount) ? amount : 0;
        }

        public void Add(ItemType item, int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            int total = Get(item) + amount;
            m_Counts[item] = total;
            Changed?.Invoke(item, total);
            Save();
        }

        public void Load()
        {
            if (!m_Storage.HasData())
            {
                return;
            }

            InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(m_Storage.Load());
            if (data == null)
            {
                return;
            }

            foreach (InventoryEntry entry in data.entries)
            {
                m_Counts[entry.item] = entry.amount;
                Changed?.Invoke(entry.item, entry.amount);
            }
        }

        private void Save()
        {
            var data = new InventorySaveData();
            foreach (KeyValuePair<ItemType, int> pair in m_Counts)
            {
                data.entries.Add(new InventoryEntry { item = pair.Key, amount = pair.Value });
            }

            m_Storage.Save(JsonUtility.ToJson(data));
        }
    }
}
