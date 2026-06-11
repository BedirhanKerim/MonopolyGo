using System;
using UnityEngine;

namespace MonopolyGo
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "MonopolyGo/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private Entry[] m_Entries;

        public Sprite GetIcon(ItemType type)
        {
            foreach (Entry entry in m_Entries)
            {
                if (entry.Type == type)
                {
                    return entry.Icon;
                }
            }

            return null;
        }

        [Serializable]
        private struct Entry
        {
            public ItemType Type;
            public Sprite Icon;
        }
    }
}
