using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour
{

  public Text name, description, costs;

  void OnEnable()
  {
    Vector3 pos = Input.mousePosition;
    bool upward = pos.y < 200;
    pos.x /= Screen.width;
    pos.y /= Screen.height;
    ((RectTransform)transform).anchorMin = new Vector2(pos.x, pos.y);
    ((RectTransform)transform).anchorMax = new Vector2(pos.x, pos.y);
    ((RectTransform)transform).anchoredPosition = new Vector3(0, upward ? 150 : 0);
  }

  void Update()
  {
    Vector3 pos = Input.mousePosition;
    bool upward = pos.y < 200;
    pos.x /= Screen.width;
    pos.y /= Screen.height;
    ((RectTransform)transform).anchorMin = new Vector2(pos.x, pos.y);
    ((RectTransform)transform).anchorMax = new Vector2(pos.x, pos.y);
    ((RectTransform)transform).anchoredPosition = new Vector3(0, upward ? 150 : 0);

    if (RoundManager.instance.stage == RoundManager.RoundStage.DAY || RoundManager.instance.stage == RoundManager.RoundStage.NIGHT || ScoreTracker.instance.isSummaryShowing)
    {
      gameObject.SetActive(false);
    }
  }

}
