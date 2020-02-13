using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile
{
    public Tile(Map map, int x, int y) {
        this.map = map;
        this.x = x;
        this.y = y;
    }

    public enum TileType { DeepWater, ShallowWater, Sand, Dirt, Grass, Gravel, Stone };
    
    // Callback functions to run if a tile's type changes.
    Action<Tile> TileTypeChangedCallback;

    Map map;
    int x;
    int y;
    public int X { get => x; }
    public int Y { get => y; }

    TileType type;
    public TileType Type {
        get => type;
        set {
            TileType oldType = type;
            type = value;

            // Run the Callback function if the TileType changed.
            if (TileTypeChangedCallback != null && oldType != type)
                TileTypeChangedCallback(this);
        }

    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback) {
        TileTypeChangedCallback += callback;
    }

    public void UnRegisterTileTypeChangedCallback(Action<Tile> callback) {
        TileTypeChangedCallback -= callback;
    }
}
