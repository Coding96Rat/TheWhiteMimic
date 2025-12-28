using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicManager : Singleton<GraphicManager>
{
    public VolumeProfile volumeProfile;

    public List<Resolution> uniqueList = new List<Resolution>();

    [HideInInspector]
    public int BaseResolutionIndex;

    public override void Awake()
    {
        base.Awake();
        // 1. 기존 Screen.resolutions 가져오기
        Resolution[] allResolutions = Screen.resolutions;
        uniqueList.Clear(); // 리스트 초기화

        // 2. 중복 제거 로직 (작성하신 코드 그대로 사용)
        foreach (var res in allResolutions)
        {
            int index = uniqueList.FindIndex(item => item.width == res.width && item.height == res.height);

            if (index == -1)
            {
                uniqueList.Add(res);
            }
            else
            {
                // 주사율 더 높은 것으로 교체
                if (res.refreshRateRatio.value > uniqueList[index].refreshRateRatio.value)
                {
                    uniqueList[index] = res;
                }
            }
        }

        for (int i = 0; i < uniqueList.Count; i++)
        {
            Resolution res = uniqueList[i];

            if (res.width == Screen.width && res.height == Screen.height)
            {
                BaseResolutionIndex = i;
            }
        }

        InitGraphicSetting();
    }


    #region Toggle Method
    private bool ToggleIntSwitch(int isOn)
    {
        if (isOn == 1)
            return true;
        else
            return false;
    }
    private int ToggleIntSwitch(bool isOn)
    {
        if (isOn)
            return 1;
        else
            return 0;
    }
    #endregion
    public void InitGraphicSetting()
    {
        //Brightness
        if (volumeProfile != null)
        {
            if (volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("brightness", 1.0f);
            }
        }

        // Fullscreen
        Screen.fullScreen = ToggleIntSwitch(PlayerPrefs.GetInt("fullscreen", 1));

        // Vsync
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync", 0);


        int resolutionIndex = PlayerPrefs.GetInt("resolution", BaseResolutionIndex);
        Resolution resolution = uniqueList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);


        // Quality
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality", QualitySettings.GetQualityLevel()));


    }

    public void ApplyGraphicSet(float brightnessAmount, bool fullScreenisOn, int vsyncCount, int resolutionIndex, int qualityIndex)
    {
        //Brightness

        if (volumeProfile != null)
        {
            if (volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = brightnessAmount;
            }
        }

        //Fullscreen

        // Set Fullscreen
        Screen.fullScreen = fullScreenisOn;

        // Set Quality
        QualitySettings.SetQualityLevel(qualityIndex);

        //Vsync

        QualitySettings.vSyncCount = vsyncCount;




        //Resolution

        Resolution resolution = uniqueList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullScreenisOn);

    }
}
