using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingleTon<UIManager>
{
    Dictionary<int, UIBase> popUpDictionary = new Dictionary<int, UIBase>();
    SceneBaseManager sceneBase;
    public void Init()
    {
        sceneBase = GameObject.FindFirstObjectByType<SceneBaseManager>();
    }
    public void LoadPrefabs()
    {
        int totalCnt = (int)PopUp_Name.Count;
        for (int i = 0; i < totalCnt; i++)
        {
            if(popUpDictionary.ContainsKey(i) == false)
            {
                popUpDictionary.Add(i, Resources.Load<UIBase>($"Prefabs/PopUpPrefabs/{Enum.GetName(typeof(PopUp_Name), i)}"));
            }
        }
    }
    public void OpenPopUp(PopUp_Name key)
    {
        if (popUpDictionary.TryGetValue((int)key, out UIBase obj))
        {
            Transform findObject = CheckNew(Enum.GetName(typeof(PopUp_Name), key));
            if (findObject == null)
            {
                GameObject popUp = sceneBase.GetPopUpLayer();
                UIBase clone = Instantiate<UIBase>(obj,popUp.GetComponent<Transform>());
                clone.name = Enum.GetName(typeof(PopUp_Name), key);
                clone.Init(key);
            }
            findObject.GetComponent<UIBase>().Draw(true);
        }
    }
    public UIBase GetPopUp(PopUp_Name key, bool active)
    {
        if (popUpDictionary.TryGetValue((int)key, out UIBase obj))
        {
            Transform findObject = CheckNew(Enum.GetName(typeof(PopUp_Name), key));
            if(findObject == null)
            {
                GameObject popUp = sceneBase.GetPopUpLayer();
                UIBase clone = Instantiate<UIBase>(obj, popUp.GetComponent<Transform>());
                clone.name = Enum.GetName(typeof(PopUp_Name), key);
                clone.Init(key);
                clone.Draw(active);
                return clone;
            }
            else
            {
                UIBase baseObject = findObject.GetComponent<UIBase>();
                if (baseObject != null)
                {
                    baseObject.Draw(active);
                    return baseObject;
                }
            }
        }
        return null;
    }
    public Transform CheckNew(string keyName)
    {
        GameObject pop = sceneBase.GetPopUpLayer();
        return pop.transform.Find(keyName);
    }
    public void Close(PopUp_Name popName)
    {
        UIBase valueBase = null;
        popUpDictionary.TryGetValue((int)popName, out valueBase);
        if(valueBase)
        {
            valueBase.Close();
        }
    }
    public void AllClose()
    {
        foreach(var key in popUpDictionary)
        {
            if(key.Value.GetState() == PopUp_State.Open)
            {
                key.Value.Close();
            }
        }
    }
}
