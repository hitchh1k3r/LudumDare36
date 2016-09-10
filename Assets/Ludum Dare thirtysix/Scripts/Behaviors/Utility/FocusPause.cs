using UnityEngine;
using System.Collections;

public class FocusPause : MonoBehaviour
{

  void OnApplicationFocus(bool focus)
  {
    if (!ScoreTracker.instance.isSummaryShowing && !focus)
    {
      DialogueManager.ShowMessage("Game Paused");
    }
  }

}
