using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{

  public Sprite timeIcon;
  public Sprite villagerIcon;
  public Sprite lumberIcon;
  public Sprite cropsIcon;
  public Sprite meatIcon;

  public int villagers = 5;
  public int lumber = 5;
  public int crops = 5;
  public int meat = 5;

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
      case Type.MEAT:
        {
          return meat;
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
      case Type.MEAT:
        {
          return meatIcon;
        }
    }
    return null;
  }

  public static string GetName(Type type)
  {
    switch (type)
    {
      case Type.TIME:
        {
          return "Turns";
        }
      case Type.VILLAGER:
        {
          return "Villagers";
        }
      case Type.LUMBER:
        {
          return "Timber";
        }
      case Type.CROP:
        {
          return "Crops";
        }
      case Type.MEAT:
        {
          return "Meat";
        }
    }
    return null;
  }

  public enum Type
  {
    TIME,
    VILLAGER,
    LUMBER,
    CROP,
    MEAT
  }

}
