using System.Collections;
using UnityEngine;

public abstract class SoundAreaCreator : MonoBehaviour
{

    [SerializeField] protected SoundConfig soundConfig;
    [SerializeField] protected int maxAllocation = 15;

    [Header("Sound duration")]
    [SerializeField] protected float soundDuration;
    [SerializeField] protected bool onlyOneFrameDuration;
   
    [Header("Mask")]
    [SerializeField] protected LayerMask layerMask;

    protected Collider[] results;

    protected bool drawSound;

    protected float drawArea;

    protected virtual void Awake()
    {
        results = new Collider[maxAllocation];
    }

    public virtual IEnumerator NewSoundArea(Transform origin, float area, float intensity)
    {
        if (onlyOneFrameDuration)
        {
            
            CreateArea(origin, area, intensity);
   
            yield return new WaitForEndOfFrame();

            drawSound = false;
        }
        else
        {
            float currentTime = 0;

            do
            {
                currentTime += Time.deltaTime;
                CreateArea(origin, area, intensity);

                yield return null;

            } while (currentTime < soundDuration);

            drawSound = false;
        }
    }

    public virtual void CreateArea(Transform origin, float area, float intensity)
    {
        drawSound = true;

        drawArea = area;

        int hitNumbers = Physics.OverlapSphereNonAlloc(origin.position, area, results, layerMask);

        if (hitNumbers > 0)
        {
           
            SoundIntensityReceptor receptor;
            for (int i = 0; i < hitNumbers; i++)
            {
                if (results[i].TryGetComponent<SoundIntensityReceptor>(out receptor))
                {
                    
                    receptor.RecibeSound(origin, intensity,soundConfig.soundType);
                }
            }
        }
    }
}

[System.Serializable]
public struct SoundConfig
{
    public Transform soundTransform;
    public SoundType soundType;
    public float soundIntensity;

}


public enum SoundType
{
    GENERIC,
    PLAYER
}
