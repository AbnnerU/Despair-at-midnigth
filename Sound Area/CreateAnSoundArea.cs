using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnSoundArea : SoundAreaCreator
{
    [SerializeField] protected bool drawGizmos;
    [SerializeField] private float soundArea;
    [SerializeField] private bool drawSoundArea;

    public void StartNewSoundArea()
    {     
        StartCoroutine(NewSoundArea(soundConfig.soundTransform, soundArea, soundConfig.soundIntensity));
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (soundConfig.soundTransform)
            {
                if (drawSoundArea)
                {
                    Gizmos.DrawSphere(soundConfig.soundTransform.position, soundArea);
                }

            }

            if (drawSound)
            {
                Gizmos.color = Color.yellow;
                drawSound = false;

                Gizmos.DrawSphere(soundConfig.soundTransform.position, drawArea);
            }
        }
    }
}
