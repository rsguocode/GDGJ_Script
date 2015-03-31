using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiData {
    	/** ai编号 */
	public int ID;
	/** 条件类型 */
	public int conditionType;
	/** 条件参数 */
	public List<int> conditionParamList;
	/** 行为类型 */
	public int actionType;
	/** 行为参数 */
    public List<int> actionParamList;
	
    public AiData()
    {
        conditionParamList = new List<int>();
        actionParamList = new List<int>();
    }
}
