using UnityEngine;
using System.Collections;

public class CostDisplay : MonoBehaviour
{

  public CostLine[] costs;

  [System.Serializable]
  public struct CostLine
  {
    public SpriteRenderer sprite;
    public TextMesh text;
  }

}
