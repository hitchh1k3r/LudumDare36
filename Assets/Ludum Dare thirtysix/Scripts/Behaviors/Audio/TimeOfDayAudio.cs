using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class TimeOfDayAudio : MonoBehaviour, IStageListener
{

  public AudioBreakPoint[] breakingPoints;
  public RoundVolume[] roundVolumes;

  private AudioSource audio;
  private float baseVolume;
  private int breakIndex;

  private ChangeStyle changeStyle = ChangeStyle.WAIT_FOR_END;
  private float fadeDuration;

  private float oldVolume;
  private float targetVolume;
  private float timer;

  private bool init;

  void OnEnable()
  {
    audio = GetComponent<AudioSource>();
    System.Array.Sort(breakingPoints, new AudioBreakPoint.Comparer());
    breakIndex = -1;
    baseVolume = audio.volume;
    audio.volume = 0;
    RoundManager.stateListners.Add(this);
    audio.mute = false;
  }

  void OnDisable()
  {
    RoundManager.stateListners.Remove(this);
  }

  void Update()
  {
    if (!init)
    {
      init = true;
      ChangeStage(RoundManager.instance.stage);
    }

    int lastBreak = breakIndex;
    for (int i = 0; i < breakingPoints.Length; ++i)
    {
      if (audio.time > breakingPoints[i].timeIndex)
      {
        breakIndex = i;
      }
    }

    if (lastBreak != breakIndex)
    {
      if (changeStyle == ChangeStyle.WAIT_FOR_END)
      {
        float earlyTime = 0;
        foreach (EarlyTrigger early in breakingPoints[breakIndex].earlyTriggers)
        {
          if (RoundManager.instance.stage == early.round)
          {
            earlyTime = early.earlyTime;
            break;
          }
        }
        RoundManager.RoundStage stagePreempt = RoundManager.instance.GetFutureStage(earlyTime);
        ChangeStage(stagePreempt);
      }

      if (changeStyle == ChangeStyle.WAIT_FOR_END)
      {
        audio.volume = targetVolume;
      }

      changeStyle = breakingPoints[breakIndex].changeStyle;
      fadeDuration = breakingPoints[breakIndex].fadeDuration;
      oldVolume = audio.volume;
      timer = 0;
    }

    if (changeStyle == ChangeStyle.CROSS_FADE)
    {
      if (timer < fadeDuration)
      {
        timer += Time.deltaTime;
        if (timer >= fadeDuration)
        {
          audio.volume = targetVolume;
        }
        else
        {
          float t = timer / fadeDuration;
          audio.volume = (1 - t) * oldVolume + t * targetVolume;
        }
      }
    }
  }

  public void ChangeStage(RoundManager.RoundStage stage)
  {
    if (changeStyle == ChangeStyle.CROSS_FADE)
    {
      oldVolume = audio.volume;
      timer = 0;
    }

    targetVolume = baseVolume;
    foreach (RoundVolume roundVolume in roundVolumes)
    {
      if (roundVolume.round == stage)
      {
        targetVolume = roundVolume.volume * baseVolume;
      }
    }
  }

  [System.Serializable]
  public struct AudioBreakPoint
  {
    public ChangeStyle changeStyle;
    public float timeIndex;
    public float fadeDuration;
    public EarlyTrigger[] earlyTriggers;

    public class Comparer : IComparer<AudioBreakPoint>
    {

      public int Compare(AudioBreakPoint x, AudioBreakPoint y)
      {
        return Comparer<float>.Default.Compare(x.timeIndex, y.timeIndex);
      }

    }
  }

  [System.Serializable]
  public struct EarlyTrigger
  {
    public RoundManager.RoundStage round;
    public float earlyTime;
  }

  [System.Serializable]
  public struct RoundVolume
  {
    public RoundManager.RoundStage round;
    public float volume;
  }

  public enum ChangeStyle
  {
    CROSS_FADE,
    WAIT_FOR_END
  }

}
