using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameTile : MonoBehaviour
{

  public bool firstTurnWork;
  public bool canDemolish;
  public bool changeOnWork;
  public GameObject workState;
  public ConstructionStates construction;
  public BuildingPrice.Cost[] demolishRefund;
  public BuildingPrice.Cost[] workGain;
  public BuildingPrice.Cost[] maxIncreses;
  public AudioClip[] workerPlaceSounds;
  public AudioClip[] workerRemoveSounds;

  [System.NonSerialized]
  public bool working;
  [System.NonSerialized]
  public GameObject[] workDestroy;

  [System.NonSerialized]
  public int x, y;
  [System.NonSerialized]
  public AudioSource audio;
  private bool isFirstTurn;

  void OnEnable()
  {
    audio = GetComponent<AudioSource>();
    isFirstTurn = true;
    if (firstTurnWork && TileGrid.instance.worldGenerated)
    {
      audio.PlayOneShot(workerPlaceSounds[Random.Range(0, workerPlaceSounds.Length)]);
      working = true;
      foreach (SpecialStates child in GetComponentsInChildren<SpecialStates>(true))
      {
        if (child.activeOnWork)
        {
          child.gameObject.SetActive(true);
        }
      }
    }

    BuildingPrice price = GetComponent<BuildingPrice>();
    CostDisplay display = Instantiate(Resources.instance.workCostDisplay).GetComponent<CostDisplay>();
    display.transform.SetParent(transform);
    display.transform.localPosition = price.heightOffset * -Vector3.up;

    int q = 0;
    foreach (BuildingPrice.Cost cost in price.workCosts)
    {
      display.costs[q].sprite.gameObject.SetActive(true);
      display.costs[q].sprite.sprite = Resources.instance.GetIcon(cost.type);
      display.costs[q].text.text = cost.amount.ToString();
      ++q;
    }

    if (!price.alwaysShowCost)
    {
      display.gameObject.SetActive(false);
    }

    foreach (BuildingPrice.Cost max in maxIncreses)
    {
      Resources.instance.SetValue(max.type, Resources.instance.GetValue(max.type, true) + max.amount, true);
    }
  }

  void OnDisable()
  {
    foreach (BuildingPrice.Cost max in maxIncreses)
    {
      Resources.instance.SetValue(max.type, Resources.instance.GetValue(max.type, true) - max.amount, true);
    }
  }

  void Upkeep()
  {
    BuildingPrice price = GetComponent<BuildingPrice>();
    if (working || !ContainsPerson(price.workCosts))
    {
      foreach (SpecialStates child in GetComponentsInChildren<SpecialStates>(true))
      {
        if (child.activeOnWork)
        {
          child.gameObject.SetActive(false);
        }
      }

      working = false;
      bool okay = true;
      for (int i = 0; i < price.workCosts.Length; ++i)
      {
        BuildingPrice.Cost cost = price.workCosts[i];
        if (cost.type == Resources.Type.TIME)
        {
          okay = false;
          --cost.amount;
          price.workCosts[i] = cost;
          // FIXME (hitch) this is a horrible hack to update time display, IRL lookup what field we are
          // updating!
          GetComponentInChildren<CostDisplay>(true).costs[0].text.text = cost.amount.ToString();
          if (construction != null)
          {
            construction.transform.localPosition += construction.deltaPos;
            construction.transform.localScale += construction.deltaScale;
          }
        }
        if (cost.amount <= 0)
        {
          okay = true;
        }
      }
      if (okay)
      {
        foreach (BuildingPrice.Cost entry in workGain)
        {
          ScoreTracker.instance.AddIncome(entry.type, entry.amount);
        }
        Resources.instance.AddResources(workGain);
        if (changeOnWork)
        {
          TileGrid.instance.SetTile(x, y, workState);
        }
      }
    }
    isFirstTurn = false;
  }

  public void ClickTile(TileGrid world, int x, int y)
  {
    if (enabled && !ScoreTracker.instance.isSummaryShowing)
    {
      if (BuildMenu.activeTool.type == "peep" && !firstTurnWork)
      {
        BuildingPrice.Cost[] workCosts = GetComponent<BuildingPrice>().workCosts;
        if (working || (ContainsPerson(workCosts) && Resources.instance.TryBuy(workCosts)))
        {
          working = !working;
          if (!working)
          {
            Resources.instance.AddResources(workCosts);
            audio.PlayOneShot(workerRemoveSounds[Random.Range(0, workerRemoveSounds.Length)]);
          }
          else
          {
            audio.PlayOneShot(workerPlaceSounds[Random.Range(0, workerPlaceSounds.Length)]);
          }
          foreach (SpecialStates child in GetComponentsInChildren<SpecialStates>(true))
          {
            if (child.activeOnWork)
            {
              child.gameObject.SetActive(working);
            }
          }
          gameObject.BroadcastMessage("WorkingToggle", SendMessageOptions.DontRequireReceiver);
          Color highlightColor = HighlightEffect.GetColor(gameObject);
          if (highlightColor != Color.black)
          {
            HighlightEffect.RemoveHighlight(gameObject);
            HighlightEffect.AddHighlight(gameObject, highlightColor);
          }
        }
      }
      else if (BuildMenu.activeTool.type == "demo")
      {
        if (canDemolish && (firstTurnWork || !working))
        {
          world.SetTile(x, y, null);
          Resources.instance.AddResources(demolishRefund);
          if (working)
          {
            BuildingPrice.Cost[] workCosts = GetComponent<BuildingPrice>().workCosts;
            Resources.instance.AddResources(workCosts);

            if (firstTurnWork)
            {
              ++Resources.instance.person;
            }
          }
          SoundEffects.PlaySound(WorldInteraction.instance.buildSound, 0.9f);
        }
        else
        {
          // TODO (hitch) ERROR (can't demolish)
        }
      }
    }
  }

  private bool ContainsPerson(BuildingPrice.Cost[] costs)
  {
    foreach (BuildingPrice.Cost cost in costs)
    {
      if (cost.type == Resources.Type.PERSON)
      {
        return true;
      }
    }
    return false;
  }

}
