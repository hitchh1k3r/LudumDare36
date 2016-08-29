using UnityEngine;
using System.Collections;

public class WorldInteraction : MonoBehaviour
{

  public TileGrid world;
  public Camera camera;
  public Camera menuCamera;
  public Transform menuLimiter;
  public LayerMask terrainLayer;
  public Color blockHoverColor;
  public GameObject house;

  private GameObject currentHover = null;
  private GameObject currentTile = null;
  private TileGrid.Point currentPoint;

  void Update()
  {
    GameObject hovering = null;
    RaycastHit hit;
    if (!ScoreTracker.instance.isSummaryShowing && menuCamera.ScreenToWorldPoint(Input.mousePosition).x < menuLimiter.position.x && Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, terrainLayer))
    {
      hovering = hit.collider.gameObject;
    }

    if (currentHover != hovering)
    {
      // Remove old:
      if (currentHover != null)
      {
        if (currentTile != null)
        {
          HighlightEffect.RemoveHighlight(currentTile);
        }
        else
        {
          HighlightEffect.RemoveHighlight(currentHover);
        }
      }

      // Add new:
      currentPoint = null;
      currentHover = null;
      currentTile = null;
      if (hovering != null)
      {
        currentPoint = world.GetTerrainPos(hovering);
        if (currentPoint != null)
        {
          GameObject tile = world.GetTile(currentPoint.x, currentPoint.y);
          if (tile != null)
          {
            HighlightEffect.AddHighlight(tile, blockHoverColor);
            currentTile = tile;
          }
          else
          {
            HighlightEffect.AddHighlight(hovering, blockHoverColor);
          }
          currentHover = hovering;
        }
      }
    }

    if (!ScoreTracker.instance.isSummaryShowing && Input.GetButtonDown("Click") && currentPoint != null)
    {
      if (currentTile == null)
      {
        if (BuildMenu.activeTool.isPlaceable)
        {
          if (BuildMenu.activeTool.construction != null)
          {
            world.SetTile(currentPoint.x, currentPoint.y, BuildMenu.activeTool.construction);
          }
          else
          {
            world.SetTile(currentPoint.x, currentPoint.y, BuildingPrice.selected.prefab);
          }
          HighlightEffect.RemoveHighlight(currentHover);
          currentHover = null;
        }
      }
      else
      {
        currentTile.GetComponent<GameTile>().ClickTile(world, currentPoint.x, currentPoint.y);
      }
    }
  }


}
