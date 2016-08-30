using UnityEngine;
using System.Collections;

public class QuitListener : MonoBehaviour
{

  public ScoreTracker scoreTracker;

  void LateUpdate()
  {
    if (!scoreTracker.isSummaryShowing && Input.GetButtonUp("Cancel"))
    {
      DialogueManager.Confirm("Are you sure you want to quit?", false);
    }
  }

}