using System;

namespace com.game.data
{ 
   /**系统功能设置表**/ 
   [Serializable]
   public class SysFunSetVo
   {
       public int mapType;                        //场景类型
       public string funs;                        //可操作功能列表

       public SysFunSetVo() { }
   } 
}