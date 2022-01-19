using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class OnPressPause : UnityEvent { }

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [Header("Level Load")]
    [SerializeField] private LoadLevelData levelData;

    [SerializeField] private string loadingScene = "LOADING";

    [Header("Configs")]
    [SerializeField] private GeneralConfig generalConfig;

    [SerializeField] private Slider volumeSlider;

    [SerializeField] private Slider brightnessSlider;

    [SerializeField] private Slider cameraSensiSlider;

    public OnPressPause OnPause;

    public OnPressPause OnResume;

    private InputMapActive lastInputMapActive;
    private InputController inputController;

    private bool paused = false;

    private void Awake()
    {
        Time.timeScale = 1;

        inputController = FindObjectOfType<InputController>();

        lastInputMapActive = inputController.GetInputActived();

        inputController.OnPause += OnPauseInput;

        generalConfig.SeeEvents();

        generalConfig.OnValueModify?.Invoke();

        UpdateSlidersValue();
    }

    public void OnPauseInput()
    {
        if (paused)
        {
            Time.timeScale = 1;

            pausePanel.SetActive(false);

            if (lastInputMapActive == InputMapActive.GAMEPLAY)
            {
                inputController.EnableGameplayInputs();
            }
            else if(lastInputMapActive == InputMapActive.INVENTORY)
            {
                inputController.EnableInventoryInputs();
            }
            else if (lastInputMapActive == InputMapActive.CONTROLSDISABLED)
            {
                inputController.EnableControlsDisabledInputs();
            }

            paused = false;

            

            OnResume?.Invoke();

        }
        else
        {
           
            lastInputMapActive = inputController.GetInputActived();

            inputController.EnablePauseInputs();

            pausePanel.SetActive(true);

            paused = true;

            Time.timeScale = 0;

            OnPause?.Invoke();

        }
    }

    public void LoadScene(string sceneName)
    {
        generalConfig.PurgeInvocationList();
        generalConfig.SeeEvents();
        levelData.levelToLoad = sceneName;
        SceneManager.LoadScene(loadingScene);
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

    private void OnDisable()
    {
        generalConfig.PurgeInvocationList();
        generalConfig.SeeEvents();
    }

    private void OnDestroy()
    {
        generalConfig.PurgeInvocationList();
        generalConfig.SeeEvents();
    }


}
