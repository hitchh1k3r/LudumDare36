using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour
{

  public GameObject mob;
  public bool spawnsAtNight;

  private bool done;

  void RoundStart(RoundManager.RoundData data)
  {
    if (!done && spawnsAtNight == data.isNight)
    {
      Transform mobTrans = Instantiate(mob).transform;
      mobTrans.position = transform.position + 0.8f * Vector3.up - 0.5f * transform.forward;
      mobTrans.rotation = transform.rotation;
      mobTrans.SetParent(data.entities);
      Destroy(gameObject);
      done = true;
    }
  }

}
