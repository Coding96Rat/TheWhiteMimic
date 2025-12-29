using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private GameObject DialougeFrame;
    private GameObject currentDialogueFrame;
    private DialogueSystem dialogueSystem;

    public override void Awake()
    {
        base.Awake();
        currentDialogueFrame = Instantiate(DialougeFrame, this.transform);
        dialogueSystem = currentDialogueFrame.GetComponent<DialogueSystem>();
    }
    private void Start()
    {
        CloseDialogueFrame();
    }

    public void OpenDialogueFrame()
    {
        if(currentDialogueFrame == null)
        {
            Debug.LogError("Dialogue Frame is not instantiated.");
        }

        dialogueSystem.DialogueIsOn(true);
    }

    public void CloseDialogueFrame()
    {
        if (currentDialogueFrame == null)
        {
            Debug.LogError("Dialogue Frame is not instantiated.");
        }

        dialogueSystem.DialogueIsOn(false);
    }
}
