using System;

namespace MonopolyGo
{
    public static class GameEvents
    {
        public static event Action<int> DiceRollCompleted;

        public static void RaiseDiceRollCompleted(int sum)
        {
            DiceRollCompleted?.Invoke(sum);
        }
    }
}
