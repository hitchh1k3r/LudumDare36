using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextMeshShadow : MonoBehaviour
{

  private bool init;

  void Update()
  {
    if (!init)
    {
      init = true;

      TextMesh source = GetComponent<TextMesh>();
      Transform newText = Instantiate(gameObject).transform;
      Destroy(newText.GetComponent<TextMeshShadow>());
      newText.GetComponent<TextMesh>().color = Color.black;
      newText.SetParent(transform);
      newText.localPosition = new Vector3(0.2f, -0.2f, 0.1f);
      newText.localRotation = Quaternion.identity;
      newText.localScale = Vector3.one;
    }
  }

}
