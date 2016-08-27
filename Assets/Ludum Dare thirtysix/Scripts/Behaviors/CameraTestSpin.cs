using UnityEngine;
using System.Collections;

public class CameraTestSpin : MonoBehaviour
{

  public float transformTime = 1;

  private float angle;
  private float timer;
  private Coroutine spin;

  void OnEnable()
  {
    angle = transform.eulerAngles.y;
  }

  void Update()
  {
    if (spin == null)
    {
      spin = StartCoroutine(SpinTo(angle + 90));
    }
  }

  IEnumerator SpinTo(float target)
  {
    timer = 0;
    while (timer < transformTime)
    {
      timer += Time.deltaTime;
      transform.rotation = Quaternion.Euler(0, Ease.QuadInOut(angle, target, timer / transformTime), 0);
      yield return null;
    }
    transform.rotation = Quaternion.Euler(0, target, 0);
    angle = target;
    spin = null;
  }

}
