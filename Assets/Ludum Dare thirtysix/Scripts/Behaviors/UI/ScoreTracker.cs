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

}
