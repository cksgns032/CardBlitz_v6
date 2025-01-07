using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    Text title;
    Text story;
    Text process;
    Image clear;
    Button btn;

    int currentProcess;
    int maxProcess;
    // Start is called before the first frame update
    public void Init()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClickBtn);
        title = transform.Find("Name").GetComponent<Text>();
        story = transform.Find("Story").GetComponent<Text>();
        process = transform.Find("Process").GetComponent<Text>();
        clear = transform.Find("Clear").GetComponent<Image>();
        clear.gameObject.SetActive(false);  
    }
    public void ClickBtn()
    {
        if(currentProcess >= maxProcess)
        {
            clear.gameObject.SetActive(true);
        }
    }
    public void Setting()
    {

    }
}
