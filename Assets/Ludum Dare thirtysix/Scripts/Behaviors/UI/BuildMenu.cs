using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

  public Resources resourcePool;
  public GameObject costTip;
  public GameObject[] menuItems;
  public Quaternion itemRotation = Quaternion.Euler(20, -132, 21);

  void OnEnable()
  {
    int i = 0;
    foreach (GameObject go in menuItems)
    {
      Transform menuItem = Instantiate(go).transform;
      menuItem.parent = transform;
      menuItem.localPosition = i * 1.57f * Vector3.down;
      menuItem.rotation = itemRotation;
      menuItem.localScale = Vector3.one;
      menuItem.gameObject.layer = gameObject.layer;

      BuildingPrice price = menuItem.GetComponent<BuildingPrice>();
      if (price != null)
      {
        BuildingPrice.Cost[] costs = price.buildCosts;
        if (!price.researched)
        {
          costs = price.researchCosts;
          foreach (Renderer render in menuItem.GetComponents<Renderer>())
          {
            render.enabled = false;
          }
          menuItem.gameObject.AddComponent<Aura>().color = new Color(1, 1, 1);
        }
        if (costs.Length > 0)
        {
          Transform costItem = Instantiate(costTip).transform;
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
      }

      ++i;
    }
  }

}
