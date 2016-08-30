using UnityEngine;
using System.Collections;

public class TimeOfDayPopper : MonoBehaviour
{

  public bool dayTimeEntity;

  private bool popped;

  void Update()
  {
    if (!popped)
    {
      transform.localScale = Vector3.zero;
      if (dayTimeEntity == (RoundManager.instance.stage == RoundManager.RoundStage.DAWN || RoundManager.instance.stage == RoundManager.RoundStage.DAY))
      {
        StartCoroutine(Pop(0.5f));
        popped = true;
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }

  private IEnumerator Pop(float time)
  {
    float timer = 0;
    while (timer < time)
    {
      timer += Time.deltaTime;
      transform.localScale = Ease.QuadInOut(0, 1, timer / time) * Vector3.one;
      yield return null;
    }
    transform.localScale = Vector3.one;
    Destroy(this);
  }

}
