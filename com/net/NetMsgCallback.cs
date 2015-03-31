﻿﻿using com.net.interfaces;
using System;
namespace com.net
{
	//public delegate void NetMsgCallback(byte[] receiveData);
	public delegate void NetMsgCallback(INetData receiveData);
}
