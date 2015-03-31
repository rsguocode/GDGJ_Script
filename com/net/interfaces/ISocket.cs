﻿using System;
using System.IO;
namespace com.net.interfaces
{
	public interface ISocket
	{
	    string ip { get; set; }

	    int port  { get; set; }
		void connect(string ip, int port);
	    void retryConnect();
		//void send(IP8583Msg p8583Msg);
//		void send(string data);
		//void send(byte[] data);
		void send(MemoryStream msdata, byte framNum, byte wayNum);
		//void send(byte framNum, byte wayNum);
		void setTryableNum(int num);
		int getTryableNum();
		void statusListener(NetStatusCallback listener);
		void msgListenner(NetMsgCallback callback);
		bool connected();
		void close();
        void Update();
	}
}