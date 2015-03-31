using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using com.game.vo;

namespace Com.Game.Module.Medal
{
    public class MedalMode : BaseMode<MedalMode>
    {
        public readonly int UPDATE_MEDAL_INFO = 1;
        public readonly int UPDATE_MEDAL_UP = 2;

        /// <summary>
        /// 当前激活勋章Id
        /// </summary>
        public int UpgradeId;
        public const int MaxMedalLvel = 60;

        public bool IsMaxLevel()
        {
            return UpgradeId >= MaxMedalLvel;
        }
        /// <summary>
        /// 是否可升级
        /// </summary>
        /// <param name="index">从1开始</param>
        /// <returns></returns>
        public bool CanMedalUp(int index)
        {
            if (IsMaxLevel())  //已经达到最高级
            {
                return false;
            }
            SysMedalVo vo = BaseDataMgr.instance.GetDataById<SysMedalVo>((uint)index);
            return vo.repu <= MeVo.instance.repu && vo.gold <= MeVo.instance.diam;
            
        }

        public void UpdateMdealInfo(int currenId)
        {
            this.UpgradeId = currenId;
            DataUpdate(this.UPDATE_MEDAL_INFO);
        }

        public void UpdateMedalUp(int currentId)
        {
            UpdateMdealInfo(currentId);
            DataUpdate(this.UPDATE_MEDAL_UP);
        }


    }

}

