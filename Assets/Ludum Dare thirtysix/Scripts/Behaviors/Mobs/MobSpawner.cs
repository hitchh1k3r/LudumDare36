using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour
{

  public GameObject mob;
  public bool spawnsAtNight;

  private int stage;
  private Transform entityBin;
  private float timer;

  void Update()
  {
    if (stage == 1 && timer > 0)
    {
      timer -= Time.deltaTime;
      if (timer <= 0)
      {
        Transform mobTrans = Instantiate(mob).transform;
        mobTrans.position = transform.position + 0.8f * Vector3.up - 0.5f * transform.forward;
        mobTrans.rotation = transform.rotation;
        mobTrans.SetParent(entityBin);
        Destroy(gameObject);
        stage = 2;
      }
    }
  }

  void RoundStart(RoundManager.RoundData data)
  {
    if (stage == 0 && spawnsAtNight == data.isNight)
    {
      entityBin = data.entities;
      timer = Random.Range(0.1f, 2.0f);
      stage = 1;
    }
  }

}
