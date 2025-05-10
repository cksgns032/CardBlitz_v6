using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    List<GameObject> TopList = new List<GameObject>();
    List<GameObject> MiddleList = new List<GameObject>();
    List<GameObject> BottomList = new List<GameObject>();

    public Player testMon;

    string currentTag = string.Empty;
    Color currentColor;
    public void Init()
    {
        for (int k = 0; k < GetComponentsInChildren<EventButton>().Length; k++)
        {
            gameObject.GetComponentsInChildren<EventButton>()[k].Init();
        }
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.RemoveData();
        surface.BuildNavMesh();
        int layerNum = LayerMask.NameToLayer("TEAMLOAD");

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.layer == layerNum)
            {
                switch (transform.GetChild(i).gameObject.tag)
                {
                    case "TOP":
                        TopList.Add(transform.GetChild(i).gameObject);
                        break;
                    case "MIDDLE":
                        MiddleList.Add(transform.GetChild(i).gameObject);
                        break;
                    case "BOTTOM":
                        BottomList.Add(transform.GetChild(i).gameObject);
                        break;
                }
            }
        }
        if (testMon)
        {
            testMon.TestInit();
        }
    }
    // layer = teamload
    //  tag  = objtag
    // ���� ����
    public void HitLine(string objTag)
    {
        switch (objTag)
        {
            case "TOP":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                    currentTag = string.Empty;
                }
                for (int i = 0; i < TopList.Count; i++)
                {
                    MeshRenderer mesh = TopList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0, 0, 0, 0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "MIDDLE":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                for (int i = 0; i < MiddleList.Count; i++)
                {
                    MeshRenderer mesh = MiddleList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0, 0, 0, 0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "BOTTOM":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                for (int i = 0; i < BottomList.Count; i++)
                {
                    MeshRenderer mesh = BottomList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0, 0, 0, 0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "NON":
                if (currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                break;
        }
        //TCPClient.Instance.SendPack(GameProtocolType.CREATEOBJ, objTag, "DarkNight", UserData.team);
    }
    // �ٴ� �� ��ȭ
    public void ResetColor(string objtag)
    {
        switch (objtag)
        {
            case "TOP":
                for (int i = 0; i < TopList.Count; i++)
                {
                    MeshRenderer mesh = TopList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            case "MIDDLE":
                for (int i = 0; i < MiddleList.Count; i++)
                {
                    MeshRenderer mesh = MiddleList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            case "BOTTOM":
                for (int i = 0; i < BottomList.Count; i++)
                {
                    MeshRenderer mesh = BottomList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            default:
                break;
        }
    }
}
