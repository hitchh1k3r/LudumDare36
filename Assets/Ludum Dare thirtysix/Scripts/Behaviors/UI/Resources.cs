using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{

  public Sprite timeIcon;
  public Sprite villagerIcon;
  public Sprite lumberIcon;
  public Sprite cropsIcon;

  public int villagers = 5;
  public int lumber = 0;
  public int crops = 20;

  public int GetValue(Type type)
  {
    switch (type)
    {
      case Type.VILLAGER:
        {
          return villagers;
        }
      case Type.LUMBER:
        {
          return lumber;
        }
      case Type.CROP:
        {
          return crops;
        }
    }
    return 0;
  }

  public Sprite GetIcon(Type type)
  {
    switch (type)
    {
      case Type.TIME:
        {
          return timeIcon;
        }
      case Type.VILLAGER:
        {
          return villagerIcon;
        }
      case Type.LUMBER:
        {
          return lumberIcon;
        }
      case Type.CROP:
        {
          return cropsIcon;
        }
    }
    return null;
  }

  public enum Type
  {
    VILLAGER,
    LUMBER,
    CROP,
    TIME
  }

}
