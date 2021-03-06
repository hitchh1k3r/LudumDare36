﻿using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

  public Resources resourcePool;
  public GameObject costTip;
  public MenuEntry[] menuEntries;
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
    if (!init)
    {
      init = true;
      int i = 0;
      foreach (MenuEntry go in menuEntries)
      {
        i = AddToMenu(i, go);
      }
    }
  }

  private int AddToMenu(int i, MenuEntry go)
  {
    Transform menuItem = Instantiate(go.prefab).transform;
    menuItem.gameObject.GetComponent<BuildingPrice>().enabled = true;
    menuItem.parent = transform;
    menuItem.localPosition = i * (1.57f + ((i % 2 == 1) ? 0.1f : 0)) * Vector3.down + ((i % 2 == 1) ? 1.5f : 0) * Vector3.right + go.yOffset * Vector3.up;
    menuItem.rotation = itemRotation;
    menuItem.localScale = Vector3.one;
    foreach (Transform trans in menuItem.gameObject.GetComponentsInChildren<Transform>(true))
    {
      trans.gameObject.layer = gameObject.layer;
    }
    foreach (SpecialStates states in menuItem.GetComponentsInChildren<SpecialStates>())
    {
      if (states.hideInMenu)
      {
        Destroy(states.gameObject);
      }
    }

    BuildingPrice price = menuItem.GetComponent<BuildingPrice>();
    if (price != null)
    {
      price.prefab = go.prefab;
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
        HighlightEffect.AddHighlight(menuItem.gameObject, new Color(1, 1, 1), true);
      }
    }

    return i + 1;
  }

  [System.Serializable]
  public struct MenuEntry
  {
    public GameObject prefab;
    public float yOffset;
  }

}
