using UnityEngine;

namespace MonopolyGo
{
    public class InventoryController : MonoBehaviour
    {
        private const string k_SaveFile = "Inventory.json";

        public Inventory Inventory { get; private set; }

        public void Init()
        {
            Inventory = new Inventory(new JsonSaveStorage(k_SaveFile));
            Inventory.Load();
        }

        private void OnEnable()
        {
            GameEvents.RewardCollected += OnRewardCollected;
        }

        private void OnDisable()
        {
            GameEvents.RewardCollected -= OnRewardCollected;
        }

        private void OnRewardCollected(ItemType item, int amount)
        {
            Inventory?.Add(item, amount);
        }
    }
}
