using UnityEngine;

public class ShowForBuild : MonoBehaviour
{

  public GameObject[] showHides;

  private bool stateTracker;

  void Update()
  {
    bool state = !ScoreTracker.instance.isSummaryShowing && (RoundManager.instance.stage == RoundManager.RoundStage.DAWN || RoundManager.instance.stage == RoundManager.RoundStage.DUSK);
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
