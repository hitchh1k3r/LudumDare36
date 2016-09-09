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
  private float[] musicBases;
  private float musicFade;

  void OnEnable()
  {
    sourceMatrix = Camera.main.projectionMatrix;
    targetMatrix = Matrix4x4.Perspective(40, Camera.main.aspect, 0.001f, 200.0f);
    ShowForBuild.endingHide = true;
  }

  void Update()
  {
    if (!animating && !ScoreTracker.instance.isSummaryShowing)
    {
      animating = true;
      ScoreTracker.instance.isSummaryShowing = true;
      musicBases = new float[RoundManager.instance.endingMute.Length];
      for (int i = 0; i < RoundManager.instance.endingMute.Length; ++i)
      {
        TimeOfDayAudio toda = RoundManager.instance.endingMute[i].GetComponent<TimeOfDayAudio>();
        if (toda != null)
        {
          toda.enabled = false;
        }
        musicBases[i] = RoundManager.instance.endingMute[i].volume;
      }

      StartCoroutine(EndingAnimation());
    }

    if (animating)
    {
      if (musicFade < 1)
      {
        musicFade += 0.075f * Time.deltaTime;
        if (musicFade >= 1)
        {
          for (int i = 0; i < RoundManager.instance.endingMute.Length; ++i)
          {
            RoundManager.instance.endingMute[i].mute = true;
          }
        }
        else
        {
          for (int i = 0; i < RoundManager.instance.endingMute.Length; ++i)
          {
            RoundManager.instance.endingMute[i].volume = musicBases[i] * (1 - musicFade);
          }
        }
      }

      CameraMover.instance.transform.position = Vector3.Lerp(CameraMover.instance.transform.position, cameraPos, trackingSpeed * Time.deltaTime);

      if (cameraStart)
      {
        rSpeed += 5.0f * Time.deltaTime;
        if (rSpeed > 10.0f)
        {
          rSpeed = 10.0f;
        }
        CameraMover.instance.transform.Rotate(0, rSpeed * Time.deltaTime, 0);
        if (cameraOffset.y > 2)
        {
          cameraOffset -= 0.25f * Time.deltaTime * new Vector3(-15.1f, 15.0f, -15.1f);
          if (cameraOffset.y < 5)
          {
            cameraOffset = new Vector3(-5, 5, -5);
          }
        }
        if (cameraAngle > 10)
        {
          cameraAngle -= Time.deltaTime;
          if (cameraAngle < 10)
          {
            cameraAngle = 10;
          }
        }
        if (matrixTimer < 1)
        {
          matrixTimer += 0.01f * Time.deltaTime;
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
          Camera.main.projectionMatrix = currentMatrix;
        }

        Camera.main.transform.localPosition = cameraOffset;
        Camera.main.transform.localRotation = Quaternion.Euler(cameraAngle, 45, 0);
      }
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
        RoundManager.instance.endAlarm.gameObject.SetActive(true);
        RoundManager.instance.endRumble.gameObject.SetActive(true);
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
    bool endingText = false;
    Vector3 basePosition = transform.position;
    timer = 0;
    while (animating && timer < 13)
    {
      timer += Time.deltaTime;
      float power = (timer * timer * timer * timer) / 1500.0f;
      if (power > 1.1f && !particlesOn)
      {
        particlesOn = true;
        foreach (GameObject go in particles)
        {
          go.SetActive(true);
        }
        RoundManager.instance.endAlarm.gameObject.SetActive(false);
        SoundEffects.PlaySound(RoundManager.instance.engineIgnite);
        RoundManager.instance.endHumm.gameObject.SetActive(true);
      }
      if (power > 4.0f && !endingText)
      {
        endingText = true;
        DialogueManager.ShowMessage("<color=#888>And thus the</color> <color=#ccc>Akkadians</color> <color=#888>left their homeworld,\nand set out to colonize</color> <color=#ccc>Planet Earth.</color>", true);
        StartCoroutine(FadeMusic(RoundManager.instance.endRumble, 0.5f, 5));
        StartCoroutine(FadeMusic(RoundManager.instance.endHumm, 0.4f, 5));
      }
      transform.rotation = Quaternion.Euler(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
      transform.position = basePosition + power * Vector3.up;
      yield return null;
    }

    // jump camera to height, activate people gfx, and display ending dialog
    CameraMover.instance.stars.SetActive(true);
    people.SetActive(true);
    trackingSpeed = 1;
    cameraPos = transform.position + 4 * Vector3.down;
    RoundManager.instance.endMusic.gameObject.SetActive(true);

    while (animating)
    {
      transform.rotation = Quaternion.Euler(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
      if (!ScoreTracker.instance.isSummaryShowing)
      {
        DialogueManager.Confirm("<size=40>Ancient Akkadians</size>\n<size=25><color=#888>A game for Ludum Dare 36\nby HitchH1k3r, Solifuge, and Naali</color>\n\nThank you for playing!</size>", true, true);
      }
      yield return null;
    }
  }

  private IEnumerator FadeMusic(AudioSource music, float newVolume, float time)
  {
    float tM = 1 / time;
    float oldVolume = music.volume;
    float t = 0;

    while (t < 1)
    {
      t += tM * Time.deltaTime;
      if (t < 1)
      {
        music.volume = (1 - t) * oldVolume + t * newVolume;
      }
      yield return null;
    }
    music.volume = newVolume;
  }

}
