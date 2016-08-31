using UnityEngine;
using System.Collections;

public class GameEnding : MonoBehaviour
{

  public GameObject[] underground;
  public GameObject[] particles;
  public GameObject people;

  public bool animating;
  private float timer;
  private Vector3 cameraPos;
  private float trackingSpeed = 2;

  private bool cameraStart;

  private Vector3 cameraOffset = new Vector3(-15.1f, 15.0f, -15.1f);
  private float cameraAngle = 35;
  private Matrix4x4 sourceMatrix, targetMatrix, currentMatrix;
  private float matrixTimer;
  private float rSpeed;

  void OnEnable()
  {
    sourceMatrix = Camera.main.projectionMatrix;
    targetMatrix = Matrix4x4.Perspective(40, Camera.main.aspect, 0.001f, 100.0f);
  }

  void Update()
  {
    if (!animating)
    {
      animating = true;
      ScoreTracker.instance.isSummaryShowing = true;
      StartCoroutine(EndingAnimation());
    }

    CameraMover.instance.transform.position = Vector3.Lerp(CameraMover.instance.transform.position, cameraPos, trackingSpeed * Time.deltaTime);

    if (cameraStart)
    {
      rSpeed += 0.25f * Time.deltaTime;
      if (rSpeed > 10)
      {
        rSpeed = 10;
      }
      CameraMover.instance.transform.Rotate(0, rSpeed * Time.deltaTime, 0);
      if (cameraOffset.y > 2)
      {
        cameraOffset -= 0.1f * Time.deltaTime * new Vector3(-15.1f, 15.0f, -15.1f);
        if (cameraOffset.y < 5)
        {
          cameraOffset = new Vector3(-5, 5, -5);
        }
      }
      if (cameraAngle > 10)
      {
        cameraAngle -= 0.2f * Time.deltaTime;
        if (cameraAngle < 10)
        {
          cameraAngle = 10;
        }
      }
      if (matrixTimer < 1)
      {
        matrixTimer += 0.001f * Time.deltaTime;
        float t = matrixTimer;
        if (t > 1)
        {
          t = 1;
        }
        for (int u = 0; u < 4; ++u)
        {
          for (int v = 0; v < 4; ++v)
          {
            currentMatrix[u, v] = (1 - t) * sourceMatrix[u, v] + t * targetMatrix[u, v];
          }
        }
      }

      Camera.main.projectionMatrix = currentMatrix;
      Camera.main.transform.localPosition = cameraOffset;
      Camera.main.transform.localRotation = Quaternion.Euler(cameraAngle, 45, 0);
    }
  }

  private IEnumerator EndingAnimation()
  {
    timer = 0;
    DialogueManager.instance.leftStatus.SetActive(false);

    // shake for 15 seconds
    cameraPos = transform.position + 2 * Vector3.up;
    while (animating && timer < 5)
    {
      timer += Time.deltaTime;
      float power = timer / 5.0f;
      if (timer > 2.5f)
      {
        // start zooming in
        cameraStart = true;
      }
      transform.rotation = Quaternion.Euler(Random.Range(-power, power), 0, Random.Range(-power, power));
      yield return null;
    }

    // unhide the base
    foreach (GameObject go in underground)
    {
      go.SetActive(true);
    }

    // fly away
    bool particlesOn = false;
    Vector3 basePosition = transform.position;
    timer = 0;
    while (animating && timer < 15)
    {
      timer += Time.deltaTime;
      float power = (timer * timer * timer * timer) / 1500.0f;
      if (power > 1 && !particlesOn)
      {
        particlesOn = true;
        foreach (GameObject go in particles)
        {
          go.SetActive(true);
        }
      }
      transform.rotation = Quaternion.Euler(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
      transform.position = basePosition + power * Vector3.up;
      yield return null;
    }

    // jump camera to height, activate people gfx, and display ending dialog
    people.SetActive(true);
    trackingSpeed = 3;
    cameraPos = transform.position + 3 * Vector3.down;
    DialogueManager.Confirm("<size=40>Ancient Akkadians</size>\n<size=25><color=#888>A game for Ludum Dare 36\nby HitchH1k3r, Solifuge, and Naali</color>\n\nThank you for playing!</size>", true);

    while (animating)
    {
      transform.rotation = Quaternion.Euler(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
      yield return null;
    }
  }

}
