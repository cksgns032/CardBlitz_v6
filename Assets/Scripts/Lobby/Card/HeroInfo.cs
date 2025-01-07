using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    Button exitBtn;
    Button levelBtn;
    Button gradeBtn;

    Image heroImg;

    Text nameTxt;
    Text levelTxt;
    Text levBtnTxt;
    Text grdBtnTxt;

    GameObject starGroup;

    // Start is called before the first frame update
    public void Init()
    {
        levelBtn = gameObject.transform.Find("LevelBtn").GetComponent<Button>();
        levelBtn.onClick.AddListener(ClickLevel);
        gradeBtn = gameObject.transform.Find("GradeBtn").GetComponent<Button>();
        gradeBtn.onClick.AddListener(ClickGrade);
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ClickExit);
    }
    public void Setting()
    {

    }
    public void ClickLevel()
    {

    }
    public void ClickGrade()
    {

    }
    public void ClickExit()
    {
        gameObject.SetActive(false);
    }
}
