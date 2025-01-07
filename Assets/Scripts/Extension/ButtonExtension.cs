using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonExtension : Button
{
    public void AddListener(UnityAction func)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        onClick.AddListener(func);
    }
}
