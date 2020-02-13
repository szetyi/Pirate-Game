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
