using UnityEngine;

public class BuildingPrice : MonoBehaviour
{

  public Cost[] costs;

  [System.Serializable]
  public struct Cost
  {
    public Resources.Type type;
    public int amount;
  }

}
