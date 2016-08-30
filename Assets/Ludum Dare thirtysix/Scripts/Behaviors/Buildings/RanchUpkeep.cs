using UnityEngine;
using System.Collections;

public class RanchUpkeep : MonoBehaviour
{

  void Upkeep()
  {
    if (Resources.instance.animal > 2 && Resources.instance.animal < Resources.instance.animalMax)
    {
      if (Random.Range(0, 10) == 0)
      {
        ++Resources.instance.animal;
        ScoreTracker.instance.AddIncome(Resources.Type.ANIMAL, 1);
      }
    }
  }

}
