using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TemporaryTextPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;

    [SerializeField] private float timeOnScreen;

    [SerializeField] private float blinkTimeOn;

    [SerializeField] private float blinkTimeOff;

    [SerializeField] private float transitionTime;

    private GameObject textObj;

    private void Awake()
    {
        textObj = textComponent.gameObject;
    }

    public void SetTextOnScreen()
    {
        StartCoroutine(TextBlink());
        StartCoroutine(TextOnScreen());
    }

    public void SetTextOnScreen(string text)
    {
        StopCoroutine(TextBlink());
        StopCoroutine(TextOnScreen());

        textComponent.text = text;

        StartCoroutine(TextOnScreen());
        StartCoroutine(TextBlink());
    }

    public void SetPermanentText(string text)
    {
        StopCoroutine(TextBlink());
        StopCoroutine(TextOnScreen());

        textComponent.text = text;

        textComponent.enabled = true;

        StartCoroutine(TextBlink());
    }

    IEnumerator TextOnScreen()
    {
        textComponent.enabled = true;

        yield return new WaitForSeconds(timeOnScreen);

        DisableText();
        textComponent.enabled = false;

        yield break;
    }

    IEnumerator TextBlink()
    {
        bool turnOn = false;
        Color color = textComponent.color;

        float alpha = 1f;

        color.a = alpha;

        textComponent.color = color;

        do
        {
            if (turnOn == false)
            {
                alpha -= (1/transitionTime)* Time.deltaTime;

                if (alpha <= 0)
                {
                    alpha = 0;

                    color.a = alpha;

                    textComponent.color = color;

                    yield return new WaitForSeconds(blinkTimeOff);
                    turnOn = true;
                }
                else
                {
                    color.a = alpha;

                    textComponent.color = color;
                }
            }
            else
            {
                alpha += (1/transitionTime) * Time.deltaTime;

                if (alpha >= 1)
                {
                    alpha = 1;

                    color.a = alpha;

                    textComponent.color = color;

                    yield return new WaitForSeconds(blinkTimeOn);
                    turnOn = false;
                }
                else
                {
                    color.a = alpha;

                    textComponent.color = color;
                }
            }

            yield return new WaitForEndOfFrame();

        } while (textComponent.enabled == true && textComponent.gameObject.activeSelf == true);

        yield break;
    }

    public void DisableText()
    {
        textComponent.enabled = false;

        StopAllCoroutines();
    }
}
