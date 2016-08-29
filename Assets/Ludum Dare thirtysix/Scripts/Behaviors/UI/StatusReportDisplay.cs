using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusReportDisplay : MonoBehaviour
{
  public ScoreTracker scoreTrackerReference;
  public GameObject statusReportUi;
  
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
    ClearScreenData();
    BuildScreenData();
  }
  void Update()
  {
    if(Input.GetButton("Click") || Input.GetButton("Activate") || Input.GetButton("Cancel"))
    {
      scoreTrackerReference.isSummaryShowing = false;
      statusReportUi.SetActive(false);
    }
  }
  void OnDisable()
  {
    scoreTrackerReference.isSummaryShowing = false;
  }
  void ClearScreenData()
  {
    lostText.left.text = "";
    lostText.right.text = "";
    capturedText.left.text = "";
    capturedText.right.text = "";    
    upkeepText.left.text = "";
    upkeepText.right.text = "";    
    incomeText.left.text = "";
    incomeText.right.text = "";    
  }
  void BuildEntry(List<ScoreTracker.ScoreEntry> seList, TwoColumnText display)
  {
    string colorModifier;
    int counter = 0;
    foreach(ScoreTracker.ScoreEntry entry in seList)
    {
      colorModifier = "";
      if(entry.amount < 0)
      {
        colorModifier = "<color=#" + badColor + ">";
      }
      else if(entry.amount > 0)
      {
        colorModifier += "<color=#" + goodColor + ">+";
      }
      if(counter % 2 == 0)
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
