using System.Collections.Generic;
using UnityEngine;

namespace MonopolyGo
{
    public class DicePool
    {
        private readonly Dice m_Prefab;
        private readonly Transform m_Parent;
        private readonly Stack<Dice> m_Free = new Stack<Dice>();

        public DicePool(Dice prefab, Transform parent, int initialSize)
        {
            m_Prefab = prefab;
            m_Parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                m_Free.Push(CreateDice());
            }
        }

        public Dice Get()
        {
            Dice dice = m_Free.Count > 0 ? m_Free.Pop() : GrowOne();
            dice.gameObject.SetActive(true);
            return dice;
        }

        public void Release(Dice dice)
        {
            dice.ResetForPool();
            dice.gameObject.SetActive(false);
            m_Free.Push(dice);
        }

        private Dice GrowOne()
        {
            Debug.Log("DicePool grew beyond its initial size.");
            return CreateDice();
        }

        private Dice CreateDice()
        {
            Dice dice = Object.Instantiate(m_Prefab, m_Parent);
            dice.gameObject.SetActive(false);
            return dice;
        }
    }
}
