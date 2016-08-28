using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour
{

  public Color color;

  private bool done = false;

  void Update()
  {
    if (!done)
    {
      done = true;
      HighlightEffect.AddHighlight(gameObject, color);
    }
  }

  void OnDisable()
  {
    HighlightEffect.RemoveHighlight(gameObject);
    done = false;
  }

}
