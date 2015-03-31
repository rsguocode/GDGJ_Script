using com.game;
using com.game.consts;
using com.game.module.battle;
using com.game.module.effect;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using System.Collections;
using UnityEngine;
using com.game.module.test;

/**移动控制基类
 * @author 骆琦
 * @date  2013-11-3
 * 基于animator的移动控制基类
 * **/

namespace com.u3d.bases.controller
{
    public delegate void DropCallBack(BaseDisplay display);
    public class MoveControllerBase : MonoBehaviour
    {
        private DropCallBack _dropCallBack;
        private const float RollSpeed = 8.0f; //瞬移的速度，程序中写死，单位：unity单位/s
        private const float Skill2Speed = 0f; //技能冲刺的速度，程序中写死，单位：unity单位/s
        private const float Skill3Speed = 3.0f; //旋风斩速度，程序中写死，单位：unity单位/s
        private const float Attack3Speed = 0.3f; //攻击3的速度，程序中写死，单位：unity单位/s
        private const float Attack4Speed = 0.3f; //攻击4的速度，程序中写死，单位：unity单位/s
        public AnimationEventController AnimationEventController; //动画事件控制器
        public AnimationParameter AnimationParameter;
        public ActionControler MeController; //控制处理中心
        public float StartY; //控制抛物线位移检测用的
        protected Transform ThisTransform; //挂载体的Transform
        private Animator _animator; //挂载体的动画控制器
        private float _curActionOffset; //记录动画的位移
        private Transform _footTransform; //脚部定位点,用与动画中描述动作的位移
        private Task _RushTask;//技能突进协程任务
        private Task _MoveTask;//技能匀速移动协程任务
        private Task _CtrlMoveDuringSkill;//技能过程中控制移动协程任务


        // Use this for initialization
        private void Start()
        {
            MeController = GetComponent<ActionControler>();
            ThisTransform = MeController.Me.GoBase.transform;
            if (MeController.Me.GoCloth.transform.Find("X_Foot"))
            {
                _footTransform = MeController.Me.GoCloth.transform.Find("X_Foot").transform;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (_animator == null)
            {
                _animator = MeController.Me.Animator;
                return;
            }
            if(_animator.speed<=0)return;
            //1. 更新动作自带位移 
            UpdateActionAutoMove();

            //2. 更新程序设定的基于状态的位移
            UpdateProgramMove();
        }

        /// <summary>
        ///     更新动作自带位移
        ///     动作自带位移是记录在动画文件中的，将美术记录的动作的位移体现游戏世界中
        /// </summary>
        private void UpdateActionAutoMove()
        {
            if (_animator.IsInTransition(0) && _curActionOffset > 0)
            {
                Vector3 position = ThisTransform.position;
                int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                position.x += _curActionOffset*dir;
                ThisTransform.position = position;
                _curActionOffset = 0;
            }
            //注意这两段代码的顺序很重要，改变顺序会使基于动画的位移偏少
            if (!_animator.IsInTransition(0) && _footTransform)
            {
                _curActionOffset = _footTransform.localPosition.x;
            }
        }

        private void UpdateProgramMove()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.nameHash == Status.NAME_HASH_RUN)
            {
                ProcessRunMove();
            }
            /*
            else if (stateInfo.nameHash == Status.NAME_HASH_ATTACK1)
                        {
                            ProcessAttack1Move();
                        }
                        else if (stateInfo.nameHash == Status.NAME_HASH_ATTACK3)
                        {
                            ProcessAttack3Move();
                        }
                        else if (stateInfo.nameHash == Status.NAME_HASH_ATTACK4)
                        {
                            ProcessAttack4Move();
                        }
                        else if (stateInfo.nameHash == Status.NAME_HASH_ROLL)
                        {
                            ProcessRollMove();
                        }
                        else if (stateInfo.nameHash == Status.NAME_HASH_SKILL2)
                        {
                            ProcessSkill2Move();
                        }
            else if (stateInfo.nameHash == Status.NAME_HASH_ATTACK5)
            {
                ProcessAttack5Move();
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_ATTACK6)
            {
                ProcessAttack6Move();
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_ROLL)
            {
                ProcessRollMove();
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_SKILL2)
            {
                ProcessSkill2Move();
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_SKILL3)
            {
                ProcessSkill3Move();
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_HURT1)
            {
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_HURT2)
            {
            }
            else if (stateInfo.nameHash == Status.NAME_HASH_HURT3)
            {
            }*/
        }

