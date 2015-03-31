﻿﻿﻿using System;

/**路径段Vo
 * 1、记录路径段信息
 * **/
namespace com.game.autoroad
{
    public class PathStepVo
    {
        public int stepType;          //阶段目标类型[TO_STEP_NPC,TO_STEP_COPY_POINT,TO_STEP_MAP_POINT,TO_STEP_WORLDMAP]
        public string targetId;       //阶段目标ID  [npcId，副本组ID，场景ID]

        internal string stepDesc() {
            if (stepType == 1)      return "到NPC前";
            else if (stepType == 2) return "到副本传送点前";
            else if (stepType == 3) return "到场景传送点前";
            else if (stepType == 4) return "在世界地图行走";
            else if (stepType == 5) return "到怪物前";
            else return string.Empty;
        }
    }
}
