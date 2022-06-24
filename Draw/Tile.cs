using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : TileBase
{
    private Sprite m_sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = m_sprite;
    }
   
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }
}
