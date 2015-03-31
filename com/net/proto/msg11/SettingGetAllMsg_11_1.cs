/**
 * 获取设置 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SettingGetAllMsg_11_1
  	{

    public List<uint> key = new List<uint>();
    public List<uint> val = new List<uint>();

    public static int getCode()
    {
        // (11, 1)
        return 2817;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, key);
        proto_util.readLoopUInt(msdata, val);
    }
   }
}