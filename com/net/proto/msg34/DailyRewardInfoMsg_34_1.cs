/**
 * 悬赏任务信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DailyRewardInfoMsg_34_1
  	{

    public List<PDailyRewardInfo> list = new List<PDailyRewardInfo>();

    public static int getCode()
    {
        // (34, 1)
        return 8705;
    }

    public void read(MemoryStream msdata)
    {
        PDailyRewardInfo.readLoop(msdata, list);
    }
   }
}