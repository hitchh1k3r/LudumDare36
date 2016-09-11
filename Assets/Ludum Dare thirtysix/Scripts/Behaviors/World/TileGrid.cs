using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TileGrid : MonoBehaviour
{

  public static TileGrid instance;
  public bool worldGenerated;

  [TextArea(5, 20)]
  public string tileString;
  public float tileSpacing = 1.0f;

  public TileMapEntry[] tileMap;
  public SpawnTile[] startingTiles;

  [System.NonSerialized]
  public int width, height;

  private Dictionary<char, GameObject[]> tileLookup = new Dictionary<char, GameObject[]>();
  [System.NonSerialized]
  public GameObject[] terrain;
  private GameObject[] tiles;

  void OnEnable()
  {
    instance = this;
  }

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

    Random.seed = System.Environment.TickCount;
    foreach (SpawnTile tile in startingTiles)
    {
      int x = Random.Range(0, width);
      int y = Random.Range(0, height);
      for (int i = 0; i < tile.count; ++i)
      {
        while (GetTile(x, y) != null)
        {
          x = Random.Range(0, width);
          y = Random.Range(0, height);
        }
        SetTile(x, y, tile.tile);
        GetTile(x, y).GetComponent<GameTile>().isFirstTurn = false;
      }
    }
    worldGenerated = true;
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

    if (prefab != null)
    {
      GameObject ngo = Instantiate(prefab);
      ngo.transform.parent = transform;
      ngo.transform.localPosition = Vector3.right * x * tileSpacing + Vector3.back * y * tileSpacing + HeightOffset(x, y) * Vector3.up;
      ngo.transform.localRotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
      ngo.layer = gameObject.layer;
      terrain[i] = ngo;
    }
    else
    {
      terrain[i] = null;
    }
  }

  public GameObject GetTile(int x, int y)
  {
    if (x < 0 || x >= width || y < 0 || y >= height)
    {
      return null;
    }
    return tiles[WorldGridIndex(x, y)];
  }

  public bool GetPassable(int x, int y)
  {
    GameObject tile = GetTile(x, y);
    if (tile != null)
    {
      BuildingPrice price = tile.GetComponent<BuildingPrice>();
      return (price.type == "pit" || price.type == "pit_full" || price.type == "construct" || price.type == "crop" || price.type == "baby_crop" || price.type == "baby_tree");
    }
    return true;
  }

  public void SetTile(int x, int y, GameObject prefab)
  {
    int i = WorldGridIndex(x, y);
    if (tiles[i] != null)
    {
      BuildingPrice price = tiles[i].GetComponent<BuildingPrice>();
      if (price.hidesTerrain)
      {
        foreach (Renderer gfx in terrain[i].GetComponentsInChildren<Renderer>())
        {
          gfx.enabled = true;
        }
      }
      Destroy(tiles[i]);
    }

    if (prefab != null)
    {
      GameObject ngo = Instantiate(prefab);
      foreach (SpecialStates states in ngo.GetComponentsInChildren<SpecialStates>())
      {
        if (states.hideInWorld)
        {
          states.gameObject.SetActive(false);
        }
      }
      BuildingPrice price = ngo.GetComponent<BuildingPrice>();
      if (price != null && price.hidesTerrain)
      {
        foreach (Renderer gfx in terrain[i].GetComponentsInChildren<Renderer>())
        {
          gfx.enabled = false;
        }
      }
      ngo.transform.parent = transform;
      ngo.transform.localPosition = Vector3.right * x * tileSpacing + Vector3.back * y * tileSpacing + (HeightOffset(x, y) + (price != null ? price.heightOffset : 0)) * Vector3.up;
      ngo.transform.localRotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
      foreach (Transform trans in ngo.GetComponentsInChildren<Transform>(true))
      {
        trans.gameObject.layer = 0;
      }

      GameTile tile = ngo.GetComponent<GameTile>();
      if (tile != null)
      {
        tile.enabled = true;
        tile.x = x;
        tile.y = y;
        price.enabled = false;
        foreach (BuildingPrice menuItem in BuildMenu.instance.GetComponentsInChildren<BuildingPrice>(true))
        {
          menuItem.BuildingComplete(price.type);
        }
        tiles[i] = ngo;
      }
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

  [System.Serializable]
  public struct SpawnTile
  {
    public int count;
    public GameObject tile;
  }

}
