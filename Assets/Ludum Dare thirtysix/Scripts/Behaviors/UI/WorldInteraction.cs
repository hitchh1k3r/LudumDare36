using UnityEngine;
using System.Collections;

public class WorldInteraction : MonoBehaviour
{

  public Camera camera;
  public LayerMask terrainLayer;
  public Color blockHoverColor;

  private GameObject currentHover = null;

  void Update()
  {
    GameObject hovering = null;
    RaycastHit hit;
    if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, terrainLayer))
    {
      hovering = hit.collider.gameObject;
    }

    if (currentHover != hovering)
    {
      if (currentHover != null)
      {
        HighlightEffect.RemoveHighlight(currentHover);
      }
      if (hovering != null)
      {
        HighlightEffect.AddHighlight(hovering, blockHoverColor);
      }
      currentHover = hovering;
    }
  }


}
