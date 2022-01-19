
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BreathVisual : MonoBehaviour
{
    [SerializeField] private BreathSound breathSound;
    [SerializeField] private Volume volume;
    [SerializeField] private float maxChromaticValue;
    [SerializeField] private float maxVignetteValue;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    private float maxBreathValue;


    private void Awake()
    {
        volume.profile.TryGet(out chromaticAberration);

        volume.profile.TryGet(out vignette);

        breathSound.OnUpdateBreathValue += UpdateBreathEffect;

        maxBreathValue = breathSound.GetBreathMaxValue();

    }

    private void UpdateBreathEffect(float value)
    {
        float percentage = (100 * value) / maxBreathValue;

        vignette.intensity.value = maxVignetteValue * (percentage / 100);
        chromaticAberration.intensity.value = maxChromaticValue * (percentage / 100);
    }
}
