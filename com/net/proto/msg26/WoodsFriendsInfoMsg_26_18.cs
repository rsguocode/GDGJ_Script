/**
 * 好友剩余协助次数列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsFriendsInfoMsg_26_18
  	{

    public List<PWoodsFriendInfo> friendList = new List<PWoodsFriendInfo>();

    public static int getCode()
    {
        // (26, 18)
        return 6674;
    }

    public void read(MemoryStream msdata)
    {
        PWoodsFriendInfo.readLoop(msdata, friendList);
    }
   }
}