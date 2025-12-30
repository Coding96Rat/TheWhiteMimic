using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup dialogueCanvasGroup;
    private float dialogueBoxAlphaChangeSpeed;

    [SerializeField]
    private TextMeshProUGUI dialogueTextArea;
    [SerializeField]
    private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    public void DialogueIsOn(bool isOn)
    {
        if (isOn)

            dialogueCanvasGroup.alpha = 1;
        else
            dialogueCanvasGroup.alpha = 0;

        StartCoroutine(FadeInOutDialogueBox(isOn));
    }

    private IEnumerator FadeInOutDialogueBox(bool isFadeIn)
    {
        float targetAlpha = isFadeIn ? 1f : 0f;

        // dialogueBoxFadeSpeed: 1초당 변하는 Alpha값 (예: 2.0f면 0.5초만에 완료, 높을수록 빠름)
        while (!Mathf.Approximately(dialogueCanvasGroup.alpha, targetAlpha))
        {
            // MoveTowards(현재값, 목표값, 이번 프레임에 이동할 최대 거리)
            dialogueCanvasGroup.alpha = Mathf.MoveTowards(
                dialogueCanvasGroup.alpha,
                targetAlpha,
                dialogueBoxAlphaChangeSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 혹시 모를 오차 제거 및 확실한 마무리를 위해 명시적 대입
        dialogueCanvasGroup.alpha = targetAlpha;
    }

    public void TypingDialogue(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypingProcess(text));
    }

    private IEnumerator TypingProcess(string text)
    {
        dialogueTextArea.text = text;
        dialogueTextArea.maxVisibleCharacters = 0;

        var wait = new WaitForSeconds(typingSpeed);

        for (int i = 0; i <= text.Length; i++)
        {
            dialogueTextArea.maxVisibleCharacters = i;
            yield return wait;
        }

        typingCoroutine = null;
    }
}
