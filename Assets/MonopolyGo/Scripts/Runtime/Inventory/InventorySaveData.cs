using System;
using System.Collections.Generic;

namespace MonopolyGo
{
    [Serializable]
    public class InventorySaveData
    {
        public List<InventoryEntry> entries = new List<InventoryEntry>();
    }

    [Serializable]
    public struct InventoryEntry
    {
        public ItemType item;
        public int amount;
    }
}
