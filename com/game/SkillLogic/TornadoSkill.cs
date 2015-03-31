﻿﻿
using System.Collections.Generic;
using com.game.module.effect;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/21 01:47:09 
 * function: 龙卷风技能
 * *******************************************************/

namespace com.game.SkillLogic
{
    public class TornadoSkill : MonoBehaviour
    {

        public EffectControler EffectControler;
        private Dictionary<uint, TornadoEnemyData> _skillEnemyData;
        private const float MoveSpeed = 6f;
        private const float MaxMoveTime = 0.35f;
        private const float CheckDuration = 0.2f;
        private float _currentInterval;
        

        // Use this for initialization
        private void Start()
        {
            EffectControler = transform.GetComponent<EffectControler>();
            _skillEnemyData = new Dictionary<uint, TornadoEnemyData>();
            _currentInterval = 0;
            EffectControler.EndCallback = ClearData;
        }

        private void ClearData()
        {
            _skillEnemyData.Clear();
            _currentInterval = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            IList<ActionDisplay> enemys = EffectControler.Effect.SkillController.GetEnemyDisplayByRange(1.2f, EffectControler.transform.position);
            foreach (var enemy in enemys)
            {
                uint enemyId = enemy.GetVo().Id;
                if (!_skillEnemyData.ContainsKey(enemyId))
                {
                    _skillEnemyData.Add(enemyId,new TornadoEnemyData(){EnemyDisplay = enemy,MoveTime = 0});
                }
            }
            foreach (var enemyData in _skillEnemyData.Values)
            {
                if (enemyData.MoveTime < MaxMoveTime)
                {
                    enemyData.MoveTime += Time.deltaTime;
                    var enemyVo = enemyData.EnemyDisplay.GetMeVoByType<BaseRoleVo>();
                    if (enemyData.EnemyDisplay.Controller == null || enemyVo.CurHp == 0) //死亡的时候不被击退
                    {
                        continue;
                    }
                    var pos = enemyData.EnemyDisplay.Controller.transform.position;
                    int dir = EffectControler.GetMeDisplay().CurFaceDire == Directions.Left ? -1 : 1;
                    float moveRatio = enemyVo.MoveRatio;
                    pos.x += dir * MoveSpeed * Time.deltaTime * moveRatio;
                    pos.x = AppMap.Instance.mapParser.GetFinalMonsterX(pos.x);          //控制不让出界
                    enemyData.EnemyDisplay.Controller.transform.position = pos;
                }
            }
            _currentInterval += Time.deltaTime;
            if (_currentInterval > CheckDuration)
            {
                _currentInterval = 0;
                EffectControler.Effect.SkillController.CheckDamage(transform.position, 0);
            }
        }
    }

    internal class TornadoEnemyData
    {
        public ActionDisplay EnemyDisplay;
        public float MoveTime;
    }
}