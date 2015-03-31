using com.game.consts;
using com.game.module.fight.vo;
using Com.Game.Speech;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game;
using com.game.module.map;
using com.u3d.bases.display.character;
using System.Collections.Generic;

/**状态控制器
 * @author 骆琦
 * @date   2013-11-3
 * 基于animator的状态控制器
 * **/

namespace com.u3d.bases.controller
{
    public class StatuControllerBase : MonoBehaviour
    {
        /// <summary>
        ///     用于查找从一个状态到另一个状态的切换是否是允许的
        /// </summary>
        private static readonly int[,] StatuChangeMatrix =
        {
            //ID|RU|RO|A1|A2|A3|A4|S1|S2|S3|H1|DE|H2|H3|H4|HD|SU|S4|Wi|S5|S6|S7|S8|A5|A6
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,0,0}, //IDLE
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1,0,0}, //RUN
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //ROLL
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //ATTACK1
            {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //ATTACK2
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,1,1}, //ATTACK3
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //ATTACK4
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL1
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL2
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL3
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //HURT1
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //DEATH
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //HURT2
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0,0,0}, //HURT3
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0,0,0}, //HURT4
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0,0,0}, //HURTDOWN
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //STANDUP
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL4
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0}, //WIN
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL5
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL6
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL7
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //SKILL8
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0}, //ATTACK5
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0,0,0} //ATTACK6
            //ID|RU|RO|A1|A2|A3|A4|S1|S2|S3|H1|DE|H2|H3|H4|HD|SU|S4|Wi|S5|S6|S7|S8|
        };

        protected Animator Animator; //模型动画控制
        public int CurStatuNameHash; //当前状态的NameHash
        public int MaxComb; //普通攻击最大连击数
        public ActionControler MeControler; //模型行为控制中心
        public int PreStatuNameHash; //前一个状态的NameHash，用于判断是否进行了状态的切换
        protected AnimatorStateInfo StateInfo; //当前状态详细信息
        private float _changeTime; //状态改变时间点
        private int _currentCombStatu; //当前连击状态  
        private int _currentStatu; //当前行为状态
        public bool CanChangeStatus = true;//是否能够改变状态

        /// <summary>
        ///     获取当前状态
        /// </summary>
        public int CurrentStatu
        {
            get { return _currentStatu; }
        }

        /// <summary>
        ///     连击状态属性
        /// </summary>
        public virtual int CurrentCombStatu
        {
            get { return _currentCombStatu; }
            set
            {
                //CancelInvoke("ResetCombStatu");
                _currentCombStatu = value;
                /*if (value == Status.COMB_4)
                {
                    Invoke("ResetCombStatu", CombInterval); // 1S后没有再处理则重置连击状态
                }
                else
                {
                    //Invoke("ResetCombStatu", 0.2f); // 1S后没有再处理则重置连击状态
                }*/
            }
        }

        public void SetIdleImmediately()
        {
            Animator.Play(Status.NAME_HASH_IDLE);
            _currentStatu = Status.IDLE;
        }

        public void SetHurtDownImmediately()
        {
            Animator.Play(Status.NAME_HASH_HURTDOWN);
            _currentStatu = Status.HURTDOWN;
        }

        public void SetHurt1Immediately()
        {
            PreStatuNameHash = Animator.GetCurrentAnimatorStateInfo(0).nameHash;
            Animator.Play(Status.IDLE);
            Animator.Play(Status.NAME_HASH_HURT1);
            _currentStatu = Status.HURT1;
        }

        public void SetHurt2Immediately()
        {
            PreStatuNameHash = Animator.GetCurrentAnimatorStateInfo(0).nameHash;
            Animator.Play(Status.IDLE);
            Animator.Play(Status.NAME_HASH_HURT2);
            _currentStatu = Status.HURT2;
        }

        public void SetHurt4Immediately()
        {
            PreStatuNameHash = Animator.GetCurrentAnimatorStateInfo(0).nameHash;
            Animator.Play(Status.IDLE);
            Animator.Play(Status.NAME_HASH_HURT4);
            _currentStatu = Status.HURT4;
        }

        public void StopAnimation()
        {
            if (Animator != null)
            {
                Animator.speed = 0;
            }
        }

        public void ResetAnimation()
        {
            if (Animator != null)
            {
                Animator.speed = 1;
            }
        }

        /// <summary>
        ///     获取animator控制
        /// </summary>
        /// <returns></returns>
        public Animator GetAnimator()
        {
            return Animator;
        }

        /// <summary>
        ///     重置连击状态
        /// </summary>
        private void ResetCombStatu()
        {
            _currentCombStatu = Status.COMB_0;
        }

        /// <summary>
        ///     初始化赋值
        /// </summary>
        private void Start()
        {
            //MeControler = GetComponent<ActionControler>();
            PreStatuNameHash = Status.NAME_HASH_IDLE;
            SetStatu(Status.IDLE);
        }

        /// <summary>
        ///     运行时每帧监控当前所处的状态并进行相应的逻辑处理;
        /// </summary>
        protected void Update()
        {
            if (Animator == null)
            {
                Animator = MeControler.Me.Animator;
                if (Animator.enabled == false)
                {
                    Animator.enabled = true;
                }
                Animator.applyRootMotion = false;
                return;
            }
            if (Animator.speed <= 0) return;
            StateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            CurStatuNameHash = StateInfo.nameHash;
            if (CurStatuNameHash == Status.NAME_HASH_ATTACK1)
            {

                ProcessAttack1State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ATTACK2)
            {
                ProcessAttack2State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ATTACK3)
            {
                ProcessAttack3State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ATTACK4)
            {
                ProcessAttack4State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ATTACK5)
            {
                ProcessAttack5State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ATTACK6)
            {
                ProcessAttack6State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_RUN)
            {
                ProcessRunState(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_ROLL)
            {
                ProcessRollState(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_IDLE)
            {
                ProcessIdleState(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_SKILL1)
            {
                ProcessSkill1State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_SKILL2)
            {
                ProcessSkill2State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_SKILL3)
            {
                ProcessSkill3State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_SKILL4)
            {
                ProcessSkill4State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_HURT1)
            {
                ProcessHurt1State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_HURT2)
            {
                ProcessHurt2State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_HURT3)
            {
                ProcessHurt3State(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_HURT4)
            {
                ProcessHurt4State(StateInfo);
                //Log.info(this, "currentStatu:" + CurrentStatu);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_HURTDOWN)
            {
                ProcessHurtDownState(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_STANDUP)
            {
                ProcessStandUpState(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_Win)
            {
                ProcessWin(StateInfo);
            }
            else if (CurStatuNameHash == Status.NAME_HASH_DEATH)
            {
                ProcessDeathState(StateInfo);
            }
            if (PreStatuNameHash != CurStatuNameHash)
            {
                DoStatuTransfered();
            }
            PreStatuNameHash = CurStatuNameHash; //设置前一个statuNameHash
            /*if (MeControler.Me.Type == DisplayType.MONSTER)
            {
                int statu = CurrentStatu;
               //Log.info(this, "=================================status:" + CurrentStatu);
            }*/
        }


        /// <summary>
        ///     请求状态改变的唯一入口，使状态的改变控制更加清晰简单
        /// </summary>
        /// <param name="nextStatu"></param>
        public virtual bool SetStatu(int nextStatu)
        {
            if (GetStatuMatrix()[CurrentStatu, nextStatu] == 0 || CurrentStatu == nextStatu || Animator == null)
            {
                return false; //如果当前状态到下一状态的切换是不允许的，则直接返回
            }
            if (MeControler.GetMeVo().NeedKeep && nextStatu != Status.IDLE)
            {
                return false;
            }
            if (false == CanChangeStatus)
            {
                return false;
            }
            if (nextStatu == Status.IDLE)
            {
                MeControler.GetMeVo().NeedKeep = false;
            }
            _changeTime = Time.time;
            _currentStatu = nextStatu;
            Animator.SetInteger(Status.STATU, _currentStatu);
            DoStatuChange();
            return true;
        }

        /// <summary>
        ///     状态开始切换时执行这个方法，便于执行一些状态变化时要处理的事情
        /// </summary>
        protected virtual void DoStatuChange()
        {
            MeControler.MoveController.MoveAtTheStatuBegin(CurrentStatu);
            if (MeControler.AnimationParameter)
            {
                MeControler.AnimationParameter.AnimationXSpeed = 0;
            }
        }

        /// <summary>
        ///     动作切换完成后执行这个方法，状态开始切换后动作不一定立刻就切换完成，中间有个动画过渡的阶段，动画过渡完后就会触发这个方法
        /// </summary>
        protected virtual void DoStatuTransfered()
        {
            if (MeControler.GetMeVo().IsRush || 
                MeControler.GetMeVo().IsMoving || 
                MeControler.GetMeVo().CanCtrlMoveDuringSkill)
            {
                if(MeControler.SkillController.CurrentActionVo.action_id != CurrentStatu)
                {
                    MeControler.GetMeVo().IsRush = false;
                    MeControler.GetMeVo().IsMoving = false;
                    MeControler.GetMeVo().CanCtrlMoveDuringSkill = false;
                }
            }
        }

        /// <summary>
        /// 子类重写，用于长按自动连击;
        /// </summary>
        protected virtual void judgeNormalCombIfLongkey()
        {

        }

        /// <summary>
        ///     处于普通攻击状态1时的状态处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected virtual void ProcessAttack1State(AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > 0.6 && CurrentStatu == Status.ATTACK1)
            {
                judgeNormalCombIfLongkey();
                if (MeControler.AttackController.AttackList.Count > 0 &&
                    MeControler.SkillController.SkillList.Count == 0)
                {
                    ActionVo actionVo = MeControler.AttackController.AttackList[0];
                    if (actionVo.ActionType == Actions.ATTACK)
                    {
                        MeControler.AttackController.AttackList.RemoveAt(0);
                        SetStatu(Status.ATTACK2);
                        CurrentCombStatu = Status.COMB_0;
                    }
                    else
                    {
                        CurrentCombStatu = Status.COMB_1;
                        SetStatu(Status.IDLE);
                    }
                }
                else
                {
                    CurrentCombStatu = Status.COMB_1;
                    SetStatu(Status.IDLE);
                }
            }
        }

        /// <summary>
        ///     处于普通攻击2状态时的状态处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessAttack2State(AnimatorStateInfo stateInfo)
        {
            if (CurrentStatu == Status.ATTACK2 && CurrentCombStatu == Status.COMB_0)
            {
                CurrentCombStatu = Status.COMB_1;
                MeControler.SkillController.RequestUseSkill(SkillController.Attack2);
            }
            if (stateInfo.normalizedTime > 0.6 && CurrentStatu == Status.ATTACK2)
            {
                judgeNormalCombIfLongkey();
                if (MeControler.AttackController.AttackList.Count > 0 &&
                    MeControler.SkillController.SkillList.Count == 0)
                {
                    ActionVo actionVo = MeControler.AttackController.AttackList[0];
                    if (actionVo.ActionType == Actions.ATTACK)
                    {
                        MeControler.AttackController.AttackList.RemoveAt(0);
                        SetStatu(Status.ATTACK3);
                        CurrentCombStatu = Status.COMB_1;
                    }
                    else
                    {
                        CurrentCombStatu = Status.COMB_2;
                        SetStatu(Status.IDLE);
                    }
                }
                else
                {
                    CurrentCombStatu = Status.COMB_2;
                    SetStatu(Status.IDLE);
                }
            }
        }

        /// <summary>
        ///     处于普通攻击3状态时的状态处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessAttack3State(AnimatorStateInfo stateInfo)
        {
            if (CurrentStatu == Status.ATTACK3 && CurrentCombStatu == Status.COMB_1)
            {
                CurrentCombStatu = Status.COMB_2;
                MeControler.SkillController.RequestUseSkill(SkillController.Attack3);
            }
            if (stateInfo.normalizedTime > 0.6 && CurrentStatu == Status.ATTACK3)
            {
                judgeNormalCombIfLongkey();
                if (MeControler.AttackController.AttackList.Count > 0 &&
                    MeControler.SkillController.SkillList.Count == 0 && MaxComb > 3)
                {
                    ActionVo actionVo = MeControler.AttackController.AttackList[0];
                    if (actionVo.ActionType == Actions.ATTACK)
                    {
                        MeControler.AttackController.AttackList.RemoveAt(0);
                        CurrentCombStatu = Status.COMB_2;
                        //魔剑士职业有投掷，冲刺连招;
                        if (GameConst.JOB_JIAN == MeVo.instance.job)
                        {
                            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                            {
                                SetStatu(Status.ATTACK5);
                            }
                            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                            {
                                SetStatu(Status.ATTACK6);
                            }
                            else
                            {
                                SetStatu(Status.ATTACK4);
                            }
                        }
                        else
                        {
                            SetStatu(Status.ATTACK4);
                        }
                    }
                    else
                    {
                        CurrentCombStatu = Status.COMB_3;
                        SetStatu(Status.IDLE);
                    }
                }
                else
                {
                    CurrentCombStatu = Status.COMB_3;
                    SetStatu(Status.IDLE);

                    //法师三连击语音播放
                    if (GameConst.JOB_FASHI == MeVo.instance.job)
                    {
                        if (SpeechMgr.Instance.IsMagicSpeech)
                        {
                            SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicNormalSkillCycle);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     处于普通攻击4状态时的状态处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessAttack4State(AnimatorStateInfo stateInfo)
        {
            if (CurrentStatu == Status.ATTACK4 && CurrentCombStatu == Status.COMB_2)
            {
                CurrentCombStatu = Status.COMB_3;
                MeControler.SkillController.RequestUseSkill(SkillController.Attack4);
            }
            if (stateInfo.normalizedTime > 0.9 && CurrentStatu == Status.ATTACK4)
            {
                judgeNormalCombIfLongkey();
                CurrentCombStatu = Status.COMB_4;
                SetStatu(Status.IDLE);
            }
        }

        protected void ProcessAttack5State(AnimatorStateInfo stateInfo)
        {
            if (CurrentStatu == Status.ATTACK5 && CurrentCombStatu == Status.COMB_2)
            {
                Debug.Log("进入攻击5**********************************(1)");
                CurrentCombStatu = Status.COMB_3;
                MeControler.SkillController.RequestUseSkill(SkillController.Attack5);
            }
            if (stateInfo.normalizedTime > 0.75 && CurrentStatu == Status.ATTACK5)
            {
                Debug.Log("进入攻击5**********************************(2)");
                judgeNormalCombIfLongkey();
                CurrentCombStatu = Status.COMB_5;
                SetStatu(Status.IDLE);
            }
        }

        protected void ProcessAttack6State(AnimatorStateInfo stateInfo)
        {
            if (CurrentStatu == Status.ATTACK6 && CurrentCombStatu == Status.COMB_2)
            {
                CurrentCombStatu = Status.COMB_3;
                MeControler.SkillController.RequestUseSkill(SkillController.Attack6);
            }
            if (stateInfo.normalizedTime > 0.75 && CurrentStatu == Status.ATTACK6)
            {
                judgeNormalCombIfLongkey();
                CurrentCombStatu = Status.COMB_6;
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     处于待机状态时的状态处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessIdleState(AnimatorStateInfo stateInfo)
        {
            if (Time.time - _changeTime > 1)
            {
                SetStatu(Status.IDLE);
            }
            if (Time.time - _changeTime > 0.1f)
            {
                CurrentCombStatu = Status.COMB_0;
            }
        }

        /// <summary>
        ///     处于移动状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected virtual void ProcessRunState(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.RUN)
            {
                MeControler.StopWalk();
            }
        }

        /// <summary>
        ///     处于翻滚状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessRollState(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.ROLL)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     处于技能1状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessSkill1State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.SKILL1)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     处于技能2状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessSkill2State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.SKILL2)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     处于技能3状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessSkill3State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.SKILL3)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     处于技能4状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessSkill4State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.SKILL4)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     受击1状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessHurt1State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.HURT1)
            {
                SetStatu(Status.IDLE);
            }
        }


        /// <summary>
        ///     受击2状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessHurt2State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.HURT2)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     受击3状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessHurt3State(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.HURT3)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     受击4状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected virtual void ProcessHurt4State(AnimatorStateInfo stateInfo)
        {
        }

        /// <summary>
        ///     受击后倒地状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected virtual void ProcessHurtDownState(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.HURTDOWN)
            {
                SetStatu(Status.STANDUP);
            }
        }

        /// <summary>
        ///     倒地后站起状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessStandUpState(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.STANDUP)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     胜利状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected void ProcessWin(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.Win)
            {
                SetStatu(Status.IDLE);
            }
        }

        /// <summary>
        ///     死亡状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        protected virtual void ProcessDeathState(AnimatorStateInfo stateInfo)
        {
        }

        /// <summary>
        ///     获取状态矩阵，使用方法的目的是为了使子类可以复写该方法
        /// </summary>
        /// <returns></returns>
        protected virtual int[,] GetStatuMatrix()
        {
            return StatuChangeMatrix;
        }
    }
}