using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class AnimalsInWorld : MonoBehaviour
{

  public AnimalsInWorld supressAnimal;

  [System.NonSerialized]
  public bool showing;

  private static int showCount;
  private bool canShow = true;
  private MeshRenderer mesh;

  void OnEnable()
  {
    mesh = GetComponent<MeshRenderer>();
    if (supressAnimal != null)
    {
      supressAnimal.canShow = false;
      mesh.enabled = true;
      showing = true;
    }
    else
    {
      mesh.enabled = false;
    }
  }

  void OnDisable()
  {
    if (supressAnimal != null)
    {
      supressAnimal.canShow = true;
      mesh.enabled = false;
      showing = false;
    }
  }

  public void KillAnimal()
  {
    if (supressAnimal != null)
    {
      gameObject.SetActive(false);
    }
    else
    {
      showing = false;
      --showCount;
      mesh.enabled = false;
      Resources.instance.animal = Mathf.Max(Resources.instance.animal - 1, 0);
      ScoreTracker.instance.AddLoss(Resources.Type.ANIMAL, 1);
    }
  }

  void WorkingToggle()
  {
    Update();
  }

  void Update()
  {
    if (supressAnimal == null)
    {
      int animals = Resources.instance.animal;
      if (showing)
      {
        if (animals < showCount || !canShow)
        {
          showing = false;
          --showCount;
          mesh.enabled = false;
        }
      }
      else
      {
        if (animals > showCount && canShow)
        {
          showing = true;
          ++showCount;
          mesh.enabled = true;
        }
      }
    }
  }

}
