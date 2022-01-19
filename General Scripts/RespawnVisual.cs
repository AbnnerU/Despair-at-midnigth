using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RespawnVisual : MonoBehaviour
{
    [SerializeField] private Respawn respawn;

    [SerializeField] private TMP_Text textComponent;

    private void Awake()
    {
        respawn.OnUpdateLife += OnUpdateLife;
    }

    private void OnUpdateLife(int value)
    {
        print("Ola");
        textComponent.text = value.ToString();
    }
}
