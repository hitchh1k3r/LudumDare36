using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{

  public static CameraMover instance;

  public TileGrid tiles;
  public Transform camera;
  public float spinTime = 0.5f;
  public float moveSpeed = 10;

  private Coroutine spin;

  void OnEnable()
  {
    instance = this;
  }

  void Update()
  {
    if (!ScoreTracker.instance.isSummaryShowing)
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
      if (transform.position.x < tiles.transform.position.x)
      {
        transform.position = new Vector3(tiles.transform.position.x, transform.position.y, transform.position.z);
      }
      if (transform.position.x > tiles.transform.position.x + tiles.width - 1)
      {
        transform.position = new Vector3(tiles.transform.position.x + tiles.width - 1, transform.position.y, transform.position.z);
      }
      if (transform.position.z < tiles.transform.position.z - tiles.height + 1)
      {
        transform.position = new Vector3(transform.position.x, transform.position.y, tiles.transform.position.z - tiles.height + 1);
      }
      if (transform.position.z > tiles.transform.position.z)
      {
        transform.position = new Vector3(transform.position.x, transform.position.y, tiles.transform.position.z);
      }

      if (Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Horizontal") < 0.1f &&
          Input.GetAxis("Vertical") > -0.1f && Input.GetAxis("Vertical") < 0.1f)
      {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)), 10 * Time.deltaTime);
      }
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