        protected virtual void ProcessRunMove()
        {
        }

        /// <summary>
        ///     攻击动作1状态时的位移处理
        /// </summary>
        protected virtual void ProcessAttack1Move()
        {
            if (AnimationParameter != null)
            {
                Vector3 position = ThisTransform.position;
                int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                float x = position.x + Time.deltaTime*dir*AnimationParameter.AnimationXSpeed;
                x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
                position.x = x;
                ThisTransform.position = position;
            }
        }

        /// <summary>
        ///     攻击动作3状态时的位移处理
        /// </summary>
        protected virtual void ProcessAttack3Move()
        {
            if (MeController.GetMeVo().job == 1)
            {
                Vector3 position = ThisTransform.position;
                int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                float x = position.x + Time.deltaTime*dir*Attack3Speed;
                x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
                position.x = x;
                ThisTransform.position = position;
            }
        }

        protected virtual void ProcessAttack4Move()
        {
            Vector3 position = ThisTransform.position;
            int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
            float x = position.x + Time.deltaTime*dir*Attack4Speed;
            x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
            position.x = x;
            ThisTransform.position = position;
        }

        protected virtual void ProcessAttack5Move()
        {

        }

        protected virtual void ProcessAttack6Move()
        {

        }

        /// <summary>
        ///     瞬移时的状态处理
        /// </summary>
        protected virtual void ProcessRollMove()
        {
            Vector3 position = ThisTransform.position;
            int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
            float x = position.x + Time.deltaTime*dir*RollSpeed;
            x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
            position.x = x;
            ThisTransform.position = position;
        }

        protected virtual void ProcessSkill2Move()
        {
            if (MeController.GetMeVo().job == 1)
            {
                Vector3 position = ThisTransform.position;
                int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                float x = position.x + Time.deltaTime*dir*Skill2Speed;
                x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
                position.x = x;
                ThisTransform.position = position;
            }
        }

        protected virtual void ProcessSkill3Move()
        {
            if (MeController.GetMeVo().job == 1)
            {
                Vector3 position = ThisTransform.position;
                int dir = 0;
                if (NGUIJoystick.IsLeft || BattleMode.Instance.IsLeft)
                {
                    dir = -1;
                }
                if (NGUIJoystick.IsRight || BattleMode.Instance.IsRight)
                {
                    dir = 1;
                }
                float x = position.x + Time.deltaTime*dir*Skill3Speed;
                x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
                position.x = x;
                ThisTransform.position = position;
            }
        }

        /// <summary>
        ///     状态开始触发的位移
        /// </summary>
        public virtual void MoveAtTheStatuBegin(int statuType)
        {
        }


        /// <summary>
        /// 怪物掉落的控制
        /// </summary>
        public void MonsterDrop(DropCallBack dropCallBack)
        {
            /*
            _dropCallBack = dropCallBack;
            var position = MeController.Me.GoBase.transform.position;
            var mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            position.y = Random.Range(mapRange.MinY, mapRange.MaxY);
            position.z = position.y*5;
            //SetHurtFlyState();
            var argsHashtable = new Hashtable
                        {
                            {"time", 1f},
                            {"position", position},
                            {"oncomplete", "OnMoveEnd"}
                        };
                        iTween.MoveTo(MeController.Me.GoBase.transform.gameObject, argsHashtable);*/
                        vp_Timer.In(0.2f, SetHurtFlyState);
                        vp_Timer.In(0.5f,SetHurtDownState);
        }


        /// <summary>
        /// 出生特效播放完毕的调用
        /// </summary>
        private void EffectCallback()
        {
            if (_dropCallBack!=null)
            {
                _dropCallBack(MeController.Me);
            }
        }
        
        /// <summary>
        /// 怪物掉落到地上的回调
        /// </summary>
        private void OnMoveEnd()
        {          
            //开始处理刷怪的位置
            //MeController.StatuController.SetStatu(Status.HURTDOWN); //摔倒动作
            if (_dropCallBack != null)
            {
                _dropCallBack(MeController.Me);
            }
           /* var effectVo = new Effect();
            effectVo.URL = UrlUtils.GetSkillEffectUrl(EffectId.Born_Drop);
            effectVo.BasePosition = ThisTransform.position; 
            effectVo.Target = ThisTransform.gameObject; 
            effectVo.NeedCache = true;
            effectVo.PlayedCallback = EffectCallback;  //特效回调函数
            EffectMgr.Instance.CreateSkillEffect(effectVo);*/
        }

