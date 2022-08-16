using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeinDuration;
    public float fadeOutDuration;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeoutIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeinDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}