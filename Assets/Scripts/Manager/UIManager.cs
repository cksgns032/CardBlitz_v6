using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    Dictionary<int, UIBase> popUpDictionary = new Dictionary<int, UIBase>();
    Dictionary<int, UIBase> UIDictionary = new Dictionary<int, UIBase>();
    SceneBaseManager sceneBase;
    public void Init()
    {
        sceneBase = GameObject.FindFirstObjectByType<SceneBaseManager>();
    }
    public void LoadPrefabs()
    {
        int PopupCnt = (int)PopUp_Name.Count;
        for (int i = 0; i < PopupCnt; i++)
        {
            if (popUpDictionary.ContainsKey(i) == false)
            {
                popUpDictionary.Add(i, Resources.Load<UIBase>($"Prefabs/PopUpPrefabs/{Enum.GetName(typeof(PopUp_Name), i)}"));
            }
        }
        int UICnt = (int)UI_Name.Count;
        for (int i = 0; i < UICnt; i++)
        {
            if (popUpDictionary.ContainsKey(i) == false)
            {
                UIDictionary.Add(i, Resources.Load<UIBase>($"Prefabs/UIPrefabs/{Enum.GetName(typeof(UI_Name), i)}"));
            }
        }
    }
    public UIBase GetUI(UI_Name key)
    {
        if (UIDictionary.TryGetValue((int)key, out UIBase obj))
        {
            Transform findObject = CheckNew(Enum.GetName(typeof(UI_Name), key));
            if (findObject == null)
            {
                GameObject popUp = sceneBase.GetUILayer();
                UIBase clone = Instantiate<UIBase>(obj, popUp.GetComponent<Transform>());
                clone.name = Enum.GetName(typeof(UI_Name), key);
                clone.Init(Layer_Type.UI, clone.name);
                return clone;
            }
            else
            {
                UIBase baseObject = findObject.GetComponent<UIBase>();
                if (baseObject != null)
                {
                    return baseObject;
                }
            }
        }
        return null;
    }
    public UIBase GetPopUp(PopUp_Name key)
    {
        if (popUpDictionary.TryGetValue((int)key, out UIBase obj))
        {
            Transform findObject = CheckNew(Enum.GetName(typeof(PopUp_Name), key));
            if (findObject == null)
            {
                GameObject popUp = sceneBase.GetPopUpLayer();
                UIBase clone = Instantiate<UIBase>(obj, popUp.GetComponent<Transform>());
                clone.name = Enum.GetName(typeof(PopUp_Name), key);
                clone.Init(Layer_Type.Popup, clone.name);
                return clone;
            }
            else
            {
                UIBase baseObject = findObject.GetComponent<UIBase>();
                if (baseObject != null)
                {
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
        if (valueBase)
        {
            valueBase.Close();
        }
    }
    public void AllClose()
    {
        foreach (var key in popUpDictionary)
        {
            if (key.Value.GetState() == Active_State.Open)
            {
                key.Value.Close();
            }
        }
    }
}
