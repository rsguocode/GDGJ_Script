﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.game.vo
{
	public class EffectVo
	{
        public string id;
        public string name;
        public string url;    
           
        public float waitTime;     //播放时间

        public bool isChangeDir;
        public Vector3 eulerAngles = Vector3.zero;  //旋转角度（人物朝向）

        public Vector3 position;    //坐标
        public Vector3 posOffset = Vector3.zero;   //位置偏移量（资源共用的时候可能用到的字段）
        public int direction;
        public float speed;

        public GameObject target;   //跟随目标               
	}
}
