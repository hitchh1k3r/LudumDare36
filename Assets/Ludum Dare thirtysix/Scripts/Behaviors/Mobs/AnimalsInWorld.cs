using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class AnimalsInWorld : MonoBehaviour
{

  public AnimalsInWorld supressAnimal;

  private static int showCount;
  private bool showing;
  private bool canShow = true;
  private MeshRenderer mesh;

  void OnEnable()
  {
    mesh = GetComponent<MeshRenderer>();
    if (supressAnimal != null)
    {
      supressAnimal.canShow = false;
      mesh.enabled = true;
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
