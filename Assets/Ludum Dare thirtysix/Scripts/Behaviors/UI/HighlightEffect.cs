﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class HighlightEffect : MonoBehaviour
{

  public LayerMask highlightMask;

  private Camera cam;
  private Camera highlightCam;
  private static Material aura;
  private Material overlay;
  private Material debug;
  private static int highlightLayer = 0;

  public static void AddHighlight(GameObject gameobject, Color color, bool force = false)
  {
    if (aura != null)
    {

      Renderer[] renderers = gameobject.GetComponentsInChildren<Renderer>();
      if (renderers != null)
      {
        foreach (Renderer renderer in renderers)
        {
          if (renderer.sharedMaterial.shader != aura.shader)
          {
            // TODO (hitch) find a better way to check if this is opaque or not,
            // right now we check the render queue (2000 = opaque, 2450 = alpha cutout)
            // this will also detect overlay materials as non-opaque
            // (<= 2500 is considered opaque, cutouts are barely opaque)
            Material mat = new Material(aura);
            if (renderer.sharedMaterial.renderQueue > 2000)
            {
              // FIXME (hitch) this probably isn't needed with the new depth test system ^_^
              mat.mainTexture = renderer.sharedMaterial.mainTexture;
            }
            mat.color = color;
            if (renderer is MeshRenderer)
            {
              MeshFilter filter = renderer.GetComponent<MeshFilter>();
              if (filter != null)
              {
                SpecialStates state = renderer.GetComponent<SpecialStates>();
                if (force || (renderer.enabled && (state == null || !state.hideFromHighlighter)))
                {
                  MeshRenderer nr = new GameObject().AddComponent<MeshRenderer>();
                  nr.transform.SetParent(renderer.transform);
                  nr.transform.localPosition = Vector3.zero;
                  nr.transform.localScale = Vector3.one;
                  nr.receiveShadows = false;
                  nr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                  nr.transform.localRotation = Quaternion.identity;
                  nr.name = renderer.name + " (Aura)";
                  nr.sharedMaterial = mat;
                  nr.gameObject.layer = highlightLayer;
                  MeshFilter f = nr.gameObject.AddComponent<MeshFilter>();
                  f.sharedMesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                }
              }
            }
          }
        }
      }
    }
  }

  public static void ChangeColor(GameObject gameobject, Color color)
  {
    Renderer[] renderers = gameobject.GetComponentsInChildren<Renderer>();
    if (renderers != null)
    {
      foreach (Renderer renderer in renderers)
      {
        if (renderer.sharedMaterial.shader == aura.shader)
        {
          renderer.sharedMaterial.color = color;
        }
      }
    }
  }

  public static Color GetColor(GameObject gameobject)
  {
    Renderer[] renderers = gameobject.GetComponentsInChildren<Renderer>();
    if (renderers != null)
    {
      foreach (Renderer renderer in renderers)
      {
        if (renderer.sharedMaterial.shader == aura.shader)
        {
          return renderer.sharedMaterial.color;
        }
      }
    }
    return Color.black;
  }

  public static void RemoveHighlight(GameObject gameobject)
  {
    Renderer[] renderers = gameobject.GetComponentsInChildren<Renderer>();
    if (renderers != null)
    {
      foreach (Renderer renderer in renderers)
      {
        if (renderer.material.shader == aura.shader)
        {
          Destroy(renderer.material);
          Destroy(renderer.gameObject);
        }
      }
    }
  }

  void Start()
  {
    cam = GetComponent<Camera>();
    cam.depthTextureMode = DepthTextureMode.Depth;
    cam.cullingMask &= ~highlightMask;
    highlightCam = new GameObject().AddComponent<Camera>();
    highlightCam.transform.parent = cam.transform;
    highlightCam.name = "Highlight Camera";
    highlightCam.gameObject.SetActive(false);
    aura = new Material(Shader.Find("Hidden/Aura"));
    overlay = new Material(Shader.Find("Hidden/AuraOverlay"));
    debug = new Material(Shader.Find("Hidden/HalfBlend"));
    for (int i = 0; i < 32; ++i)
    {
      if (((int)highlightMask >> i) == 1)
      {
        highlightLayer = i;
        break;
      }
    }
  }

  private RenderTexture buffer1, buffer2;
  private int buffWidth, buffHeight;

  void OnRenderImage(RenderTexture src, RenderTexture dest)
  {
    if (buffWidth != src.width || buffHeight != src.height)
    {
      if (buffer1 != null)
      {
        DestroyImmediate(buffer1);
      }
      if (buffer2 != null)
      {
        DestroyImmediate(buffer2);
      }

      buffWidth = src.width;
      buffHeight = src.height;
      buffer1 = new RenderTexture(buffWidth, buffHeight, 0, RenderTextureFormat.ARGB32);
      buffer2 = new RenderTexture(buffWidth, buffHeight, 0, RenderTextureFormat.ARGB32);
    }

    if (cam.clearFlags == CameraClearFlags.Depth || cam.clearFlags == CameraClearFlags.Nothing)
    {
      RenderTexture rt = RenderTexture.active;
      RenderTexture.active = buffer1;
      GL.Clear(false, true, Color.clear);
      RenderTexture.active = buffer2;
      GL.Clear(false, true, Color.clear);
      RenderTexture.active = rt;
    }

    highlightCam.CopyFrom(cam);
    highlightCam.cullingMask = highlightMask;
    highlightCam.targetTexture = buffer1;
    highlightCam.depthTextureMode = DepthTextureMode.None;
    highlightCam.renderingPath = RenderingPath.Forward;
    highlightCam.RenderWithShader(aura.shader, null);

    Graphics.Blit(buffer1, buffer2, overlay, 0);

    buffer1.DiscardContents();

    Graphics.Blit(src, dest);
    Graphics.Blit(buffer2, dest, overlay, 1);

    buffer2.DiscardContents();
  }

}
