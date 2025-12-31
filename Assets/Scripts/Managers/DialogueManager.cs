using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private GameObject DialougeFrame;
    private GameObject currentDialogueFrame;
    private DialogueSystem dialogueSystem;

    private FileSaverLoader fileSaverLoader;
    public SceneDialogue sceneDialogue;
    int currentSentenceIndex = 0;

    public override void Awake()
    {
        base.Awake();
        currentDialogueFrame = Instantiate(DialougeFrame, this.transform);
        // 이건 추후에 DataManager로 옮길 수도 있음
        fileSaverLoader = new FileSaverLoader(Application.persistentDataPath + "/Dialogues");
        Debug.Log("Dialogue Save Path: " + Application.persistentDataPath + "/Dialogues");
    }
    private void Start()
    {
        dialogueSystem = currentDialogueFrame.GetComponent<DialogueSystem>();

    }

    public void SetSceneDialogue()
    {
        sceneDialogue = fileSaverLoader.Load<SceneDialogue>("StartMovieScene.json");
        currentSentenceIndex = -1;
    }


    public void OpenDialogueFrame()
    {
        if (currentDialogueFrame == null)
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

    public void NextSentence()
    {
        

        if (currentSentenceIndex == -1)
        {
            OpenDialogueFrame();
            currentSentenceIndex = 0;
        }
        else if (currentSentenceIndex >= sceneDialogue.dialogues.Count)
        {
            CloseDialogueFrame();
            currentSentenceIndex = -1;
            sceneDialogue = null;
        }
        Debug.Log("Current Sentence Index: " + currentSentenceIndex);
        if (sceneDialogue != null)
        {
            dialogueSystem.TypingDialogue(sceneDialogue.dialogues[currentSentenceIndex].sentence);
        }
        else
        {
            Debug.LogError("Scene Dialogue is null.");
        }
    }
}
