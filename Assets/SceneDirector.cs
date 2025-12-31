using System.Collections;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    [SerializeField]
    private GameObject SceneFilm;

    string currentSceneName;

    private float stayTime = 3.0f;
    private void Start()
    {
        currentSceneName = GameManager.Instance.GetCurrentSceneName();
        DialogueManager.Instance.SetSceneDialogue();
        StartCoroutine(PlayScene());
    }

    IEnumerator PlayScene()
    {
        yield return new WaitForSeconds(stayTime);
        DialogueManager.Instance.NextSentence();
    }

}
