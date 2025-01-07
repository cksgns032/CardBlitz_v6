using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 카드 테이블 필요
// 유저가 가지고 있는 카드고유 번호를 비교해서 
// 테이블에서 정보들을 가지고와서
// 스탯에 넣어줌
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
            // 사용하는 카드 리스트에 추가
        }*/
    }
}
