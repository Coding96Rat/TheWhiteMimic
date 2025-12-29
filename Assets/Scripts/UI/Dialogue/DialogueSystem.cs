using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup dialogueCanvasGroup;

    public void DialogueIsOn(bool isOn)
    {
        if (isOn)
            dialogueCanvasGroup.alpha = 1;
        else
            dialogueCanvasGroup.alpha = 0;
    }
}
