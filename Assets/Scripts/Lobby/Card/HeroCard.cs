using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ī�� ���̺� �ʿ�
// ������ ������ �ִ� ī����� ��ȣ�� ���ؼ� 
// ���̺��� �������� ������ͼ�
// ���ȿ� �־���
public class HeroCard : MonoBehaviour
{
    Button btn;
    CardStat stat;
    // Start is called before the first frame update
    public void Init()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClickCard);
    }
    public void Setting()
    {
        stat = new CardStat();
    }
    public void ClickCard()
    {
        /*if(UserData.state == State.CARD)
        {
            HeroInfo info = GameObject.FindObjectOfType<HeroInfo>(true);
            info.gameObject.SetActive(true);
        }
        else if(UserData.state == State.SETTING)
        {
            // ����ϴ� ī�� ����Ʈ�� �߰�
        }*/
    }
}
