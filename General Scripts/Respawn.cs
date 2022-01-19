using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GenericAction : UnityEvent { }

public class Respawn : MonoBehaviour
{
    [SerializeField] private int lifes;

    [SerializeField] private Transform playerRef;

    [SerializeField] private Vector3 respawnPoint;

    [SerializeField] private Transform enemyRef;

    [SerializeField] private Vector3 enemyRespawnPoint;

    [SerializeField] private bool useDelay;

    [SerializeField] private float delayToDeath;

    [SerializeField] private float delayToRespanw;

    public GenericAction OnRespawnActions;

    public GenericAction OnDeath;

    public Action<int> OnUpdateLife;

    private int currentLife;

    private void Awake()
    {
        currentLife = lifes;
    }

    public void DecreaseLife()
    {
        currentLife--;

        OnUpdateLife?.Invoke(currentLife);

        if (useDelay)
        {
            if (currentLife <= 0)
            {              
                StartCoroutine(DeathDelay());
            }
            else
            {              
                StartCoroutine(RespanwDelay());
            }
        }
        else
        {
            if (currentLife <= 0)
            {
                OnDeath?.Invoke();             
            }
            else
            {
                RespawnPlayer();               
            }
        }
    }

    private void RespawnPlayer()
    {
        playerRef.transform.position = respawnPoint;

        enemyRef.transform.position = enemyRespawnPoint;

        OnRespawnActions?.Invoke();
    }

    public int GetCurrentLife()
    {
        return currentLife;
    }

    public void SetCurrentLife(int value)
    {
        currentLife = value;

        OnUpdateLife?.Invoke(currentLife);
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(delayToDeath);
        OnDeath?.Invoke();

        yield break;
    }

    IEnumerator RespanwDelay()
    {
        yield return new WaitForSeconds(delayToRespanw);
        RespawnPlayer();

        yield break;
    }
}
