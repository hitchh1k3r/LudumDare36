using UnityEngine;
using System.Collections;

public class ScreenRelative : MonoBehaviour
{

  public Camera camera;
  public Corner attachCorner;

  void Update()
  {
    Vector3 viewportPoint = new Vector3(0, 0, 10);
    if (attachCorner == Corner.TOP_LEFT)
    {
      viewportPoint = new Vector3(0, 1, 10);
    }
    else if (attachCorner == Corner.TOP_RIGHT)
    {
      viewportPoint = new Vector3(1, 1, 10);
    }
    else if (attachCorner == Corner.BOTTOM_RIGHT)
    {
      viewportPoint = new Vector3(1, 0, 10);
    }
    else if (attachCorner == Corner.TOP_MIDDLE)
    {
      viewportPoint = new Vector3(0.5f, 1, 10);
    }
    else if (attachCorner == Corner.MIDDLE_RIGHT)
    {
      viewportPoint = new Vector3(1, 0.5f, 10);
    }
    else if (attachCorner == Corner.BOTTOM_MIDDLE)
    {
      viewportPoint = new Vector3(0.5f, 0, 10);
    }
    else if (attachCorner == Corner.MIDDLE_LEFT)
    {
      viewportPoint = new Vector3(0, 0.5f, 10);
    }
    transform.position = camera.ViewportToWorldPoint(viewportPoint);
  }

  public enum Corner
  {
    TOP_LEFT,
    TOP_RIGHT,
    BOTTOM_LEFT,
    BOTTOM_RIGHT,
    TOP_MIDDLE,
    MIDDLE_RIGHT,
    BOTTOM_MIDDLE,
    MIDDLE_LEFT,
  }

}
