using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    public SpriteAtlas atlas;

    GameObject worldTiles;
    MouseController mouseController;

    World world;
    public World World { get => world; }

    Dictionary<string, InstalledObject> installedObjectDict = new Dictionary<string, InstalledObject>();

    Map currentMap;
    public Map CurrentMap {
        get => currentMap;
        set {
            currentMap = value;
            Debug.Log("Current map is " + currentMap.Name);
        }
    }

    void Start()
    {
        worldTiles = GameObject.Find("/WorldTiles");
        mouseController = (MouseController)GameObject.Find("/Controllers/MouseController").GetComponent(typeof(MouseController));
        world = new World();
        GenerateWorldTileGameObjects();
        world.tileMap.GenerateMap();
        CurrentMap = world.tileMap;
        generateObjectDictionary();
        /*
        worldTiles.SetActive(false);
        
        world.maps.Add(new Map(world, 0, 1, "map_0_1"));
        GenerateMapTileGameObjects(world.maps[0]);
        world.maps[0].RandomizeTiles();
        GameObject.Find("/World/Map_0_1").SetActive(false);

        world.maps.Add(new Map(world, 3, 3, "map_3_3"));
        GenerateMapTileGameObjects(world.maps[1]);
        world.maps[1].RandomizeTiles();
        */
    }

    void generateObjectDictionary() {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/Configs/installedObjects.cfg");
        string line;
        int counter = 0;

        while ((line = file.ReadLine()) != null) {
            string[] parts = line.Split('>');
            InstalledObject prototype = new InstalledObject();
            InstalledObject.ObjectType prototypeType;
            Enum.TryParse(parts[1], out prototypeType);
            prototype = prototype.createInstalledObjectPrototype(prototypeType, parts[2], Convert.ToBoolean(parts[3]), Convert.ToBoolean(parts[4])
                                                                , Convert.ToInt32(parts[5]), Convert.ToInt32(parts[6]), parts[7]);

            installedObjectDict.Add(parts[0], prototype);

            counter++;

        }
    }

    public void ButtonSelectMap() {

        int x = mouseController.selectedTile.X;
        int y = mouseController.selectedTile.Y;

        if (worldTiles.activeSelf) worldTiles.SetActive(false);

        if(GameObject.Find("/World/Map_"+ x + "_" + y) == null) { 
 
            world.maps.Add(new Map(world, x, y, "map_" + x + "_" + y));


            // ITT JÁRTAM LOL
            GenerateMapTileGameObjects(world.maps[0]);
            world.maps[0].RandomizeTiles();
        }
        else {
            GameObject.Find("/World/Map_" + x + "_" + y).SetActive(true);
        }


        for (int i = 0; i < world.maps.Count; i++) {
            if (world.maps[i].Name == "map_" + x + "_" + y) 
            {
                CurrentMap = world.maps[i];
                return;
            }
        }
        

    }

    // This generates game objects for the world map, which are stored in the WorldTiles parent.
    // They get a SpriteRenderer defaulted to deep water.
    void GenerateWorldTileGameObjects() {
        for (int x = 0; x < world.tileMap.Width; x++) {
            for (int y = 0; y < world.tileMap.Height; y++) {
                Tile tile_data = world.tileMap.GetTileAt(x, y);
                GameObject tile_go = new GameObject();
                tile_go.name = "World_Tile_" + x + "_" + y;
                tile_go.transform.SetParent(worldTiles.transform);
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.AddComponent<SpriteRenderer>().sprite = atlas.GetSprite("deep_water1");

                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }
    }

    // Generates Game Objects for the map Tiles, adds a sprite renderer to them.
    // They are created in a new parent Game Object in World.
    void GenerateMapTileGameObjects(Map map) {
        GameObject map_go = new GameObject();
        map_go.name = "Map_" + map.X + "_" + map.Y;
        map_go.transform.SetParent(GameObject.Find("World").transform);

        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                Tile tile_data = map.GetTileAt(x, y);
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.SetParent(GameObject.Find("Map_" + map.X + "_" + map.Y).transform);
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.AddComponent<SpriteRenderer>().sprite = atlas.GetSprite("deep_water1");

                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }
    }

    // This gets called in Tile.cs whenever a tile's Type gets changed using SET.
    void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
        SpriteRenderer tile_sprite = tile_go.GetComponent<SpriteRenderer>();
        switch (tile_data.Type) {
            case Tile.TileType.ShallowWater:
                tile_sprite.sprite = atlas.GetSprite("shallow_water1");
                break;
            case Tile.TileType.Sand:
                tile_sprite.sprite = atlas.GetSprite("sand1"); ;
                break;
            case Tile.TileType.Gravel:
                tile_sprite.sprite = atlas.GetSprite("gravel1"); ;
                break;
            case Tile.TileType.Dirt:
                tile_sprite.sprite = atlas.GetSprite("dirt1"); ;
                break;
            case Tile.TileType.Stone:
                tile_sprite.sprite = atlas.GetSprite("stone1"); ;
                break;
            case Tile.TileType.Grass:
                tile_sprite.sprite = atlas.GetSprite("grass1"); ;
                break;
            case Tile.TileType.DeepWater:
                tile_sprite.sprite = atlas.GetSprite("deep_water1"); ;
                break;
            default:
                Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
                break;
        }
    }

}
