﻿﻿﻿using com.game.cmd;
using com.net.interfaces;
using com.net;

/**注册模拟包**/
namespace com.test
{
    public class Test100001
    {
        public static IP8583Msg tel()
	   {
            IP8583Msg p8583Msg = NetFactory.newP8583Msg();
		   // p8583Msg.setField(1, CMD.CMD_400000);      
		    p8583Msg.setField(10,"8");     
		    p8583Msg.setField(60,"哈哈，模拟注册！");
            return p8583Msg;
	   }
    }
}
