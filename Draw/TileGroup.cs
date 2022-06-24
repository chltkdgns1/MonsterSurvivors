using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileGroup
{
    private List<Tile> m_vTile = new List<Tile>();

    void AddTile(Tile tile)
    {
        m_vTile.Add(tile);
    }
}
