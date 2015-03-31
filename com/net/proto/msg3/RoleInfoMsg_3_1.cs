/**
 * 角色信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleInfoMsg_3_1
  	{

    public PRoleAttr role = new PRoleAttr();

    public static int getCode()
    {
        // (3, 1)
        return 769;
    }

    public void read(MemoryStream msdata)
    {
        role.read(msdata);
    }
   }
}