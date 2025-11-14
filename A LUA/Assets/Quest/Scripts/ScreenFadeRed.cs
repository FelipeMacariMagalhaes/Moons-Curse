using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ScreenFadeRed : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f;    
    public float fadeDuration = 0.4f;     
    public Color flashColor = new Color(1, 0, 0, 0.4f);  

    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Fade in
        float t = 0;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            flashImage.color = Color.Lerp(Color.clear, flashColor, t / flashDuration);
            yield return null;
        }

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            flashImage.color = Color.Lerp(flashColor, Color.clear, t / fadeDuration);
            yield return null;
        }

        flashImage.color = Color.clear;
        flashRoutine = null;
    }
}

