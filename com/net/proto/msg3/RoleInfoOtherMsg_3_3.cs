/**
 * 其他人信息（角色属性面板） (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleInfoOtherMsg_3_3
  	{

    public ushort code = 0;
    public List<PRoleAttr> role = new List<PRoleAttr>();
    public List<PGoods> goodsList = new List<PGoods>();

    public static int getCode()
    {
        // (3, 3)
        return 771;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PRoleAttr.readLoop(msdata, role);
        PGoods.readLoop(msdata, goodsList);
    }
   }
}