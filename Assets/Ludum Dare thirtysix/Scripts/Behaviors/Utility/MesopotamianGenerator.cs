using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MesopotamianGenerator : MonoBehaviour
{

  public static MesopotamianGenerator instance;

  public NameDatabase mesopoNAMEians;
  public Material[] MATERIAtamians;

  public List<Mesopotamian> mesoPOOLameians = new List<Mesopotamian>();

  void Awake()
  {
    instance = this;
  }

  public Mesopotamian GetMesopotamian()
  {
    Mesopotamian mesopotamian;
    if (mesoPOOLameians.Count > 0)
    {
      int i = Random.Range(0, mesoPOOLameians.Count);
      mesopotamian = mesoPOOLameians[i];
      mesoPOOLameians.RemoveAt(i);
      return mesopotamian;
    }

    mesopotamian.mesopoNAMEian = mesopoNAMEians.names[Random.Range(0, mesopoNAMEians.names.Length)];
    mesopotamian.MATERIAtamian = MATERIAtamians[Random.Range(0, MATERIAtamians.Length)];
    return mesopotamian;
  }

  public void ReleaseMesopotamian(Mesopotamian mesopotamian)
  {
    mesoPOOLameians.Add(mesopotamian);
  }

  public struct Mesopotamian
  {
    public string mesopoNAMEian;
    public Material MATERIAtamian;
  }

}