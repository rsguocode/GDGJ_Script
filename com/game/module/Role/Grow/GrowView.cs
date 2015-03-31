using System.Collections.Generic;
using com.game.consts;
using com.game.manager;
using com.game.module.test;
using Com.Game.Module.VIP;
using com.game.Public.Confirm;
using com.game.vo;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;
using com.game.Public.Message;


//255,199,15  255,227,0
namespace Com.Game.Module.Role
{
    public class GrowView : BaseView<GrowView>
    {

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

        private List<UIWidgetContainer> leftItem = new List<UIWidgetContainer>();
        private List<UIWidgetContainer> costList = new List<UIWidgetContainer>();
        private List<UILabel> cdList = new List<UILabel>();
        private List<ItemContainer> buttonList = new List<ItemContainer>();

        private UILabel leftLabel;//剩余次数
        public ItemContainer GuideButton;

        
        protected override void Init()
        {
            leftLabel = FindInChild<UILabel>("right/lefttime");
            ItemContainer button;
            for (int i = 1; i < 4; i++)
            {
                cdList.Add(FindInChild<UILabel>("right/item"+ i+ "/cd"));
                costList.Add(FindChild("right/item" + i + "/cost").AddComponent<UIWidgetContainer>());
                button = FindChild("right/item" + i + "/btn_fs").AddComponent<ItemContainer>();
                button.onClick = GrowOnClick;
                buttonList.Add(button);
            }
            for (int i = 1; i < 5; i++)
            {
                leftItem.Add(FindInChild<UIWidgetContainer>("left/item"+ i));
            }
            GuideButton = FindChild("right/item" + 1 + "/btn_fs").GetComponent<ItemContainer>();
            //初始化标签
            InitLabel();
        }

        private void InitLabel()
        {
        }

        protected override void HandleAfterOpenView()
        {
            GrowMode.Instance.GrowInfoApply();
            UIWidgetContainer wc;
            for (int i = 0; i < 4; i++)   //清空加成属性
            {
                wc = leftItem[i];
                wc.FindInChild<UILabel>("value").text = string.Empty;
            }
        }
        
        public override void RegisterUpdateHandler()
        {
            Singleton<GrowMode>.Instance.dataUpdated += UpdateGrowHandle;
            VIPMode.Instance.dataUpdated += UpdateVipHandle;
        }

        private void UpdateVipHandle(object sender,int code)
        {
            if (code == VIPMode.Instance.UPDATE_VIP_INFO)
                UpdateVipGrowInfo();

        }

