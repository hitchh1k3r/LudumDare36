using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{

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

  public enum Type
  {
    VILLAGER,
    LUMBER,
    CROP
  }

}
