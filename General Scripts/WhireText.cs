using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class OnEndWriting : UnityEvent { }

[System.Serializable]
public class OnStartWriting : UnityEvent { }

public class WhireText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;

    [TextArea]
    [SerializeField] private string text;

    [SerializeField] private float writingSpeed;

    public OnStartWriting onStartWriting;

    public OnEndWriting onEndWriting;

    private bool alreadyWhire = false;

    private int totalCharacters;

    private void Awake()
    {
        totalCharacters = text.Length;

        textComponent.text = text;

        textComponent.maxVisibleCharacters = 0;
    }

    public void StartWritingText()
    {
        if (alreadyWhire == false)
        {
            StartCoroutine(WritingText());
            alreadyWhire = true;
        }
    }


    IEnumerator WritingText()
    {
        print("Começou");
        onStartWriting?.Invoke();

        textComponent.maxVisibleCharacters = 0;
        int currentNumber = 1;

        do
        {
            textComponent.maxVisibleCharacters = currentNumber;
            currentNumber++;

            yield return new WaitForSeconds(writingSpeed);

        } while (currentNumber < totalCharacters);


        onEndWriting?.Invoke();
    }
}
