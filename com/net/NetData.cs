using UnityEngine;
using System.Collections;
using System;
using System.IO;
using com.game;
using Proto;
using com.net.interfaces;
namespace com.net
{
	public class NetData : INetData{
		private byte[] bodyBuffer;

		public NetData(byte[] data)
		{
			bodyBuffer = data;
		}
		public string GetCMD()
		{
//			Debug.LogWarning("debug receive byte[]-----------------------");
//			for (int i = 0; i < bodyBuffer.Length; ++i)
//			{
//				Debug.Log(bodyBuffer[i]);
//			}
//			Debug.LogWarning("debug receive byte[] end-----------------------");

//			Debug.Log("Start Decode receive data");
			byte[] receiveFramNum = new byte[1];
			Array.Copy (bodyBuffer, 0, receiveFramNum, 0, receiveFramNum.Length);

			byte[] receiveWayNum = new byte[1];
			Array.Copy (bodyBuffer, receiveFramNum.Length, receiveWayNum, 0, receiveWayNum.Length);

			int key = 0;
//			Debug.Log ("receiveFramNum: " + receiveFramNum [0]);
//			Debug.Log ("receiveWayNum: " + receiveWayNum [0]);
			key += (int)receiveFramNum[0] * 256  + receiveWayNum[0];
//			Debug.Log("Key: " + key);
			return key.ToString ();
		}

		public MemoryStream GetMemoryStream()
		{
			byte[] receiveData = new byte[bodyBuffer.Length - 2];
			Array.Copy (bodyBuffer, 2, receiveData, 0, receiveData.Length);
			MemoryStream msd = new MemoryStream();
			msd.Write(receiveData, 0, receiveData.Length);
			msd.Position = 0;
			return msd;
		}



		public static byte[] Encode(MemoryStream msdata, byte framNum, byte wayNum)
		{
			byte[] head = new byte[2];
			byte[] fram = new byte[1];
			byte[] uniq = new byte[2];
			fram[0] = framNum;
			byte[] way = new byte[1];
			way[0] = wayNum;

			byte[] data = msdata.ToArray ();
			head = ByteUtil.Number2Bytes ((4 + data.Length), 2);

			// gen unique id
			int unique = 0;
			if (AppNet.uniq > 65535) AppNet.uniq = 99;
			unique = AppNet.uniq + 1;
			AppNet.uniq = unique;
			uniq = ByteUtil.Number2Bytes (unique, 2);
			
			//contact byte[]
			byte[] sendBytes = new byte[head.Length + fram.Length + way.Length + data.Length + uniq.Length];
			head.CopyTo (sendBytes, 0);
			uniq.CopyTo (sendBytes, head.Length);
			fram.CopyTo (sendBytes, head.Length + uniq.Length);
			way.CopyTo (sendBytes, head.Length + uniq.Length + fram.Length);
			data.CopyTo (sendBytes, head.Length + uniq.Length + fram.Length + way.Length);
			
//			Debug.LogWarning("debug send byte[]-----------------------");
//			for (int i = 0; i < sendBytes.Length; ++i)
//			{
//				Debug.Log(sendBytes[i]);
//			}
//			Debug.LogWarning("debug send byte[] end-----------------------");

			return sendBytes;
			//this.startReceive ();
		}

//		public static void SendNetData(MemoryStream msdata, byte framNum, byte wayNum)
//		{
//			AppNet.gameNet.send (Encode(msdata, framNum, wayNum));
//		}

	}
}