using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class UnitMover : MonoBehaviour
{

  public static int moverCount;

  public GameObject animalPit;

  public float moveTime = 1;
  public float turnTime = 0.5f;
  public MoveStyle moveType;
  public AudioClip fallSound;
  public AudioClip attackSound;
  public AudioClip[] stepSounds;

  private AudioSource audio;
  private Queue<IEnumerator> queuedMoves = new Queue<IEnumerator>();
  private Coroutine movement;
  private float timer;
  private bool disablePathing;
  private bool delete;

  private float fall_volume = 1;
  private float attack_volume = 1;
  private float step_volume = 1;

  void OnEnable()
  {
    ++moverCount;
    audio = GetComponent<AudioSource>();
  }

  void OnDisable()
  {
    --moverCount;
  }

  void Update()
  {
    if (delete)
    {
      Destroy(gameObject);
    }
    if (!disablePathing && !ScoreTracker.instance.isSummaryShowing)
    {
      if (queuedMoves.Count == 0)
      {
        queuedMoves.Enqueue(Path());
      }

      if (movement == null && queuedMoves.Count > 0)
      {
        movement = StartCoroutine(queuedMoves.Dequeue());
      }
    }
  }

  void RoundEnd()
  {
    StartCoroutine(Despawn());
  }

  private IEnumerator Despawn()
  {
    disablePathing = true;
    yield return movement;
    yield return ScaleTo(Vector3.zero, 0.25f);
    delete = true;
    Destroy(gameObject);
  }

  private IEnumerator Path()
  {
    Vector3 block = transform.position + transform.forward;
    Vector3 left = transform.position + 0.5f * transform.forward - 0.5f * transform.right;
    Vector3 right = transform.position + 0.5f * transform.forward + 0.5f * transform.right;
    int xLeft = Mathf.RoundToInt(left.x);
    int yLeft = -Mathf.RoundToInt(left.z);
    int xRight = Mathf.RoundToInt(right.x);
    int yRight = -Mathf.RoundToInt(right.z);

    if (block.x < -0.75f || block.z > 0.75f || block.x > TileGrid.instance.width - 0.25f || block.z < -TileGrid.instance.height + 0.25f)
    {
      StartCoroutine(ScaleTo(Vector3.zero, moveTime / 2));
      yield return StartCoroutine(MoveTo(Vector3.forward));
      delete = true;
      Destroy(gameObject);
      yield break;
    }

    for (int pass = 0; pass < 3; ++pass)
    {
      bool leftFirst = (Random.value > 0.5f);
      if (leftFirst && InteractWith(xLeft, yLeft, pass))
      {
        disablePathing = true;
        StartCoroutine(ScaleTo(Vector3.zero, moveTime / 2));
        yield return StartCoroutine(MoveTo(1 * Vector3.forward));
        delete = true;
        Destroy(gameObject);
        yield break;
      }
      if (InteractWith(xRight, yRight, pass))
      {
        disablePathing = true;
        StartCoroutine(ScaleTo(Vector3.zero, moveTime / 2));
        yield return StartCoroutine(MoveTo(1 * Vector3.forward));
        delete = true;
        Destroy(gameObject);
        yield break;
      }
      if (!leftFirst && InteractWith(xLeft, yLeft, pass))
      {
        disablePathing = true;
        StartCoroutine(ScaleTo(Vector3.zero, moveTime / 2));
        yield return StartCoroutine(MoveTo(1 * Vector3.forward));
        delete = true;
        Destroy(gameObject);
        yield break;
      }
    }

    if (TileGrid.instance.GetPassable(xLeft, yLeft) || TileGrid.instance.GetPassable(xRight, yRight))
    {
      yield return MoveTo(1.0f * Vector3.forward);
    }
    else
    {
      yield return TurnTo(90 * ((Random.Range(0, 2) * 2) - 1));
    }
  }

  IEnumerator Highlight(GameObject go, Color color, float time)
  {
    HighlightEffect.AddHighlight(go, color);
    yield return new WaitForSeconds(time);
    HighlightEffect.RemoveHighlight(go);
  }

  private bool InteractWith(int x, int y, int pass)
  {
    GameObject go = TileGrid.instance.GetTile(x, y);
    if (go != null)
    {
      BuildingPrice price = go.GetComponent<BuildingPrice>();
      if (price.type == "pit" && pass == 0)
      {
        audio.PlayOneShot(fallSound, fall_volume);
        TimeOfDayPopper.overrideMaterial = GetComponentInChildren<MeshRenderer>().sharedMaterial;
        TileGrid.instance.SetTile(x, y, animalPit);
        return true;
      }

      GameTile tile = go.GetComponent<GameTile>();
      if (moveType == MoveStyle.STALK)
      {
        if ((price.type == "baby_tree" || price.type == "baby_crop" || price.type == "crop") && !tile.working && pass == 1)
        {
          audio.PlayOneShot(attackSound, attack_volume);
          ScoreTracker.instance.LostTile((price.type == "baby_tree") ? "Sapling" : "Crop");
          Destroy(go);
          return true;
        }
      }
      else if (moveType == MoveStyle.WADDLE)
      {
        if (price.type == "ranch" && pass == 1)
        {
          bool okay = false;
          foreach (AnimalsInWorld aiw in go.GetComponentsInChildren<AnimalsInWorld>(true))
          {
            if (aiw.showing)
            {
              aiw.KillAnimal();
              okay = true;
            }
          }
          if (okay)
          {
            return true;
          }
        }
        if (tile.working && price.type != "fence" && pass == 2)
        {
          audio.PlayOneShot(attackSound, attack_volume);
          MesopotamianRandomizer[] people = go.GetComponentsInChildren<MesopotamianRandomizer>();
          // This is a hack to work (we don't know how many workers are here, so we just count the worker graphics!)
          foreach (MesopotamianRandomizer person in people)
          {
            person.doNotReleaseName = true;
            ScoreTracker.instance.LostVillager(1, new string[] { person.mesopoNAMEian });
          }
          Resources.instance.personLive -= people.Length;
          tile.working = false;
          foreach (SpecialStates child in go.GetComponentsInChildren<SpecialStates>(true))
          {
            if (child.activeOnWork)
            {
              child.gameObject.SetActive(false);
            }
          }
          return true;
        }
      }
    }
    return false;
  }

  private IEnumerator TurnTo(float deltaRotation)
  {
    Vector3 oldRot = transform.eulerAngles;
    Vector3 oldPos = transform.position;
    timer = 0;
    while (timer < turnTime)
    {
      timer += Time.deltaTime;
      float t = timer / turnTime;
      float bounce = Ease.QuadPop(0, 2, t);
      if (bounce > 1)
      {
        bounce = 2 - bounce;
      }
      if (moveType == MoveStyle.WADDLE)
      {
        bounce *= 0.1f;
      }
      else if (moveType == MoveStyle.STALK)
      {
        bounce *= 0.25f;
      }
      transform.position = oldPos + bounce * Vector3.up;
      transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y + Ease.QuadInOut(0, 1, t) * deltaRotation, oldRot.z);
      yield return null;
    }
    transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y + deltaRotation, oldRot.z);
    transform.position = oldPos;
    if (queuedMoves.Count > 0)
    {
      movement = StartCoroutine(queuedMoves.Dequeue());
    }
    else
    {
      movement = null;
    }
  }

  private IEnumerator ScaleTo(Vector3 target, float totalTime)
  {
    Vector3 oldScale = transform.localScale;
    timer = 0;
    while (timer < totalTime)
    {
      float t = timer / totalTime;
      t = Ease.QuadInOut(0, 1, t);
      transform.localScale = t * target + (1 - t) * oldScale;
      yield return null;
    }
    transform.localScale = target;
  }

  private IEnumerator MoveTo(Vector3 deltaPos)
  {
    audio.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)], step_volume);
    Vector3 oldRot = transform.eulerAngles;
    Vector3 oldPos = transform.position;
    Vector3 target = oldPos + transform.rotation * deltaPos;
    timer = 0;
    while (timer < moveTime)
    {
      timer += Time.deltaTime;
      float t = timer / moveTime;
      float bounce = Ease.QuadPop(0.0f, 2.0f, t);
      if (moveType == MoveStyle.WADDLE)
      {
        if (t < 0.5f)
        {
          bounce = Ease.QuadPop(0.0f, 2.0f, t * 2);
        }
        else
        {
          bounce = Ease.QuadPop(0.0f, 2.0f, (t * 2) - 1);
        }
      }
      if (bounce > 1)
      {
        bounce = 2 - bounce;
      }
      if (moveType == MoveStyle.WADDLE)
      {
        bounce *= 0.15f;
      }
      else if (moveType == MoveStyle.STALK)
      {
        bounce *= 0.3334f;
      }
      transform.position = Vector3.Lerp(oldPos, target, t) + bounce * Vector3.up;
      if (moveType == MoveStyle.WADDLE)
      {
        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y, 15 * Mathf.Sin(2 * Mathf.PI * t));
      }
      else if (moveType == MoveStyle.STALK)
      {
        transform.rotation = Quaternion.Euler(20 * Mathf.Sin(2 * Mathf.PI * t), oldRot.y, oldRot.z);
      }
      yield return null;
    }
    transform.rotation = Quaternion.Euler(oldRot);
    transform.position = target;
    if (queuedMoves.Count > 0)
    {
      movement = StartCoroutine(queuedMoves.Dequeue());
    }
    else
    {
      movement = null;
    }
  }

  public enum MoveStyle
  {
    WADDLE,
    STALK
  }

}
