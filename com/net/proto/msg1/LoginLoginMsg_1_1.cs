/**
 * 登陆 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class LoginLoginMsg_1_1
  	{

    public ushort code = 0;
    public List<PLoginInfo> loginInfo = new List<PLoginInfo>();

    public static int getCode()
    {
        // (1, 1)
        return 257;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PLoginInfo.readLoop(msdata, loginInfo);
    }
   }
}