using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : UIBase
{
    Image bg;
    public Coroutine fadeCoroutine;
    // Start is called before the first frame update
    public override void Init(Layer_Type type, string name)
    {
        base.Init(type, name);
        bg = GetComponentInChildren<Image>(true);
        bg.gameObject.SetActive(true);
    }
    public override void Draw()
    {
        base.Draw();
        gameObject.SetActive(true);
    }
    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        fadeCoroutine = StartCoroutine(IEFade(Color.black, new Color(0, 0, 0, 0), 2f, false));
    }
    public void FadeOut()
    {
        fadeCoroutine = StartCoroutine(IEFade(new Color(0, 0, 0, 0), Color.black, 1f, true));
    }
    IEnumerator IEFade(Color startColor, Color endColor, float delayTime, bool active)
    {
        bg.gameObject.SetActive(true);
        bg.color = startColor;
        float elapsed = 0;
        while (true)
        {
            elapsed += Time.deltaTime / delayTime;
            bg.color = Color.Lerp(startColor, endColor, elapsed);
            if (elapsed >= 1.0f)
            {
                if (!active)
                {
                    StopCoroutine(fadeCoroutine);
                    Close();
                }
                break;
            }
            yield return null;
        }
        yield return null;
    }
}
