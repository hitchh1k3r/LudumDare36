using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour
{

  [System.NonSerialized]
  public bool working; // Tile is currently being Worked.

  public int workerCount; // Number of Workers required to Work this tile. Cannot be Worked if <= 0.
  public bool canDemolish; // Tile can be demolished.

  public Reaction[] reactions; // Reactions for this GameTile; evaluated sequentially on End of Day.
  
  public void ClickTile()
  {
    if (enabled && !ScoreTracker.instance.isSummaryShowing)
    {
      working = !working;
      foreach (SpecialStates child in GetComponentsInChildren<SpecialStates>(true))
      {
        if (child.activeOnWork)
        {
          child.gameObject.SetActive(working);
        }
      }

      Color highlightColor = HighlightEffect.GetColor(gameObject);
      HighlightEffect.RemoveHighlight(gameObject);
      HighlightEffect.AddHighlight(gameObject, highlightColor);
    }
  }

  // (Dale) Reactions contain the logic for how GameTiles generate Resources during gameplay.
  // ReactionCheck is called at the end of the Day; it iterates through each Reaction in each GameTile, and each Cost in that Reaction.
  // If all conditions are met, the Reaction changes Global Resources and optionally replaces itself with a new type of GameTile.
  // Because they evaluate sequentially, GameTiles and their Reactions are prioritized in order.

  public void ReactionCheck()
  {
    GameObject global = GameObject.Find("GLOBAL"); // (Dale) TODO: THIS SHOULD BE REPLACED WITH A MORE EFFICIENT METHOD OR A CACHED VALUE, BUT I DUNNO WHERE TO PUT IT YET!
    Resources resources = global.GetComponent<Resources>();

    for (int r = 0; r < reactions.Length; r++) // foreach (Reaction check in reactions);
    {
      int successfulCostChecks = 0; // Number of Cost Checks that evaluated True. If >= check.productionCost.Length, then all Costs are currently Met.
      
      if (reactions[r].workerRequired && working)
      {
        for (int p = 0; p < p.Length; r++) // foreach (Reaction check in reactions);
        {

        }
      }

      // TODO: Fix The Shit
      
      foreach (Production cost in check.productionCost)
      {
        if (ResourceCountCheck(cost.type, cost.amount))
        {
          successfulCostChecks++;

          if (successfulCostChecks >= check.productionCost.Length)
          {
            check.timer++;
          }
          
          if (check.timer >= check.timeDelay)
          {

          }
        }
      }
    }
  }

bool ResourceCountCheck(Resources.Type resourceType, int count)
{
  switch (resourceType)
    {
      case Type.PERSON:
        {
          return (resources.person >= count);
        }
      case Type.ANIMAL:
        {
          return (resources.animal >= count);
        }
      case Type.WOOD:
        {
          return (resources.wood >= count);
        }
      case Type.STONE:
        {
          return (resources.stone >= count);
        }
      case Type.FOOD:
        {
          return (resources.food >= count);
        }
    }
  return false;
}

// (Dale) Used for building production behaviors, such as resource production and consumption, and transformation into other buildings.
// Special Logic/Handling:
//    Time: Duration of this Reaction. Evaluated against BuildingBehavior.TimeSinceReaction. If Null or <0, Defaults to 1 Day.

  [System.Serializable]
  public struct Reaction
  {
    public bool workerRequired; // Reaction requires that the GameTile is currently being Worked.
    public GameObject tileReplacement; // GameObject this building is replaced by when the Reaction evaluates (Ignored if Null).

    public Production[] productionCost; // Resources consumed by this Reaction.
    public Production[] productionYield; // Resources produced by this Reaction.
    
    public int timer; // Incremented when a Reaction's Condition is met.
    public int timeDelay; // Value reaction is tested against.
  }

  [System.Serializable]
  public struct Production
  {
    public Resources.Type type;
    public int amount;
  }

}
