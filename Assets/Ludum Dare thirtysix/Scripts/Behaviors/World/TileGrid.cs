using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TileGrid : MonoBehaviour
{

  [TextArea(5, 20)]
  public string tileString;
  public float tileSpacing = 1.0f;

  public TileMapEntry[] tileMap;

  [System.NonSerialized]
  public int width, height;

  private Dictionary<char, GameObject[]> tileLookup = new Dictionary<char, GameObject[]>();
  private GameObject[] terrain;
  private GameObject[] tiles;

  void Start()
  {
    int r, c;
    string[] lines = tileString.Split('\n');
    height = lines.Length;
    width = lines[0].Length;

    InitWorldDictionary();

    terrain = new GameObject[height * width];
    tiles = new GameObject[height * width];
    for (r = 0; r < lines.Length; ++r)
    {
      if (lines[r].Length != width)
      {
        Debug.LogError("World grid is not of even length on line " + r + "\nText: " + lines[r] + "\nGot: " + lines[r].Length + " Expected: " + width);
      }
      else
      {
        for (c = 0; c < lines[r].Length; ++c)
        {
          SetTerrain(c, r, lines[r][c]);
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

  public Point GetTerrainPos(GameObject block)
  {
    Point point = null;
    for (int i = 0; i < terrain.Length; ++i)
    {
      if (block == terrain[i])
      {
        int x = i % width;
        int y = i / width;
        return new Point(x, y);
      }
    }
    return point;
  }

  public float HeightOffset(int x, int y)
  {
    Random.seed = (x * 179424691 + (y * 314606869));
    return Random.Range(0, 0.1f);
  }

  public void SetTerrain(int x, int y, char type)
  {
    SetTerrain(x, y, tileLookup[type][Random.Range(0, tileLookup[type].Length)]);
  }

  public void SetTerrain(int x, int y, GameObject prefab)
  {
    int i = WorldGridIndex(x, y);
    if (terrain[i] != null)
    {
      Destroy(terrain[i]);
    }

    GameObject ngo = Instantiate(prefab);
    ngo.transform.parent = transform;
    ngo.transform.localPosition = Vector3.right * x * tileSpacing + Vector3.back * y * tileSpacing + HeightOffset(x, y) * Vector3.up;
    ngo.transform.localRotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
    ngo.layer = gameObject.layer;
    terrain[i] = ngo;
  }

  public GameObject GetTile(int x, int y)
  {
    return tiles[WorldGridIndex(x, y)];
  }

  public void SetTile(int x, int y, GameObject prefab)
  {
    int i = WorldGridIndex(x, y);
    if (tiles[i] != null)
    {
      Destroy(tiles[i]);
    }

    if (prefab != null)
    {
      GameObject ngo = Instantiate(prefab);
      ngo.transform.parent = transform;
      ngo.transform.localPosition = Vector3.right * x * tileSpacing + Vector3.back * y * tileSpacing + (HeightOffset(x, y) + 1) * Vector3.up;
      ngo.transform.localRotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
      ngo.GetComponent<BuildingPrice>().enabled = false;
      tiles[i] = ngo;
    }
  }

  [System.Serializable]
  public struct TileMapEntry
  {
    public char code;
    public GameObject[] prefab;
  }

  public class Point
  {

    public int x, y;

    public Point(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

  }

}
