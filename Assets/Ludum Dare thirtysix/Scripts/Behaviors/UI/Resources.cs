using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{

  public Sprite timeIcon;
  public Sprite personIcon;
  public Sprite animalIcon;
  public Sprite woodIcon;
  public Sprite stoneIcon;
  public Sprite foodIcon;

  public int person = 5;
  public int personMax = 10;
  public int animal = 0;
  public int animalMax = 0;
  public int wood = 10;
  public int woodMax = 10;
  public int stone = 10;
  public int stoneMax = 10;
  public int food = 20;
  public int foodMax = 50;

  public int GetValue(Type type, bool max)
  {
    switch (type)
    {
      case Type.PERSON:
        {
          return max ? personMax : person;
        }
      case Type.ANIMAL:
        {
          return max ? animalMax : animal;
        }
      case Type.WOOD:
        {
          return max ? woodMax : wood;
        }
      case Type.STONE:
        {
          return max ? stoneMax : stone;
        }
      case Type.FOOD:
        {
          return max ? foodMax : food;
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
      case Type.PERSON:
        {
          return personIcon;
        }
      case Type.ANIMAL:
        {
          return animalIcon;
        }
      case Type.WOOD:
        {
          return woodIcon;
        }
      case Type.STONE:
        {
          return stoneIcon;
        }
      case Type.FOOD:
        {
          return foodIcon;
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
      case Type.PERSON:
        {
          return "Villager";
        }
      case Type.ANIMAL:
        {
          return "Animal";
        }
      case Type.WOOD:
        {
          return "Timber";
        }
      case Type.STONE:
        {
          return "Stone";
        }
      case Type.FOOD:
        {
          return "Food";
        }
    }
    return null;
  }

  public enum Type
  {
    TIME,
    PERSON,
    ANIMAL,
    WOOD,
    STONE,
    FOOD
  }

}
