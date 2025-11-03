using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FadeScreenHouse : MonoBehaviour
{
   public Image fadeImage; // arraste o FadeImage aqui
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            // Garante que o fade comece transparente
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    public void FadeOutAndChangeScene(string sceneName)
    {
        StartCoroutine(FadeOutRoutine(sceneName));
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        float timer = 0f;
        Color c = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Espera 1 seg antes de trocar
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}


