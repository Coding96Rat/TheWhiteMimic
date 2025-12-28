using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menu List")]
    [SerializeField]
    private Button StartBtn;
    [SerializeField]
    private Button ContinueBtn;
    [SerializeField]
    private Button OptionBtn;
    [SerializeField]
    private Button ExitBtn;
    [Space(15)]
    [Header("Option")]
    [SerializeField]
    private GameObject Option;
    private CanvasGroup mainMenuCanvasGroup;

    private SelectHandler selectHandler;
    private bool isMenuOn;

    [Space(15)]
    [Header("Loading Screen")]
    [SerializeField]
    private CanvasGroup loadingScreen;
    [SerializeField]
    private Slider loadingSlider;
    [Tooltip("Loading speed")]
    [SerializeField] 
    private float fakeLoadingSpeed = 2.0f; // 여기서 시간 조절하세요!


    private void Awake()
    {

        loadingScreen.alpha = 0f;
        loadingScreen.interactable = false;
        loadingScreen.blocksRaycasts = false;

        selectHandler = FindFirstObjectByType<SelectHandler>();

        StartBtn.onClick.AddListener(() => selectHandler.OpenAskStartPanel(true));
        ContinueBtn.onClick.AddListener(()=> selectHandler.OpenSaveSlotPanel(true));
        OptionBtn.onClick.AddListener(() => selectHandler.OpenOptionPanel(true));
        ExitBtn.onClick.AddListener(() => selectHandler.OpenAskExitPanel(true));

        mainMenuCanvasGroup = GetComponentInParent<CanvasGroup>();
    }

    private void Start()
    {
        // InGame   
        if (GameManager.Instance.InGame)
        {
            StartBtn.interactable = false;
            mainMenuCanvasGroup.interactable = false;
            mainMenuCanvasGroup.blocksRaycasts = false;
            mainMenuCanvasGroup.alpha = 0f;
        }
        else
        {
            StartBtn.interactable = true;
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
            mainMenuCanvasGroup.alpha = 1.0f;
        }
    }

    public bool MainMenuTrigger()
    {
        isMenuOn = !isMenuOn;
        if (isMenuOn)
        {
            mainMenuCanvasGroup.alpha = 1;
        }
        else
        {
            mainMenuCanvasGroup.alpha = 0;
        }

        mainMenuCanvasGroup.blocksRaycasts = isMenuOn;
        mainMenuCanvasGroup.interactable = isMenuOn;

        if (Option.activeInHierarchy)
        {
            if(Option.TryGetComponent<OptionHandler>(out OptionHandler optionHandler))
            {
                optionHandler.Return();
            }
        }

        return isMenuOn;
    }


    public void Loading(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        
            loadingScreen.alpha = 1f;
            loadingScreen.interactable = true;
            loadingScreen.blocksRaycasts = true;
        

        loadingSlider.value = 0f;
        //if (loadingPercentText != null) loadingPercentText.text = "0%";

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        // 자동 전환을 잠시 막아 로딩 완료 화면/애니메이션을 보여줄 수 있음
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

                
            // op.progress는 0..0.9 까지 채워지고, 0.9는 '로딩 완료 대기' 상태
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, progress, fakeLoadingSpeed * Time.deltaTime);
            Debug.Log($"Loading Progress: {progress * 100f}%");
            //if (loadingPercentText != null) loadingPercentText.text = $"{Mathf.RoundToInt(progress * 100f)}%";

            // 로드가 끝나고 activation 대기 상태
            if (op.progress >= 0.9f && loadingSlider.value >= 0.9f)
            {
                loadingSlider.value = 1f;
                //if (loadingPercentText != null) loadingPercentText.text = "100%";

                // 원하면 페이드아웃/애니메이션/유저 확인 등을 여기서 처리
                yield return new WaitForSeconds(0.4f);

                // 씬 활성화
                op.allowSceneActivation = true;
            }

        }
    }

}
