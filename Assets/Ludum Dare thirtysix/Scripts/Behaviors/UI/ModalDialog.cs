using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModalDialog : MonoBehaviour
{

  public string message = "Hello World!";

  public ScoreTracker scores;
  public Text text;

  void OnEnable()
  {
    scores.isSummaryShowing = true;
    text.text = message;
  }

  void LateUpdate()
  {
    if (Input.GetButtonUp("Activate") || Input.GetButtonUp("Cancel"))
    {
      scores.isSummaryShowing = false;
      gameObject.SetActive(false);
    }
  }

  void OnDisable()
  {
    scores.isSummaryShowing = false;
  }

}
