using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IDragHandler
{
    // 카드 데이터 받아 오기
    // 코스트, 타입, 공격력, 피, 이름
    string cardName;
    int selectCardNum;

    CardInfo cardinfo;
    Image charImg;
    CardGroup cardGroup;
    RectTransform rectCom;
    Vector2 rectVec2;

    // 드래그
    public void OnDrag(PointerEventData eventData)
    {
        SelectLine();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BtnClick();
        cardGroup.DeSelect();
    }
    // 클릭
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.GetClear())
            return;
        selectCardNum = cardGroup.SelectCard(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.GetClear())
            return;
        cardGroup.DeSelect();
    }
    public void Init()
    {
        cardGroup = GetComponentInParent<CardGroup>();
        rectCom = GetComponent<RectTransform>();
        rectVec2 = new Vector2(rectCom.anchoredPosition.x,rectCom.anchoredPosition.y);
    }
    public void Setting(CardInfo cardInfo)
    {
        cardinfo = cardInfo;
        switch (cardinfo.id)
        {
            case "0":
                cardName = "Catcher";
                break;
            case "1":
                cardName = "Fishguard";
                break;
            case "2":
                cardName = "Monkeydong";
                break;
        }
        charImg = gameObject.transform.GetChild(0).GetComponent<Image>();
        Sprite imga = Resources.Load<Sprite>("Image/Charactor/" + cardName);
        charImg.sprite = imga;
    }
    public CardInfo GetCardInfo()
    {
        return cardinfo;
    }
    public void ResetCardInfo()
    {
        cardinfo = null;
    }
    void BtnClick()
    {
        GameMap map = GameManager.Instance.GetGameMap();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("TEAMLOAD");
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
        if (UserData.gauge - selectCardNum >= 0)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                string monName = "";
                switch (selectCardNum)
                {
                    case 1:
                        monName = cardName + "_Small";
                        break;
                    case 2:
                        monName = cardName + "_Medium";
                        break;
                    case 3:
                        monName = cardName + "_Big";
                        break;
                }
                // test
                monName = "Unit_Test";

                GameManager.Instance.CreateHero(hit.transform.gameObject.tag, monName, UserData.team, selectCardNum);
                cardGroup.UseCard();
            }
        }
        if(hit.transform != null)
            map.ResetColor(hit.transform.gameObject.tag);
        selectCardNum = 0;
    }
    void SelectLine()
    {
        GameMap map = GameManager.Instance.GetGameMap();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("TEAMLOAD");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            map.HitLine(hit.transform.tag);
        }
        else
            map.HitLine("NON");
    }
    public void UpPosition()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectVec2.x,rectVec2.y+60);
    }
    public void DownPosition()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = rectVec2;
    }
}
