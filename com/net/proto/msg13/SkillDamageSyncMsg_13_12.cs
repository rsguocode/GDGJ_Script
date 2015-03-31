/**
 * 技能s伤害同步,客户端发起(暂时) (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SkillDamageSyncMsg_13_12
  	{

    public List<PDamage> damage = new List<PDamage>();

    public static int getCode()
    {
        // (13, 12)
        return 3340;
    }

    public void read(MemoryStream msdata)
    {
        PDamage.readLoop(msdata, damage);
    }
   }
}