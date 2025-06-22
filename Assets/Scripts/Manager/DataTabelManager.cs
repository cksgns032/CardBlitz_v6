using System;
using System.Collections.Generic;
using UnityEngine;

public class DataTabelManager : SingleTon<DataTabelManager>
{
    Dictionary<int, UnitData> UnitDataTable = new Dictionary<int, UnitData>();
    public void LoadDataTable()
    {
        string[] names = Enum.GetNames(typeof(DataTable));
        for (int i = 1; i < names.Length; i++)
        {
            TextAsset tableData = Resources.Load<TextAsset>($"DataTable/{names[i]}");
            Debug.Log(tableData);
            string[][] data = ReadText(tableData);
            switch (names[i])
            {
                case "UnitData":
                    {
                        for (int j = 0; j < data.Length; j++)
                        {
                            UnitData unit = new UnitData();
                            unit.Setting(data[j]);
                            int keyNum = int.Parse(data[j][0]);
                            UnitDataTable.Add(keyNum, unit);
                        }
                    }
                    break;
            }
        }
    }
    private string[][] ReadText(TextAsset textAsset)
    {
        string[] lines = textAsset.text.Split('\n');
        string[][] dataList = new string[lines.Length - 1][];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] types = lines[i].Split(",");
            for (int j = 0; j < types.Length; j++)
            {
                if (types[j].Contains('\r'))
                {
                    types[j] = types[j].Substring(0, types[j].IndexOf("\r"));
                }
            }
            dataList[i - 1] = types;
        }
        return dataList;
    }
    public string GetUnitName(ushort id)
    {
        if (UnitDataTable.ContainsKey(id))
        {
            return UnitDataTable[id].name;
        }
        else
        {
            return null;
        }
    }
}