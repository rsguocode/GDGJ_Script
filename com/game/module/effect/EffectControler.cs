using System.Collections.Generic;
using com.game.module.map;
using com.game.SkillLogic;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using UnityEngine;

namespace com.game.module.effect
{
    public class EffectControler : MonoBehaviour
    {
        public delegate void EffectEndCallback();

        private const float INITWAITTIME = -1f;
        private readonly Vector3 hiddenPos = Vector3.one*10000;
        public EffectEndCallback EndCallback; //特效播放后回调
        private float _calculateInterval = 10;
        private float _curTravelDistance;
        private Animator animator;
        private bool canPlay;
        private float deltaTime;
        private Effect effect;
        private Vector3 orgPos;
        private Transform selfTransform;
        private AnimatorStateInfo stateInfo;
        private Transform targetTransform;

        private float waitTime = INITWAITTIME;

        public Effect Effect
        {
            get { return effect; }
        }

        public void SetVo(Effect vo)
        {
            effect = vo;
            deltaTime = 0;
            _curTravelDistance = 0;

            if (null != vo.Target)
            {
                targetTransform = vo.Target.transform;
            }

            if (vo.LastTime > 0)
            {
                waitTime = vo.LastTime;
            }

            if (vo.Id == int.Parse(EffectId.Skill_Tornado))
            {
                gameObject.AddMissingComponent<TornadoSkill>();
            }

            if (vo.DelayTime > 0)
            {
                orgPos = vo.BasePosition + vo.Offset;
                transform.position = hiddenPos;                
                vp_Timer.In(vo.DelayTime, StartUse);
            }
            else
            {
                canPlay = true;
            }
        }

        private void StartUse()
        {
            gameObject.SetActive(false);
            transform.position = orgPos;
            gameObject.SetActive(true);
            canPlay = true;
        }

        public BaseDisplay GetMeDisplay()
        {
            return Effect.SkillController.MeController.Me;
        }

        public void Clear()
        {
            canPlay = false;

			//特效结束后回调
			if (null != effect.PlayedCallback)
			{
				effect.PlayedCallback();
			}

            if (effect.AutoDestroy)
            {                
				EffectMgr.Instance.RemoveEffect(effect.DictKey);
            }
            else
            {
                enabled = false;
                gameObject.SetActive(false);    

				if (null != EndCallback)
				{
					EndCallback();
				}
            }
		}
		
		private void Start()
        {
            selfTransform = gameObject.transform;
            animator = gameObject.GetComponentInChildren<Animator>();
		    if (animator != null)
		    {
		        animator.applyRootMotion = false;
		    }
        }

        private void Update()
        {
            if ((null == Effect) || !canPlay)
            {
                return;
            }

            //获得特效的播放时间
            if ((null != animator) && (waitTime <= 0f))
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!stateInfo.loop)
                {
                    waitTime = stateInfo.length;
                }
            }

            //目标跟随处理
            if (Effect.Target != null)
            {
                Vector3 pos = targetTransform.position + Effect.Offset;
				pos.z = EffectMgr.Instance.GetMainCameraEffectZ(pos.y);
                selfTransform.position = pos;
            }

            //非循环播放，时间到后销毁
            if (waitTime > 0f)
            {
                deltaTime += Time.deltaTime;
                if (deltaTime >= waitTime)
                {
                    Clear();
                }
            }

            //特效移动处理
            if (Effect.Speed > 0)
            {
                Vector3 movePos = Vector3.zero;
                int direction = (selfTransform.localRotation.y < 90f) ? 1 : -1;
                movePos.x = Time.deltaTime*Effect.Speed*direction;
                _curTravelDistance += movePos.x;
                selfTransform.Translate(movePos);
                if (Effect.MaxTravelDistance > 0 && _curTravelDistance > Effect.MaxTravelDistance)
                {
                    Clear();
                }
            }

            //子弹技能处理逻辑
            if (Effect.IsBullet)
            {
                if (selfTransform.position.x < MapControl.Instance.MyCamera.LeftBoundX ||
                    selfTransform.position.x > MapControl.Instance.MyCamera.RightBoundX)
                {
                    return;
                }
                switch (Effect.BulletType)
                {
                    case 1:
                        bool result = Effect.SkillController.CheckDamage(selfTransform.position, Effect.EffectIndex);
                        if (result)
                        {
                            Clear();
                        }
                        break;
                    case 2:
                        _calculateInterval += Time.deltaTime;
                        if (_calculateInterval > Effect.CheckInterval*0.001f)
                        {
                            _calculateInterval = 0;
                            Effect.SkillController.CheckDamage(selfTransform.position, Effect.EffectIndex);
                        }
                        List<ActionDisplay> enemys = Effect.SkillController.GetSkillCoveredEnemy(selfTransform.position);
                        foreach (ActionDisplay enemy in enemys)
                        {
                            int dir = Effect.Direction == Directions.Left ? -1 : 1;
                            Vector3 pos = enemy.Controller.transform.position;
                            pos.x = transform.position.x +
                                    dir*(Effect.SkillController.CurrentSkillVo.cover_width*0.001f - 0.2f);
                            pos.x = AppMap.Instance.mapParser.GetFinalMonsterX(pos.x);
                            enemy.Controller.transform.position = pos;
                        }
                        break;
                    case 3:
                        _calculateInterval += Time.deltaTime;
                        if (_calculateInterval > Effect.CheckInterval*0.001f)
                        {
                            _calculateInterval = 0;
                            Effect.SkillController.CheckDamage(selfTransform.position, Effect.EffectIndex);
                        }
                        break;
                }
            }
        }

        /*void OnDrawGizmos()
        {
            Gizmos.color = Color.green;     //在变换位置处绘制一个绿色圆
            Gizmos.DrawWireSphere(transform.position,0.2f);
        }*/
    }
}