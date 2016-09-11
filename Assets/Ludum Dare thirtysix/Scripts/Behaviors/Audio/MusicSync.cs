using UnityEngine;
using System.Collections;

public class MusicSync : MonoBehaviour
{

  public AudioSource sourceTrack;
  public AudioSource[] matchTracks;

  public int lastInterval = -1;

  void Update()
  {
    int time = sourceTrack.timeSamples;
    int interval = time / 22050; // should be 0.5 seconds at 44KHz
    if (lastInterval != interval)
    {
      lastInterval = interval;
      foreach (AudioSource track in matchTracks)
      {
        if (time < track.clip.samples && Mathf.Abs(track.timeSamples - time) > 882) // 1/50th second
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

}
