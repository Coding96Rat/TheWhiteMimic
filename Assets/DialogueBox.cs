using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueBox : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueManager.Instance.NextSentence();
    }

}
