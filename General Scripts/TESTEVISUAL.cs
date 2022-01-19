using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TESTEVISUAL : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;

    [SerializeField] private Volume volume;

    [SerializeField] private float minDistance;

    [SerializeField] private float maxDistance;

    [SerializeField] private float normalValue;
    [SerializeField] private float closeValue;


    private ColorAdjustments colorAdjustments;

    private float distanceDifference;

    private float valueDiferrence;

    private void Awake()
    {
        volume.profile.TryGet(out colorAdjustments);

        distanceDifference = minDistance-maxDistance;

        valueDiferrence = closeValue - normalValue;

        
    }


    private void FixedUpdate()
    {
        if (enemy.gameObject.activeSelf == true)
        {
            float distance = (enemy.position - player.position).magnitude;

            if (distance < minDistance)
            {
                float diferrence = minDistance - distance;

                float percentage = ((100 * diferrence) / distanceDifference) / 100;

                colorAdjustments.saturation.value = valueDiferrence * percentage;

               
            }
        }
    }

    public void ResetColor()
    {
        colorAdjustments.saturation.value = normalValue;
    }
}
