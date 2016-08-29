using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

  public Resources resourcePool;
  public GameObject costTip;
  public GameObject[] menuItems;
  public Quaternion itemRotation = Quaternion.Euler(20, -132, 21);

  public static BuildMenu instance;
  public static BuildingPrice activeTool;

  private bool init;

  void OnEnable()
  {
    instance = this;
  }

  void Update()
  {
    if(!init)
    {
      init = true;
      int i = 0;
      foreach (GameObject go in menuItems)
      {
        i = AddToMenu(i, go);
      }
    }
  }

  private int AddToMenu(int i, GameObject go)
  {
    Transform menuItem = Instantiate(go).transform;
    menuItem.parent = transform;
    menuItem.localPosition = i * 1.57f * Vector3.down;
    menuItem.rotation = itemRotation;
    menuItem.localScale = Vector3.one;
    foreach (Transform trans in menuItem.gameObject.GetComponentsInChildren<Transform>(true))
    {
      trans.gameObject.layer = gameObject.layer;
    }

    BuildingPrice price = menuItem.GetComponent<BuildingPrice>();
    if (price != null)
    {
      price.prefab = go;
      BuildingPrice.Cost[] costs = price.buildCosts;
      Transform costItem = null;
      if (costs.Length > 0)
      {
        costItem = Instantiate(costTip).transform;
        costItem.parent = menuItem;
        costItem.localPosition = Vector3.zero;
        costItem.localRotation = Quaternion.identity;
        costItem.localScale = Vector3.one;
        costItem.gameObject.layer = gameObject.layer;
        CostDisplay display = costItem.GetComponent<CostDisplay>();
        int q = 0;
        foreach (BuildingPrice.Cost cost in costs)
        {
          display.costs[q].sprite.gameObject.SetActive(true);
          display.costs[q].sprite.sprite = resourcePool.GetIcon(cost.type);
          display.costs[q].text.text = cost.amount.ToString();
          ++q;
        }
      }
      if (price.requirement != "")
      {
        if (costItem != null)
        {
          costItem.gameObject.SetActive(false);
        }
        foreach (Renderer render in menuItem.GetComponentsInChildren<Renderer>())
        {
          render.enabled = false;
        }
        HighlightEffect.AddHighlight(menuItem.gameObject, new Color(1, 1, 1));
      }
    }

    return i + 1;
  }

}
