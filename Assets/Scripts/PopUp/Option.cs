using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Option : UIBase
{
    public ButtonExtension exit;
    Slider bgmSlider;
    Slider soundSlider;
    // Start is called before the first frame update
    public override void Init(PopUp_Name uiname)
   {
        base.Init(uiname);

        exit = gameObject.transform.Find("ExitBtn").GetComponent<ButtonExtension>();
        exit.AddListener(()=>Close());
        bgmSlider = gameObject.transform.Find("BGMSlider").GetComponent<Slider>();
        soundSlider = gameObject.transform.Find("SoundSlider").GetComponent<Slider>();
        bgmSlider.value = UserData.bgmVolume;
        soundSlider.value = UserData.soundVolume;
    }
    public override void Draw(bool active)
    {
        base.Draw(active);

    }
    public override void Close()
    {
        base.Close();
        //AudioManager.Instance.BGMVolume(bgmSlider.value);
        //AudioManager.Instance.SoundVolume(soundSlider.value);
    }
}
