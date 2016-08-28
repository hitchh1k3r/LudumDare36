using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

  public bool lateralOnly;

  public Transform camera;

  void Awake()
  {
    if (camera == null)
    {
      camera = Camera.main.transform;
    }
  }

  void Update()
  {
    transform.rotation = Quaternion.Euler(lateralOnly ? 0 : camera.eulerAngles.x, camera.eulerAngles.y, 0);
  }

}
