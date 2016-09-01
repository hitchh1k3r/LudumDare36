using UnityEngine;
using System.Collections;

public class MusicSync : MonoBehaviour
{

  public AudioSource sourceTrack;
  public AudioSource[] matchTracks;

  void Update()
  {
    int time = sourceTrack.timeSamples;
    foreach (AudioSource track in matchTracks)
    {
      if (time < track.clip.samples)
      {
        track.timeSamples = time;
        if (!track.isPlaying)
        {
          track.Play();
        }
      }
    }
  }

}
