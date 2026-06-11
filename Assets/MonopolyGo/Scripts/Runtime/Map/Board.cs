using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonopolyGo
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private TextAsset m_MapJson;
        [SerializeField] private Tile m_TilePrefab;
        [SerializeField] private ItemDatabase m_ItemDatabase;
        [SerializeField] private Vector3 m_Direction = Vector3.forward;
        [SerializeField] private float m_Spacing = 2f;

        private readonly List<Tile> m_Tiles = new List<Tile>();

        public int TileCount => m_Tiles.Count;

        public void Generate()
        {
            MapData data = JsonUtility.FromJson<MapData>(m_MapJson.text);
            if (data == null || data.tiles == null || data.tiles.Length == 0)
            {
                return;
            }

            Vector3 step = m_Direction.normalized * m_Spacing;
            for (int i = 0; i < data.tiles.Length; i++)
            {
                Tile tile = Instantiate(m_TilePrefab, transform.position + step * i, Quaternion.identity, transform);
                ItemType item = ParseItem(data.tiles[i].item);
                tile.Configure(i + 1, item, data.tiles[i].amount, m_ItemDatabase);
                m_Tiles.Add(tile);
            }
        }

        public Tile GetTile(int index)
        {
            return m_Tiles[index];
        }

        public Vector3 GetTilePosition(int index)
        {
            return m_Tiles[index].transform.position;
        }

        private ItemType ParseItem(string raw)
        {
            if (string.IsNullOrEmpty(raw) || !Enum.TryParse(raw, true, out ItemType item))
            {
                return ItemType.None;
            }

            return item;
        }
    }
}
