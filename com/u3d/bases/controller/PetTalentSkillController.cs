using System;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.fight.vo;
using com.game.Public.Hud;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/05/04 10:59:50 
 * function: 宠物天赋技能控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class TalentSkillType
    {
        public const int AddMasterHp = 30; //给主人加血
        public const int MakeMasterUnbeatable = 31; //使主人无敌
    }

    public class PetTalentSkillController : MonoBehaviour
    {
        public float LastAttackTime;
        public ActionControler MeController;
        private PlayerDisplay _masterDisplay;
        private PlayerVo _masterVo;
        private PetVo _petVo;
        private SysSkillBaseVo _talentSkillVo;
        private SysSkillActionVo _talentSkillActionVo;

        public void Start()
        {
            _petVo = MeController.GetMeVoByType<PetVo>();
            _talentSkillVo = BaseDataMgr.instance.GetSysSkillBaseVo(_petVo.SkillId);
            if (_talentSkillVo != null)
            {
                _talentSkillActionVo = BaseDataMgr.instance.GetSysSkillActionVo(_talentSkillVo.skill_group);
            }
            _masterVo = _petVo.MasterVo;
            _masterDisplay = _petVo.MasterDisplay;
        }

        public bool CheckCanUseTalentSkill()
        {
            switch (_talentSkillVo.key)
            {
                case TalentSkillType.AddMasterHp:
                    if (_masterVo.CurHp < _masterVo.Hp/2 && _masterVo.CurHp > 0 &&
                        MeController.SkillController.LeftCdTimes[0] <= 0)
                    {
                        return true;
                    }
                    break;
                case TalentSkillType.MakeMasterUnbeatable:
                    SysMapVo mapVo = AppMap.Instance.mapParser.MapVo;
                    if (_masterVo.CurHp < _masterVo.Hp*0.3f && _masterVo.CurHp > 0 &&
                        MeController.SkillController.LeftCdTimes[0] <= 0 && mapVo.type == MapTypeConst.COPY_MAP)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public bool TryTalentAttack()
        {
            if (_talentSkillVo.target_type == PetAiController.TargetEnemy)
            {
                if (Time.time - LastAttackTime > _talentSkillVo.cd*0.001)
                {
                    foreach (MonsterDisplay display in AppMap.Instance.monsterList)
                    {
                        var baseRoleVo = display.GetVo() as BaseRoleVo;
                        if (baseRoleVo.CurHp==0) continue;
                        Vector3 petPosition = display.Controller.transform.position;
                        Vector3 monsterPosition = MeController.transform.position;
                        if (Math.Abs(petPosition.x - monsterPosition.x) < _talentSkillVo.cover_width*0.001f&&
                            Math.Abs(petPosition.y - monsterPosition.y) < _talentSkillVo.cover_thickness * 0.001f * 0.5f)
                        {
                            var attackVo = new ActionVo
                            {
                                ActionType = Actions.ATTACK,
                                TargetPoint = display.GoBase.transform.position
                            };
                            MeController.AttackController.AddAttackList(attackVo);
                            MeController.TalentSkillController.LastAttackTime = Time.time;
                            return true;
                            break;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsSkillCdReady()
        {
            return Time.time - LastAttackTime > _talentSkillVo.cd*0.001;
        }

        public void UseTalentSkill()
        {
            switch (_talentSkillVo.key)
            {
                case TalentSkillType.AddMasterHp:
                    AddPlayerHp(); // 给主人加血
                    break;
                case TalentSkillType.MakeMasterUnbeatable:
                    MakePlayerUnBeatable(); // 使主人无敌
                    break;
            }
        }

        private void AddPlayerHp()
        {
            var addHp = (int) (_masterVo.Hp*_talentSkillVo.data_per/10000f);
            var curHp = (uint) (_masterVo.CurHp + addHp);
            _masterVo.CurHp = curHp < _masterVo.Hp ? curHp : _masterVo.Hp;
            Vector3 pos = MeController.transform.position;
            pos.y = pos.y + 1.0f;
            HeadInfoItemManager.Instance.ShowHeadInfo("自然之力", Color.white, pos, Directions.Left,
                HeadInfoItemType.TypePetTalentHp, 3.0f, 42);
            ShowTargetEffect();
            vp_Timer.In(1, ShowMasterAddHp);
        }


        private void ShowMasterAddHp()
        {
            var addHp = (int) (_masterVo.Hp*_talentSkillVo.data_per/10000f);
            Vector3 pos = _masterDisplay.Controller.transform.position;
            pos.y = _masterDisplay.BoxCollider2D.size.y + pos.y;
            HeadInfoItemManager.Instance.ShowHeadInfo("生命+" + addHp, Color.white, pos, Directions.Left,
                HeadInfoItemType.TypePetTalentHp, 2.0f, 36);
        }

        /// <summary>
        /// 显示目标特效
        /// </summary>
        private void ShowTargetEffect()
        {
            string[] effectIds = StringUtils.GetValueListFromString(_talentSkillActionVo.tar_eff);
            if (effectIds.Length > 0)
            {
                var effectId = effectIds[0];
                var usePoint = MeController.transform.position;
                var target = _masterDisplay.Controller.gameObject;
                var effectVo = new Effect
                {
                    URL = UrlUtils.GetSkillEffectUrl(effectId),
                    Direction =
                        (MeController.transform.position.x > usePoint.x) ? Directions.Right : Directions.Left,
                    BasePosition = MeController.transform.position,
                    Offset = new Vector3(0f, 0f, 0f),
                    Target = target,
                    Zoom = 1,
                    EulerAngles = new Vector3(0f, 0f, 0f),
                    NeedCache = true
                };
                EffectMgr.Instance.CreateSkillEffect(effectVo);
            }
        }

        private void MakePlayerUnBeatable()
        {
            int time = _talentSkillVo.data_fixed;
            Vector3 pos = MeController.transform.position;
            pos.y = pos.y + 1.0f;
            _masterVo.LeftUnbeatableTime += time;
            HeadInfoItemManager.Instance.ShowHeadInfo("圣者无敌", Color.white, pos, Directions.Left,
                HeadInfoItemType.TypePetTalentUnbeatable, 3.0f, 42);
        }
    }
}