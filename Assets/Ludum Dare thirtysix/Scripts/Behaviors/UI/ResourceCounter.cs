using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ResourceCounter : MonoBehaviour
{

  public Resources resourcePool;
  public Resources.Type displayType;

  private Text text;

  void Awake()
  {
    text = GetComponent<Text>();
  }

  void Update()
  {
    text.text = resourcePool.GetValue(displayType, false) + "/" + resourcePool.GetValue(displayType, true);
  }

}
