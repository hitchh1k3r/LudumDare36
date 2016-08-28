using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TileGrid : MonoBehaviour
{

  // W = Water
  // D = Dirt
  // G = Ground/Grass
  [TextArea(10, 10)]
  public string tileString;
  public float tileSpacing = 1.0f;

  public TileMapEntry[] tileMap;

  // TODO: change dictionary into a struct
  private Dictionary<char, GameObject[]> tileLookup = new Dictionary<char, GameObject[]>();
  private GameObject[] terrain;
  private GameObject[] tiles;
  private int width;

  void Start()
  {
    int r, c;
    string[] lines = tileString.Split('\n');
    int totalRows = lines.Length;
    width = lines[0].Length;

    InitWorldDictionary();

    terrain = new GameObject[totalRows * width];
    tiles = new GameObject[totalRows * width];
    for (r = 0; r < lines.Length; r++)
    {
      if (lines[r].Length != width)
      {
        Debug.LogError("World grid is not of even length on line r: " + r + "\nstring: " + lines[r]);
      }
      else
      {
        for (c = 0; c < lines[r].Length; c++)
        {
          GameObject ngo = Instantiate(tileLookup[lines[r][c]][Random.Range(0, tileLookup[lines[r][c]].Length)]);
          ngo.transform.parent = transform;
          ngo.transform.localPosition = Vector3.right * c * tileSpacing + Vector3.back * r * tileSpacing + (Random.value * 0.1f) * Vector3.up;
          ngo.transform.localRotation = Quaternion.identity;
          ngo.layer = gameObject.layer;
          terrain[WorldGridIndex(c, r)] = ngo;
        }
      }
    }
  }

  public int WorldGridIndex(int column, int row)
  {
    int index = column + (row * width);
    return index;
  }

  private void InitWorldDictionary()
  {
    foreach (TileMapEntry tile in tileMap)
    {
      tileLookup.Add(tile.code, tile.prefab);
    }
  }

  [System.Serializable]
  public struct TileMapEntry
  {
    public char code;
    public GameObject[] prefab;
  }

}
