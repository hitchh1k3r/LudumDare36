using UnityEngine;
using System.Collections;

public class HouseUpkeep : MonoBehaviour
{

  void Upkeep()
  {
    if (Resources.instance.personLive < Resources.instance.personMax)
    {
      if (Random.Range(0, 5) == 0)
      {
        ++Resources.instance.personLive;
        ScoreTracker.instance.AddIncome(Resources.Type.PERSON, 1);
      }
    }
  }

}
