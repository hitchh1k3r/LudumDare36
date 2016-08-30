using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{

  public StatusReportDisplay statusScreen;
  public ModalDialog modal;
  public ExitConfirm confirm;
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

  public static void ShowMessage(string message)
  {
    instance.modal.message = message;
    instance.enableNextFrame = instance.modal.gameObject;
  }

  public static void Confirm(string message, bool resetGame)
  {
    instance.confirm.message = message;
    instance.confirm.restart = resetGame;
    instance.enableNextFrame = instance.confirm.gameObject;
  }

  public static void StatusReport()
  {
    instance.enableNextFrame = instance.statusScreen.gameObject;
  }

}
