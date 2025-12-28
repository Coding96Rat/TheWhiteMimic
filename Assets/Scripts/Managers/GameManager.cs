using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private MainMenuHandler MenuHandler;
    private GameSettingHandler gameSettingHandler;


    // InGame true 면 MainMenuHandler에서 Start Disabled 하기
    public bool InGame = false;

    public override void Awake()
    {
        base.Awake();
        MenuHandler = FindFirstObjectByType<MainMenuHandler>();
        gameSettingHandler = GetComponent<GameSettingHandler>();
        Application.targetFrameRate = 60;

    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartGame()
    {
        // In main menu
        if (!InGame)
        {
            MenuHandler.Loading("StartMovieScene");
            InGame = true;
        }
    }

    public void ExitGame()
    {
        // GameManager Quit 으로
        Application.Quit();
    }

    public void UpdateGameSetting(string settingType)
    {
        if (!InGame)            
            return;
        
        // 현재로썬 Gameplay 만이 해당 기능 필요
        if(gameSettingHandler != null)
        {
            gameSettingHandler.UpdateGameplaySettingValue();
        }


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"{scene.name} 씬이 로드되었습니다! 초기화 작업을 시작합니다.");

        // 예: 그래픽 설정 재적용
        GraphicManager.Instance.InitGraphicSetting();

    }

}
