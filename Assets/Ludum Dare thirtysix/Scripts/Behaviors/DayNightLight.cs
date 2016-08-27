using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class DayNightLight : MonoBehaviour
{

  public float transformTime = 5;
  public int phaseIndex;
  public Color[] phaseColors = { new Color(1, 0, 0) };

  private Light light;
  private float timer;
  private bool running;
  private Vector3 eulers;
  private Color color;

  void Awake()
  {
    light = GetComponent<Light>();
  }

  void OnEnable()
  {
    eulers = transform.eulerAngles;
    color = light.color;
  }

  void Update()
  {
    if (!running)
    {
      running = true;
      ++phaseIndex;
      phaseIndex %= phaseColors.Length;
      StartCoroutine(PhaseTo(eulers.x + 90, phaseColors[phaseIndex]));
    }
  }

  IEnumerator PhaseTo(float targetX, Color targetColor)
  {
    timer = 0;
    while (timer < transformTime)
    {
      timer += Time.deltaTime;
      float x = Ease.QuadInOut(eulers.x, targetX, timer / transformTime);
      if (x > 160 && x < 200)
      {
        light.shadowStrength = ((190 - x) * 0.025f);
      }
      else if (x > 340)
      {
        light.shadowStrength = 0.5f + (x - 360) * 0.025f;
      }
      else if (x < 20)
      {
        light.shadowStrength = 0.5f + ((x) * 0.025f);
      }
      transform.rotation = Quaternion.Euler(x, eulers.y, eulers.z);
      float mix = Ease.QuadInOut(0, 1, timer / transformTime);
      light.color = ((1 - mix) * color) + (mix * targetColor);
      yield return null;
    }

    targetX %= 360;
    eulers = new Vector3(targetX, eulers.y, eulers.z);
    color = targetColor;
    transform.rotation = Quaternion.Euler(eulers);
    light.color = color;
    running = false;
  }

}
