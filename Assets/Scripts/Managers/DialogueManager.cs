using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private GameObject DialougeFrame;
    private GameObject currentDialogueFrame;


    public override void Awake()
    {
        base.Awake();
        currentDialogueFrame = Instantiate(DialougeFrame, this.transform);
    }

    public void OpenDialogueFrame()
    {
        if(currentDialogueFrame == null)
        {
            Debug.LogError("Dialogue Frame is not instantiated.");
        }

        currentDialogueFrame.SetActive(true);
    }

    public void CloseDialogueFrame()
    {
        if (currentDialogueFrame == null)
        {
            Debug.LogError("Dialogue Frame is not instantiated.");
        }

        currentDialogueFrame.SetActive(false);
    }
}
