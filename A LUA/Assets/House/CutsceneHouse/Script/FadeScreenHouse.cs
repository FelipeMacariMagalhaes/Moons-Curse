using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class FadeScreenHouse : MonoBehaviour
{
    public static FadeScreenHouse Instance;
     public Image fadeImage; // arraste uma Image preta no Canvas
    public float fadeDuration = 1.5f;
    public TMP_Text fadeText;

    private void Awake()
    {
        Instance = this;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeText.color = new Color(1, 1, 1, 0);
    }

    public IEnumerator PlayFinalSequence()
    {
        // Fade in total
        yield return StartCoroutine(FadePanel(0, 1, 2f));

        // Mostra texto 1
        yield return StartCoroutine(ShowText("O telefone é o único que posso confiar.", 2f));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(HideText(1f));

        // Mostra texto 2
        yield return StartCoroutine(ShowText("Por enquanto...", 2f));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(HideText(1.5f));

        // Fade out pra próxima cena
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("NextScene"); // troca de cena aqui
    }

    IEnumerator FadePanel(float start, float end, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(start, end, t / duration));
            yield return null;
        }
    }

    IEnumerator ShowText(string text, float fadeTime)
    {
        fadeText.text = text;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeText.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t / fadeTime));
            yield return null;
        }
    }

    IEnumerator HideText(float fadeTime)
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeText.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t / fadeTime));
            yield return null;
        }
    }
    public void FadeOutAndChangeScene(string sceneName)
    {
        StartCoroutine(FadeOutRoutine(sceneName));
    }
     private IEnumerator FadeOutRoutine(string sceneName)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
