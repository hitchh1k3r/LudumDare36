using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusReportDisplay : MonoBehaviour
{
  public ScoreTracker scoreTrackerReference;

  public ThreeColumnText incomeText;
  public ThreeColumnText expenseText;
  public ThreeColumnText lossText;

  [System.Serializable]
  public struct ThreeColumnText
  {
    public Text left;
    public Text middle;
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
    scoreTrackerReference.loss.Clear();
    scoreTrackerReference.namedLoss.Clear();
    scoreTrackerReference.expenses.Clear();
    scoreTrackerReference.income.Clear();
  }

  void BuildEntry(List<ScoreTracker.ScoreEntry> seList, List<ScoreTracker.NamedEntry> neList, ThreeColumnText display, Colorizer colorizer)
  {
    display.left.text = "";
    display.middle.text = "";
    display.right.text = "";
    int counter = 0;
    bool namePeople = false;
    if (neList != null)
    {
      namePeople = (seList.Count + neList.Count <= 9);
    }
    foreach (ScoreTracker.ScoreEntry entry in seList)
    {
      if (entry.amount != 0)
      {
        if (namePeople && entry.amount < 0 && entry.type == Resources.Type.PERSON)
        {
          continue;
        }
        string colorPrefix = "";
        string colorSuffix = "";
        if (entry.amount > 0)
        {
          colorPrefix = colorizer.positivePrefix;
          colorSuffix = colorizer.positiveSuffix;
        }
        else if (entry.amount < 0)
        {
          colorPrefix = colorizer.negativePrefix;
          colorSuffix = colorizer.negativeSuffix;
        }
        string line = colorPrefix + entry.amount + "   " + Resources.GetName(entry.type) + colorSuffix + "\n";
        if (entry.type == Resources.Type.PERSON && (entry.amount < -1 || entry.amount > 1))
        {
          line = colorPrefix + entry.amount + "   " + Resources.GetName(entry.type) + "s" + colorSuffix + "\n";
        }
        if (counter % 3 == 0)
        {
          display.left.text += line;
        }
        else if (counter % 3 == 1)
        {
          display.middle.text += line;
        }
        else
        {
          display.right.text += line;
        }
        counter++;
      }
      if (counter > 9)
      {
        break;
      }
    }
    if (neList != null)
    {
      foreach (ScoreTracker.NamedEntry entry in neList)
      {
        if (entry.count != 0)
        {
          if (!namePeople && entry.type == ScoreTracker.NamedLossType.PERSON)
          {
            continue;
          }
          string colorPrefix = "";
          string colorSuffix = "";
          if (entry.count > 0)
          {
            colorPrefix = colorizer.positivePrefix;
            colorSuffix = colorizer.positiveSuffix;
          }
          else if (entry.count < 0)
          {
            colorPrefix = colorizer.negativePrefix;
            colorSuffix = colorizer.negativeSuffix;
          }
          string line = "";
          if (entry.count < -1 || entry.count > 1)
          {
            line = colorPrefix + entry.count + "   " + entry.name + "s" + colorSuffix + "\n";
          }
          else
          {
            line = colorPrefix + entry.name + colorSuffix + "\n";
          }
          if (counter % 3 == 0)
          {
            display.left.text += line;
          }
          else if (counter % 3 == 1)
          {
            display.middle.text += line;
          }
          else
          {
            display.right.text += line;
          }
          counter++;
        }
        if (counter > 9)
        {
          break;
        }
      }
    }
  }

  void BuildScreenData()
  {
    BuildEntry(scoreTrackerReference.income, null, incomeText, Colorizer.income);
    BuildEntry(scoreTrackerReference.expenses, null, expenseText, Colorizer.expenses);
    BuildEntry(scoreTrackerReference.loss, scoreTrackerReference.namedLoss, lossText, Colorizer.losses);
  }

  public struct Colorizer
  {
    public static string badColor = "9D3C3CFF";
    public static string goodColor = "5F904EFF";
    public static string maxCapColor = "CCCC00FF";

    public static Colorizer income = new Colorizer("<color=#" + goodColor + ">+", "</color>", "", "");
    public static Colorizer expenses = new Colorizer("<color=#" + goodColor + ">+", "</color>", "<color=#" + badColor + ">", "</color>");
    public static Colorizer losses = new Colorizer("<color=#" + maxCapColor + ">", "</color>", "<color=#" + badColor + ">", "</color>");

    public readonly string positivePrefix, positiveSuffix;
    public readonly string negativePrefix, negativeSuffix;

    private Colorizer(string positivePrefix, string positiveSuffix, string negativePrefix, string negativeSuffix)
    {
      this.positivePrefix = positivePrefix;
      this.positiveSuffix = positiveSuffix;
      this.negativePrefix = negativePrefix;
      this.negativeSuffix = negativeSuffix;
    }
  }

}
