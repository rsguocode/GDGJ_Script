﻿﻿using com.net.interfaces;
using com.net.p8583.vo;
using System;
namespace com.net
{
	public class NetFactory
	{
		public static IP8583Msg newP8583Msg()
		{
			return new P8583Msg();
		}
		public static ISocket newSocket()
		{
			return new NetSocket();
		}
	}
}