        private void UpdateVipGrowInfo()
        {
            //leftLabel.text = leftLabel.text = string.Format(LanguageManager.GetWord("Gorw.LeftTimes"), GrowMode.Instance.LeftTimes);
        }
        public override void CancelUpdateHandler()
        {
            Singleton<GrowMode>.Instance.dataUpdated -= UpdateGrowHandle;
            VIPMode.Instance.dataUpdated -= UpdateVipHandle;
        }
        // 0 表示未开启， 1 表示CD没到，但有付费次数， 2 表示CD没到（有免费次数）， 3 表示没有免费次数
        // 4 表示
        // 0 表示未开启， 1 表示CD没到， 2表示CD到了， 3表示剩余免费次数为0，4表示剩余付费次数为0
        private void UpdateCDTime()
        {
            GrowattrInfoMsg_17_1 growInfo = GrowMode.Instance.GrowAttrInfo;

            List<uint> CDList = new List<uint>();
            List<uint> FreeTimes = new List<uint>();
            List<uint> CostTimes = new List<uint>();
            //方便迭代
            CDList.Add(growInfo.lv1Cd);    //CD时间
            CDList.Add(growInfo.lv2Cd);
            CDList.Add(growInfo.lv3Cd);
            FreeTimes.Add(growInfo.lv1Free);   //剩余免费次数
            FreeTimes.Add(growInfo.lv2Free);
            FreeTimes.Add(growInfo.lv3Free);
            int free;//可免费次数
            int total;//总次数
            
            string cd1format = LanguageManager.GetWord("Grow.CD1");
            string cd2format = LanguageManager.GetWord("Grow.CD2");
            cd1format = cd1format.Replace(@"\n", "\n");
            cd2format = cd2format.Replace(@"\n", "\n");
            string cdformat = null;
            string freeString = LanguageManager.GetWord("Grow.Free");
            string unOpen = LanguageManager.GetWord("Grow.UnOpen");
            UILabel cdLabel;
            int[] freeCount = {5, 2, 1};  //三种培育的免费次数
            int[] vipRequire = {0, 4, 6};  //vip开启
            UIWidgetContainer wc;
            UILabel valueLabel;//花费文本
            ItemContainer ic;
            int leftTime;//剩余付费次数
            int leftFree;//剩余免费次数
            for (int i = 0; i < 3; i++)
            {

                //0 表示未开启， 
                //1 表示CD没到，但有付费次数， 2 表示CD没到（有免费次数）没有付费次数， 3 表示CD时间到，有免费次数， 
                //4 表示没有免费次数，有付费次数 5 表示没有免费次数，没有付费次数 
                int state = GrowMode.Instance.GetGrowState(i + 1);

                wc = costList[i];  //花费
                ic = buttonList[i];  //按钮
                cdLabel = cdList[i];  //CD 文本
                valueLabel = wc.FindInChild<UILabel>("value");  
                free = (int)FreeTimes[i];
                leftFree = freeCount[i] - free;

                int buttonState = 0;  //按钮状态
                int costState = 0;  //钻石信息显示状态
                int cdState = 0;   //cd显示状态
                
                if (state == 0) //没有开启
                {
                    //Log.info(this, "没有开启");
                    cdState = 1;
                    buttonState = 1;
                    costState = 1;
                }
                else if (state == 5 )
                {
                    //Log.info(this, "免费次数和付费次数都用完了");
                    cdState = 4;
                    buttonState = 2;
                    costState = 2;
                }
                else if (state == 4)
                {
                    //Log.info(this, "没有免费次数，有付费次数");
                    cdState = 4;
                    buttonState = 2;
                    costState = 2;
                }
                else if (state == 3)   //免费
                {
                    //Log.info(this, "表示CD时间到，有免费次数");
                    cdState = 3;
                    buttonState = 2;
                    costState = 1;
                }
                else if (state == 2)  
                {
                    cdState = 2;
                    buttonState = 2;
                    costState = 2;
                }
                else if (state == 1 )   //付费
                {
                    //Log.info(this, "表示CD没到，但有付费次数");
                    cdState = 2;
                    buttonState = 2;
                    costState = 2;
                }
                if (cdState == 1) // 未开启 
                {
                    cdLabel.text = string.Format(LanguageManager.GetWord("Grow.VIPOpen").Replace(@"\n", "\n"),
                        vipRequire[i]);
                    cdLabel.color = Color.yellow;
                }
                else if (cdState == 2)  //cd 时间未到
                {
                    vp_TimeUtility.Units units = vp_TimeUtility.TimeToUnits(CDList[i]);
                    string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", units.hours, units.minutes, units.seconds);
                    cdLabel.text = string.Format(cd1format, timeStr,string.Format(ColorConst.GREEN_FORMAT,free + "/" + freeCount[i]));
                    //cdLabel.color = Color.white;
                }
                else if (cdState == 3)  //CD时间到
                {
                    cdLabel.text = string.Format(cd2format, string.Format(ColorConst.GREEN_FORMAT, free + "/" + freeCount[i]));
                    //cdLabel.color = Color.white;
                }
                else if (cdState == 4) //免费用完
                {
                    cdLabel.text = string.Format(cd2format, string.Format(ColorConst.RED_FORMAT, free + "/" + freeCount[i]));
                    //cdLabel.color = Color.red;
                }
                if (buttonState == 1) //未开启
                {
                    ic.FindInChild<UISprite>("background").spriteName = "anniuh1";
                    ic.FindInChild<UILabel>("label").effectColor = ColorConst.YellowOutline;
                    ic.buttonType = Button.ButtonType.None;
                    ic.SetHighLightState(false);
                }
                else if (buttonState == 2) //开启可点击
                {
                    ic.FindInChild<UISprite>("background").spriteName = "anniulv";
                    ic.FindInChild<UILabel>("label").effectColor = ColorConst.BlueOutline;
                    ic.buttonType = Button.ButtonType.Toggle;
                }
                else if (buttonState == 3) //开启不可点击
                {
                    ic.FindInChild<UISprite>("background").spriteName = "anniulv";
                    ic.buttonType = Button.ButtonType.None;
                    ic.SetHighLightState(false);
                }
                if (costState == 2)  //显示
                {
                    wc.SetActive(true);
                    ic.FindInChild<UILabel>("label").cachedTransform.localPosition = new Vector3(6.3f, 19f, 0f);
                    valueLabel.text = string.Empty + GrowMode.Instance.GrowCost[i];
                }
                else if (costState == 1)  //不显示
                {
                    wc.SetActive(false);
                    ic.FindInChild<UILabel>("label").cachedTransform.localPosition = new Vector3(6.3f, -4.0f, 0f);
                }
                ic.Id = (uint)state;
            }
        }

