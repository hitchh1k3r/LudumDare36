using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FadeIn : MonoBehaviour
{

  public float fadeDuration = 1.0f;

  private AudioSource audio;
  private float targetVolume;
  private float timer;

  void Awake()
  {
    audio = GetComponent<AudioSource>();
    targetVolume = audio.volume;
    audio.volume = 0;
  }

  void LateUpdate()
  {
    if (timer < fadeDuration)
    {
      timer += Time.deltaTime;
      float t = timer / fadeDuration;
      if (t > 1)
      {
        audio.volume = t * targetVolume;
      }
    }
  }

}
