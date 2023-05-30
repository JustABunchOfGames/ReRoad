using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    public enum TileType
    {
        Plain,
        Forest,
        RockyPlain,
        AbandonnedVillage,
        Mountain,
        River,
        Sand,
        MineEntrance
    }

    public class Tile : MonoBehaviour
    {
        private TileType _type;

        private List<Ressources> _tileInventory;
    }
}