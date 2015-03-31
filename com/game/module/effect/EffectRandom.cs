//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：EffectRandom
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace com.game.module.effect
{
	public class EffectRandom : MonoBehaviour
	{		
		public float ZoomMin = 1f;
		public float ZoomMax = 1f;
		public float RotateZMin = 0f;
		public float RotateZMax = 0f;
		public bool RandomPos = false;
		public float MinX = -1f;
		public float MinY = -1f;
		public float MaxX = 1f;
		public float MaxY = 1f;
		private float z;

		public void Start()
		{
			float realZoom = Random.Range(ZoomMin, ZoomMax);
			float realRotateZ = Random.Range(RotateZMin, RotateZMax);
			Vector3 localScale = new Vector3(realZoom, realZoom, realZoom);
			Vector3 eulerAngles = new Vector3(0f, 0f, realRotateZ);
			Quaternion localRotation = Quaternion.identity;
			localRotation.eulerAngles = eulerAngles;

			transform.localScale = localScale;
			transform.localRotation = localRotation;

			if (RandomPos)
			{
				z = transform.position.z;
				float localZ = transform.localPosition.z;
				float randomX = Random.Range(MinX, MaxX); 
				float randomY = Random.Range(MinY, MaxY); 
				Vector3 randLoacalPos = new Vector3(randomX, randomY, localZ);
				transform.localPosition = randLoacalPos;
			}
		}

		void OnDrawGizmosSelected()
		{
			if (RandomPos)
			{
				Gizmos.color = Color.green;   
				Gizmos.DrawLine(new Vector3(MinX, MaxY, z), new Vector3(MaxX, MaxY, z));
				Gizmos.DrawLine(new Vector3(MaxX, MaxY, z), new Vector3(MaxX, MinY, z));
				Gizmos.DrawLine(new Vector3(MaxX, MinY, z), new Vector3(MinX, MinY, z));
				Gizmos.DrawLine(new Vector3(MinX, MinY, z), new Vector3(MinX, MaxY, z));
			}
		}
		
	}
}
