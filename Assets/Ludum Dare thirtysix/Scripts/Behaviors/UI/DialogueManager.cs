using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{

  public StatusReportDisplay statusScreen;
  public ModalDialog modal;
  public ExitConfirm confirm;
  public Tooltip tooltip;
  public GameObject leftStatus;

  public static DialogueManager instance;

  private GameObject enableNextFrame;

  void OnEnable()
  {
    instance = this;
  }

  void Update()
  {
    if (enableNextFrame != null)
    {
      enableNextFrame.SetActive(true);
      enableNextFrame = null;
    }
  }

  public static void ShowMessage(string message, bool bottom = false)
  {
    if (bottom)
    {
      ((RectTransform)instance.modal.transform).anchorMin = new Vector2(0.5f, 0);
      ((RectTransform)instance.modal.transform).anchorMax = new Vector2(0.5f, 0);
      ((RectTransform)instance.modal.transform).anchoredPosition = new Vector3(0, 200);
    }
    else
    {
      ((RectTransform)instance.modal.transform).anchorMin = new Vector2(0.5f, 0.5f);
      ((RectTransform)instance.modal.transform).anchorMax = new Vector2(0.5f, 0.5f);
      ((RectTransform)instance.modal.transform).anchoredPosition = new Vector3(0, 0, 0);
    }
    instance.modal.message = message;
    instance.enableNextFrame = instance.modal.gameObject;
  }

  public static void Confirm(string message, bool resetGame, bool bottom = false)
  {
    if (bottom)
    {
      ((RectTransform)instance.confirm.transform).anchorMin = new Vector2(0.5f, 0);
      ((RectTransform)instance.confirm.transform).anchorMax = new Vector2(0.5f, 0);
      ((RectTransform)instance.confirm.transform).anchoredPosition = new Vector3(0, 200);
    }
    else
    {
      ((RectTransform)instance.confirm.transform).anchorMin = new Vector2(0.5f, 0.5f);
      ((RectTransform)instance.confirm.transform).anchorMax = new Vector2(0.5f, 0.5f);
      ((RectTransform)instance.confirm.transform).anchoredPosition = new Vector3(0, 0, 0);
    }
    instance.confirm.message = message;
    instance.confirm.restart = resetGame;
    instance.enableNextFrame = instance.confirm.gameObject;
  }

  public static void StatusReport()
  {
    instance.enableNextFrame = instance.statusScreen.gameObject;
  }

  public static void Tooltip(string name, string description, string costs)
  {
    instance.tooltip.name.text = name;
    instance.tooltip.description.text = description;
    instance.tooltip.costs.text = costs;
    instance.enableNextFrame = instance.tooltip.gameObject;
  }

  public static void HideTooltip()
  {
    instance.tooltip.gameObject.SetActive(false);
  }

}
