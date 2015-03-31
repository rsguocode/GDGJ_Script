﻿﻿﻿using com.game.data;

/**系统功能--控制管理类**/
namespace com.game.manager
{
	public class SysFunMgr
	{
        public static SysFunMgr instance = new SysFunMgr();


        /**根据当前场景的场景类型+系统功能ID--返回其可操作状态
         * @param sysFunId 系统功能ID
         * @return [true:可操作,false:禁止操作]
         * **/
        public bool operable(string sysFunId)
        {
            uint mapId = AppMap.Instance.mapParser.MapId;
            int mapType = BaseDataMgr.instance.GetMapVo(mapId).type;
            return operable(mapType, sysFunId);
        }

        /**根据场景类型+系统功能ID--返回其可操作状态
         * @param mapType  场景类型
         * @param sysFunId 系统功能ID
         * @return [true:可操作,false:禁止操作]
         * **/
        public bool operable(int mapType,string sysFunId)
        {
            //SysFunSetVo sySetVo = BaseDataMgr.instance.getSysFunSetVo(mapType);
            //if (sySetVo == null) return true;
            //return sySetVo.funs.IndexOf(sysFunId) != -1 ? true : false;
            return true;
        }

	}
}