        private void UpdateAddAttrInfo()
        {
            UIWidgetContainer wc;
            
            leftLabel.text = string.Format(LanguageManager.GetWord("Gorw.LeftTimes"),GrowMode.Instance.LeftTimes);
            List<uint> attrList = GrowMode.Instance.GrowAttrInfo.attr;
            List<sbyte> addList = GrowMode.Instance.AddList;

            for (int i = 0; i < 4; i++)
            {
                wc = leftItem[i];
                wc.FindInChild<UILabel>("label").text = LanguageManager.GetWord("Equip.Attr" + (i + 1)) + ": "
                                                        + attrList[i] + "/" + MeVo.instance.Level * 10;
                if (addList.Count < 4)
                    wc.FindInChild<UILabel>("value").text = string.Empty;
                else
                    wc.FindInChild<UILabel>("value").text = "+" + addList[i];
            }
        }

        private void UpdateGrowInfo()
        {
            UpdateCDTime();
            UpdateAddAttrInfo();
        }
        //更新培育的主面板
        private void UpdateGrowHandle(object sender, int type)
        {

            if (type == Singleton<GrowMode>.Instance.UPDATE_PANNEL_INFO)
            {
                //面板打开后要确定各个培养是否开启
                UpdateGrowInfo();
            }
            else if (type == GrowMode.Instance.UPDATE_CD_TIME)
            {
                UpdateCDTime();
            }
            else if(type == GrowMode.Instance.UPDATE_SHUXING)
            {
                UpdateAddAttrInfo();
            }
        }

        private int type;

        private void GrowBtnClick()
        {
            GrowControl.Instance.ApplyGrow(type);
        }
        private void GrowOnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            type = 0;
            for (int i = 1; i < 4; i++)
            {
                if (buttonList[i - 1].Equals(ic))
                {
                    type = i;
                    break;
                }
            }
            uint state = ic.Id;
            if (state == 5 )
            {
                Log.info(this, "免费次数和付费次数都用完了");
                MessageManager.Show("培育次数已用完");
            }
            else if (state == 4 || state  == 1 )
            {
                Log.info(this, "没有免费次数，有付费次数 state " + state);
                string formatStr = LanguageManager.GetWord("Grow.CostTips1");
                if(type != 1)
                    formatStr = LanguageManager.GetWord("Grow.CostTips2");
                ConfirmMgr.Instance.ShowSelectOneAlert(string.Format(formatStr,GrowMode.Instance.GrowCost[type-1]), ConfirmCommands.SELECT_ONE, GrowBtnClick, "确定", null,
                    "取消");
            }
            else if (state == 3)   //免费
            {
                Log.info(this, "表示CD时间到，有免费次数");
                GrowBtnClick();
            }
            else if (state == 2)  
            {
                Log.info(this, "表示CD没到（有免费次数）没有付费次数");
                MessageManager.Show("CD冷却时间没有到");
            }
            
        }
    
    }
}
