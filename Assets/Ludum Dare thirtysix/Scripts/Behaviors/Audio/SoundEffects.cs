using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour
{

  private static SoundEffects instance;
  private AudioSource soundSource;

  void OnEnable()
  {
    instance = this;
    soundSource = GetComponent<AudioSource>();
  }

  public static void PlaySound(AudioClip sound, float volume = 1)
  {
    instance.soundSource.PlayOneShot(sound, volume);
  }

}
