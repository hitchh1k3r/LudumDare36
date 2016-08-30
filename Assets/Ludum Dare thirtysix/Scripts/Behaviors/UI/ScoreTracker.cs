using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker : MonoBehaviour
{

  public bool isSummaryShowing;

  public List<ScoreEntry> lost = new List<ScoreEntry>();
  public List<ScoreEntry> captured = new List<ScoreEntry>();
  public List<ScoreEntry> upkeep = new List<ScoreEntry>();
  public List<ScoreEntry> income = new List<ScoreEntry>();

  public static ScoreTracker instance;

  void OnEnable()
  {
    instance = this;
  }

  [System.Serializable]
  public struct ScoreEntry
  {
    public Resources.Type type;
    public int amount;
  }

  public void AddLost(Resources.Type type, int amount)
  {
    for (int i = 0; i < lost.Count; ++i)
    {
      if (lost[i].type == type)
      {
        ScoreEntry entry = lost[i];
        entry.amount -= amount;
        lost[i] = entry;
        return;
      }
    }
    ScoreEntry e;
    e.type = type;
    e.amount = -amount;
    lost.Add(e);
  }

  public void AddIncome(Resources.Type type, int amount)
  {
    for (int i = 0; i < income.Count; ++i)
    {
      if (income[i].type == type)
      {
        ScoreEntry entry = income[i];
        entry.amount += amount;
        income[i] = entry;
        return;
      }
    }
    ScoreEntry e;
    e.type = type;
    e.amount = amount;
    income.Add(e);
  }

  public void AddUpkeep(Resources.Type type, int amount)
  {
    for (int i = 0; i < upkeep.Count; ++i)
    {
      if (upkeep[i].type == type)
      {
        ScoreEntry entry = upkeep[i];
        entry.amount -= amount;
        upkeep[i] = entry;
        return;
      }
    }
    ScoreEntry e;
    e.type = type;
    e.amount = -amount;
    upkeep.Add(e);
  }

}
