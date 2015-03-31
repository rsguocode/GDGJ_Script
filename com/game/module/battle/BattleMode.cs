using com.game.module.test;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/24 02:41:53 
 * function: 战斗控制数据类
 * *******************************************************/
using com.game.vo;
using UnityEngine;

namespace com.game.module.battle
{
    public class BattleMode : BaseMode<BattleMode>
    {
        public const int UpdataAutoSystemStatu = 1; //挂机状态改变事件
        public const int UpdateTopRightHpBar = 2; //更新中间的血条
        public const int UpdateStageInfo = 3; //更新阶段信息
        public const int UpdateMonsterDeath = 4;//发送怪物死亡信息
        public float CurrentHpRate; //当前的血量
        public string HpString; //更新后的血量信息
        public string MonsterName; //怪物名字
        public int MonsterLvl; //怪物等级
        public int MonsterType;  //怪物类型;
        public int LeftCount;  //剩余血管数量
        public int MonsterIcon; // 怪物头像
        public float PreviousHpRate; //上一次的血量
        private bool _isAutoSystem; //是否挂机
        private string _stageInfo; //阶段信息
        public bool IsLeft; //向左按键是否按住
        public bool IsRight; //向右按键是否按住

        public bool IsAutoSystem
        {
            get { return _isAutoSystem; }
            set
            {
                _isAutoSystem = value;
                DataUpdate(UpdataAutoSystemStatu);
            }
        }

        public string StageInfo
        {
            get { return _stageInfo; }
            set
            {
                _stageInfo = value;
                DataUpdate(UpdateStageInfo);
            }
        }

        public void DoMonsterDeath()
        {
            Debug.Log("****更新怪物死亡数据 DataUpdate(UpdateMonsterDeath);");
            DataUpdate(UpdateMonsterDeath);
        }

        /// <summary>
        ///     调用这个方法会更新战斗UI中间的血条
        /// </summary>
        /// <param name="monsterName"></param>
        /// <param name="currentHp"></param>
        /// <param name="lvl"></param>
        /// <param name="monsterType"></param>
        /// <param name="monsterIcon"></param>
        /// <param name="maxCount"></param>
        /// <param name="maxHp"></param>
        /// <param name="previousHp"></param>
        public void SetCurrentHp(string monsterName, uint maxHp, uint previousHp, uint currentHp, int lvl, int monsterType,int monsterIcon, int maxCount = 1)
        {
            int perValue = Mathf.CeilToInt((float)maxHp / maxCount);
            int leftCount = (int)currentHp / perValue;
            var left = (int)(currentHp % perValue);
            var last = (int)(previousHp % perValue);
            if (left != 0)
            {
                leftCount = leftCount + 1;
            }
            string hpStr = currentHp + "/" + maxHp;
            float currentRate = (float)left / perValue;
            float lastRate = (float)last / perValue;
            if (currentHp == maxHp)
            {
                currentRate = 1;
            }
            if (previousHp==maxHp)
            {
                lastRate = 1;
            }
            MonsterName = monsterName;
            HpString = hpStr;
            PreviousHpRate = lastRate;
            CurrentHpRate = currentRate;
            LeftCount = leftCount;
            MonsterLvl = lvl;
            MonsterType = monsterType;
            MonsterIcon = monsterIcon;
            DataUpdate(UpdateTopRightHpBar);
        }
    }
}