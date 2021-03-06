﻿using UnityEngine;

public class ShowForBuild : MonoBehaviour
{

  public GameObject[] showHides;

  public static bool endingHide;

  private bool stateTracker;

  void OnEnable()
  {
    endingHide = false;
  }

  void Update()
  {
    bool state = !ScoreTracker.instance.isSummaryShowing && (RoundManager.instance.stage == RoundManager.RoundStage.DAWN || RoundManager.instance.stage == RoundManager.RoundStage.DUSK) && !endingHide;
    if (stateTracker != state)
    {
      stateTracker = state;
      foreach (GameObject go in showHides)
      {
        go.SetActive(state);
      }
    }
  }

}