        private void SetHurtFlyState()
        {
            MeController.StatuController.SetStatu(Status.HURT4); //摔倒动作
        }

        private void SetHurtDownState()
        {
            MeController.StatuController.SetStatu(Status.HURTDOWN); //摔倒动作
        }

        public void StartRush()
        {
            _RushTask = CoroutineManager.StartCoroutine(Rush());
        }

        public void StopRush()
        {
            if (_RushTask != null)
            {
                CoroutineManager.StopCoroutine(_RushTask);
            }
        }

        public void StartMove()
        {
            _MoveTask = CoroutineManager.StartCoroutine(Move());
        }

        public void StopMove()
        {
            if (_MoveTask != null)
            {
                CoroutineManager.StopCoroutine(_MoveTask);
            }
        }

        public void StartCtrlMoveDuringSkill()
        {
            _CtrlMoveDuringSkill = CoroutineManager.StartCoroutine(CtrlMoveDuringSkill());
        }

        public void StopCtrlMoveDuringSkill()
        {
            if (_CtrlMoveDuringSkill != null)
            {
                CoroutineManager.StopCoroutine(_CtrlMoveDuringSkill);
            }
        }

        /// <summary>
        /// 技能冲刺逻辑处理
        /// </summary>
        /// <returns></returns>
        private IEnumerator Rush()
        {
            var meVo = MeController.GetMeVo();
            if (meVo.IsRush)
            {
                while (meVo.RushSpeed > 0 && meVo.Distance > 0)
                {
                    yield return 0;
                    int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                    Vector3 pos = MeController.Me.GoBase.transform.position;
                    float distance = meVo.RushSpeed * Time.deltaTime + 0.5f * meVo.Acceleration * Time.deltaTime * Time.deltaTime;
                    pos.x += meVo.Direction * dir * distance;
                    pos.x = AppMap.Instance.mapParser.GetFinalMonsterX(pos.x);
                    MeController.Me.GoBase.transform.position = pos;
                    meVo.RushSpeed -= Time.deltaTime * Mathf.Abs(meVo.Acceleration);
                    meVo.Distance -= distance;
                }
            }
            meVo.IsRush = false;
        }

        /// <summary>
        /// 技能位移逻辑处理
        /// </summary>
        /// <returns></returns>
        private IEnumerator Move()
        {
            var meVo = MeController.GetMeVo();
            if (meVo.IsMoving)
            {
                while (meVo.MoveDistance > 0 && meVo.MoveTime > 0)
                {
                    yield return 0;
                    int dir = MeController.Me.CurFaceDire == Directions.Left ? -1 : 1;
                    Vector3 pos = MeController.Me.GoBase.transform.position; 
                    float distance = Time.deltaTime * meVo.MoveSpeed;
                    pos.x += meVo.MoveDirection * dir * distance;
                    pos.x = AppMap.Instance.mapParser.GetFinalMonsterX(pos.x);
                    MeController.Me.GoBase.transform.position = pos;
                    meVo.MoveDistance -= distance;
                    meVo.MoveTime -= Time.deltaTime;
                }
            }
            meVo.IsMoving = false;
        }

        /// <summary>
        /// 技能过程中控制移动逻辑处理
        /// </summary>
        /// <returns></returns>
        private IEnumerator CtrlMoveDuringSkill()
        {
            var meVo = MeController.GetMeVo();
            while (meVo.CanCtrlMoveDuringSkill)
            {
                Vector3 position = ThisTransform.position;
                int dir = 0;
                if (NGUIJoystick.IsLeft || BattleMode.Instance.IsLeft)
                {
                    dir = -1;
                }
                if (NGUIJoystick.IsRight || BattleMode.Instance.IsRight)
                {
                    dir = 1;
                }
                float x = position.x + Time.deltaTime * dir * meVo.MoveSpeedDuringSkill;
                x = AppMap.Instance.mapParser.GetFinalPlayerX(x); //控制不让出界
                position.x = x;
                ThisTransform.position = position;
                yield return 0;
            }
        }
    }
}