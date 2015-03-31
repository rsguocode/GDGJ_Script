using UnityEngine;
using System.Collections;

namespace Proto
{
	public class ByteUtil : MonoBehaviour {

		public static int Byte2Int(byte[] data)
		{
			int result = 0;;
			for (int i = 0; i < data.Length - 1; ++i)
			{
				result += (int) data[i] * 256 * (data.Length - i - 1);
			}
			result += (int) data[data.Length - 1];
			return result;
		}
		
		public static byte[] Number2Bytes(int src, int len)
		{
			byte[] array = new byte[len];
			short num = 0;
			while ((int)num < len)
			{
				int num2 = src % 256;
				array[(int)len - num - 1] = (byte)(num2 & 255);
				//array[num] = (byte)(num2 & 255);
				src /= 256;
				num += 1;
			}
			return array;
		}
	}
}
