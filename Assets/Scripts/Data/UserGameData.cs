using System.Collections.Generic;
using UnityEngine;

public class UserGameData
{
    public List<CardInfo> gameDeck = new List<CardInfo>();
    public TeamType team;
    public CardInfo[] HandCards = new CardInfo[5];
    public float gauge;

    public void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.level = 1;
            cardInfo.id = i.ToString();
            gameDeck.Add(cardInfo);
        }
    }
}
