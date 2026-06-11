using System;
using UnityEngine;

namespace MonopolyGo
{
    public static class GameEvents
    {
        public static event Action<int> DiceRollCompleted;
        public static event Action<Vector3, bool> PlayerLanded;
        public static event Action<ItemType, int> RewardCollected;

        public static void RaiseDiceRollCompleted(int sum)
        {
            DiceRollCompleted?.Invoke(sum);
        }

        public static void RaisePlayerLanded(Vector3 position, bool hasReward)
        {
            PlayerLanded?.Invoke(position, hasReward);
        }

        public static void RaiseRewardCollected(ItemType item, int amount)
        {
            RewardCollected?.Invoke(item, amount);
        }
    }
}
