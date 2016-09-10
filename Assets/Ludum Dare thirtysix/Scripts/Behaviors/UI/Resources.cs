using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{

  // NOTE (hitch) at some point (6 hours before deadline) I decided everything can be a singleton!!!
  public static Resources instance;

  public GameObject workCostDisplay;

  public Sprite timeIcon;
  public Sprite personIcon;
  public Sprite animalIcon;
  public Sprite woodIcon;
  public Sprite stoneIcon;
  public Sprite foodIcon;

  public int person = 5;
  public int personLive = 5;
  public int personMax = 10;
  public int animal = 0;
  public int animalMax = 0;
  public int wood = 10;
  public int woodMax = 10;
  public int stone = 10;
  public int stoneMax = 10;
  public int food = 20;
  public int foodMax = 50;

  public void AddResources(BuildingPrice.Cost[] resources)
  {
    foreach (BuildingPrice.Cost resource in resources)
    {
      int val = GetValue(resource.type, false) + resource.amount;
      if (val > GetValue(resource.type, true))
      {
        val = GetValue(resource.type, true);
      }
      SetValue(resource.type, val, false);
    }
  }

  public bool TryBuy(BuildingPrice.Cost[] costs)
  {
    bool okay = true;
    foreach (BuildingPrice.Cost cost in costs)
    {
      if (cost.amount > GetValue(cost.type, false))
      {
        okay = false;
        break;
      }
    }
    if (okay)
    {
      foreach (BuildingPrice.Cost cost in costs)
      {
        SetValue(cost.type, GetValue(cost.type, false) - cost.amount, false);
      }
    }
    return okay;
  }

  void OnEnable()
  {
    instance = this;
  }

  public int GetValue(Type type, bool max)
  {
    switch (type)
    {
      case Type.TIME:
        {
          return 999;
        }
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

  public void SetValue(Type type, int value, bool max)
  {
    switch (type)
    {
      case Type.PERSON:
        {
          if (max)
          {
            personMax = value;
            if (personLive > personMax)
            {
              person -= personLive - personMax;
              if (person < 0)
              {
                // TODO (hitch) ideally this would remove a worker... but as you could maybe over work
                // people by increasing person over personLive (for a turn before it resyncs)
                person = 0;
              }
              ScoreTracker.instance.LostVillager(personLive - personMax, MesopotamianGenerator.instance.RemoveFromPool(personLive - personMax), true);
              personLive = personMax;
            }
          }
          else
          {
            person = value;
          }
        }
        break;
      case Type.ANIMAL:
        {
          if (max)
          {
            animalMax = value;
            if (animal > animalMax)
            {
              ScoreTracker.instance.AddLoss(type, animalMax - animal);
              animal = animalMax;
            }
          }
          else
          {
            animal = value;
          }
        }
        break;
      case Type.WOOD:
        {
          if (max)
          {
            woodMax = value;
            if (wood > woodMax)
            {
              ScoreTracker.instance.AddLoss(type, woodMax - wood);
              wood = woodMax;
            }
          }
          else
          {
            wood = value;
          }
        }
        break;
      case Type.STONE:
        {
          if (max)
          {
            stoneMax = value;
            if (stone > stoneMax)
            {
              ScoreTracker.instance.AddLoss(type, stoneMax - stone);
              stone = stoneMax;
            }
          }
          else
          {
            stone = value;
          }
        }
        break;
      case Type.FOOD:
        {
          if (max)
          {
            foodMax = value;
            if (food > foodMax)
            {
              ScoreTracker.instance.AddLoss(type, foodMax - food);
              food = foodMax;
            }
          }
          else
          {
            food = value;
          }
        }
        break;
    }
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
