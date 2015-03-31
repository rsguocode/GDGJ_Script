using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ai : MonoBehaviour {
    protected AiData aidata;
    protected List<AiData> _aiDataCtr;
    protected Dictionary<int, bool> dicAiData;

    public Ai(List<AiData>AiDataCtr)
    {
        _aiDataCtr = AiDataCtr;
        dicAiData = new Dictionary<int, bool>();
    }

    bool isAiDone(int id)
    {
        if (dicAiData.ContainsKey(id))
            return dicAiData[id];
        return false;
    }

    void aiDone(int id)
    {
        dicAiData[id] = true;
    }

    public void checkHp(int curHp, int maxHp)
    {
        foreach (AiData aid in _aiDataCtr)
        {
            switch (aid.conditionType)
            {
                case 100:
                    {
                        int rate1 = aid.conditionParamList[0];
                        int rate2 = aid.conditionParamList[1];
                        int now = curHp;
                        if (now >= rate1 && now <= rate2)
                        {
                            aiDone(aid.ID);
                            doAction(aid.actionType, aid.actionParamList);
                        }
                        break;
                    }
                case 101:
                    {
                        int setHp = aid.conditionParamList[0];
                        if (curHp < setHp)
                        {
                            aiDone(aid.ID);
                            doAction(aid.actionType, aid.actionParamList);
                        }
                        break;
                    }
            }
        }
    }	

    void doAction(int actionType, List<int> lsActionParam)
    {
        switch(actionType)
        {
            case 200:
                break;
        }
    }
}
