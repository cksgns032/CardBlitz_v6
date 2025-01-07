using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    Animation ani;

    // Start is called before the first frame update
    public void Init()
    {
        ani = GetComponent<Animation>();
    }

    public void AniPlay()
    {
        gameObject.SetActive(true);
        ani.Play("Loading");
    }
}
