using UnityEngine;
using System.Collections;

public class PitUpkeep : MonoBehaviour
{

  public GameObject revertState;

  void Upkeep()
  {
    if (Resources.instance.animal < Resources.instance.animalMax)
    {
      GameTile tile = GetComponent<GameTile>();
      TileGrid.instance.SetTile(tile.x, tile.y, revertState);
      ScoreTracker.instance.AddIncome(Resources.Type.ANIMAL, 1);
      ++Resources.instance.animal;
    }
  }

}
