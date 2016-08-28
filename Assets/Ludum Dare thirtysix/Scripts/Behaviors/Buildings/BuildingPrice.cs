using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildingPrice : MonoBehaviour
{

  public bool researched;
  public Cost[] researchCosts;
  public Cost[] buildCosts;
  public Color selectionColor = new Color(0, 1, 1);

  public static GameObject selected;

  void OnMouseUpAsButton()
  {
    if (researched)
    {
      if (selected == null)
      {
        HighlightEffect.AddHighlight(gameObject, selectionColor);
        selected = gameObject;
      }
      else
      {
        HighlightEffect.RemoveHighlight(selected);
        if (gameObject != selected)
        {
          HighlightEffect.AddHighlight(gameObject, selectionColor);
          selected = gameObject;
        }
        else
        {
          selected = null;
        }
      }
    }
  }

  [System.Serializable]
  public struct Cost
  {
    public Resources.Type type;
    public int amount;
  }

}
