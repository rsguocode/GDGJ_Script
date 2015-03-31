/**
 * 属性更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleAttrMsg_3_4
  	{

    public PBaseAttr attr = new PBaseAttr();

    public static int getCode()
    {
        // (3, 4)
        return 772;
    }

    public void read(MemoryStream msdata)
    {
        attr.read(msdata);
    }
   }
}