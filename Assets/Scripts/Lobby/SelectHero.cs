using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHero : MonoBehaviour
{
    Button upgrade;
    Text priceTxt;
    Text levelTxt;
    Text heartTxt;
    Text defenceTxt;
    Text attackTxt;
    Text rangeTxt;

    public void Init()
    {
        levelTxt = gameObject.transform.Find("LevelText").GetComponent<Text>();
        levelTxt.text = "100";
        heartTxt = gameObject.transform.Find("HeartText").GetComponent<Text>();
        heartTxt.text = "100";
        defenceTxt = gameObject.transform.Find("DefenceText").GetComponent<Text>();
        defenceTxt.text = "100";
        attackTxt = gameObject.transform.Find("AttackText").GetComponent<Text>();
        attackTxt.text = "100";
        rangeTxt = gameObject.transform.Find("RangeText").GetComponent<Text>();
        rangeTxt.text = "100";
        upgrade = gameObject.transform.Find("UpgradeBtn").GetComponent<Button>();
        upgrade.onClick.AddListener(Upgrade);
        priceTxt = upgrade.gameObject.transform.Find("Price").GetComponent<Text>();
        priceTxt.text = "100";
    }
    public void Upgrade()
    {

    }
}
