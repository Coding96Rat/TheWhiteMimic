using System.Collections;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup SceneFilm;

    string currentSceneName;
    [SerializeField]
    private float fadeDuration = 1.0f;
    [SerializeField]
    private float stayTime = 3.0f;

    private void Awake()
    {
        SceneFilm.alpha = 1.0f;
    }
    private void Start()
    {
        currentSceneName = GameManager.Instance.GetCurrentSceneName();

        // 로컬라이제이션 도입으로 인해 주석 처리
        //DialogueManager.Instance.SetSceneDialogue();

        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(stayTime);
        yield return StartCoroutine(FadeInOut(true));

        // 대사 시작
        // 로컬라이제이션 도입으로 인해 주석 처리
        DialogueManager.Instance.NextSentence();
    }

    IEnumerator FadeInOut(bool isFadeIn)
    {

        float elapsed = 0.0f;
        if (isFadeIn)
        {
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                SceneFilm.alpha = 1.0f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            SceneFilm.alpha = 0.0f;
        }
        else
        {
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                SceneFilm.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            SceneFilm.alpha = 1.0f;
        }
    }
}
