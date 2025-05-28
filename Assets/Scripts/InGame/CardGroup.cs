using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    List<Card> cards = new List<Card>();
    List<Card> select;
    UserGameData userData;
    public void Init()
    {
        userData = GameManager.Instance.GetMyGameData();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (child != null)
            {
                Card card = child.GetComponentInChildren<Card>();
                if (card != null)
                {
                    cards.Add(card);
                }
            }
        }
        foreach (Card card in cards)
        {
            card.Init();
        }
    }
    // todo : 서버 생기면 연결
    public void Shuffle()
    {
        GameUI gameUI = GameManager.Instance.GetGameUI();
        gameUI.StopCardFill();
        for (var i = 0; i < cards.Count; i++)
        {
            if (cards[i].gameObject.activeSelf == true && cards[i].GetIsUse())
            {
                int num = Random.Range(0, userData.gameDeck.Count);
                cards[i].Setting(userData.gameDeck[num], false);
                userData.HandCards[i] = userData.gameDeck[num];
                cards[i].PlayAni(i * 100);
            }
        }
        gameUI.CardFill();
    }
    // ī�� �߰�
    public void AddCard()
    {
        bool isFull = true;
        foreach (CardInfo i in userData.HandCards)
        {
            if (i == null)
            {
                isFull = false;
                break;
            }
        }
        if (!isFull)
        {
            for (int i = 0; i < this.cards.Count; i++)
            {
                if (cards[i].gameObject.activeSelf == false && cards[i].GetIsUse() == false)
                {
                    cards[i].gameObject.SetActive(true);
                    int num = UnityEngine.Random.Range(0, userData.gameDeck.Count);
                    cards[i].Setting(userData.gameDeck[num], false);
                    userData.HandCards[i] = userData.gameDeck[num];
                    cards[i].PlayAni(0);
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
        for (int i = 0; i < cards.Count; i++)
        {
            if (obj.gameObject == cards[i].gameObject)
            {
                // ������ �ֳ�
                bool isleft = false;
                // �������� �ֳ�
                bool isright = false;
                // ���ʿ� ������ �ֳ�
                bool getleft = false;
                // �����ʿ� ������ �ֳ�
                bool getright = false;

                // �߾� ����
                // ���� Ȯ��
                if (i - 1 >= 0)
                {
                    //CardInfo card = cards[i - 1].GetCardInfo();
                    if (cards[i - 1].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 1].GetCardInfo().id)
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
                    if (getleft)
                    {
                        if (i - 2 >= 0)
                        {
                            if (cards[i - 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
                            {
                                cards[i - 2].UpPosition();
                                select.Add(cards[i - 2]);
                            }
                        }
                    }
                    else if (getright)
                    {
                        if (i + 2 < 5)
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
            select[k].SetUse(false);
            select[k].gameObject.SetActive(false);
        }
        // �̻�� ī�� ����
        for (int j = cards.Count - 1; j >= 1; j--)//
        {
            if (cards[j].gameObject.activeSelf == true && cards[j - 1].gameObject.activeSelf == false)
            {
                cards[j - 1].SetUse(true);
                cards[j - 1].gameObject.SetActive(true);
                cards[j - 1].Setting(cards[j].GetCardInfo(), true);
                userData.HandCards[j - 1] = cards[j].GetCardInfo();
                cards[j].SetUse(false);
                cards[j].gameObject.SetActive(false);
                cards[j].ResetCardInfo();
                for (int i = j; i < cards.Count; i++)
                {
                    if (cards[i].gameObject.activeSelf == true)
                    {
                        cards[i - 1].SetUse(true);
                        cards[i - 1].gameObject.SetActive(true);
                        cards[i - 1].Setting(cards[i].GetCardInfo(), true);
                        userData.HandCards[i - 1] = cards[i].GetCardInfo();
                        cards[i].SetUse(false);
                        cards[i].gameObject.SetActive(false);
                        cards[i].ResetCardInfo();
                    }
                }
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].gameObject.activeSelf == false)
            {
                userData.HandCards[i] = null;
            }
        }
    }
}
