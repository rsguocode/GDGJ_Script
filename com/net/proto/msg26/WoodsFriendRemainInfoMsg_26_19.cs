/**
 * 剩余协助次数（给好友广播） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsFriendRemainInfoMsg_26_19
  	{

    public PWoodsFriendInfo remainInfo = new PWoodsFriendInfo();

    public static int getCode()
    {
        // (26, 19)
        return 6675;
    }

    public void read(MemoryStream msdata)
    {
        remainInfo.read(msdata);
    }
   }
}