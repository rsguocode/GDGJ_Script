using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.fight.arpg;
using com.game.module.map;
using Com.Game.Module.Role;
using com.game.sound;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using UnityEngine;
using Com.Game.Speech;

/**技能控制类
 * @author 骆琦 
 * @date  2013-11-6
 * 技能控制，技能CD控制，技能特效触发，技能伤害检测
 * **/

namespace com.u3d.bases.controller
{
    public class SkillController : MonoBehaviour
    {
        public const int Attack1 = 0;
        public const int Attack2 = 1;
        public const int Attack3 = 2;
        public const int Attack4 = 3;
        /// <summary>
        /// 普攻冲刺终结;
        /// </summary>
        public const int Attack5 = 4;
        /// <summary>
        /// 普通抓投终结;
        /// </summary>
        public const int Attack6 = 5;
        public const int Skill1 = 6;
        public const int Skill2 = 7;
        public const int Skill3 = 8;
        public const int Skill4 = 9;
        public const int Roll = 10; //翻滚||瞬移
        private readonly IDictionary<uint, float> _skillDelayDic = new Dictionary<uint, float>();
		private readonly IDictionary<uint, bool> _skillSoundDic = new Dictionary<uint, bool>();
		private string skillCacheSound = string.Empty;
		private uint skillCacheId = 0;
        public List<uint> LearnedSkillList; //已学会的技能列表
        public float[] LeftCdTimes;
        public ActionControler MeController;
        public List<int> SkillList; //缓存待施放的技能
        public int[] SkillMpCost;
        public float[] TotalCdTimes;
        private int _checkedTime; //当前技能已经触发的伤害检测次数
        private SysSkillActionVo _currentActionVo;
        private uint _currentSkillId = 10000; //当前正在使用的技能
        private int _currentSkillType; //当前正在使用的技能类型
        private SysSkillBaseVo _currentSkillVo;
        private Transform _selfTransform;
        private IList<string> _attackSoundList;
        private IList<string> _hitSoundList;

		public void ResetCacheSound()
		{
			skillCacheId = 0;
			skillCacheSound = string.Empty;
		}

        public SysSkillBaseVo CurrentSkillVo
        {
            get { return _currentSkillVo; }
        }

        public SysSkillActionVo CurrentActionVo
        {
            get { return _currentActionVo; }
        }

        private void InitSkillDelayDic()
        {
            _skillDelayDic[SkillConst.Sword_Attack1_ID] = 0.13f;
            _skillDelayDic[SkillConst.Sword_Attack2_ID] = 0.13f;
            _skillDelayDic[SkillConst.Sword_Attack3_ID] = 0.07f;
            _skillDelayDic[SkillConst.Sword_Attack4_ID] = 0.23f;
            _skillDelayDic[SkillConst.Magic_Attack1_ID] = 0.17f;
            _skillDelayDic[SkillConst.Magic_Attack2_ID] = 0.067f;
            _skillDelayDic[SkillConst.Magic_Attack3_ID] = 0.1f;
			_skillDelayDic[SkillConst.Assassin_Attack1_ID] = 0.17f;
			_skillDelayDic[SkillConst.Assassin_Attack2_ID] = 0.07f;
			_skillDelayDic[SkillConst.Assassin_Attack3_ID] = 0.03f;
			_skillDelayDic[SkillConst.Assassin_Attack4_ID] = 0.27f;

			for (uint skillId = SkillConst.Assassin_Skill2_ID_Min; skillId <= SkillConst.Assassin_Skill2_ID_Max; skillId++)
			{
				_skillSoundDic[skillId] = true;
			}

			for (uint skillId = SkillConst.Assassin_Skill3_ID_Min; skillId <= SkillConst.Assassin_Skill3_ID_Max; skillId++)
			{
				_skillSoundDic[skillId] = true;
			}

			for (uint skillId = SkillConst.Assassin_Skill4_ID_Min; skillId <= SkillConst.Assassin_Skill4_ID_Max; skillId++)
			{
				_skillSoundDic[skillId] = true;
			}
        }

