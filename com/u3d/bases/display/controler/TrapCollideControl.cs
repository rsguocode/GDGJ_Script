using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.fight.arpg;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.character;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/05 10:38:25 
 * function: 碰撞受击类型陷阱
 * *******************************************************/

namespace com.u3d.bases.display.controler
{
    public class TrapCollideControl : MonoBehaviour
    {
        public ActionControler ActionControler;
        public List<ActionDisplay> LastAttackedActionDisplay;
        public TrapVo ThisTrapVo;
        public TrapDisplay TrapMeDisplay;
        private SysSkillBaseVo _skillBaseVo;
        private float _startTime;
        private float _lastAttackTime = 10;

        // Use this for initialization
        private void Start()
        {
            ActionControler.StatuController.SetStatu(Status.IDLE);
            TrapMeDisplay = ActionControler.Me as TrapDisplay;
            ThisTrapVo = TrapMeDisplay.GetTrapVo();
            _skillBaseVo = BaseDataMgr.instance.GetSysSkillBaseVo(uint.Parse(StringUtils.GetValueListFromString(ThisTrapVo.SysTrapVo.SkillIds)[0]));
            LastAttackedActionDisplay = new List<ActionDisplay>();
            _startTime = 0;
            Vector3 pos = transform.position;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_CylinderStandby, pos, true,null,false);
        }

        // Update is called once per frame
        void Update()
        {
            _startTime += Time.deltaTime;
            _lastAttackTime += Time.deltaTime;
            if (_startTime > 0.5f)
            {
                ActionAttack();
            }
        }

        private void ActionAttack()
        {
            if (ActionControler == null)
            {
                return;
            }
            bool result = DamageCheck.Instance.TrapColliderCheckInjured2D(_skillBaseVo, ActionControler.transform.position,
                       ActionControler.Me.CurFaceDire, TrapMeDisplay.BoxCollider2D, LastAttackedActionDisplay);
            if (result)
            {
                if (_lastAttackTime < 1)
                {
                    return;
                }
                _lastAttackTime = 0;
                Vector3 pos = transform.position;
                EffectMgr.Instance.CreateMainEffect(EffectId.Main_CylinderTrigger, pos, true,null,true,0.7f);
            }
        }

        private void DeleteMainCylinderTrigger()
        {
            GameObject effect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_CylinderTrigger);
            if (effect != null)
            {
               Object.Destroy(effect);
            }
        }
    }
}