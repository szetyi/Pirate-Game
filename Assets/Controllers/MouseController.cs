using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    private Vector3 pos;
    WorldController worldController;

    public Tile selectedTile;
    Text toolTip_Text;

    Vector3 dragStartPos;
    Vector3 dragEndPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject worldController_go = GameObject.Find("/Controllers/WorldController");
        worldController = (WorldController)worldController_go.GetComponent(typeof(WorldController));
        GameObject text_go = GameObject.Find("/UI/Panel_Tooltip/Text");
        toolTip_Text = (Text)text_go.GetComponent(typeof(Text));
    }

    // Update is called once per frame
    void Update()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // check if we're on world or map view.
        if(worldController.CurrentMap.Name == "WorldMap") {
            SelectTile(pos);
        }
        else {
            MouseOver(pos);
            dragBuild(InstalledObject.ObjectType.Wall, pos);
        }

    }

    void dragBuild(InstalledObject.ObjectType type, Vector3 position) {
        
        if (Input.GetMouseButtonDown(0)) {
            dragStartPos = position;
        }
        if (Input.GetMouseButton(0)) {
            dragEndPos = position;
        }
        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("Drag start: " + Mathf.Floor(dragStartPos.x) + "," + Mathf.Floor(dragStartPos.y));
            Debug.Log("Drag end: " + Mathf.Floor(dragEndPos.x) + "," + Mathf.Floor(dragEndPos.y));


            int x1 = Mathf.FloorToInt(dragStartPos.x);
            int x2 = Mathf.FloorToInt(dragEndPos.x);
            int y1 = Mathf.FloorToInt(dragStartPos.y);
            int y2 = Mathf.FloorToInt(dragEndPos.y);
            int temp;

            if(x2 < x1) {
                temp = x1;
                x1 = x2;
                x2 = temp;
            }

            if(y2 < y1) {
                temp = y1;
                y1 = y2;
                y2 = temp;
            }

            for (int x = x1; x < x2 + 1; x++) {
                for (int y = y1; y < y2 + 1; y++) {
                    //Debug.Log("Affected " + x + "," + y + " tile");
                    GameObject obj = new GameObject(("object_" + x + "_" + y));
                    obj.transform.position = new Vector3(x, y, -1);
                    obj.AddComponent<SpriteRenderer>().sprite = worldController.buildSprite;

                }
            }

        }

    }


    void SelectTile(Vector3 position) {
        if (position.x > worldController.World.tileMap.Width || position.x < 0 || position.y > worldController.World.tileMap.Height || position.y < 0) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0)) {
            Tile tile = worldController.World.tileMap.GetTileAt(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

            selectedTile = tile;
            toolTip_Text.text = "Selected: " + selectedTile.X + ", " + selectedTile.Y + "\nType: " + selectedTile.Type;

        }

    }


    void MouseOver(Vector3 position) {
        if (position.x > worldController.CurrentMap.Width || position.x < 0 || position.y > worldController.CurrentMap.Height || position.y < 0) return;
        Tile tile = worldController.CurrentMap.GetTileAt(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

        switch (tile.Type) {
            case Tile.TileType.ShallowWater:
                toolTip_Text.text = "Shallow Water";
                break;
            case Tile.TileType.Sand:
                toolTip_Text.text = "Sand";
                break;
            case Tile.TileType.Gravel:
                toolTip_Text.text = "Gravel";
                break;
            case Tile.TileType.Dirt:
                toolTip_Text.text = "Dirt";
                break;
            case Tile.TileType.Stone:
                toolTip_Text.text = "Stone";
                break;
            case Tile.TileType.Grass:
                toolTip_Text.text = "Grass";
                break;
            case Tile.TileType.DeepWater:
                toolTip_Text.text = "Deep Water";
                break;
            default:
                Debug.LogError("MouseOver - Unrecognized tile type.");
                break;
        }
    }

}
