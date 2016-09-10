using UnityEngine;
using System.Collections;

public class HouseUpkeep : MonoBehaviour
{

  void Upkeep()
  {
    if (Resources.instance.personLive < Resources.instance.personMax)
    {
      if (Random.Range(0, 3) == 0)
      {
        ScoreTracker.instance.AddIncome(Resources.Type.PERSON, 1);
        ++Resources.instance.personLive;
      }
    }
  }

}
