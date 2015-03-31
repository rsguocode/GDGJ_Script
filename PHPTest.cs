//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：PHPTest
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Module.PHPTest
{
	public class PHPTest : MonoBehaviour
	{		
		void OnGUI()
		{
			if (GUI.Button(new Rect(0, 0, 100, 50), "GetPHPPage"))
			{
				StartCoroutine(GetText());
			}
		}

		IEnumerator GetText()
		{
			//WWW myWWW = new WWW("http://localhost/helloWorld.php");
			WWW myWWW = new WWW("http://172.16.10.140/xml/updated_board.xml");
			yield return myWWW;
			print (myWWW.text);
		}
		
	}
}
