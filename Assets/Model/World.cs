using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    int width;
    int height;
    public int Width { get => width; }
    public int Height { get => height; }

    int size;

    string seed;
    public string Seed { get => seed; }

    public Map tileMap;

    public List<Map> maps = new List<Map>();

    public World(int width = 100, int height = 100, string seed = "Default") {
        this.width = width;
        this.height = height;
        this.size = width * height;
        this.seed = seed;
        

        tileMap = new Map(this, 0, 0, "WorldMap" , width, height);
    }

    

}
