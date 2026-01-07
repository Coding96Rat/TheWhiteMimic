using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject OptionFilm;

    [Space(20)]
    [Header("Graphics Setting")]
    [SerializeField]
    private GameObject GraphicsPanel;
    [SerializeField]
    private Button GraphicsBtn;

    [Space(15)]
    [Header("Sound Setting")]
    [SerializeField]
    private GameObject SoundPanel;
    [SerializeField]
    private Button SoundBtn;


    [Space(15)]
    [Header("GamePlay Setting")]
    [SerializeField]
    private GameObject GameplayPanel;
    [SerializeField]
    private Button GameplayBtn;

    [Space(15)]
    [Header("Language Setting")]
    [SerializeField]
    private GameObject LanguagePanel;
    [SerializeField]
    private Button LanguageBtn;


    [Space(15)]
    [Header("Return Setting")]
    [SerializeField]
    private Button ReturnBtn;



    private void Awake()
    {
        GraphicsBtn.onClick.AddListener(() => OpenGraphicPanel(true));
        SoundBtn.onClick.AddListener(() => OpenSoundPanel(true));
        GameplayBtn.onClick.AddListener(() => OpenGamePlayPanel(true));
        LanguageBtn.onClick.AddListener(() => OpenLanguagePanel(true));
        ReturnBtn.onClick.AddListener(Return);

    }

    public void OpenGraphicPanel(bool isOn)
    {
        OptionFilm.SetActive(isOn);
        GraphicsPanel.SetActive(isOn);
    }
    public void OpenSoundPanel(bool isOn)
    {
        OptionFilm.SetActive(isOn);
        SoundPanel.SetActive(isOn);
    }
    public void OpenGamePlayPanel(bool isOn)
    {
        OptionFilm.SetActive(isOn);
        GameplayPanel.SetActive(isOn);
    }

    public void OpenLanguagePanel(bool isOn)
    {
        OptionFilm.SetActive(isOn);
        LanguagePanel.SetActive(isOn);
    }
    public void Return()
    {
        OpenGamePlayPanel(false);
        OpenGraphicPanel(false);
        OpenSoundPanel(false);
        OptionFilm.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void CloseFilm()
    {
        OptionFilm.SetActive(false);
    }
}
