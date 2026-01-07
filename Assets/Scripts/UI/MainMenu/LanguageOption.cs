using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageOption : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown languageDropdown;

    [SerializeField]
    private Button BackBtn;

    bool isChanging;

    private OptionHandler optionHandler;

    private void Awake()
    {
        optionHandler = GetComponentInParent<OptionHandler>();
        languageDropdown.onValueChanged.AddListener(ChangeLocate);

        BackBtn.onClick.AddListener(ExitSoundSetting);
    }
    private void Start()
    {
        languageDropdown.RefreshShownValue();
        // 게임 시작 시 현재 설정된 언어를 확인하여 드롭다운 UI를 맞춤
        StartCoroutine(SyncDropdownToCurrentLocale());
    }

    IEnumerator SyncDropdownToCurrentLocale()
    {
        // 로컬라이제이션 시스템 초기화 대기
        yield return LocalizationSettings.InitializationOperation;

        // 현재 선택된 언어 코드 가져오기
        var currentCode = LocalizationSettings.SelectedLocale.Identifier.Code; // "ko", "en", "ja" 등

        // 이벤트가 발동되어 다시 언어를 설정하는 것을 방지하기 위해 리스너 잠깐 제거 혹은 플래그 사용
        isChanging = true;

        switch (currentCode)
        {
            case "en": languageDropdown.value = 0; break;
            case "ja": languageDropdown.value = 1; break;
            case "ko": languageDropdown.value = 2; break;
        }

        isChanging = false;
    }

    private void ChangeLocate(Int32 value)
    {
        if (isChanging)
            return;

        StartCoroutine(ChangeRoutine(value));
        Debug.Log(value);
    }

    IEnumerator ChangeRoutine(Int32 value)
    {
        isChanging = true;

        // 초기화가 진행도중이면 대기
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
        languageDropdown.RefreshShownValue();

        isChanging = false;
    }

    private void ExitSoundSetting()
    {

        optionHandler.CloseFilm();
        gameObject.SetActive(false);
    }
}
