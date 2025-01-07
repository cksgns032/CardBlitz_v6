using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    Text textPrefab;
    ScrollRect scrollRect;
    InputField inpuField;

    // Start is called before the first frame update
    void Start()
    {
        inpuField = GetComponentInChildren<InputField>(true);
        textPrefab = Resources.Load<Text>("ServerTest/Text (Legacy)");
        scrollRect = GetComponentInChildren<ScrollRect>(true);
    }

    public void AddChat(string chat)
    {
        Text addText = Instantiate(textPrefab, scrollRect.content);
        addText.text = chat;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(inpuField.isFocused)
            {
                // 포커스가 비활성화면 활성화 시킴
                inpuField.ActivateInputField();
            }
            if(inpuField.text.Length > 0)
            {
                /*TCPClient client = FindObjectOfType<TCPClient>();
                client.SendMsg(inpuField.text);

                inpuField.text = string.Empty;*/
            }
        }
    }
}
