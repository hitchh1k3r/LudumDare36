using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour
{

  public bool canDemolish;
  public bool changeOnWork;
  public GameObject workState;
  public BuildingPrice.Cost[] demolishRefund;
  public BuildingPrice.Cost[] workGain;

  [System.NonSerialized]
  public bool working;
  [System.NonSerialized]
  public GameObject[] workDestroy;

  public void ClickTile(TileGrid world, int x, int y)
  {
    if (enabled && !ScoreTracker.instance.isSummaryShowing)
    {
      if (BuildMenu.activeTool.type == "peep")
      {
        working = !working;
        foreach (SpecialStates child in GetComponentsInChildren<SpecialStates>(true))
        {
          if (child.activeOnWork)
          {
            child.gameObject.SetActive(working);
          }
        }
        Color highlightColor = HighlightEffect.GetColor(gameObject);
        if (highlightColor != Color.black)
        {
          HighlightEffect.RemoveHighlight(gameObject);
          HighlightEffect.AddHighlight(gameObject, highlightColor);
        }
      }
      else if (BuildMenu.activeTool.type == "demo")
      {
        if (canDemolish)
        {
          world.SetTile(x, y, null);
        }
        else
        {
          // TODO (hitch) ERROR (can't demolish)
        }
      }
    }
  }

}
