using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{

  public Transform camera;
  public float spinTime = 0.5f;
  public float moveSpeed = 10;

  private Coroutine spin;

  void Update()
  {
    transform.position += moveSpeed * Time.deltaTime * (Quaternion.Euler(0, camera.eulerAngles.y, 0) * (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward));
    if (spin == null)
    {
      if (Input.GetAxis("Rotate") < -0.5f)
      {
        spin = StartCoroutine(SpinTo(transform.eulerAngles.y + 90));
      }
      else if (Input.GetAxis("Rotate") > 0.5f)
      {
        spin = StartCoroutine(SpinTo(transform.eulerAngles.y - 90));
      }
    }
    if (Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Horizontal") < 0.1f &&
        Input.GetAxis("Vertical") > -0.1f && Input.GetAxis("Vertical") < 0.1f)
    {
      transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)), 10 * Time.deltaTime);
    }
  }

  IEnumerator SpinTo(float target)
  {
    float angle = transform.eulerAngles.y;
    float timer = 0;
    while (timer < spinTime)
    {
      timer += Time.deltaTime;
      transform.rotation = Quaternion.Euler(0, Ease.QuadInOut(angle, target, timer / spinTime), 0);
      yield return null;
    }
    transform.rotation = Quaternion.Euler(0, target, 0);
    spin = null;
  }


}
