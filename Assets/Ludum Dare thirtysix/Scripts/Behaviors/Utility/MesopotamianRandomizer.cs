using UnityEngine;
using System.Collections;

public class MesopotamianRandomizer : MonoBehaviour
{

  public bool doNotReleaseName;
  public string mesopoNAMEian;
  public TextMesh text, shadow;
  public Renderer renderer;

  private MesopotamianGenerator.Mesopotamian mesopotamian;

  void OnEnable()
  {
    doNotReleaseName = false;
    mesopotamian = MesopotamianGenerator.instance.GetMesopotamian();
    mesopoNAMEian = mesopotamian.mesopoNAMEian;
    text.text = mesopoNAMEian;
    if (shadow != null)
    {
      shadow.text = mesopoNAMEian;
    }

    renderer.sharedMaterial = mesopotamian.MATERIAtamian;
  }

  void OnDisable()
  {
    if (!doNotReleaseName)
    {
      MesopotamianGenerator.instance.ReleaseMesopotamian(mesopotamian);
    }
  }

}
