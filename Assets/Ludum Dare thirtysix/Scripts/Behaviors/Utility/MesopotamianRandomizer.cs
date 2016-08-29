using UnityEngine;
using System.Collections;

public class MesopotamianRandomizer : MonoBehaviour
{

  public bool doNotReleaseName;
  public TextMesh text, shadow;
  public Renderer renderer;

  private MesopotamianGenerator.Mesopotamian mesopotamian;

  void OnEnable()
  {
    doNotReleaseName = false;
    mesopotamian = MesopotamianGenerator.instance.GetMesopotamian();
    text.text = mesopotamian.mesopoNAMEian;
    shadow.text = mesopotamian.mesopoNAMEian;

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
