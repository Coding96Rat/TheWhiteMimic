using UnityEngine;
using UnityEngine.Localization; // 공식 패키지
using UnityEngine.Localization.Settings; // 언어 설정 감지용
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoLocalizeFont : MonoBehaviour
{
    // 인스펙터에서 Asset Table에 등록한 'MainFont' 키를 연결하세요.
    public LocalizedAsset<TMP_FontAsset> localizedFontAsset;

    private TextMeshProUGUI textComp;

    void Awake()
    {
        textComp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // 1. 언어가 바뀔 때마다 실행될 함수 등록 (옵저버 패턴)
        localizedFontAsset.AssetChanged += UpdateFont;

        // 2. 씬이 켜질 때 초기화 (현재 언어에 맞는 폰트 로드)
        // LoadAssetAsync는 내부적으로 캐싱되므로 성능 문제 없음
        var handle = localizedFontAsset.LoadAssetAsync();
        if (handle.IsDone)
            UpdateFont(handle.Result);
    }

    void OnDisable()
    {
        // 메모리 누수 방지를 위해 이벤트 해제
        localizedFontAsset.AssetChanged -= UpdateFont;
    }

    // 실제로 폰트를 바꾸는 함수
    void UpdateFont(TMP_FontAsset newFont)
    {
        if (newFont != null)
        {
            textComp.font = newFont;

            // 폰트가 바뀌면 텍스트가 깨져 보일 수 있으므로 갱신
            textComp.UpdateFontAsset();
        }
    }
}