		private bool IsBossAttack(int skillGroupId)
		{
			int[] bossGroupArr = {4000, 4021, 4022, 4121, 4122, 4151, 4152, 4171, 4172};

			for (int i=0; i<bossGroupArr.Length; i++)
			{
				if (skillGroupId == bossGroupArr[i])
				{
					return true;
				}
			}

			return false;
		}

        private float GetSkillDelay(uint skillId)
        {
            if (_skillDelayDic.ContainsKey(skillId))
            {
                return _skillDelayDic[skillId];
            }
            return 0f;
        }

        // Use this for initialization
        private void Start()
        {
            _selfTransform = transform;
            _hitSoundList = new List<string>();
            _attackSoundList = new List<string>();
            LeftCdTimes = new float[11];
            TotalCdTimes = new float[11];
            SkillMpCost = new int[11];
            SkillList = new List<int>();
            if (MeController.Me.Type == DisplayType.ROLE)
            {
                LearnedSkillList = new List<uint>();
                var playerVo = MeController.Me.GetMeVoByType<PlayerVo>();
                string[] skillIds = StringUtils.GetValueListFromString(playerVo.SysRoleBaseInfo.SkillList);
                foreach (string skill in skillIds)
                {
                    LearnedSkillList.Add(uint.Parse(skill)); //角色基础配置表读取角色的技能id
                }
            }
            else if (MeController.Me.Type == DisplayType.MONSTER)
            {
                LearnedSkillList = new List<uint>();
                var vo = MeController.Me.GetMeVoByType<MonsterVo>();
                if (vo != null)
                {
                    SysMonsterVo monsterVo = vo.MonsterVO;
                    string[] skillIds = StringUtils.GetValueListFromString(monsterVo.skill_ids);
                    foreach (string skill in skillIds)
                    {
                        LearnedSkillList.Add(uint.Parse(skill)); //从怪物表读取怪物的技能id
                    }
                }
            }
            else if (MeController.Me.Type == DisplayType.Trap)
            {
                LearnedSkillList = new List<uint>();
                var vo = MeController.Me.GetVo() as TrapVo;
                if (vo != null)
                {
                    SysTrap trapVo = vo.SysTrapVo;
                    string[] skillIds = StringUtils.GetValueListFromString(trapVo.SkillIds);
                    foreach (string skill in skillIds)
                    {
                        LearnedSkillList.Add(uint.Parse(skill)); //从陷阱表读取怪物的技能id
                    }
                }
            }
            else if (MeController.Me.Type == DisplayType.PET)
            {
                LearnedSkillList = new List<uint>();
                var vo = MeController.Me.GetVo() as PetVo;
                if (vo != null)
                {
                   /* SysPet sysPetVo = vo.SysPet;
                    string[] skillIds = StringUtils.GetValueListFromString(sysPetVo.unique_skill.ToString());
                    foreach (string skill in skillIds)
                    {
                        LearnedSkillList.Add(uint.Parse(skill)); //从宠物表读取怪物的技能id
                    }*/
                    LearnedSkillList.Add(vo.SkillId);
                }
            }
            InitTotalCdTime();
            InitSkillDelayDic();
        }

