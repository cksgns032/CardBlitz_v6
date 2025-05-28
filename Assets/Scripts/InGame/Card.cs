using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IDragHandler
{
    // ī�� ������ �޾� ����
    // �ڽ�Ʈ, Ÿ��, ���ݷ�, ��, �̸�
    string cardName;
    int selectCardNum;

    UserGameData userData;
    CardInfo cardinfo;
    Image charImg;
    CardGroup cardGroup;
    RectTransform rectCom;
    Vector2 rectVec2;
    Animation ani;
    Coroutine cardCoroutine;
    bool isUse;
    // �巡��
    public void OnDrag(PointerEventData eventData)
    {
        SelectLine();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BtnClick();
        cardGroup.DeSelect();
    }
    // Ŭ��
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
        userData = GameManager.Instance.GetMyGameData();
        cardGroup = GetComponentInParent<CardGroup>();
        rectCom = GetComponent<RectTransform>();
        rectVec2 = new Vector2(rectCom.anchoredPosition.x, rectCom.anchoredPosition.y);
        ani = GetComponent<Animation>();
        isUse = false;
        gameObject.SetActive(false);
    }
    public bool GetIsUse()
    {
        return isUse;
    }
    public void Setting(CardInfo cardInfo, bool active)
    {
        cardinfo = cardInfo;
        switch (cardinfo.id)
        {
            case "0":
                cardName = "Thumbnail_Unit_Archer";
                break;
            case "1":
                cardName = "Thumbnail_Unit_Buffer";
                break;
            case "2":
                cardName = "Thumbnail_Unit_Knight_OneHandLongSword";
                break;
        }
        charImg = gameObject.transform.GetChild(0).GetComponent<Image>();
        Sprite imga = Resources.Load<Sprite>("Texture/Charactor/" + cardName);
        charImg.sprite = imga;
        gameObject.SetActive(active);
    }
    async public void PlayAni(int delayTime)
    {
        await Task.Delay(delayTime);
        if (gameObject == null || ani == null)
        {
            return;
        }
        gameObject.SetActive(true);
        ani.Play(ani.clip.name);
        GameUI gameUI = GameManager.Instance.GetGameUI();
        if (gameUI != null)
        {
            Button shuffle = gameUI.GetShuffle();
            if (shuffle != null)
            {
                if (cardCoroutine != null)
                {
                    StopCoroutine(cardCoroutine);
                    cardCoroutine = null;
                }
                cardCoroutine = StartCoroutine(MoveAlongParabola(shuffle.transform.position, transform.parent.position, 5, 0.5f));
            }
        }
    }
    IEnumerator MoveAlongParabola(Vector3 start, Vector3 end, float height, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;  // 0~1 보간 값

            // XZ 평면에서 직선 보간
            Vector3 linear = Vector3.Lerp(start, end, t);

            // 포물선 높이 계산 (포물선 공식)
            float arc = height * Mathf.Sin(Mathf.PI * t);

            // 새로운 위치 적용
            transform.position = new Vector3(linear.x, linear.y + arc, linear.z);

            yield return null;
        }

        // 이동 완료 후 정확한 목표 위치 설정
        transform.position = end;
        isUse = true;
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
        if (userData.gauge - selectCardNum >= 0)
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
                // todo : 서버 연결 후 수정
                GameManager.Instance.CreateHero(hit.transform.gameObject.tag, monName, userData.team, selectCardNum);
                cardGroup.UseCard();
            }
        }
        if (hit.transform != null)
        {
            map.ResetColor(hit.transform.gameObject.tag);
        }

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
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectVec2.x, rectVec2.y + 60);
    }
    public void DownPosition()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = rectVec2;
    }
    public void SetUse(bool Use)
    {
        isUse = Use;
    }
}
