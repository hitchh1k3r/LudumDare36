using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMover : MonoBehaviour
{

  public float moveTime = 1;
  public float turnTime = 0.5f;
  public MoveStyle moveType;

  private Queue<IEnumerator> queuedMoves = new Queue<IEnumerator>();
  private Coroutine movement;
  private float timer;

  void Update()
  {
    if (queuedMoves.Count == 0)
    {
      MoveForward();
      MoveForward();
      MoveForward();
      TurnLeft();
    }

    if (movement == null && queuedMoves.Count > 0)
    {
      movement = StartCoroutine(queuedMoves.Dequeue());
    }
  }

  public void MoveForward()
  {
    queuedMoves.Enqueue(MoveTo(1.1f * Vector3.forward));
  }

  public void TurnLeft()
  {
    queuedMoves.Enqueue(TurnTo(-90));
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

  private IEnumerator MoveTo(Vector3 deltaPos)
  {
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
