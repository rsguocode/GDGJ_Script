using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game.manager;
using com.game.module.map;

public class AiDataParser
{
    static AiDataParser It;
    
    public static AiDataParser Me()
    {
        if (It == null)
            It = new AiDataParser();
        return It;
    }

    public List<AiData> Parser(string aiList)
    {
      // 将要做
        AiData _aiData = new AiData();
        List<AiData> listAiDataCtr = new List<AiData>();

        SysMonsterAiVo monsterAiVo = BaseDataMgr.instance.GetDataById<SysMonsterAiVo>(uint.Parse(aiList));
        string strAiValue = monsterAiVo.AiValue; // 122=720000,100|218=1;104=5|200=930035

        _aiData.ID = int.Parse(aiList);
        string[] strEachState = strAiValue.Split(';');
        for (int i = 0; i < strEachState.Length; i++)
        {
            string[] strConditionState = strEachState[i].Split('|');

            // 122=720000,100
            string[] strConditionValue = strConditionState[0].Split('=');
            _aiData.conditionType = int.Parse(strConditionValue[0]);

            string[] value = strConditionValue[1].Split(',');
            for (int k = 0; k < value.Length; k++)
            {
                _aiData.conditionParamList.Add(int.Parse(value[k]));
            }

            // 218=1 
            string[] strActionValue = strConditionState[1].Split('=');
            _aiData.actionType = int.Parse(strActionValue[0]);
            value = strActionValue[1].Split(',');
            for (int k = 0; k < value.Length; k++)
                _aiData.actionParamList.Add(int.Parse(value[k]));

            listAiDataCtr.Add(_aiData); // 一个条件就一个 _aiData;           
        }

        return listAiDataCtr;        
    }
}
