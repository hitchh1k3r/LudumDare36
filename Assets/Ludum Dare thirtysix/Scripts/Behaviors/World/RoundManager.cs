using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{

  public static List<IStageListener> stateListners = new List<IStageListener>();
  public static RoundManager instance;

  public float buildTime = 90;
  public float mobTime = 30;

  public RoundStage stage = RoundStage.DAWN;
  public Text dayText;
  public Text timeValue;
  public int dayCounter = 1;
  public float debugScale = 1;

  public Transform entityContainer;
  public GameObject indicatorContainer;
  public GameObject wolfIndicator;
  public GameObject deerIndicator;

  public Transform lightContainer;
  public Color[] dayPallet = { new Color(1, 0, 0), new Color(1, 0, 0), new Color(1, 0, 0), new Color(1, 0, 0) };
  public Color[] nightPallet = { new Color(1, 0, 0), new Color(1, 0, 0), new Color(1, 0, 0), new Color(1, 0, 0) };
  public Light dayLight;
  public Light nightLight;

  [System.NonSerialized]
  public float timeLeft;

  private  Vector3 eulers;
  private Color dayColor;
  private Color nightColor;
  private bool init;
  private bool shortCircuit;

  void OnEnable()
  {
    instance = this;

    timeLeft = buildTime * 2;
    dayColor = dayPallet[3];
    nightColor = nightPallet[3];
    eulers = lightContainer.eulerAngles;
    StartCoroutine(PhaseTo(90, dayPallet[0], nightPallet[0]));

    dayLight.color = dayColor;
    nightLight.color = nightColor;
  }

  void Update()
  {
    if (!init)
    {
      init = true;
      AddIndicators(true);
      AddIndicators(false);
    }

    if (!ScoreTracker.instance.isSummaryShowing)
    {
      shortCircuit = false;
      if (stage == RoundStage.DAWN || stage == RoundStage.DUSK)
      {
        shortCircuit = (Resources.instance.person == 0 || Input.GetButton("Activate"));
      }
      else
      {
        shortCircuit = (UnitMover.moverCount == 0);
      }
      timeLeft -= Time.deltaTime * debugScale * (shortCircuit ? ((timeLeft / 2) + 5) : 1);
      if (timeLeft < 0)
      {
        int p = 0;
        bool build = false;
        switch (stage)
        {
          case RoundStage.DAWN:
            {
              stage = RoundStage.DAY;
              p = 1;
            }
            break;
          case RoundStage.DAY:
            {
              stage = RoundStage.DUSK;
              dayText.text = "Night " + dayCounter;
              build = true;
              p = 2;
            }
            break;
          case RoundStage.DUSK:
            {
              stage = RoundStage.NIGHT;
              p = 3;
            }
            break;
          case RoundStage.NIGHT:
            {
              stage = RoundStage.DAWN;
              ++dayCounter;
              dayText.text = "Day " + dayCounter;
              build = true;
              p = 0;
            }
            break;
        }
        foreach (IStageListener listner in stateListners)
        {
          listner.ChangeStage(stage);
        }
        if (build)
        {
          timeLeft = buildTime;
          int peopleToFeedInit = Resources.instance.personLive;
          int peopleToFeed = Resources.instance.personLive;
          int foodSpent = Resources.instance.food;
          int foodShortage = peopleToFeed - Resources.instance.food;
          if (foodShortage > 0)
          {
            peopleToFeed -= Resources.instance.food;
            Resources.instance.food = 0;
          }
          else
          {
            Resources.instance.food -= peopleToFeed;
            peopleToFeed = 0;
          }

          TileGrid.instance.BroadcastMessage("Upkeep", SendMessageOptions.DontRequireReceiver);
          entityContainer.BroadcastMessage("RoundEnd", SendMessageOptions.DontRequireReceiver);
          AddIndicators(stage == RoundStage.DAWN);

          if (foodShortage > 0)
          {
            foodSpent += Resources.instance.food;
            if (foodShortage - Resources.instance.food > 0)
            {
              foodShortage -= Resources.instance.food;
              int peepsLost = Mathf.CeilToInt(foodShortage / 2.0f);
              ScoreTracker.instance.AddLost(Resources.Type.PERSON, peepsLost);
              if (foodSpent != 0)
              {
                ScoreTracker.instance.AddUpkeep(Resources.Type.FOOD, foodSpent);
              }
              Resources.instance.personLive -= peepsLost;
              MesopotamianGenerator.instance.RemoveFromPool(peepsLost);
              Resources.instance.food = 0;
            }
            else
            {
              Resources.instance.food -= foodShortage;
              if (Resources.instance.personLive != 0)
              {
                ScoreTracker.instance.AddUpkeep(Resources.Type.FOOD, peopleToFeedInit);
              }
            }
          }
          else
          {
            Resources.instance.food -= peopleToFeed;
            if (Resources.instance.personLive != 0)
            {
              ScoreTracker.instance.AddUpkeep(Resources.Type.FOOD, peopleToFeedInit);
            }
          }

          Resources.instance.person = Resources.instance.personLive;
          DialogueManager.StatusReport();
        }
        else
        {
          timeLeft = mobTime;
          RoundData data;
          data.entities = entityContainer;
          data.isNight = stage == RoundStage.NIGHT;
          indicatorContainer.BroadcastMessage("RoundStart", data, SendMessageOptions.DontRequireReceiver);
        }
        StartCoroutine(PhaseTo(90 * (p + 1), dayPallet[p], nightPallet[p]));
      }

      int seconds = Mathf.CeilToInt(timeLeft);
      string time = Mathf.CeilToInt(seconds / 60) + ":";
      seconds %= 60;
      if (seconds < 10)
      {
        time += "0";
      }
      time += seconds;
      timeValue.text = time;
    }
  }

  private void AddIndicators(bool isDay)
  {
    GameObject indicator;
    int count = Random.Range(Mathf.Max(dayCounter - 5, 1), dayCounter);
    if (isDay)
    {
      indicator = wolfIndicator;
      count = Mathf.CeilToInt(count * 0.5f);
    }
    else
    {
      indicator = deerIndicator;
    }
    if (count > 10)
    {
      count = 10;
    }
    for (int i = 0; i < count; ++i)
    {
      int side = Random.Range(0, 4);
      int pos;
      if (side % 2 == 0)
      {
        pos = Random.Range(0, TileGrid.instance.width - 1);
      }
      else
      {
        pos = Random.Range(0, TileGrid.instance.height - 1);
      }
      Transform indTran = Instantiate(indicator).transform;
      indTran.SetParent(indicatorContainer.transform);
      indTran.localRotation = Quaternion.Euler(0, 90 * side, 0);
      switch (side)
      {
        case 1:
          {
            indTran.localPosition = new Vector3(0, 0, -pos - 0.5f);
          }
          break;
        case 0:
          {
            indTran.localPosition = new Vector3(pos + 0.5f, 0, -TileGrid.instance.height + 1);
          }
          break;
        case 3:
          {
            indTran.localPosition = new Vector3(TileGrid.instance.width - 1, 0, -pos - 0.5f);
          }
          break;
        case 2:
          {
            indTran.localPosition = new Vector3(pos + 0.5f, 0, 0);
          }
          break;
      }
    }
  }

  IEnumerator PhaseTo(float targetX, Color targetDayColor, Color targetNightColor)
  {
    float time = timeLeft;
    float lastTime = timeLeft;
    while (timeLeft <= lastTime)
    {
      lastTime = timeLeft;
      float t = 1 - (timeLeft / time);
      float x = Ease.QuadInOut(eulers.x, targetX, t);
      float mix = Ease.QuadInOut(0, 1, t);
      lightContainer.localRotation = Quaternion.Euler(x, eulers.y, eulers.z);
      if (x > 160 && x < 200)
      {
        dayLight.shadowStrength = ((200 - x) * 0.025f);
      }
      else if (x > 340)
      {
        dayLight.shadowStrength = 0.5f + (x - 360) * 0.025f;
      }
      else if (x < 20)
      {
        dayLight.shadowStrength = 0.5f + ((x) * 0.025f);
      }
      else if (x > 0 && x < 180)
      {
        dayLight.shadowStrength = 1;
      }
      else
      {
        dayLight.shadowStrength = 0;
      }
      nightLight.shadowStrength = 1 - dayLight.shadowStrength;
      dayLight.color = ((1 - mix) * dayColor) + (mix * targetDayColor);
      nightLight.color = ((1 - mix) * nightColor) + (mix * targetNightColor);
      yield return null;
    }

    targetX %= 360;
    eulers = new Vector3(targetX, eulers.y, eulers.z);
    dayColor = targetDayColor;
    nightColor = targetNightColor;
    lightContainer.localRotation = Quaternion.Euler(eulers);
    dayLight.color = dayColor;
    nightLight.color = nightColor;
  }

  public RoundStage GetFutureStage(float lookAhead)
  {
    if (timeLeft < lookAhead * (shortCircuit ? 10 : 1))
    {
      switch (stage)
      {
        case RoundStage.DAWN:
          return RoundStage.DAY;
        case RoundStage.DAY:
          return RoundStage.DUSK;
        case RoundStage.DUSK:
          return RoundStage.NIGHT;
        case RoundStage.NIGHT:
          return RoundStage.DAWN;
      }
    }

    return stage;
  }

  public enum RoundStage
  {
    DAWN,
    DAY,
    DUSK,
    NIGHT
  }

  public struct RoundData
  {
    public bool isNight;
    public Transform entities;
  }

}
