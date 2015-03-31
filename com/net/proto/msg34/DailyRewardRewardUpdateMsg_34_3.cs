/**
 * 增量更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DailyRewardRewardUpdateMsg_34_3
  	{

    public PDailyRewardInfo info = new PDailyRewardInfo();

    public static int getCode()
    {
        // (34, 3)
        return 8707;
    }

    public void read(MemoryStream msdata)
    {
        info.read(msdata);
    }
   }
}