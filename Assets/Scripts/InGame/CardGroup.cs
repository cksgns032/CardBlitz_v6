using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    Card[] cards;
    List<Card> select;
    public void Init()
    {
        //ī�� ����
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
    // ī�� �߰�
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
            // ī�� Ȱ��ȭ
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

    // ������ ī��
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
                // ������ �ֳ�
                bool isleft = false;
                // �������� �ֳ�
                bool isright = false;
                // ���ʿ� ������ �ֳ�
                bool getleft = false;
                // �����ʿ� ������ �ֳ�
                bool getright = false;

                Debug.Log(obj.gameObject.name);
                // �߾� ����
                // ���� Ȯ��
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
                // ������ Ȯ��
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

                // ������ ����
                // ������ ���� ��
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
                // �������� ���� ��
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
                // �ι�° ��������
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
    // ī�� ����ġ
    public void DeSelect()
    {
        for (int i = 0; i < select.Count; i++)
        {
            select[i].DownPosition();
        }
    }
    // ī�� ������ ����
    public void UseCard()
    {
        // ������ �ִ� ������ : ������ ī�� ����Ʈ , ������ �ִ� ī�� ����Ʈ
        for (int k = 0; k < select.Count; k++)
        {
            select[k].gameObject.SetActive(false);
        }
        // �̻�� ī�� ����
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
