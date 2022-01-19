
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private GeneralConfig generalConfig;

    [Range(0, 1)]
    [SerializeField] private float defaltVolume=1;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        generalConfig.OnValueModify += UpdateVolume;

        UpdateVolume();
    }

    public void UpdateVolume()
    {
        if (audioSource)
        {
            float volumePercentage = generalConfig.gameVolume;

            volumePercentage = volumePercentage / 100;

            audioSource.volume = defaltVolume * volumePercentage;
        }
    }

    public void SetVolumeValue(float volumeValue)
    {
        volumeValue = Mathf.Clamp(volumeValue, 0, 1);

        audioSource.volume = volumeValue;
    }

}
