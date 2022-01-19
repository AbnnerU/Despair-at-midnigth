using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject confirmationScreen;  
    [SerializeField] private string onNewGameScreen;

    [Header("Configs")]
    [SerializeField] private GeneralConfig generalConfig;
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private Slider brightnessSlider;

    [SerializeField] private Slider cameraSensiSlider;

    private void Awake()
    {
        if (levelData.hasSaveData==false)
        {
            continueButton.interactable = false;
        }

        UpdateSlidersValue();
    }

    public void NewGame()
    {
        if (levelData.hasSaveData)
        {
            confirmationScreen.SetActive(true);
        }
        else
        {
            loadingScreen.LoadScene(onNewGameScreen);
        }
        
    }

    public void ConfirmNemGame()
    {
        Save.DeleteData(levelData.interagibleDataloadFilePath);
        Save.DeleteData(levelData.objectDataloadFilePath);
        Save.DeleteData(levelData.inventoryDataPath);
        Save.DeleteData(levelData.lifesAmountPath);
        levelData.hasSaveData = false;

        loadingScreen.LoadScene(onNewGameScreen);
    }

    public void CancelNewGame()
    {
        confirmationScreen.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateSoundVolume(float value)
    {
        generalConfig.gameVolume = (int)value;

        generalConfig.OnValueModify?.Invoke();
    }

    public void UpdateBrightness(float value)
    {
        generalConfig.brightness = value;

        generalConfig.OnValueModify?.Invoke();
    }

    public void UpdateCameraSensi(float value)
    {
        generalConfig.cameraSensi = value;

        generalConfig.OnValueModify?.Invoke();
    }


    public void UpdateSlidersValue()
    {
        volumeSlider.value = generalConfig.gameVolume;

        brightnessSlider.value = generalConfig.brightness;

        cameraSensiSlider.value = generalConfig.cameraSensi;
    }
}
