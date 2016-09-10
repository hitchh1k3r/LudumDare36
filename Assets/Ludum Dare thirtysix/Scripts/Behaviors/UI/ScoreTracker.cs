using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker : MonoBehaviour
{

  public bool isSummaryShowing;

  public List<ScoreEntry> income = new List<ScoreEntry>();
  public List<ScoreEntry> expenses = new List<ScoreEntry>();
  public List<ScoreEntry> loss = new List<ScoreEntry>();
  public List<NamedEntry> namedLoss = new List<NamedEntry>();

  public static ScoreTracker instance;

  void OnEnable()
  {
    instance = this;
  }

  public void LostVillager(int amount, string[] names = null, bool leftNotKilled = false)
  {
    AddLoss(Resources.Type.PERSON, amount);
    if (names != null)
    {
      for (int i = 0; i < names.Length; ++i)
      {
        NamedEntry name = new NamedEntry();
        name.type = NamedLossType.PERSON;
        name.name = names[i];
        name.count = leftNotKilled ? 1 : -1;
        namedLoss.Add(name);
      }
    }
    else
    {
      for (int i = 0; i < amount; ++i)
      {
        NamedEntry name = new NamedEntry();
        name.type = NamedLossType.PERSON;
        name.name = MesopotamianGenerator.instance.GetMesopotamian().mesopoNAMEian;
        name.count = leftNotKilled ? 1 : -1;
        namedLoss.Add(name);
      }
    }
  }

  public void LostTile(string tileName, int amount = 1)
  {
    for (int i = 0; i < namedLoss.Count; ++i)
    {
      if (namedLoss[i].type == NamedLossType.TILE && namedLoss[i].name == tileName)
      {
        // if it exists increase it's (negative) magnitude:
        NamedEntry entry = namedLoss[i];
        entry.count -= amount;
        namedLoss[i] = entry;
        return;
      }
    }

    // otherwise add it:
    NamedEntry e;
    e.type = NamedLossType.TILE;
    e.name = tileName;
    e.count = -amount;
    namedLoss.Add(e);
  }

  public void AddLoss(Resources.Type type, int amount)
  {
    for (int i = 0; i < loss.Count; ++i)
    {
      if (loss[i].type == type)
      {
        // if it exists increase it's (negative) magnitude:
        ScoreEntry entry = loss[i];
        entry.amount -= amount;
        loss[i] = entry;
        return;
      }
    }

    // otherwise add it:
    ScoreEntry e;
    e.type = type;
    e.amount = -amount;
    loss.Add(e);
  }

  public void AddIncome(Resources.Type type, int amount)
  {
    int current = Resources.instance.GetValue(type, false);
    int max = Resources.instance.GetValue(type, true);
    if (current + amount > max)
    {
      AddLoss(type, max - current - amount);
      amount = max - current;
    }
    for (int i = 0; i < income.Count; ++i)
    {
      if (income[i].type == type)
      {
        // if it exists increase it's value:
        ScoreEntry entry = income[i];
        entry.amount += amount;
        income[i] = entry;
        return;
      }
    }

    // otherwise add it:
    ScoreEntry e;
    e.type = type;
    e.amount = amount;
    income.Add(e);
  }

  public void RemoveExpenses(Resources.Type type, int amount)
  {
    int current = Resources.instance.GetValue(type, false);
    int max = Resources.instance.GetValue(type, true);
    if (current + amount > max)
    {
      AddLoss(type, max - current - amount);
      amount = max - current;
    }
    for (int i = 0; i < expenses.Count; ++i)
    {
      if (expenses[i].type == type)
      {
        // if it exists increase it's (negative) magnitude:
        ScoreEntry entry = expenses[i];
        entry.amount += amount;
        expenses[i] = entry;
        return;
      }
    }

    // otherwise add it:
    ScoreEntry e;
    e.type = type;
    e.amount = amount;
    expenses.Add(e);
  }

  public void AddExpenses(Resources.Type type, int amount)
  {
    for (int i = 0; i < expenses.Count; ++i)
    {
      if (expenses[i].type == type)
      {
        // if it exists increase it's (negative) magnitude:
        ScoreEntry entry = expenses[i];
        entry.amount -= amount;
        expenses[i] = entry;
        return;
      }
    }

    // otherwise add it:
    ScoreEntry e;
    e.type = type;
    e.amount = -amount;
    expenses.Add(e);
  }

  [System.Serializable]
  public struct ScoreEntry
  {
    public Resources.Type type;
    public int amount;
  }

  [System.Serializable]
  public struct NamedEntry
  {
    public NamedLossType type;
    public string name;
    public int count;
  }

  public enum NamedLossType
  {
    PERSON,
    TILE
  }

}
