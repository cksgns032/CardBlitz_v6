using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : UIBase
{
    Image bg;
    public Coroutine fadeCoroutine;
    // Start is called before the first frame update
    public override void Init(PopUp_Name uiname)
    {
        base.Init(uiname);

        bg = GetComponentInChildren<Image>(true);
        bg.gameObject.SetActive(true);
    }
    public override void Draw(bool active)
    {
        base.Draw(active);
    }
    public override void Close()
    {
        base.Close();
    }

    public void FadeIn()
    {
        Draw(true);
        fadeCoroutine = StartCoroutine(IEFade(Color.black, new Color(0, 0, 0, 0), 2f, false));
    }
    public void FadeOut()
    {
        Draw(true);
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
