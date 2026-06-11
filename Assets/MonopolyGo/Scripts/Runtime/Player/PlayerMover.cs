using System.Collections;
using UnityEngine;

namespace MonopolyGo
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float m_HopDuration = 0.25f;
        [SerializeField] private float m_HopHeight = 0.6f;
        [SerializeField] private Vector3 m_TileOffset = Vector3.zero;

        private Board m_Board;
        private int m_CurrentIndex;

        public bool IsMoving { get; private set; }

        public void Init(Board board)
        {
            m_Board = board;
            m_CurrentIndex = 0;
            transform.position = TilePosition(0);
        }

        private void OnEnable()
        {
            GameEvents.DiceRollCompleted += OnDiceRollCompleted;
        }

        private void OnDisable()
        {
            GameEvents.DiceRollCompleted -= OnDiceRollCompleted;
        }

        private void OnDiceRollCompleted(int steps)
        {
            if (IsMoving)
            {
                return;
            }

            StartCoroutine(MoveRoutine(steps));
        }

        private IEnumerator MoveRoutine(int steps)
        {
            IsMoving = true;

            int count = m_Board.TileCount;
            for (int i = 0; i < steps; i++)
            {
                int next = (m_CurrentIndex + 1) % count;
                if (next == 0)
                {
                    // Wrapped past the last tile: jump straight back to the start.
                    transform.position = TilePosition(next);
                }
                else
                {
                    yield return Hop(TilePosition(next));
                }

                m_CurrentIndex = next;

                if (i < steps - 1)
                {
                    GameEvents.RaisePlayerLanded(transform.position, false);
                }
            }

            Tile tile = m_Board.GetTile(m_CurrentIndex);
            Debug.Log($"Landed on tile {tile.Number}.");
            if (tile.HasReward)
            {
                GameEvents.RaisePlayerLanded(transform.position, true);
                GameEvents.RaiseRewardCollected(tile.Item, tile.Amount);
            }
            else
            {
                GameEvents.RaisePlayerLanded(transform.position, false);
            }

            IsMoving = false;
        }

        private IEnumerator Hop(Vector3 target)
        {
            Vector3 from = transform.position;
            float elapsed = 0f;
            while (elapsed < m_HopDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / m_HopDuration);
                Vector3 pos = Vector3.Lerp(from, target, t);
                pos.y += m_HopHeight * 4f * t * (1f - t);
                transform.position = pos;
                yield return null;
            }

            transform.position = target;
        }

        private Vector3 TilePosition(int index)
        {
            return m_Board.GetTilePosition(index) + m_TileOffset;
        }
    }
}
