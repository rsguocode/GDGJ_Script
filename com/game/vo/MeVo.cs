using System;
using Com.Game.Module.Pet;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using UnityEngine;
using com.game.utils;
using com.u3d.bases.debug;
using com.game.consts;
using Object = UnityEngine.Object;

/**本角色属性**/

namespace com.game.vo
{
    //主角数据更新代理
    public delegate void MeDataUpateHandler(object sender, int code);

    public delegate void DataUpateHandlerWithParam(object sender, int code, object param);

    public class MeVo : PlayerVo
    {
        public static MeVo instance = new MeVo();
        public static readonly int DataHpMpUpdate = 1; //主角血和魔法更新
        public static readonly int DataHpUpdate = 2; //主角血更新
        public static readonly int DataHurtOverPercent10Update = 3; //一次受击伤害超过最大生命值的10%

        public static readonly int MonsterDataHpUpdateWithParam = 1; //怪物血更新
		
        public uint guildId = 0; //公会Id
        public string guildName = ""; //公会名字

        private DataUpateHandlerWithParam handlListWithParam;
        public uint lastLoginTime;
        public uint preMapId;
        public uint repu; //声望
        public uint serverId;
		//货币类型
		public uint diamond; //钻石
		public uint bindingDiamond; //绑钻：绑定钻石
        public uint diam; //钻石

		public string BustUrl
		{
			get
			{			
				switch (job)
				{
				case 1:
					return UrlUtils.npcBustUrl("100001");
				case 2:
					return UrlUtils.npcBustUrl("100002");
				case 3:
					return UrlUtils.npcBustUrl("100003");
				default:
					return UrlUtils.npcBustUrl("100003");
				}
			}
		}

		public void InitRoleBustPosition(GameObject roleBust)
		{
			if (GameConst.JOB_FASHI == MeVo.instance.job)
			{
				Vector3 localPos = roleBust.transform.localPosition;
				localPos.x = GameConst.MagicBustX;
				roleBust.transform.localPosition = localPos;
			}
		}

        public override uint CurHp
        {
            get { return base.CurHp; }
            set
            {
                if (value != _curHp)
                {
                    if (value < _curHp)
                    {
                        if ((_curHp - value) / (float)Hp > 0.1)
                        {
                            DataUpdate(DataHurtOverPercent10Update);
                        }
                    }
                    base.CurHp = value;
                    DataUpdate(DataHpUpdate);
                }
            }
        }

        protected override PetVo GetFightPetVo()
        {
            var petVo = PetMode.Instance.GetFightPetVo();
            return petVo;
        }

        protected override void LoadPetCallback(BaseDisplay petDisplay)
        {
            if (AppMap.Instance.mapParser.MapVo.type == MapTypeConst.COPY_MAP)
            {
                petDisplay.Controller.AiController.SetAi(true);
            }
            else
            {
                petDisplay.Controller.AiController.SetAi(false);
            }
        }

        //钻石格式字符串
		public string DiamondStr
		{
			get { return StringUtils.formatCurrency((int) diamond); }
		}

		//绑钻：绑定钻石
		public string BindingDiamondStr
		{
			get { return StringUtils.formatCurrency((int) bindingDiamond); }
		}
		
        //钻石
        public string DiamStr
        {
			get { return StringUtils.formatCurrency((int) diam); }
		}

        public event DataUpateHandlerWithParam DataUpdatedWithParam
        {
            add
            {
                //先移除，然后添加防止多次添加，有其他的实现 GetInvokeList().Contains();但是应该要using Linq，为了少用高级特效，现在就先这样实现
                handlListWithParam -= value;
                handlListWithParam += value;
            }

            remove { handlListWithParam -= value; }
        }


        //=================================================================================
        //数据更新处理
        public event MeDataUpateHandler DataUpdated;

        //数据更新必须调用的方法，code为业务协议号码
        public void DataUpdate(int code = 0)
        {
            //线程安全处理
            MeDataUpateHandler localUpdated = DataUpdated;
            if (localUpdated != null)
            {
                foreach (MeDataUpateHandler handler in localUpdated.GetInvocationList())
                {
                    /*try
                    {*/
                        //事件处理
                        handler(this, code);
                    /*}
                    catch (Exception e)
                    {
                        Log.error(this, e.Message);
                    }*/
                }
            }
        }

        public void DataUpdateWithParam(int code = 0, object param = null)
        {
            //线程安全处理
            DataUpateHandlerWithParam localUpdatedWithParam = handlListWithParam;

            if (localUpdatedWithParam != null)
            {
                foreach (DataUpateHandlerWithParam handler in localUpdatedWithParam.GetInvocationList())
                {
                    //事件处理
                    handler(this, code, param);
                }
            }
        }
    }
}