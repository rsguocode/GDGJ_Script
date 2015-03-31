using System.Collections.Generic;
using com.game.data;
using com.game.manager;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/22 11:21:53 
 * function: 指引管理类
 * *******************************************************/

namespace com.game.module.Guide.GuideLogic
{
    public class GuideManager
    {
        public static GuideManager Instance = new GuideManager();
        public SysGuideVo CurrentTriggeredGuideVo;
        private Dictionary<int, GuideBase> _guideDictionary;
        public int CurrentGuideType;

        public GuideManager()
        {
            RegisterGuide();
        }

        /// <summary>
        ///     注册指引实现逻辑，key：指引类型 value：指引实现逻辑
        /// </summary>
        public void RegisterGuide()
        {
            _guideDictionary = new Dictionary<int, GuideBase>();
            //_guideDictionary.Add(GuideType.GuideRoleOpen, new GuideRoleOpen());
            _guideDictionary.Add(GuideType.GuideEquip,new GuideEquip());
            _guideDictionary.Add(GuideType.GuideGrow,new GuideGrow());
            //_guideDictionary.Add(GuideType.GuideMedal, new GuideMedal());  //勋章功能修改，暂时注释掉
            _guideDictionary.Add(GuideType.GuideForgeOpen, new GuideForgeOpen());
            _guideDictionary.Add(GuideType.GuideSkillOpen,new GuideSkillOpen());
            _guideDictionary.Add(GuideType.GuideSkill3Learn, new GuideSkill3Learn());
            _guideDictionary.Add(GuideType.GuideSkill4Learn, new GuideSkill4Learn());
            _guideDictionary.Add(GuideType.GuideSkillRollLearn, new GuideSkillRollLearn());
            _guideDictionary.Add(GuideType.GuideEquipRefine,new GuideEquipRefine());
            _guideDictionary.Add(GuideType.GuideEquipInlay,new GuideEquipInlay());
            _guideDictionary.Add(GuideType.GuideEquipMerge,new GuideEquipMerge());
            _guideDictionary.Add(GuideType.GuidePetOpen, new GuidePetOpen());
            _guideDictionary.Add(GuideType.GuidePetEquip, new GuidePetEquip());
            _guideDictionary.Add(GuideType.GuidePetSkill,new GuidePetSkill());
            //_guideDictionary.Add(GuideType.GuideGuildOpen, new GuideGuildOpen()); 公会暂时不上
            _guideDictionary.Add(GuideType.GuideGoldSilverIslandOpen, new GuideGoldSilverIslandOpen());
            _guideDictionary.Add(GuideType.GuideGoldHitOpen, new GuideGoldHitOpen());
            _guideDictionary.Add(GuideType.GuideArenaOpen, new GuideArenaOpen());
            _guideDictionary.Add(GuideType.GuideDaemonIslandOpen, new GuideDaemonIslandOpen());

            _guideDictionary.Add(GuideType.GuideShopOpen, new GuideStoreShopOpen());
            _guideDictionary.Add(GuideType.GuideLuckDraw, new GuideLuckDrawOpen());
            _guideDictionary.Add(GuideType.GuideGoldBoxOpen, new GuideGoldBox());
            _guideDictionary.Add(GuideType.GuideWorldBoss, new GuideWorldBossOpen());
            _guideDictionary.Add(GuideType.GuideCopy, new GuideCopy());
        }

        /// <summary>
        ///     根据指引类型获取指引的执行逻辑类
        /// </summary>
        /// <param name="guideType"></param>
        /// <returns></returns>
        public GuideBase GetGuideLogic(int guideType)
        {
            if (_guideDictionary.ContainsKey(guideType))
            {
                return _guideDictionary[guideType];
            }
            return null;
        }

        /// <summary>
        ///     触发指引
        /// </summary>
        public void TriggerGuide(uint taskId, int triggerType)
        {
            List<SysGuideVo> listGuideVoList = BaseDataMgr.instance.GetGuideVoList();
            CurrentTriggeredGuideVo = null;
            foreach (SysGuideVo guideVo in listGuideVoList)
            {
                if (guideVo.trigger_type == triggerType && guideVo.condition.Contains(taskId + ""))
                {
                    CurrentTriggeredGuideVo = guideVo;
                }
            }
            if (CurrentTriggeredGuideVo != null)
            {
                GuideBase guideLogic = GetGuideLogic(CurrentTriggeredGuideVo.guideID);
                if (guideLogic != null)
                {
                    guideLogic.BeginGuide();
                    CurrentGuideType = CurrentTriggeredGuideVo.guideID;
                }
            }
        }
    }
}