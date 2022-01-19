
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Brightness : MonoBehaviour
{
    [SerializeField] private GeneralConfig generalConfig;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;


    private void Awake()
    {
        volume.profile.TryGet(out colorAdjustments);

        generalConfig.OnValueModify += UpdateBrigthness;

        UpdateBrigthness();
    }

    private void UpdateBrigthness()
    {
        //Brilho
        colorAdjustments.postExposure.value = generalConfig.brightness;
    }
}
