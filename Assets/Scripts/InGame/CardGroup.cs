using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    Card[] cards;
    List<Card> select;
    public void Init()
    {
        //카드 생성
        for (int i = 0; i < 3; i++)
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.level = 1;
            cardInfo.id = i.ToString();
            UserData.gameDeck.Add(cardInfo);
        }
        cards = GetComponentsInChildren<Card>(true);
        foreach(Card card in cards)
        {
            card.Init();
        }

        Shuffle();
    }
    public void Shuffle()
    {
        UserData.gem = 9999;
        for (var i = 0; i < cards.Length; i++)
        {
            if (cards[i].gameObject.activeSelf == true)
            {
                int num = UnityEngine.Random.Range(0, UserData.gameDeck.Count);
                cards[i].Setting(UserData.gameDeck[num]);
            }
        }
    }
    // 카드 추가
    public void AddCard()
    {
        bool isFull = true;
        foreach(Card card in this.cards)
        {
            if(card.gameObject.activeSelf == false)
            {
                isFull = false;
                break;
            }
        }
        if(!isFull)
        {
            // 카드 활성화
            for (int i = 1; i < this.cards.Length; i++)
            {
                if (cards[i].gameObject.activeSelf == true)
                {
                    cards[i - 1].gameObject.SetActive(true);
                    int num = UnityEngine.Random.Range(0, UserData.gameDeck.Count);
                    cards[i - 1].Setting(UserData.gameDeck[num]);
                    break;
                }
            }
        }
    }

    // 선택할 카드
    public int SelectCard(Card obj)
    {
        select = new List<Card>();
        select.Add(obj);
        obj.UpPosition();
        //cursorArrow.anchoredPosition = obj.gameObject.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < cards.Length; i++)
        {
            if(obj.gameObject == cards[i].gameObject)
            {
                // 왼쪽이 있냐
                bool isleft = false;
                // 오른쪽이 있냐
                bool isright = false;
                // 왼쪽에 같은게 있냐
                bool getleft = false;
                // 오른쪽에 같은게 있냐
                bool getright = false;

                Debug.Log(obj.gameObject.name);
                // 중앙 기준
                // 왼쪽 확인
                if (i-1>=0)
                {
                    //CardInfo card = cards[i - 1].GetCardInfo();
                    if (cards[i-1].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 1].GetCardInfo().id)
                    {
                        cards[i - 1].UpPosition();
                        getleft = true;
                        select.Add(cards[i - 1]);
                    }
                    isleft = true;
                }
                // 오른쪽 확인
                if (i + 1 < 5)
                {
                    if (cards[i + 1].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 1].GetCardInfo().id)
                    {
                        cards[i + 1].UpPosition();
                        getright = true;
                        select.Add(cards[i + 1]);
                    }
                    isright = true;
                }

                // 가생이 기준
                // 왼쪽이 없을 때
                if (isleft == false && isright == true && getright == true)
                {
                    if (cards[i + 2] != null)
                    {
                        //CardInfo card = cards[i + 2].GetCardInfo();
                        if (cards[i + 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
                        {
                            cards[i + 2].UpPosition();
                            select.Add(cards[i + 2]);
                        }
                    }
                }
                // 오른쪽이 없을 때
                else if (isright == false && isleft == true && getleft == true)
                {
                    if (cards[i - 2] != null)
                    {
                        if (cards[i - 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
                        {
                            cards[i - 2].UpPosition();
                            select.Add(cards[i - 2]);
                        }
                    }
                }
                // 두번째 구역까지
                if (select.Count < 3)
                {
                    if(getleft)
                    {
                        if(i-2 >= 0)
                        {
                            if (cards[i - 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
                            {
                                cards[i - 2].UpPosition();
                                select.Add(cards[i - 2]);
                            }
                        }
                    }
                    else if(getright)
                    {
                        if(i+2 < 5)
                        {
                            if (cards[i + 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
                            {
                                cards[i + 2].UpPosition();
                                select.Add(cards[i + 2]);
                            }
                        }
                    }
                }
            }
        }
        return select.Count;
    }
    // 카드 정위치
    public void DeSelect()
    {
        for (int i = 0; i < select.Count; i++)
        {
            select[i].DownPosition();
        }
    }
    // 카드 데이터 정리
    public void UseCard()
    {
        // 가지고 있는 데이터 : 선택한 카드 리스트 , 가지고 있는 카드 리스트
        for (int k = 0; k < select.Count; k++)
        {
            select[k].gameObject.SetActive(false);
        }
        // 미사용 카드 정렬
        for (var j = 0; j < cards.Length - 1; j++)
        {
            if (cards[j].gameObject.activeSelf == true && cards[j + 1].gameObject.activeSelf == false)
            {
                cards[j + 1].gameObject.SetActive(true);
                cards[j + 1].Setting(cards[j].GetCardInfo());
                cards[j].gameObject.SetActive(false);
                cards[j].ResetCardInfo();
                for (int i = j; i >= 0; i--)
                {
                    if (cards[i].gameObject.activeSelf == true)
                    {
                        cards[i + 1].gameObject.SetActive(true);
                        cards[i + 1].Setting(cards[i].GetCardInfo());
                        cards[i].gameObject.SetActive(false);
                        cards[i].ResetCardInfo();
                    }
                }
            }
        } 
    }
}
