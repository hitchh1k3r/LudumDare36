using UnityEngine;

public class EnableOnStart : MonoBehaviour
{

  public GameObject[] objectsToActivate;

  void Awake()
  {
    foreach (GameObject go in objectsToActivate)
    {
      go.SetActive(true);
    }
  }

}