        public void InitTotalCdTime()
        {
            for (int i = 0; i < LearnedSkillList.Count; i++)
            {
                SysSkillBaseVo skillVo = BaseDataMgr.instance.GetSysSkillBaseVo(LearnedSkillList[i]);
                if (skillVo == null)
                {
                    continue;
                }
                TotalCdTimes[i] = skillVo.cd*0.001f;
                SkillMpCost[i] = skillVo.need_value;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //更新技能CD
            for (int i = 0; i < LeftCdTimes.Length; i++)
            {
                if (!(LeftCdTimes[i] > 0)) continue;
                LeftCdTimes[i] -= Time.deltaTime;
                if (LeftCdTimes[i] < 0)
                {
                    LeftCdTimes[i] = 0;
                }
            }
            if (MeController.GetMeVo().CurHp == 0)
            {
                SkillList.Clear();
            }
            //缓存的技能施放
            if (SkillList.Count > 0 &&
                (MeController.StatuController.CurrentStatu == Status.IDLE ||
                 MeController.StatuController.CurrentStatu == Status.RUN))
            {
                int skillType = SkillList[0];
                SkillList.RemoveAt(0);
                if (MeController.AttackController.AttackList.Count > 0)
                {
                    MeController.AttackController.AttackList.Clear();
                }
                RequestUseSkill(skillType);
            }
            //被抓取时, 执行操作;
            if (MeController.Me.IsGrasped)
            {
                if ((Global.Times_Left + Global.Times_Right) > PlayConst.GRASP_RELEASE_MIN_KEY_COUNT)
                {
                    MeController.GraspThrowController.InterceptGrasp();
                }
            }
        }

        /// <summary>
        ///     请求使用技能，但是请求不一定会被执行，还需要进行条件检验
        /// </summary>
        /// <param name="skillType">技能类型</param>
        public void RequestUseSkill(int skillType)
        {
            //判断抓取打断;
            MeController.JudgeGraspIntercept();
            //是否已经使用了技能;
            bool isRealUseSkill = false;
            if (LeftCdTimes[skillType] > 0 ||
                (MeController.GetMeVo().Id == MeVo.instance.Id && SkillMpCost[skillType] > MeController.GetMeVo().CurMp))
            {
                return; //CD状态不能使用技能
            }
            if (MeController.StatuController.CurrentStatu == Status.IDLE ||
                MeController.StatuController.CurrentStatu == Status.RUN)
            {
                isRealUseSkill = true;
                UseSkill(skillType); //待机状态或者跑动状态可使用技能
            }
            else if ((MeController.StatuController.CurrentStatu == Status.ATTACK1 && skillType == Attack2) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK2 && skillType == Attack2) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK2 && skillType == Attack3) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK3 && skillType == Attack3) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK3 && skillType == Attack4) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK5 && skillType == Attack5) ||
                     (MeController.StatuController.CurrentStatu == Status.ATTACK6 && skillType == Attack6) )
            {
                isRealUseSkill = true;
                UseSkill(skillType); //连击技能的触发
            }
            if (MeController.StatuController.CurrentStatu == Status.ATTACK1 ||
                MeController.StatuController.CurrentStatu == Status.ATTACK2
                || MeController.StatuController.CurrentStatu == Status.ATTACK3 ||
                MeController.StatuController.CurrentStatu == Status.ATTACK4 ||
                MeController.StatuController.CurrentStatu == Status.ATTACK5 ||
                MeController.StatuController.CurrentStatu == Status.ATTACK6 ||
                ((MeController.StatuController.CurrentStatu == Status.SKILL1 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL2 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL3 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL4 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL5 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL6 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL7 ||
                 MeController.StatuController.CurrentStatu == Status.SKILL8) &&
                 isRealUseSkill == false))
            {
                if (skillType >= Skill1)
                {
                    if(SkillList.Count > 0){
                        SkillList.RemoveAt(0);
                    }
                    SkillList.Add(skillType);
                }
                //if (SkillList.Count <= 1 && skillType >= Skill1)
                //{
                //    SkillList.Add(skillType); //只缓存一个技能
                //    Debug.Log("RequestUseSkill 测试进入");
                //}
            }
        }

        /// <summary>
        ///     使用怪物起身技能
        /// </summary>
        public void RequestUseMonsterStandUpSkill()
        {
            UseSkill(LearnedSkillList.Count - 1);
        }

        /// <summary>
        ///     使用技能，表明条件判断已经通过，可以执行使用技能
        /// </summary>
        /// <param name="skillType">技能类型</param>
        private void UseSkill(int skillType)
        {
			//清技能缓存音效
			ResetCacheSound();

            _checkedTime = 0;
            _currentSkillType = skillType;
            _currentSkillId = LearnedSkillList[skillType];
            if(_currentSkillId==0) return;
            UseSkillById(_currentSkillId);
            //启动技能CD
            for (int i = 0; i < LearnedSkillList.Count; i++)
            {
                if (i == skillType)
                {
                    LeftCdTimes[i] = TotalCdTimes[i];
                }
                else
                {
                    LeftCdTimes[i] = Mathf.Max(LeftCdTimes[i], _currentSkillVo.cd_public * 0.001f);
                }
            }
            if (_currentSkillVo.need_slow_speed)
            {
                //StartCoroutine(SlowSpeed(2));
            }
            if (_currentSkillVo.need_type == 1)
            {
                int leftMp = (int)MeController.GetMeVo().CurMp - _currentSkillVo.need_value;
                leftMp = leftMp > 0 ? leftMp : 0;
                MeController.GetMeVo().CurMp = (uint) leftMp;
            }

            if (MeController.GetMeVo().Id == MeVo.instance.Id || MeController.GetMeVo().Type != DisplayType.ROLE)
            {
                PlaySkillSound(_currentSkillVo, false);
            }

            if (MeController.GetMeVo().Id == MeVo.instance.Id)
            {
                MeVo.instance.DataUpdate(MeVo.DataHpMpUpdate);
                if (AppMap.Instance.mapParser.NeedSyn)
                {
                    //主角玩家使用技能通知服务器
                    SkillMode.Instance.SendHeroUseSkill(MeVo.instance.Id, (byte)SkillSynType.Player, _currentSkillId,
                        (byte) AppMap.Instance.me.CurDire);
                }

				if (skillType>=Skill1 && skillType<=Skill4)
				{
					int monsterNumber = AppMap.Instance.MonsterNumber;
					if (monsterNumber > 7)
					{
						if (SpeechMgr.Instance.IsMagicSpeech)
						{
							SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicUseSkillAndMonsterOver7);
						}
					}
				}

				PlayAssassinSkillSpeech(skillType);
            }

            if (_currentSkillVo.scale > 0 && MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
            {
                MapControl.Instance.MyCamera.ScaleCamera(_currentSkillVo.scale * 0.001f, 0.3f);
            }
        }

		private void PlayAssassinSkillSpeech(int skillType)
		{
			if (SpeechMgr.Instance.IsAssassinSpeech)
			{
				switch (skillType)
				{
				case Attack1:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinNormalAttack1);
					break;

				case Attack2:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinNormalAttack2);
					break;

				case Attack3:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinNormalAttack3);
					break;

				case Attack4:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinNormalAttack4);
					break;

				case Skill1:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinSkill1);
					break;

				case Skill2:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinSkill2);
					break;

				case Skill3:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinSkill3);
					break;

				case Skill4:
					SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinSkill4);
					break;

				default:
					break;
				}
			}
		}

        public void UseSkillById(uint skillId)
        {
            SysSkillBaseVo skillVo = BaseDataMgr.instance.GetSysSkillBaseVo(skillId);
            if (skillVo == null)
            {
                Log.debug(this, "SkillBase表不存在id为" + skillId + "的技能");
                return;
            }
            _currentSkillVo = skillVo;
            var actionVo = BaseDataMgr.instance.GetDataByTypeAndId<SysSkillActionVo>(BaseDataConst.SYS_SKILL_ACTION_VO,
                skillVo.skill_group);
            if (actionVo == null)
            {
                Log.debug(this, "Action表不存在id为" + skillVo.skill_group + "的技能组");
                return;
            }
            _currentActionVo = actionVo;
            MeController.StatuController.SetStatu(actionVo.action_id);
            ShowSkillEffect(actionVo, skillVo);
            if (skillVo.need_keep)
            {
                MeController.GetMeVo().NeedKeep = true;
            }
            if (MeController.Me.Type == DisplayType.PET)
            {
                MeController.TalentSkillController.UseTalentSkill();
            }
        }

        public void ScaleCamera()
        {
            if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
            {
                MapControl.Instance.MyCamera.ScaleCamera(0, 0.06f);
            }
        }

        /// <summary>
        ///     施放某些技能的时候世界速度变慢
        /// </summary>
        /// <param name="delayFrameNumber"></param>
        /// <returns></returns>
        private IEnumerator SlowSpeed(int delayFrameNumber)
        {
            for (int i = 0; i < delayFrameNumber; i++)
            {
                yield return 0;
            }
            for (int i = 0; i < 7; i++)
            {
                Time.timeScale -= 0.1f;
            }
            yield return 0;
        }

        /// <summary>
        ///     在回复TimeScale之前停止协程，防止TimeScale控制出现问题
        /// </summary>
        public void StopCoroutine()
        {
            StopCoroutine("SlowSpeed");
        }

        /// <summary>
        ///     显示技能对应的特效
        /// </summary>
        /// <param name="actionVo"></param>
        public void ShowSkillEffect(SysSkillActionVo actionVo, SysSkillBaseVo skillVo)
        {
            string[] effectIds = StringUtils.GetValueListFromString(actionVo.use_eff);
            if (effectIds.Length > 0&&!effectIds[0].Equals(""))
            {
                string effectId = effectIds[0];
                var effectVo = new Effect
                {
                    URL = UrlUtils.GetSkillEffectUrl(effectId),
                    Direction = MeController.Me.CurFaceDire,
                    BasePosition = MeController.transform.position,
                    Offset = Vector3.zero,
                    SkillController = this,
                    NeedCache = true
                };

                //使用者特效是否跟随主角
                if (actionVo.use_eff_need_follow)
                {
                    effectVo.Target = MeController.gameObject;
                }

                EffectMgr.Instance.CreateSkillEffect(effectVo);
            }



			//Boss预警特效
			if (IsBossAttack(actionVo.id))
			{
				EffectMgr.Instance.CreateMainFollowEffect(EffectId.Main_BossWarning, MeController.gameObject, Vector3.zero, false);
			}

            //显示子弹特效
            if (actionVo.IsBullet)
            {
                string[] effectList = StringUtils.SplitVoString(actionVo.process_eff, "],[");
                for (int i = 0; i < effectList.Length; i++)
                {
                    string[] effectInfo = StringUtils.GetValueListFromString(effectList[i]);
                    if (effectInfo.Length < 2)
                    {
                        continue;
                    }
                    float delayTime = int.Parse(effectInfo[1])*0.001f;
                    var effectBullet = new Effect
                    {
                        URL = UrlUtils.GetSkillEffectUrl(effectInfo[0]),
                        Id = uint.Parse(effectInfo[0]),
                        Direction = MeController.Me.CurFaceDire,
                        BasePosition = MeController.transform.position,
                        Offset = Vector3.zero,
                        Target = null,
                        Speed = actionVo.bullet_speed,
                        IsBullet = actionVo.IsBullet,
                        BulletType = actionVo.BulletType,
                        CheckInterval = actionVo.CheckInterval,
                        MaxTravelDistance = actionVo.BulletTravelDistance*0.001f,
                        SkillController = this,
                        DelayTime = delayTime,
                        EffectIndex = i,
                        NeedCache = true
                    };
                    EffectMgr.Instance.CreateSkillEffect(effectBullet);
                }

                //剑士技能3子弹需要播放音效
                if (skillVo.id >= SkillConst.Sword_Skill3_ID_Min && skillVo.id <= SkillConst.Sword_Skill3_ID_Max)
                {
                    SoundMgr.Instance.PlaySkillAudio(SoundId.Sound_SwordSkill3Bullet);
                }
            }
        }

        /// <summary>
        /// 施放技能时做带加速度的突进
        /// </summary>
        /// <param name="index"></param>
        public void SkillRush(int index)
        {
            if (!MeController.Me.IsUsing) return;
            SysSkillBaseVo skillVo = CurrentSkillVo;
            int[][] list = StringUtils.Get2DArrayStringToInt(skillVo.Rush_Data);
            if (list != null)
            {
                var meVo = MeController.GetMeVo();
                meVo.RushSpeed = list[index][Actions.RUSH_VELOCITY] * 0.001f;
                meVo.Acceleration = list[index][Actions.RUSH_ACCELERATION] * 0.001f;
                meVo.Distance = list[index][Actions.RUSH_DISTANCE] * 0.001f;
                meVo.Direction = list[index][Actions.RUSH_DIRECTION];
                meVo.IsRush = true;
            }
        }

        /// <summary>
        /// 施放技能时做匀速的突进
        /// </summary>
        /// <param name="index"></param>
        public void SkillMove(int index)
        {
            if (!MeController.Me.IsUsing) return;
            SysSkillBaseVo skillVo = CurrentSkillVo;
            int[][] list = StringUtils.Get2DArrayStringToInt(skillVo.Move_Data);
            if (list != null)
            {
                var meVo = MeController.GetMeVo();
                meVo.MoveDistance = list[index][Actions.MOVE_DISTANCE] * 0.001f;
                meVo.MoveTime = list[index][Actions.MOVE_TIME] * 0.001f;
                meVo.MoveDirection = list[index][Actions.MOVE_DIRECTION];
                meVo.MoveSpeed = meVo.MoveDistance / meVo.MoveTime;
                meVo.IsMoving = true;
            }
        }

        /// <summary>
        /// 施放技能时开始控制单位移动
        /// </summary>
        public void SkillStartCtrlMove()
        {
            print("SkillStartCtrlMove");
            if (!MeController.Me.IsUsing) return;
            SysSkillBaseVo skillVo = CurrentSkillVo;
            int[] list = StringUtils.GetStringToInt(skillVo.Move_During_Skilling);
            if (list.Length > 0)
            {
                var meVo = MeController.GetMeVo();
                meVo.MoveSpeedDuringSkill = list[Actions.CTRL_MOVE_SPEED] * 0.001f;
                meVo.CanCtrlMoveDuringSkill = true;
            }
        }

        /// <summary>
        /// 施放技能时停止控制单位移动
        /// </summary>
        public void SkillStopCtrlMove()
        {
            MeController.GetMeVo().CanCtrlMoveDuringSkill = false;
        }

        /// <summary>
        ///     攻击伤害检测
        /// </summary>
        public bool CheckDamage(Vector3 effectPosition, int index)
        {
            SysSkillBaseVo skillVo = CurrentSkillVo;
            print(skillVo.unikey);
            if (!MeController.Me.IsUsing) return false;
            bool result = false;
            switch (MeController.Me.Type)
            {
                case DisplayType.PET:
                    result = DamageCheck.Instance.CheckPetInjured2D(MeController, skillVo, effectPosition,
                        _selfTransform.position,
                        MeController.Me.CurFaceDire, GetEnemyDisplay(), _currentActionVo.IsBullet, _checkedTime, index,
                        AppMap.Instance.mapParser.NeedSyn);
                    break;
                case DisplayType.ROLE:
                    //主角攻击的伤害检测
                    bool needSyn = AppMap.Instance.mapParser.NeedSyn;
                    if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
                    {
                        result = DamageCheck.Instance.CheckMeInjured2D(MeController, skillVo, effectPosition,
                            _selfTransform.position,
                            MeController.Me.CurFaceDire, GetEnemyDisplay(), _currentActionVo.IsBullet, _checkedTime,
                            index, needSyn);
                        if (result)
                        {
                            CameraEffectManager.NormalAttackShake();
                            if (MeVo.instance.CurMp < MeVo.instance.Mp)
                            {
                                MeVo.instance.CurMp += 1;
                            }
                            PlayBeAttackedSound(skillVo.unikey); //播放受击音效
                        }
                    }
                    else
                    {
                        //其他玩家攻击的伤害检测
                        var playerVo = MeController.GetMeVoByType<PlayerVo>();
                        result = DamageCheck.Instance.CheckPlayerInjured2D(MeController, skillVo, playerVo, effectPosition,
                            _selfTransform.position,
                            MeController.Me.CurFaceDire, GetEnemyDisplay(), _currentActionVo.IsBullet, _checkedTime, index, needSyn);
                        if (!needSyn && result)
                        {
                            if (playerVo.CurMp < playerVo.Mp)
                            {
                                playerVo.CurMp += 1;
                            }
                        }
                    }
                    break;
                case DisplayType.MONSTER: //怪物攻击的碰撞检测
                    result = DamageCheck.Instance.MonsterCheckInjured2D(MeController, skillVo, effectPosition, _selfTransform.position,
                        MeController.Me.CurFaceDire, (MeController.Me.GetVo() as MonsterVo), index);
                    if (MeController.Me.DefenceEffect != null)
                    {
                        MeController.Me.DefenceEffect.transform.position = Vector3.one*1000;
                    }
                    break;
                case DisplayType.Trap:
                    DamageCheck.Instance.TrapCheckInjured2D(skillVo, effectPosition,
                        MeController.Me.CurFaceDire, (MeController.GetMeByType<TrapDisplay>().BoxCollider2D));
                    break;
            }
            if (skillVo.shake && MeController.GetMeVo().Id == MeVo.instance.Id)
            {
                CameraEffectManager.ShakeCamera(0, 0.6f); //增加技能震屏效果
            }
            if (result)
            {
                _checkedTime++;
            }
            return result;
        }

        private string GetRandSound(string sounds, IList<string> soundList)
        {
            string[] soundIds = StringUtils.GetValueListFromString(sounds);

            if (1 == soundIds.Length)
            {
                return soundIds[0];
            }
            if (0 == soundIds.Length)
            {
                return string.Empty;
            }

            if (0 == soundList.Count)
            {
                foreach (string item in soundIds)
                {
                    soundList.Add(item);
                }
            }

            int index = Random.Range(0, soundList.Count);
            string result = soundList[index];
            soundList.RemoveAt(index);

            return result;
        }

        private string GetRandHitSound(string sounds)
        {
            return GetRandSound(sounds, _hitSoundList);
        }

        private string GetRandAttackSound(string sounds)
        {
            return GetRandSound(sounds, _attackSoundList);
        }

        private string GetRandSound(string sounds)
        {
            string[] soundIds = StringUtils.GetValueListFromString(sounds);

            if (0 == soundIds.Length)
            {
                return string.Empty;
            }

            int index = Random.Range(0, soundIds.Length);
            string result = soundIds[index];

            return result;
        }

        /// <summary>
        ///     播放技能音效
        /// </summary>
        /// <param name="skillVo">技能</param>
        /// <param name="result">是否命中</param>
        private void PlaySkillSound(SysSkillBaseVo skillVo, bool result)
        {
            if (result)
            {
                if (StringUtils.IsValidConfigParam(skillVo.sound_hit))
                {
					SoundMgr.Instance.PlaySkillAudio(GetRandSound(skillVo.sound_hit));
                }
            }
            else
            {
                if (StringUtils.IsValidConfigParam(skillVo.sound_id))
                {
					SoundMgr.Instance.PlaySkillAudio(GetRandSound(skillVo.sound_id), GetSkillDelay((uint) skillVo.id));

                    //剑士普通攻击3
                    if (SkillConst.Sword_Attack3_ID == skillVo.id)
                    {
						SoundMgr.Instance.PlaySkillAudio(GetRandSound(skillVo.sound_id), 0.17f);
                    }
					//刺客普通攻击3
					else if (SkillConst.Assassin_Attack3_ID == skillVo.id)
					{
						SoundMgr.Instance.PlaySkillAudio(GetRandSound(skillVo.sound_id), 0.27f);
					}
                }
            }
        }

        //播放受击音效
        private void PlayBeAttackedSound(uint skillId)
        {
			if (_skillSoundDic.ContainsKey(skillId) && skillCacheId==skillId)
			{
				SoundMgr.Instance.PlaySkillAudio(skillCacheSound);
				return;
			}

            string sounds = GetSoundHitPathByAttack(skillId);
            string[] soundIds = StringUtils.GetValueListFromString(sounds);

            if (soundIds.Length < 1)
            {
                return;
            }

            int index = Random.Range(0, soundIds.Length);
            string soundId = soundIds[index];

            if (!StringUtils.IsValidConfigParam(soundId))
            {
                return;
            }

			if (_skillSoundDic.ContainsKey(skillId))
			{
				skillCacheId = skillId;
				skillCacheSound = soundId;
			}
			else
			{
				ResetCacheSound();
			}

			SoundMgr.Instance.PlaySkillAudio(soundId);
        }

        //获得击中目标音效
        private string GetSoundHitPathByAttack(uint skillId)
        {
            SysSkillBaseVo skillVo = BaseDataMgr.instance.GetSysSkillBaseVo(skillId);
            if (null == skillVo)
            {
                Log.info(this, "SkillBaseVo表不存在id为" + skillId + "的技能");
                return null;
            }
            return skillVo.sound_hit.ToString(CultureInfo.InvariantCulture);
        }

        public bool IsSkillCdReady(int skillType)
        {
            return LeftCdTimes[skillType] <= 0;
        }

        //获取敌人信息
        public IList<ActionDisplay> GetEnemyDisplay()
        {
            IList<ActionDisplay> tempList = AppMap.Instance.monsterList.Cast<ActionDisplay>().ToList();
            if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS)
            {
                if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id ||
                    MeController.GetMeVo().Type == DisplayType.PET)
                {
                    foreach (PlayerDisplay display in AppMap.Instance.playerList)
                    {
                        if (display != AppMap.Instance.me)
                        {
                            tempList.Add(display);
                        }
                    }
                }
                else
                {
                    tempList.Add(AppMap.Instance.me);
                }
            }
            return tempList;
        }

        public List<ActionDisplay> GetSkillCoveredEnemy(Vector3 pos)
        {
            return GetEnemyDisplayByRange(CurrentSkillVo.cover_width*0.001f, pos);
        }

        public List<ActionDisplay> GetEnemyDisplayByRange(float range, Vector3 pos)
        {
            var result = new List<ActionDisplay>();
            foreach (MonsterDisplay display in AppMap.Instance.monsterList)
            {
                float dis1 = display.Controller.transform.position.x - display.BoxCollider2D.size.x*0.5f - pos.x;
                float dis2 = display.Controller.transform.position.x + display.BoxCollider2D.size.x*0.5f - pos.x;
                if (Mathf.Abs(dis1) < range || Mathf.Abs(dis2) < range)
                {
                    result.Add(display);
                }
            }
            if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS)
            {
                if (MeController.GetMeVo().Id == AppMap.Instance.me.GetVo().Id)
                {
                    foreach (PlayerDisplay display in AppMap.Instance.playerList)
                    {
                        if (display.Controller == null) continue;
                        float dis1 = display.Controller.transform.position.x - display.BoxCollider2D.size.x * 0.5f - pos.x;
                        float dis2 = display.Controller.transform.position.x + display.BoxCollider2D.size.x * 0.5f - pos.x;
                        if (display != AppMap.Instance.me && (Mathf.Abs(dis1) < range || Mathf.Abs(dis2) < range))
                        {
                            result.Add(display);
                        }
                    }
                }
                else
                {
                    float dis1 = AppMap.Instance.me.Controller.transform.position.x -
                                 AppMap.Instance.me.BoxCollider2D.size.x * 0.5f - pos.x;
                    float dis2 = AppMap.Instance.me.Controller.transform.position.x +
                                 AppMap.Instance.me.BoxCollider2D.size.x * 0.5f - pos.x;
                    if (Mathf.Abs(dis1) < range || Mathf.Abs(dis2) < range)
                    {
                        result.Add(AppMap.Instance.me);
                    }
                }
            }
            return result;
        }
    }
}