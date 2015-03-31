﻿﻿﻿using com.game.cmd;
using com.net.interfaces;
using com.net;

/**登陆模拟包
 * 1、[模拟类的类命名规则为 Test+要模拟的协议号]，比如 登陆模拟包类名为：Test100000
 * **/
namespace com.test
{
    public class Test100000
    {
        public static IP8583Msg tel()
	   {
            IP8583Msg p8583Msg = NetFactory.newP8583Msg();
            //p8583Msg.setField(1, CMD.CMD_400000);      
		    p8583Msg.setField(10,"8");     
		    p8583Msg.setField(60,"哈哈，模拟登陆！");
            return p8583Msg;
	   }
    }
}
