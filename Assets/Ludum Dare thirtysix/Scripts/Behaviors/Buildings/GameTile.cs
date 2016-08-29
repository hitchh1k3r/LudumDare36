using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour
{

  [System.NonSerialized]
  public bool working;

  public void ClickTile()
  {
    if (enabled && !ScoreTracker.instance.isSummaryShowing)
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
      HighlightEffect.RemoveHighlight(gameObject);
      HighlightEffect.AddHighlight(gameObject, highlightColor);
    }
  }

}
