using System;
using UnityEngine;

[CreateAssetMenu(fileName = "General Config", menuName = "Assets/New General Config")]
public class GeneralConfig : ScriptableObject
{
   
    [Header("DEFALT VALUES")]

    [Range(0, 100)]
    [SerializeField] private int defaltGameVolume=100;

    [Range(-2, 1.2f)]
    [SerializeField] private float defaltBrightness=0;

    [Range(50, 1000)]
    [SerializeField] private float defaltCameraSensi=100;

    [Range(0, 1)]
    [SerializeField] private float defaltCameraAccelTime=0.02f;

    [Range(0, 1)]
    [SerializeField] private float defaltCameraDeccelTime=0.02f;

    [Space(20)]
    [Range(0,100)]
    public int gameVolume;

    [Range(-2,1.2f)]
    public float brightness;

    [Range(50,500)]
    public float cameraSensi;

    [Range(0, 1)]
    public float cameraAccelTime;

    [Range(0, 1)]
    public float cameraDeccelTime;

    public Action OnValueModify;
  
    public void SetAllToDefalt()
    {
        gameVolume = defaltGameVolume;

        brightness = defaltBrightness;

        cameraSensi = defaltCameraSensi;

        cameraAccelTime = defaltCameraAccelTime;

        cameraDeccelTime = defaltCameraDeccelTime;
    }

    public void SeeEvents()
    {
        if(OnValueModify!=null)
        Debug.Log(OnValueModify.GetInvocationList().Length>0);
    }

    public void PurgeInvocationList()
    {
        //int invocationLenght = OnValueModify.GetInvocationList().Length;
        //for (int i = 0; i < invocationLenght; i++)
        //{
            OnValueModify = null;
           
        //}
    }

}
