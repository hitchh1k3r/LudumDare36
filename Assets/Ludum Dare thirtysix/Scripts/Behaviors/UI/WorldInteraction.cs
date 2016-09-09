using UnityEngine;
using System.Collections;

public class WorldInteraction : MonoBehaviour
{

  public static WorldInteraction instance;

  public TileGrid world;
  public Camera camera;
  public Camera menuCamera;
  public Transform menuLimiter;
  public LayerMask terrainLayer;
  public Color blockHoverColor;
  public GameObject house;
  public AudioClip buildSound;

  private GameObject currentHover = null;
  private GameObject currentTile = null;
  private TileGrid.Point currentPoint;

  void OnEnable()
  {
    instance = this;
  }

  void Update()
  {
    GameObject hovering = null;
    RaycastHit hit;
    bool buildStage = (RoundManager.instance.stage == RoundManager.RoundStage.DAWN || RoundManager.instance.stage == RoundManager.RoundStage.DUSK);
    if (!ScoreTracker.instance.isSummaryShowing && buildStage && menuCamera.ScreenToWorldPoint(Input.mousePosition).x < menuLimiter.position.x && Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, terrainLayer))
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

          BuildingPrice price = currentTile.GetComponent<BuildingPrice>();
          CostDisplay display = currentTile.GetComponentInChildren<CostDisplay>(true);
          if (price != null && display != null && !price.alwaysShowCost)
          {
            display.gameObject.SetActive(false);
          }
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

            BuildingPrice price = currentTile.GetComponent<BuildingPrice>();
            CostDisplay display = currentTile.GetComponentInChildren<CostDisplay>(true);
            if (price != null && display != null && !price.alwaysShowCost)
            {
              display.gameObject.SetActive(true);
            }
          }
          else
          {
            HighlightEffect.AddHighlight(hovering, blockHoverColor);
          }
          currentHover = hovering;
        }
      }
    }

    if (!ScoreTracker.instance.isSummaryShowing && buildStage && Input.GetButtonDown("Click") && currentPoint != null)
    {
      if (currentTile == null)
      {
        if (BuildMenu.activeTool != null && BuildMenu.activeTool.isPlaceable && Resources.instance.TryBuy(BuildMenu.activeTool.buildCosts))
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
          world.GetTile(currentPoint.x, currentPoint.y).GetComponent<GameTile>().audio.PlayOneShot(buildSound);
          SoundEffects.PlaySound(buildSound);
        }
      }
      else
      {
        currentTile.GetComponent<GameTile>().ClickTile(world, currentPoint.x, currentPoint.y);
      }
    }
  }

}
