using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class RandomMaterial : MonoBehaviour
{

  public Material[] materials;

  void OnEnable()
  {
    GetComponent<MeshRenderer>().sharedMaterial = materials[Random.Range(0, materials.Length)];
  }

}
