using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildingPrice : MonoBehaviour
{

  public string type;
  public bool alwaysShowCost;
  public bool isPlaceable;
  public float heightOffset = 1;
  public bool hidesTerrain;
  public GameObject construction;
  public string requirement;
  public Cost[] buildCosts;
  public Cost[] workCosts;
  public Color selectionColor = new Color(0, 1, 1);
  [TextArea]
  public string tooltipName, tooltipCosts, tooltipDescription;

  [System.NonSerialized]
  public GameObject prefab;

  public static BuildingPrice selected;

  void Update()
  {
    // STUB SO UNITY CAN DISABLE THIS SCRIPT
  }

  public void BuildingComplete(string type)
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

  void OnMouseEnter()
  {
    if (enabled && requirement == "" && !ScoreTracker.instance.isSummaryShowing)
    {
      DialogueManager.Tooltip(tooltipName, tooltipDescription, tooltipCosts);
    }
  }

  void OnMouseExit()
  {
    if (enabled && requirement == "" && !ScoreTracker.instance.isSummaryShowing)
    {
      DialogueManager.HideTooltip();
    }
  }

  void OnMouseUpAsButton()
  {
    if (enabled && requirement == "" && !ScoreTracker.instance.isSummaryShowing)
    {
      if (selected != null)
      {
        HighlightEffect.RemoveHighlight(selected.gameObject);
      }
      HighlightEffect.AddHighlight(gameObject, selectionColor, true);
      selected = this;
      BuildMenu.activeTool = this;
    }
  }

  [System.Serializable]
  public struct Cost
  {
    public Resources.Type type;
    public int amount;
  }

}
