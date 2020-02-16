using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject
{
    public enum ObjectType { Wall, Door, Furniture, Plant};
    enum Direction { North, East, South, West };

    Tile tile;

    ObjectType type;
    ObjectType Type { get => type; set => type = value; }
    Direction faceDirection;

    int width;
    int length;

    string name;
    string description;

    bool isDeconstructable;
    bool isUninstallable;


    public InstalledObject() {

    }

    public InstalledObject(Tile tile, InstalledObject proto) {
        this.tile   = tile;
        this.type   = proto.type;
        this.width  = proto.width;
        this.length = proto.length;
        this.name   = proto.name;        
    }

    public InstalledObject createInstalledObjectPrototype(  ObjectType type,
                                                            string name, 
                                                            bool deconstructable = true, 
                                                            bool uninstallable = true,
                                                            int width = 1, 
                                                            int length = 1,
                                                            string description = "Description missing") {
        InstalledObject prototype = new InstalledObject();
        prototype.type = type;
        prototype.width = width;
        prototype.length = length;
        prototype.name = name;
        prototype.description = description;
        prototype.isDeconstructable = deconstructable;
        prototype.isUninstallable = uninstallable;

        return prototype;
    }

    

}
