
using System.Collections;
using com.game.module.effect;
using com.game.utils;
using UnityEngine;
using com.u3d.bases.debug;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/28 11:39:06 
 * function: 蝴蝶技能光柱子弹
 * *******************************************************/

namespace com.game.SkillLogic
{
    public class ButterflyBullet : MonoBehaviour
    {
        public EffectControler EffectControler;

        /// <summary>
        /// 子弹打向目标位置
        /// </summary>
        /// <param name="targetPosition"></param>
        public void MoveTo(Vector3 targetPosition)
        {
            Hashtable argsHashtable = new Hashtable();
            argsHashtable.Add("time",0.25f);
            argsHashtable.Add("position",targetPosition);
            argsHashtable.Add("oncomplete","OnMoveEnd");
            iTween.MoveTo(gameObject, argsHashtable);
        }

        private void OnMoveEnd()
       {
            EffectControler.Effect.SkillController.CheckDamage(transform.position, 0);
            var effectVo = new Effect
            {
				URL = UrlUtils.GetSkillEffectUrl(EffectId.Skill_Butterfly_Bom),
                BasePosition = transform.position,
                Offset = new Vector3(0f, 0f, 0f),
                NeedCache = true
            };
            EffectMgr.Instance.CreateSkillEffect(effectVo);
            Destroy(gameObject);
        }


    }
}