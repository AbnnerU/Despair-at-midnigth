using UnityEngine;
using Cinemachine;
using System;

public class VitualCameraSpeedControler : MonoBehaviour
{
    [SerializeField] private GeneralConfig generalConfig;

    private CinemachineVirtualCamera virtualCamera;

    private CinemachinePOV cinemachinePOV;


    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        generalConfig.OnValueModify += UpdateCameraMovementValues;
    }

    public void StopRecibeInputs(bool stopInputs)
    {
        if (stopInputs)
            cinemachinePOV.enabled = false;
        else
            cinemachinePOV.enabled = true;

    }

    public void ChangeCameraDefaltSpeedValues(float newValue)
    {
        generalConfig.cameraSensi = newValue;
    }

    public void SetHorizontalSpeed(float newSpeed)
    {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = newSpeed;
    }

    public void SetVerticalSpeed(float newSpeed)
    {
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = newSpeed;
    }

    public void SetDefaltValues()
    {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = generalConfig.cameraSensi;

        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = generalConfig.cameraSensi;
    }

    public void UpdateCameraMovementValues()
    {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = generalConfig.cameraSensi;

        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = generalConfig.cameraSensi;

        //cinemachinePOV.m_HorizontalAxis.m_AccelTime = generalConfig.cameraAccelTime;

        //cinemachinePOV.m_HorizontalAxis.m_DecelTime = generalConfig.cameraDeccelTime;

        //cinemachinePOV.m_VerticalAxis.m_AccelTime = generalConfig.cameraAccelTime;

        //cinemachinePOV.m_VerticalAxis.m_DecelTime = generalConfig.cameraDeccelTime;
    }

    public void SpeedZero()
    {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = 0;

        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = 0;
    }
}

