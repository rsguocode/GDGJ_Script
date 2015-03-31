﻿﻿﻿using UnityEngine;
using System.Collections;
using System.IO;
namespace com.net.interfaces
{
	public interface INetData
	{
		string GetCMD();
		MemoryStream GetMemoryStream();
	}
	
}
