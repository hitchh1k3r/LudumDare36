using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitConfirm : MonoBehaviour
{

  public bool restart;
  public string message = "Are you sure you want to exit?";

  public ScoreTracker scores;
  public Text text;
  public Text options;

  void OnEnable()
  {
    scores.isSummaryShowing = true;
    text.text = message;
    if (restart)
    {
      options.text = "- Press ESC to Exit Game -     - Press SPACE to Restart -";
    }
    else
    {
      options.text = "- Press ESC to Continue -     - Press SPACE to Exit Game -";
    }
  }

  void LateUpdate()
  {
    if (restart)
    {
      if (Input.GetButtonUp("Activate"))
      {
        Application.LoadLevel(Application.loadedLevel);
      }
      else if (Input.GetButtonUp("Cancel"))
      {
        Application.Quit();
      }
    }
    else
    {
      if (Input.GetButtonUp("Activate"))
      {
        Application.Quit();
      }
      else if (Input.GetButtonUp("Cancel"))
      {
        scores.isSummaryShowing = false;
        gameObject.SetActive(false);
      }
    }
  }

  void OnDisable()
  {
    scores.isSummaryShowing = false;
  }

}
