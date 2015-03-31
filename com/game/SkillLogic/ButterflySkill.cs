﻿using System.Collections;
using System.Collections.Generic;
﻿using Assets.Plugins;
﻿using com.game.module.effect;
using com.game.utils;
using com.u3d.bases.display;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/28 10:34:27 
 * function: 蝴蝶技能
 * *******************************************************/

namespace com.game.SkillLogic
{
    public class ButterflySkill : MonoBehaviour
    {
        private ButterflySkillData _butterflySkillData;
        private List<Transform> _bomTransforms;
        private float _currentBomIndex;
        public EffectControler EffectControler;

        void Start()
        {
            _butterflySkillData = transform.GetComponent<ButterflySkillData>();
            _bomTransforms = _butterflySkillData.BomTransforms;
            _currentBomIndex = _butterflySkillData.CurrentBomIndex;
            EffectControler = transform.parent.GetComponent<EffectControler>();
        }

        /// <summary>
        /// 蝴蝶爆炸，产生一个光柱射向敌人
        /// </summary>
        public void Bom()
        {
            var butterflyBullet = Instantiate(EffectMgr.Instance.GetSkillEffectGameObject(EffectId.Skill_Butterfly_Bullet)) as GameObject;
            butterflyBullet.transform.position = _bomTransforms[(int)_currentBomIndex].position;
            butterflyBullet.SetActive(true);
            var bullet = butterflyBullet.GetComponent<ButterflyBullet>(); 
            if (bullet == null)
            {
                bullet = butterflyBullet.AddComponent<ButterflyBullet>();
            }
            bullet.EffectControler = EffectControler;
            bullet.MoveTo(GetTargetPosition());
        }


        /// <summary>
        /// 获取光柱子弹的目标位置
        /// </summary>
        /// <returns></returns>
        private Vector3 GetTargetPosition()
        {
            IList<ActionDisplay> enemys = EffectControler.Effect.SkillController.GetEnemyDisplayByRange(4.5f, EffectControler.Effect.SkillController.transform.position);
            if (enemys.Count > 0)
            {
                int index = Random.Range(0, enemys.Count - 1);
                return enemys[index].Controller.transform.position;
            }
            else
            {
                Vector3 positon = EffectControler.Effect.SkillController.transform.position;
                positon.x += Random.Range(1.5f, 2.0f);
                return positon;
            }
        }

    }
}