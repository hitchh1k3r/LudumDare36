using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TileGrid : MonoBehaviour
{
  // W = Water
  // D = Dirt
  // G = Ground/Grass
  [TextArea]
  public string tileString;
  public GameObject water;
  public GameObject dirt;
  public GameObject ground;
  // TODO: change dictionary into a struct
  private Dictionary<char, GameObject> tileLookup;
  private GameObject[] worldGrid;
  public float tileSpacing = 1.1f;
  void Start()
  {
    int r = 0;
    int c = 0;
    initWorldDictionary();
    string[] strings = tileString.Split('\n');
    int totalRows = strings.Length;
    int totalColumns = strings[0].Length;
    worldGrid = new GameObject[totalRows * totalColumns];
    for(r = 0; r < strings.Length; r++)
    {
      if(strings[r].Length != totalColumns)
      {
        Debug.LogError("World grid is not of even length on line r: " + r + "\nstring: " + strings[r]);
      }
      else
      {
        for(c = 0; c < strings[r].Length; c++)
         {
           GameObject ngo = Instantiate(tileLookup[strings[r][c]]);
           ngo.transform.parent = transform;
           ngo.transform.localPosition = Vector3.left * r * tileSpacing + Vector3.forward * c * tileSpacing;
           ngo.transform.localRotation = Quaternion.identity;
           ngo.layer = gameObject.layer;
           worldGrid[worldGridIndex(r,c)] = ngo;
        }
      }
    }
  }
  public int worldGridIndex(int row, int column)
  {
    int index = row*column + column;
    return index;
  }
  void initWorldDictionary()
  {
    tileLookup = new Dictionary<char, GameObject>();
    tileLookup.Add('W', water);
    tileLookup.Add('D', dirt);
    tileLookup.Add('G', ground);
  }
}
