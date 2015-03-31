using com.game;
using com.game.consts;
using com.game.manager;
using com.game.module.battle;
using Com.Game.Module.Boss;
using Com.Game.Module.Copy;
using com.game.module.effect;
using Com.Game.Module.GoldHit;
using com.game.module.main;
using com.game.module.map;
using Com.Game.Module.Role;
using Com.Game.Module.Story;
using com.game.module.test;
using com.game.sound;
using com.game.vo;
using com.u3d.bases.consts;
using Com.Game.Speech;
using UnityEngine;

/**主角状态控制器
 * @author 骆琦
 * @date   2013-11-14
 * 基于animator的主角状态控制器
 * 将主角的状态控制分离出来，便于处理一些主角的特殊情况
 * **/

namespace com.u3d.bases.controller
{
    public class MeStatuController : PlayerStatuController
    {
        //脚底灰尘特效对象
        private float _autoMoveTime;
        private GameObject _autoRoundEffect;
        private GameObject _bossFootEffect;

        private bool _footSmokeActive;
        private GameObject _footSmokeEffect;
        private bool _heroArriveStoryPlaying;
        private bool _isBattling;
        private float _roleHeiht = 1.5f;

        public GameObject AutoRoundEffect
        {
            get { return _autoRoundEffect; }
        }

        public GameObject BossFootEffect
        {
            get { return _bossFootEffect; }
        }


        private void Start()
        {
            PreStatuNameHash = Status.NAME_HASH_IDLE;
            _footSmokeEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_FootSmoke);
            _autoRoundEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_AutoSerachRoad);
            _bossFootEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_BossFootBuff);


            var boxCollider2D = MeControler.gameObject.GetComponentInChildren<BoxCollider2D>();
            if (null != boxCollider2D)
            {
                _roleHeiht = boxCollider2D.size.y;
            }
        }

        /// <summary>
        ///     主角状态改变时触发该方法
        /// </summary>
        protected override void DoStatuChange()
        {
            base.DoStatuChange();

            switch (CurrentStatu)
            {
                case Status.DEATH:
                    _footSmokeActive = false;
                    _footSmokeEffect.SetActive(false);
                    MapMode.Instance.RoleDeath(MeVo.instance.Id);
                    break;

                case Status.RUN:
                    _footSmokeEffect.transform.position = MeControler.transform.position;
                    _footSmokeEffect.transform.rotation = MeControler.transform.rotation;
                    _footSmokeEffect.SetActive(true);

                    if (!_footSmokeActive)
                    {
                        EffectMgr.Instance.ResetParticleSystem(_footSmokeEffect);
                        _footSmokeActive = true;
                    }

                    if (MapControl.CanPlayHeroArriveStory)
                    {
                        PlayHeroArriveStory();
                    }

                    break;

                case Status.Win:
                    CameraEffectManager.ScaleOutCamera(0.05f, MyCamera.MainCameraSize * 0.7f);
                    break;

                default:
                    _footSmokeActive = false;
                    _footSmokeEffect.SetActive(false);
                    break;
            }
        }

        private void PlayHeroArriveStory()
        {
            uint mapId = AppMap.Instance.mapParser.MapId;
            Vector3 pos = MeControler.transform.localPosition;

            if (Singleton<StoryControl>.Instance.PlayHeroArriveStory(mapId, pos, HeroArriveCallback))
            {
                _heroArriveStoryPlaying = true;
                MapControl.CanPlayHeroArriveStory = false;
                MapMode.InStory = true;
                AppMap.Instance.StopAllMonstersAi();
                Singleton<CopyMode>.Instance.PauseCopy();

                GameObject battleObj = Singleton<BattleView>.Instance.gameObject;
                _isBattling = (null != battleObj) && battleObj.activeSelf;
                if (_isBattling)
                {
                    Singleton<BattleView>.Instance.CloseView();
                }
                else
                {
                    Singleton<MainView>.Instance.CloseView();
                }
            }
			else
			{
				HeroArriveCallback();
			}
        }

        private void HeroArriveCallback()
        {
            if (_heroArriveStoryPlaying)
            {
                _heroArriveStoryPlaying = false;
                MapMode.InStory = false;
                Singleton<CopyMode>.Instance.ResumeCopy();
                AppMap.Instance.StartAllMonstersAi();

                if (_isBattling)
                {
                    Singleton<BattleView>.Instance.OpenView();
                }
                else
                {
                    Singleton<MainView>.Instance.OpenView();
                }
            }
        }

        public override bool SetStatu(int nextStatu)
        {
            bool result = base.SetStatu(nextStatu);
            if (result && NeedSyn(nextStatu) && AppMap.Instance.mapParser.NeedSyn)
            {
                RoleMode.Instance.SendStatuChange();
            }
            return result;
        }

        private bool NeedSyn(int statu)
        {
            return statu == Status.IDLE || statu == Status.RUN || statu == Status.DEATH;
        }

        protected override void DoStatuTransfered()
        {
            base.DoStatuTransfered();
            if (PreStatuNameHash == Status.NAME_HASH_DEATH)
            {
                if (AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS) //世界Boss 
                {
                    Singleton<BossTips>.Instance.OpenYDView(BossMode.Instance.BossName, 20, Singleton<RoleMode>.Instance.ReLife);
                }
                else if (AppMap.Instance.mapParser.MapId != MapTypeConst.ARENA_MAP
                         && AppMap.Instance.mapParser.MapId != MapTypeConst.GoldHit_MAP
                         && AppMap.Instance.mapParser.MapId != MapTypeConst.GoldSilverIsland_MAP) //普通副本地图时
                {
					if (SpeechMgr.Instance.IsAssassinSpeech)
					{
						SpeechMgr.Instance.PlaySpeech(SpeechConst.AssassinDead);
					}

                    Singleton<CopyControl>.Instance.OpenCopyFailView((int) CopyFailReason.DEATH);
                }
                else if (AppMap.Instance.mapParser.MapId == MapTypeConst.GoldHit_MAP)
                {
                    Singleton<GoldHitMainView>.Instance.OpenFailPannel();
                }
            }
            else if (PreStatuNameHash == Status.NAME_HASH_STANDUP)
            {
                //显示复活无敌特效
            }
            else if (PreStatuNameHash == Status.NAME_HASH_RUN)
            {
                AutoRoundEffect.SetActive(false);
            }
            else if (PreStatuNameHash == Status.NAME_HASH_Win)
            {
                CameraEffectManager.ScaleInCamera(0.05f, MyCamera.MainCameraSize);
            }
            
            _autoMoveTime = 0;
        }


        protected override void ProcessRunState(AnimatorStateInfo stateInfo)
        {
            base.ProcessRunState(stateInfo);
            _footSmokeEffect.transform.position = MeControler.transform.position;
            _footSmokeEffect.transform.rotation = MeControler.transform.rotation;
            _footSmokeEffect.SetActive(true);
            if (display.controler.MeControler.IsPressedMoveButton||NGUIJoystick.IsPressed)
            {
                _autoMoveTime = 0;
            }
            else
            {
                _autoMoveTime += Time.deltaTime;
            }
            if (_autoMoveTime > 1)
            {
                AutoRoundEffect.transform.position = MeControler.transform.position + new Vector3(0, _roleHeiht + 1f, 0);
                AutoRoundEffect.transform.rotation = MeControler.transform.rotation;

				if (AppMap.Instance.mapParser.MapVo.type == MapTypeConst.CITY_MAP)
				{
                	AutoRoundEffect.SetActive(true);
				}
            }
            else
            {
                AutoRoundEffect.SetActive(false);
            }
        }
    }
}