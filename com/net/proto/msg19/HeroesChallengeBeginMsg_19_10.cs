/**
 * 开始挑战，加锁 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class HeroesChallengeBeginMsg_19_10
  	{

    public ushort code = 0;
    public List<PBaseAttr> role = new List<PBaseAttr>();
    public List<uint> skills = new List<uint>();

    public static int getCode()
    {
        // (19, 10)
        return 4874;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PBaseAttr.readLoop(msdata, role);
        proto_util.readLoopUInt(msdata, skills);
    }
   }
}