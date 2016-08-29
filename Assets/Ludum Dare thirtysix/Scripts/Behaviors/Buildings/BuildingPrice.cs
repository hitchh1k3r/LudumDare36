using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildingPrice : MonoBehaviour
{

  public string type;
  public bool isPlaceable;
  public GameObject construction;
  public string requirement;
  public Cost[] buildCosts;
  public Cost[] workCosts;
  public Color selectionColor = new Color(0, 1, 1);

  [System.NonSerialized]
  public GameObject prefab;

  public static BuildingPrice selected;

  void Update()
  {
    // STUB SO UNITY CAN DISABLE THIS SCRIPT
  }

  void BuildingComplete(string type)
  {
    if (type == requirement)
    {
      requirement = "";
      CostDisplay cost = GetComponentInChildren<CostDisplay>(true);
      if (cost != null)
      {
        cost.gameObject.SetActive(true);
      }
      foreach (Renderer render in GetComponentsInChildren<Renderer>())
      {
        render.enabled = true;
      }
      HighlightEffect.RemoveHighlight(gameObject);
    }
  }

  void OnMouseUpAsButton()
  {
    if (enabled && requirement == "" && !ScoreTracker.instance.isSummaryShowing)
    {
      if (selected == null)
      {
        HighlightEffect.AddHighlight(gameObject, selectionColor);
        selected = this;
        BuildMenu.activeTool = this;
      }
      else
      {
        HighlightEffect.RemoveHighlight(selected.gameObject);
        if (gameObject != selected)
        {
          HighlightEffect.AddHighlight(gameObject, selectionColor);
          selected = this;
          BuildMenu.activeTool = this;
        }
        else
        {
          selected = null;
          BuildMenu.activeTool = null;
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
