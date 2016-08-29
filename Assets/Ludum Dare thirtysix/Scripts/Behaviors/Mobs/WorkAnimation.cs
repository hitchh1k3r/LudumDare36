using UnityEngine;
using System.Collections;

public class WorkAnimation : MonoBehaviour
{

  private float timer;
  private Vector3 originalPosition;
  private Vector3 originalRotation;

  void Start()
  {
    originalPosition = transform.localPosition;
    originalRotation = transform.localRotation.eulerAngles;
    timer += Random.Range(0, 2 * Mathf.PI);
  }

  void Update()
  {
    timer += Time.deltaTime;
    timer %= 2 * Mathf.PI;
    transform.localPosition = originalPosition + (Mathf.Sin(timer * 10) + 1) * 0.05f * Vector3.up;
    transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, 10 * Mathf.Sin(timer * 5));
  }

}
