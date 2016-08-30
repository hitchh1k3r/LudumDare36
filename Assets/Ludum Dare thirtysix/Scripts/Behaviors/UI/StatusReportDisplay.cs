using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusReportDisplay : MonoBehaviour
{
  public ScoreTracker scoreTrackerReference;

  public TwoColumnText lostText;
  public TwoColumnText capturedText;
  public TwoColumnText upkeepText;
  public TwoColumnText incomeText;

  public string badColor = "9D3C3CFF";
  public string goodColor = "5F904EFF";

  [System.Serializable]
  public struct TwoColumnText
  {
    public Text left;
    public Text right;
  }

  void OnEnable()
  {
    scoreTrackerReference.isSummaryShowing = true;
    BuildScreenData();
    ClearData();
  }

  void LateUpdate()
  {
    if (Input.GetButtonUp("Activate") || Input.GetButtonUp("Cancel"))
    {
      if (Resources.instance.personLive <= 0)
      {
        DialogueManager.Confirm("All of your villagers have died.\n\nWould you like to try again?", true);
      }

      scoreTrackerReference.isSummaryShowing = false;
      gameObject.SetActive(false);
    }
  }

  void OnDisable()
  {
    scoreTrackerReference.isSummaryShowing = false;
  }

  void ClearData()
  {
    scoreTrackerReference.lost.Clear();
    scoreTrackerReference.captured.Clear();
    scoreTrackerReference.upkeep.Clear();
    scoreTrackerReference.income.Clear();
  }

  void BuildEntry(List<ScoreTracker.ScoreEntry> seList, TwoColumnText display)
  {
    display.left.text = "";
    display.right.text = "";
    string colorModifier;
    int counter = 0;
    foreach (ScoreTracker.ScoreEntry entry in seList)
    {
      colorModifier = "";
      if (entry.amount < 0)
      {
        colorModifier = "<color=#" + badColor + ">";
      }
      else if (entry.amount > 0)
      {
        colorModifier += "<color=#" + goodColor + ">+";
      }
      if (counter % 2 == 1)
      {
        display.right.text += colorModifier + entry.amount + "   " + Resources.GetName(entry.type) + "</color>\n";
      }
      else
      {
        display.left.text += colorModifier + entry.amount + "   " + Resources.GetName(entry.type) + "</color>\n";
      }
      counter++;
    }
  }

  void BuildScreenData()
  {
    BuildEntry(scoreTrackerReference.lost, lostText);
    BuildEntry(scoreTrackerReference.captured, capturedText);
    BuildEntry(scoreTrackerReference.upkeep, upkeepText);
    BuildEntry(scoreTrackerReference.income, incomeText);
  }

}
