using System;

namespace MonopolyGo
{
    // DTOs for JsonUtility. Public fields named to match the JSON keys; the tile
    // count comes from tiles.Length, so there is no separate count field.
    [Serializable]
    public class MapData
    {
        public TileData[] tiles;
    }

    [Serializable]
    public class TileData
    {
        public string item;
        public int amount;
    }
}
