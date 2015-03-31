using System.Collections;
using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using UnityEngine;

/**玩家属性**/

namespace com.game.vo
{
    public class PlayerVo : BaseRoleVo
    {
        public uint DiamNeedForVigour; //下次购买体力需要消耗的钻石
        private uint _petId; //宠物配置ID，0为没有宠物，其他为当前出战的宠物的配置ID
        private SysRoleBaseInfoVo _SysRoleBaseInfoVo;
        private bool _isMpTimerStart;
        private float _leftUnbeatableTime;
        public string customFace = "";
        public ulong exp = 0;
        public ulong expFull = 0;
        public uint fightPoint = 0; //战斗力
        public byte hasCombine = 0;
        public byte nation = 0;
        public byte sex; //性别
        public List<uint> skills = new List<uint>();
        public List<uint> titleList = new List<uint>();
        public ushort vigour = 0; //当前体力值
        public ushort vigourFull = 0; //满的体力值
        public byte vip = 0;

        public SysRoleBaseInfoVo SysRoleBaseInfo
        {
            get
            {
                if (_SysRoleBaseInfoVo == null)
                {
                    _SysRoleBaseInfoVo = BaseDataMgr.instance.GetSysRoleBaseInfo(job);
                }
                return _SysRoleBaseInfoVo;
            }
        }

        public uint SeqAttack { get; set; }

        public float LeftUnbeatableTime
        {
            get { return _leftUnbeatableTime; }
            set
            {
                _leftUnbeatableTime = value;
                if (_leftUnbeatableTime > 0)
                {
                    IsUnbeatable = true;
                    CoroutineManager.StartCoroutine(UpdateLeftUnbeatableTime());
                    DoLeftUnbeatableTimeChanged();
                }
            }
        }

        public uint PetId
        {
            get { return _petId; }
            set
            {
                _petId = value;
                UpdatePetDisplay();
            }
        }

        public void UpdatePetDisplay()
        {
            if(Controller == null) return;
            if (_petId == 0)
            {
                var petDisplay = Controller.GetMeByType<PlayerDisplay>().PetDisplay;
                if (petDisplay != null)
                {
                     Object.Destroy(petDisplay.GoBase);
                     Controller.GetMeByType<PlayerDisplay>().PetDisplay = null;
                }
            }
            else
            {
                CreateMyPet();
            }
        }

        public void CreateMyPet()
        {
            var petDisplay = Controller.GetMeByType<PlayerDisplay>().PetDisplay;
            if (petDisplay != null)
            {
                Object.Destroy(petDisplay.GoBase);
                Controller.GetMeByType<PlayerDisplay>().PetDisplay = null;
            }
            if (MeVo.instance.mapId == MapTypeConst.ARENA_MAP)
            {
                return;
            }
            var meDisplay = Controller.GetMeByType<PlayerDisplay>();
            var petVo = GetFightPetVo();
            if(petVo==null) return;
            var target = Controller.transform.position;
            petVo.X = target.x - 0.5f;
            petVo.Y = target.y;
            petVo.ModelLoadCallBack = LoadPetCallback;
            petVo.MasterVo = this;
            petVo.MasterDisplay = Controller.GetMeByType<PlayerDisplay>();
            meDisplay.PetDisplay = AppMap.Instance.CreatePet(petVo);
        }

        protected virtual PetVo GetFightPetVo()
        {
            var petVo = new PetVo();
            petVo.petId = PetId;
            return petVo;
        }

        protected virtual void LoadPetCallback(BaseDisplay petDisplay)
        {
            petDisplay.Controller.AiController.SetAi(false);
        }

        private IEnumerator UpdateLeftUnbeatableTime()
        {
            while (_leftUnbeatableTime > 0)
            {
                _leftUnbeatableTime -= Time.deltaTime;
                yield return 0;
            }
            _leftUnbeatableTime = 0;
            IsUnbeatable = false;
            DoLeftUnbeatableTimeChanged();
            yield return 0;
        }

        private void DoLeftUnbeatableTimeChanged()
        {
            if (Controller == null) return;
            if (IsUnbeatable)
            {
                Controller.GetMeByType<PlayerDisplay>().ShowInvincibleEffect();
            }
            else
            {
                Controller.GetMeByType<PlayerDisplay>().RemoveInvincibleEffect();
            }
        }
    }
}