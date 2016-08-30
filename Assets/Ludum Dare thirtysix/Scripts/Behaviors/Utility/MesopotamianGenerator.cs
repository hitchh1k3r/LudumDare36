using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MesopotamianGenerator : MonoBehaviour
{

  public static MesopotamianGenerator instance;

  [TextArea(10, 10)]
  public string nameList;

  [TextArea(10, 10)]
  public string rareNamesList;

  public Material[] MATERIAtamians;

  public List<Mesopotamian> mesoPOOLameians = new List<Mesopotamian>();
  private string[] names;
  private string[] rareNames;

  void Awake()
  {
    instance = this;
    names = nameList.Split('\n');
    rareNames = rareNamesList.Split('\n');
  }

  public void RemoveFromPool(int count)
  {
    for (int i = 0; i < count; ++i)
    {
      ReleaseMesopotamian(GetMesopotamian());
    }
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

    mesopotamian.mesopoNAMEian = names[Random.Range(0, names.Length)];
    if (Random.Range(0, 15) == 0)
    {
      mesopotamian.mesopoNAMEian = rareNames[Random.Range(0, rareNames.Length)];
    }
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