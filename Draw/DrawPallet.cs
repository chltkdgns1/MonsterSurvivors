using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawPallet 
{
    public static DrawPallet instance = new DrawPallet();

    private DrawPallet()
    {
        
    }

    public void PrintTile(Tilemap pallet, Vector3 vPos, Tile tTile)
    {
        Vector3Int cellPoint = pallet.WorldToCell(vPos);
        pallet.SetTile(cellPoint, tTile);
    }

    public void PrintTile(Tilemap pallet, List<Vector3> vPosList, List<Tile> tTileList)
    {
        if (vPosList.Count != tTileList.Count) return;
        int cnt = vPosList.Count;

        for(int i = 0; i < cnt; i++)
        {
            Vector3Int cellPoint = pallet.WorldToCell(vPosList[i]);
            pallet.SetTile(cellPoint, tTileList[i]);
        }
    }
}
