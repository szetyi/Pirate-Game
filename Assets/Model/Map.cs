using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    World world;

    Tile[,] tiles;
    public Tile[,] Tiles { get => tiles; }

    string name;
    public string Name { get => name; }

    int x;
    public int X { get => x; }
    int y;
    public int Y { get => y; }


    int width;
    int height;
    public int Width { get => width; }
    public int Height { get => height; }
    int size;

    // Initializes the map.
    public Map(World world, int x, int y, string name = "ErrorName", int width = 100, int height = 100) {
        this.name = name;
        this.world = world;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.size = width * height;

        tiles = new Tile[width, height];

        for (int xcoord = 0; xcoord < width; xcoord++) {
            for (int ycoord = 0; ycoord < height; ycoord++) {
                tiles[xcoord, ycoord] = new Tile(this, xcoord, ycoord);
            }
        }

        Debug.Log("Map named " + name + " created with " + width * height + " tiles.");
    }

    // returns the tile of the map at X and Y for reading purposes.
    public Tile GetTileAt(int x, int y) {
        if (x > width || x < 0 || y > height || y < 0) {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        return tiles[x, y];
    }


    // Generates a new map using Perlin Noise and Radial falloff. Most logic is in MapGenerator.cs.
    public void GenerateMap() {
        GameObject go = GameObject.Find("/Controllers/MapGen");
        MapGenerator mapGenerator = (MapGenerator)go.GetComponent(typeof(MapGenerator));
        float[,] heightMap = mapGenerator.GenerateHeights(width, height);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                if (heightMap[x, y] > .90f) {
                    tiles[x, y].Type = Tile.TileType.Stone;
                }
                else if (heightMap[x, y] > .60f) {
                    tiles[x, y].Type = Tile.TileType.Dirt;
                }
                else if (heightMap[x, y] > .55f) {
                    tiles[x, y].Type = Tile.TileType.Gravel;
                }
                else if (heightMap[x, y] > .40f) {
                    tiles[x, y].Type = Tile.TileType.Sand;
                }
                else if (heightMap[x, y] > 0.25f) {
                    tiles[x, y].Type = Tile.TileType.ShallowWater;
                }
                else {
                    tiles[x, y].Type = Tile.TileType.DeepWater;
                }
            }
        }

        // TO-DO

    }

    public void RandomizeTiles() {

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                switch (UnityEngine.Random.Range(0, 6)) {
                    case 1:
                        tiles[x, y].Type = Tile.TileType.ShallowWater;
                        break;
                    case 2:
                        tiles[x, y].Type = Tile.TileType.Sand;
                        break;
                    case 3:
                        tiles[x, y].Type = Tile.TileType.Gravel;
                        break;
                    case 4:
                        tiles[x, y].Type = Tile.TileType.Dirt;
                        break;
                    case 5:
                        tiles[x, y].Type = Tile.TileType.Grass;
                        break;
                    case 6:
                        tiles[x, y].Type = Tile.TileType.Stone;
                        break;
                    default:
                        tiles[x, y].Type = Tile.TileType.DeepWater;
                        break;
                }
            }
        }
    }
}
