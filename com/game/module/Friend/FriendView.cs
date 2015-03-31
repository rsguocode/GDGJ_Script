
﻿﻿﻿using com.game.consts;
﻿﻿﻿using com.game.manager;
﻿﻿﻿using Com.Game.Module.Chat;
﻿﻿﻿using Com.Game.Module.Role;
﻿﻿﻿using com.game.Public.Message;
﻿﻿﻿using com.u3d.bases.debug;
﻿﻿﻿using PCustomDataType;
﻿﻿﻿using Proto;
﻿﻿﻿using UnityEngine;
using com.game.module.test;
using System.Collections.Generic;
using Com.Game.Module.Tips;

namespace Com.Game.Module.Friend
{
	/// <summary>
	/// 好友视图
	/// </summary>
    public class FriendView : BaseView<FriendView>
	{   
		/// <summary>
		/// 预设路径
		/// </summary>
        public override string url{get {return "UI/Friend/FriendView.assetbundle";}}

	    public override ViewLayer layerType
	    {
            get { return ViewLayer.MiddleLayer; }
	    }

	    public override bool IsFullUI
	    {
	        get { return true; }
	    }

        public override bool isDestroy
        {
            get { return true; }
        }


	    private List<GameObject> roleList;
	    private int roleInfoNum = 0; //信息条数

	    private GameObject role;
	    private UIGrid onCenterGrid;
	    private GameObject container;

        private UIToggle ask;
        private UIToggle blackList;
        private UIToggle friend;
        private UIToggle nearby;

        private UIToggle currentUIToggle;

	    private GameObject currentRole;

	    private GameObject title;

	    private GameObject funcView;//功能按钮
	    private GameObject addView;

	    private UILabel inputName;
	    private UIInput input;

        private readonly Vector3 scale = new Vector3(1,1,1);

	    //private readonly int[] funPosx5 = {-210, -70, 70, 210, 210};
        private readonly int[] funPosx5 = { 208, -210, -70, 70, 70 };
        //private readonly int[] funPosx4 = { -210, -70, 70, 210};
        private readonly int[] funPosx4 = { 208, -210, -70, 70 };

	    private readonly int gapX = 18;

		protected override  void Init()
		{
		    roleList = new List<GameObject>(); //对象缓存;
   
		    title = FindChild("body/title");
            container = FindChild("body/container");
            onCenterGrid = FindInChild<UIGrid>("body/container/oncenter");
		    FindInChild<Button>("button_close").onClick = CloseClick;
		    role = FindChild("body/container/oncenter/role");
            role.SetActive(false);
            funcView = FindChild("function");
		    FindInChild<Button>("function/btn_close").onClick = FuncCloseClick;

		    addView = FindChild("add");
            FindInChild<Button>("add/btn_close").onClick = AddCloseClick;
            FindInChild<Button>("add/button").onClick = AddFriendByNameClick;
            inputName = FindInChild<UILabel>("add/input/name");
            input = FindInChild<UIInput>("add/input");

            FindInChild<Button>("body/container/oncenter/role/handle/reject").onClick = RejectClick;
            FindInChild<Button>("body/container/oncenter/role/handle/accept").onClick = AcceptClick;

		    FindInChild<Button>("button").onClick = AddClick;

            ask = FindInChild<UIToggle>("head/ask");
            
            blackList = FindInChild<UIToggle>("head/blacklist");
            
            friend = FindInChild<UIToggle>("head/friend");
            currentUIToggle = friend;
            nearby = FindInChild<UIToggle>("head/nearby");

		    initLabel();
		}

	    private void initLabel()
	    {
            FindInChild<UILabel>("head/ask/label").text = LanguageManager.GetWord("FriendView.Ask");

            FindInChild<UILabel>("head/friend/label").text = LanguageManager.GetWord("FriendView.Friend");
            FindInChild<UILabel>("head/blacklist/label").text = LanguageManager.GetWord("FriendView.BlackList");
            FindInChild<UILabel>("head/nearby/label").text = LanguageManager.GetWord("FriendView.Nearby");

            FindInChild<UILabel>("body/title/name").text = LanguageManager.GetWord("FriendView.Name");
            FindInChild<UILabel>("body/title/time").text = LanguageManager.GetWord("FriendView.Time");
            FindInChild<UILabel>("body/title/job").text = LanguageManager.GetWord("FriendView.Job");
            FindInChild<UILabel>("body/title/lvl").text = LanguageManager.GetWord("FriendView.LVL");
            FindInChild<UILabel>("body/title/fight").text = LanguageManager.GetWord("FriendView.Fight");
            FindInChild<UILabel>("body/title/handle").text = LanguageManager.GetWord("FriendView.Handle");

            FindInChild<UILabel>("num").text = LanguageManager.GetWord("FriendView.FriendNum");
            NGUITools.FindInChild<UILabel>(role, "handle/accept/label").text = LanguageManager.GetWord("FriendView.Accept");
            NGUITools.FindInChild<UILabel>(role, "handle/reject/label").text = LanguageManager.GetWord("FriendView.Reject");
            inputName.text = LanguageManager.GetWord("FriendView.InputName");

	    }



	    private GameObject  AddRoleObjet()
	    {
	        if (roleList.Count == 0)
            {
                role.name = "role" + (1000+roleList.Count + 1) + '_';
	            roleList.Add(role);
                role.GetComponent<Button>().onClick = RoleInfoClick;
                NGUITools.FindInChild<Button>(role,"button").onClick = OnHandleClick;
	            return role;
	        }
	        else
	        {
                GameObject roleInfo = (GameObject)GameObject.Instantiate(roleList[0]);
                roleInfo.name = "role" + (1000+roleList.Count+1)+"_";
	            roleInfo.transform.parent = onCenterGrid.transform;
	            roleInfo.transform.localScale = scale;
                roleInfo.GetComponent<Button>().onClick = RoleInfoClick;
                NGUITools.FindInChild<Button>(roleInfo, "button").onClick = OnHandleClick;
                roleList.Add(roleInfo);
                return roleInfo;
	        }
	        
	    }

	    private void OnHandleClick(GameObject obj)
	    {
	        if (currentUIToggle == blackList)
	        {

                Singleton<FriendControl>.Instance.SendDeleteBlackList(getRoleId(obj.transform.parent.gameObject));

            }
            else if (currentUIToggle == nearby)
            {
                Singleton<FriendControl>.Instance.SendFriendAskInfo(getRoleId(obj.transform.parent.gameObject));
            }
            else if(currentUIToggle = friend)
            {
                string name = NGUITools.FindInChild<UILabel>(obj.transform.parent.gameObject, "name").text;
                Singleton<ChatView>.Instance.OpenPrivateChatView(name, getRoleId(obj.transform.parent.gameObject));
            }
	    }

	    protected override void HandleAfterOpenView()
	    {
            nearby.onStateChange = OnToggleChange;
            ask.onStateChange = OnToggleChange;
            blackList.onStateChange = OnToggleChange;
            friend.onStateChange = OnToggleChange;

            List<RelationPushAcceptMsg_7_3> askList = Singleton<FriendMode>.Instance.askList;
            if (askList.Count > 0)
            {
                if (currentUIToggle != ask)
                {
                    ask.value = true;
                }
                else
                {
                    SetAskListInfo();
                }
                return;
            }

            if (currentUIToggle == friend)
            {
                SetFriendInfo(false);
            }

            if (currentUIToggle == nearby)
            {
                SetNearbyInfo(false);
            }

            if (currentUIToggle == ask)
            {
                SetAskListInfo();
            }
	        if (currentUIToggle == blackList)
	        {
	            SetBlackListInfo();
	        }

	    }


        //注册数据更新器
        public override void RegisterUpdateHandler()
        {
            Singleton<FriendMode>.Instance.dataUpdated += RoleDataUpdated;
        }


        //数据更新响应
        public void RoleDataUpdated(object sender, int code)
        {
            if (code == FriendMode.NEARBY && currentUIToggle == nearby)
            {
                SetNearbyInfo(true);
            }

            if (code == FriendMode.ASKLIST)
            {
                if (currentUIToggle == ask)
                {
                    SetAskListInfo();
                    NGUITools.FindChild(ask.gameObject, "tips").SetActive(false);
                }
                else
                {
                    NGUITools.FindChild(ask.gameObject, "tips").SetActive(FriendMode.Instance.ShowTips);
                }
            }
            if (code == FriendMode.BLACKLIST && currentUIToggle == blackList)
            {
                SetBlackListInfo();
            }

            if (code == FriendMode.FRIEND && currentUIToggle == friend)
            {
                SetFriendInfo(true);
            }

            if (code == FriendMode.FRIENDNUM)
            {
                SetFriendNumInfo();
            }
        }


        //取消数据更新器
        public override void CancelUpdateHandler()
        {
            Singleton<FriendMode>.Instance.dataUpdated -= RoleDataUpdated;
        }

	    private void CloseClick(GameObject go)
	    {
            CloseView();
	    }

        private void FuncCloseClick(GameObject go)
        {
            funcView.SetActive(false);
            if(currentRole!=null && currentRole.activeSelf)
            {
                SetRoleClickEffect(currentRole, "fgx5656", -1);
            }
            currentRole = null;
        }


        private void AddCloseClick(GameObject go)
        {
            addView.SetActive(false);
        }

        private void RoleViewCloseClick(GameObject go)
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 标签页改动响应
        /// </summary>
        /// <param name="state"></param>
	    private void OnToggleChange(bool state)
	    {
	        if (UIToggle.current == nearby)
	        {
	            if (state)
	            {
                    currentUIToggle = nearby;
	                SetNearbyInfo(false);
	            }

	        }
            else if (UIToggle.current == ask)
            {
                
	            if (state)
	            {
                    currentUIToggle = ask;
                    SetAskListInfo();
	            }
	        }
            else if (UIToggle.current == blackList)
            {
                
                if (state)
                {
                    currentUIToggle = blackList;
                    SetBlackListInfo();
                }
            }
            else if (UIToggle.current == friend)
            {
                if (state)
                {
                    currentUIToggle = friend;
                    SetFriendInfo(false);
                }
            }

            if (UIToggle.current != ask)
            {
                NGUITools.FindChild(ask.gameObject, "tips").SetActive(FriendMode.Instance.ShowTips);
            }
            else
            {
                NGUITools.FindChild(ask.gameObject, "tips").SetActive(false);
            }

	    }
	    private void SetFriendNumInfo()
	    {
	        int num = Singleton<FriendMode>.Instance.friendNum;
	        int maxNum = Singleton<FriendMode>.Instance.maxNum;
	        FindInChild<UILabel>("numvalue").text = num.ToString();
	        FindInChild<UILabel>("numvalue").color = ColorConst.FONT_YELLOW;
            FindInChild<UILabel>("numvalue2").text = "/" + maxNum;

	    }

        /// <summary>
        /// 设置附近玩家信息
        /// </summary>
        /// <param name="dataUpdate">是否是数据更新</param>
	    private void SetNearbyInfo(bool dataUpdate)
	    {
	        ReSetRoleListActive();
	        SetNearByTitle(title);
            SetNearbyFunctionButtons();
            if (!dataUpdate)
	        {
	            Singleton<FriendControl>.Instance.SendRequestForNearByInfo();
	        }
	        else
	        {
	            List<PRelationNear> nearList =   Singleton<FriendMode>.Instance.nearByList;
                ShowAllPRelationNearInfo(nearList);
	        }
            onCenterGrid.repositionNow = true;
	    }

        private void SetFriendInfo(bool dataUpdate)
        {
            ReSetRoleListActive();
            SetFriendTitle(title);
            SetFriendFunctionButtons();
            if (!dataUpdate)
            {
                Singleton<FriendControl>.Instance.SendRequestForFriendInfo();
            }
            else
            {
                List<PRelationInfo> friendList = Singleton<FriendMode>.Instance.friendsList;
                ShowAllPRelationInfo(friendList);
            }
            onCenterGrid.repositionNow = true;
        }


        private void SetBlackListInfo()
        {
            ReSetRoleListActive();
            SetBlackListTitle(title);
            SetBlackListFunctionButtons();
            if (Singleton<FriendMode>.Instance.blacksList == null)
            {
                Singleton<FriendControl>.Instance.SendRequestForFriendInfo();
            }
            else
            {
                List<PRelationInfo> blackList = Singleton<FriendMode>.Instance.blacksList;
                ShowAllPRelationInfo(blackList);
            }
            onCenterGrid.repositionNow = true;
        }

        private void SetAskListInfo()
        {
            ReSetRoleListActive();
            SetASkTitle(title);
            SetAskFunctionButtons();
            List<RelationPushAcceptMsg_7_3> askList = Singleton<FriendMode>.Instance.askList;
            ShowAllAskInfo(askList);

            onCenterGrid.repositionNow = true;
        }

        private void ShowAllPRelationNearInfo(List<PRelationNear> nearList)
        {
            foreach (PRelationNear near in nearList)
            {
                role = GetNextRoleObject();
                SetNearbyRoleInfo(role, near);
            }
        }


        private void ShowAllPRelationInfo(List<PRelationInfo> relationList)
        {
            foreach (PRelationInfo relation in relationList)
            {
                if (relation.isOnline == 1)
                {
                    role = GetNextRoleObject();
                    Log.info(this, "relation.roleId:" + relation.roleId);
                    SetRelationRoleInfo(role, relation);
                }

                
            }

            foreach (PRelationInfo relation in relationList)
            {
                if (relation.isOnline != 1)
                {
                    role = GetNextRoleObject();

                    SetRelationRoleInfo(role, relation);
                }
            }
        }

	    private void ShowAllAskInfo(List<RelationPushAcceptMsg_7_3> askList)
	    {
            foreach (RelationPushAcceptMsg_7_3 ask in askList)
	        {
	            role = GetNextRoleObject();

                SetAskRoleInfo(role, ask);
	        }
	    }

        private void SetNearbyRoleInfo(GameObject obj, PRelationNear near )
        {
            obj.name = obj.name + near.roleId.ToString();
            NGUITools.FindInChild<UILabel>(obj, "name").text = near.name;
            NGUITools.FindInChild<UILabel>(obj, "lvl").text = near.lvl.ToString();
            NGUITools.FindInChild<UISprite>(obj, "job").spriteName = GetJobInfo(near.job);
            NGUITools.FindInChild<UILabel>(obj, "fight").text = near.fightpoint.ToString();

            NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_LIGHT;
            NGUITools.FindInChild<UILabel>(obj, "lvl").color = ColorConst.FONT_BLUE;
            NGUITools.FindInChild<UILabel>(obj, "fight").color = ColorConst.FONT_BLUE;

            if (near.vip > 0)
            {
                NGUITools.FindChild(obj, "vip").SetActive(true);
                NGUITools.FindInChild<UILabel>(obj, "vip/vipvalue").text = near.vip.ToString();
            }
            else
            {
                NGUITools.FindChild(obj, "vip").SetActive(false);
            }

            obj.transform.FindChild("time").gameObject.SetActive(false);
           obj.transform.FindChild("button").gameObject.SetActive(true);
           NGUITools.FindInChild<UILabel>(obj, "button/label").text = "加为好友";
           obj.transform.FindChild("handle").gameObject.SetActive(false);
           obj.SetActive(true);
        }

	    private void SetRelationRoleInfo(GameObject obj, PRelationInfo relation)
        {
            obj.name = obj.name + relation.roleId.ToString();
            NGUITools.FindInChild<UILabel>(obj, "name").text = relation.name;
            NGUITools.FindInChild<UILabel>(obj, "lvl").text = relation.lvl.ToString();
            NGUITools.FindInChild<UISprite>(obj, "job").spriteName = GetJobInfo(relation.job);
	        NGUITools.FindInChild<UILabel>(obj, "fight").text = relation.fightpoint.ToString();

            if (relation.vip > 0)
            {
                NGUITools.FindChild(obj, "vip").SetActive(true);
                NGUITools.FindInChild<UILabel>(obj, "vip/vipvalue").text = relation.vip.ToString();
            }
            else
            {
                NGUITools.FindChild(obj, "vip").SetActive(false);
            }

	        if (relation.isOnline == 1)
            {
                NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_LIGHT;
                NGUITools.FindInChild<UILabel>(obj, "lvl").color = ColorConst.FONT_BLUE;
	            NGUITools.FindInChild<UILabel>(obj, "time").color = ColorConst.FONT_GREEN;
                NGUITools.FindInChild<UILabel>(obj, "fight").color = ColorConst.FONT_BLUE;
	        }
	        else
	        {
                NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_GRAY;
                NGUITools.FindInChild<UILabel>(obj, "lvl").color = ColorConst.FONT_GRAY;
                NGUITools.FindInChild<UILabel>(obj, "time").color = ColorConst.FONT_GRAY;
                NGUITools.FindInChild<UILabel>(obj, "fight").color = ColorConst.FONT_GRAY;
	        }

	        if(currentUIToggle == friend)
	        {
	            if (relation.isOnline == 1)
                {
                    obj.transform.FindChild("time").gameObject.SetActive(false);
                    obj.transform.FindChild("button").gameObject.SetActive(true);
	                NGUITools.FindInChild<UILabel>(obj, "button/label").text = "私聊";
	            }
	            else
	            {
                    obj.transform.FindChild("time").gameObject.SetActive(true);
                    obj.transform.FindChild("button").gameObject.SetActive(false);
	            }
	        }

	        if (currentUIToggle == blackList)
	        {
                obj.transform.FindChild("time").gameObject.SetActive(false);
                obj.transform.FindChild("button").gameObject.SetActive(true);
                NGUITools.FindInChild<UILabel>(obj, "button/label").text = "移除";
	        }
	        obj.transform.FindChild("handle").gameObject.SetActive(false);
            obj.SetActive(true);

        }

	    private void SetAskRoleInfo(GameObject obj, RelationPushAcceptMsg_7_3 ask)
        {
            obj.name = obj.name + ask.roleId.ToString();
            NGUITools.FindInChild<UILabel>(obj, "name").text = ask.name;
            NGUITools.FindInChild<UILabel>(obj, "lvl").text = ask.lvl.ToString();
            NGUITools.FindInChild<UISprite>(obj, "job").spriteName = GetJobInfo(ask.job);
            NGUITools.FindInChild<UILabel>(obj, "fight").text = ask.fightpoint.ToString();

            NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_LIGHT;
            NGUITools.FindInChild<UILabel>(obj, "lvl").color = ColorConst.FONT_BLUE;
            NGUITools.FindInChild<UILabel>(obj, "fight").color = ColorConst.FONT_BLUE;


            if (ask.vip > 0)
            {
                NGUITools.FindChild(obj, "vip").SetActive(true);
                NGUITools.FindInChild<UILabel>(obj, "vip/vipvalue").text = ask.vip.ToString();
            }
            else
            {
                NGUITools.FindChild(obj, "vip").SetActive(false);
            }

            obj.transform.FindChild("button").gameObject.SetActive(false);
            obj.transform.FindChild("time").gameObject.SetActive(false);
            obj.transform.FindChild("handle").gameObject.SetActive(true);

            obj.SetActive(true);

        }

        private void SetNearByTitle(GameObject obj)
        {

            obj.transform.FindChild("handle").gameObject.SetActive(true);
            obj.transform.FindChild("time").gameObject.SetActive(false);

        }

        private void SetFriendTitle(GameObject obj)
        {

            obj.transform.FindChild("handle").gameObject.SetActive(true);
            obj.transform.FindChild("time").gameObject.SetActive(false);

        }

        private void SetBlackListTitle(GameObject obj)
        {

            obj.transform.FindChild("handle").gameObject.SetActive(true);
            obj.transform.FindChild("time").gameObject.SetActive(false);

        }

        private void SetASkTitle(GameObject obj)
        {

            obj.transform.FindChild("handle").gameObject.SetActive(true);
            obj.transform.FindChild("time").gameObject.SetActive(false);
        }

	    private void SetFriendFunctionButtons()
	    {
            NGUITools.FindInChild<Button>(funcView, "button1").onClick = FuncLook;
            NGUITools.FindInChild<UILabel>(funcView, "button1/label").text = LanguageManager.GetWord("FriendView.Look");
            funcView.transform.FindChild("button1").gameObject.SetActive(true);


            NGUITools.FindInChild<Button>(funcView, "button2").onClick = FuncDeleteFriend;
            NGUITools.FindInChild<UILabel>(funcView, "button2/label").text = LanguageManager.GetWord("FriendView.DeleteFriend");
            funcView.transform.FindChild("button2").gameObject.SetActive(true);


            NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncAddBlackList;
            NGUITools.FindInChild<UILabel>(funcView, "button3/label").text = LanguageManager.GetWord("FriendView.AddBlackList");
            funcView.transform.FindChild("button3").gameObject.SetActive(true);
  

            //NGUITools.FindInChild<Button>(funcView, "button4").onClick = FuncMail;
            //NGUITools.FindInChild<UILabel>(funcView, "button4/label").text = LanguageManager.GetWord("FriendView.Mail");
            //funcView.transform.FindChild("button4").gameObject.SetActive(true);


            NGUITools.FindInChild<Button>(funcView, "button5").onClick = FuncTalk;
            NGUITools.FindInChild<UILabel>(funcView, "button5/label").text = LanguageManager.GetWord("FriendView.Talk");
            funcView.transform.FindChild("button5").gameObject.SetActive(true);

	        setFunButtonPos5();

	    }


        private void SetNearbyFunctionButtons()
        {
            NGUITools.FindInChild<Button>(funcView, "button1").onClick = FuncLook;
            NGUITools.FindInChild<UILabel>(funcView, "button1/label").text = LanguageManager.GetWord("FriendView.Look");
            funcView.transform.FindChild("button1").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button2").onClick = FuncAddFriend;
            NGUITools.FindInChild<UILabel>(funcView, "button2/label").text = LanguageManager.GetWord("FriendView.AddFriend");
            funcView.transform.FindChild("button2").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncAddBlackList;
            NGUITools.FindInChild<UILabel>(funcView, "button3/label").text = LanguageManager.GetWord("FriendView.AddBlackList");
            funcView.transform.FindChild("button3").gameObject.SetActive(true);

            //NGUITools.FindInChild<Button>(funcView, "button4").onClick = FuncMail;
            //NGUITools.FindInChild<UILabel>(funcView, "button4/label").text = LanguageManager.GetWord("FriendView.Mail");
            //funcView.transform.FindChild("button4").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button5").onClick = FuncTalk;
            NGUITools.FindInChild<UILabel>(funcView, "button5/label").text = LanguageManager.GetWord("FriendView.Talk");
            funcView.transform.FindChild("button5").gameObject.SetActive(true);
            setFunButtonPos5();

        }



        private void SetBlackListFunctionButtons()
        {
            NGUITools.FindInChild<Button>(funcView, "button1").onClick = FuncLook;
            NGUITools.FindInChild<UILabel>(funcView, "button1/label").text = LanguageManager.GetWord("FriendView.Look");
            funcView.transform.FindChild("button1").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button2").onClick = FuncDeleteBlackList;
            NGUITools.FindInChild<UILabel>(funcView, "button2/label").text = LanguageManager.GetWord("FriendView.DeleteBlackList");
            funcView.transform.FindChild("button2").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncAddFriend;
            NGUITools.FindInChild<UILabel>(funcView, "button3/label").text = LanguageManager.GetWord("FriendView.AddFriend");
            funcView.transform.FindChild("button3").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button5").onClick = FuncTalk;
            NGUITools.FindInChild<UILabel>(funcView, "button5/label").text = LanguageManager.GetWord("FriendView.Talk");
            funcView.transform.FindChild("button5").gameObject.SetActive(false);
            
            setFunButtonPos4();

        }


        private void SetAskFunctionButtons()
        {
            NGUITools.FindInChild<Button>(funcView, "button1").onClick = FuncLook;
            NGUITools.FindInChild<UILabel>(funcView, "button1/label").text = LanguageManager.GetWord("FriendView.Look");
            funcView.transform.FindChild("button1").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button2").onClick = FuncAddBlackList;
            NGUITools.FindInChild<UILabel>(funcView, "button2/label").text = LanguageManager.GetWord("FriendView.AddBlackList");
            funcView.transform.FindChild("button2").gameObject.SetActive(true);

            //NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncMail;
            //NGUITools.FindInChild<UILabel>(funcView, "button3/label").text = LanguageManager.GetWord("FriendView.Mail");
            //funcView.transform.FindChild("button3").gameObject.SetActive(true);

            NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncTalk;
            NGUITools.FindInChild<UILabel>(funcView, "button3/label").text = LanguageManager.GetWord("FriendView.Talk");
            funcView.transform.FindChild("button3").gameObject.SetActive(true);

            funcView.transform.FindChild("button5").gameObject.SetActive(false);

            setFunButtonPos4();
        }


	    private void setFunButtonPos5()
	    { 
            funcView.transform.FindChild("button1").gameObject.transform.localPosition = new Vector3(funPosx5[0],-8,0);
            funcView.transform.FindChild("button2").gameObject.transform.localPosition = new Vector3(funPosx5[1], -8, 0);
            funcView.transform.FindChild("button3").gameObject.transform.localPosition = new Vector3(funPosx5[2], -8, 0);
            funcView.transform.FindChild("button4").gameObject.transform.localPosition = new Vector3(funPosx5[3], -8, 0);
            funcView.transform.FindChild("button5").gameObject.transform.localPosition = new Vector3(funPosx5[4], -8, 0);

	    }

        private void setFunButtonPos4()
        {
            funcView.transform.FindChild("button1").gameObject.transform.localPosition = new Vector3(funPosx4[0], -8, 0);
            funcView.transform.FindChild("button2").gameObject.transform.localPosition = new Vector3(funPosx4[1], -8, 0);
            funcView.transform.FindChild("button3").gameObject.transform.localPosition = new Vector3(funPosx4[2], -8, 0);
            funcView.transform.FindChild("button4").gameObject.transform.localPosition = new Vector3(funPosx4[3], -8, 0);
        }

	    private void FuncLook(GameObject obj)
	    {
			//Singleton<RoleInfo>.Instance.ShowRoleInfo(getRoleId(currentRole), 0,RoleViewOpenCallBack,RoleViewCloseClick);
			Singleton<PlayerDetailView>.Instance.ShowWindow(getRoleId(currentRole));
	    }

        private void RoleViewOpenCallBack(GameObject obj)
        {
            //roleView.transform.parent = gameObject.transform.parent;
            //obj.transform.parent = roleView.transform;
            //obj.transform.localPosition = rolePos;
            //roleView.SetActive(true);
            gameObject.SetActive(false);
			//RoleDisplay.Instance.ChangePosition(-8.733765f,-114.6877f);
            FuncCloseClick(null);
	    }


        private void FuncTalk(GameObject obj)
        {

            if (NGUITools.FindInChild<UILabel>(currentRole, "name").color == ColorConst.FONT_GRAY)
            {
                MessageManager.Show(LanguageManager.GetWord("FriendView.OffLine"));
                return;
            }
            string name = NGUITools.FindInChild<UILabel>(currentRole, "name").text;
            Singleton<ChatView>.Instance.OpenPrivateChatView(name, getRoleId(currentRole));
            FuncCloseClick(null);
        }
        private void FuncAddFriend(GameObject obj)
        {
            if (NGUITools.FindInChild<UILabel>(currentRole, "name").color == ColorConst.FONT_GRAY)
            {
                MessageManager.Show(LanguageManager.GetWord("FriendView.OffLine"));
                return;
            }

            if (currentRole != null)
            {
                Singleton<FriendControl>.Instance.SendFriendAskInfo(getRoleId(currentRole));
            }
            FuncCloseClick(null);
        }
        private void FuncDeleteFriend(GameObject obj)
        {
            if (currentRole != null)
            {
                Singleton<FriendControl>.Instance.SendDeleteFriendInfo(getRoleId(currentRole));
            }
            FuncCloseClick(null);
        }
        private void FuncAddBlackList(GameObject obj)
        {
            if (currentRole != null)
            {
                Singleton<FriendControl>.Instance.MoveToBlackList(getRoleId(currentRole));
            }
            FuncCloseClick(null);
        }
        private void FuncDeleteBlackList(GameObject obj)
        {
            if (currentRole != null)
            {
                Singleton<FriendControl>.Instance.SendDeleteBlackList(getRoleId(currentRole));
            }
            FuncCloseClick(null);

        }

	    private string GetJobInfo(byte job)
        {
	        if (job == 1)
	        {
	            return "zyjs";
	        }
            else if (job == 2)
            {
                return "zyfs";
            }
            else if (job == 3)
            {
                return "zyck";
            }
	        return "";
        }


	    private string getOnlineInfo(PRelationInfo info)
	    {
	        if (info.isOnline == 1)
	        {
	            return LanguageManager.GetWord("FriendView.Online");
	        }
	        else
	        {
                long s = (long)(Time.time - info.lastLogoutTime);
                return getTimeDesByS(s);
	        }
	    }

	    private string getTimeDesByS(long s)
	    {
	        int hour =  (int)(s/3600);
	        if (hour <= 0)
	        {
                return LanguageManager.GetWord("FriendView.InHour");
	        }
	        if (hour > 24)
	        {
	            return LanguageManager.GetWord("FriendView.DayBefore");
	        }
            return LanguageManager.GetWord(hour + "FriendView.HourBefore");
	    }


	    private void RefreshClick(GameObject obj)
	    {
            Singleton<FriendControl>.Instance.SendRequestForNearByInfo();
	    }

	    private void AddClick(GameObject obj)
        {
            addView.SetActive(true);
        }

        private void AddFriendByNameClick(GameObject obj)
        {
            if (inputName.text != null && !inputName.text.Equals(string.Empty) && inputName.text.Length>1)
            {
                if (inputName.text.Length > 6)
                {
                    MessageManager.Show(LanguageManager.GetWord("FriendView.MaxLength"));
                }
                else
                {
                    Singleton<FriendControl>.Instance.SendFriendAskByName(inputName.text);
                    addView.SetActive(false);
                    inputName.text = LanguageManager.GetWord("FriendView.InputName");
                    input.value = string.Empty;
                }
            }
            else
            {
                MessageManager.Show(LanguageManager.GetWord("FriendView.Empty"));
            }


        }


        private void AddMaxClick(GameObject obj)
	    {
            MessageManager.Show(LanguageManager.GetWord("FriendView.MaxNum"));
	    }

	    private void RoleInfoClick(GameObject obj)
        {
	        SetRoleClickEffect(obj,"exp",1);
	        currentRole = obj;
            funcView.SetActive(true);
            //在线判断
            //if (currentUIToggle == friend)
            //{
            //    if (NGUITools.FindInChild<UILabel>(currentRole, "name").color == ColorConst.GREY_NO)
            //    {
            //        NGUITools.FindInChild<Button>(funcView, "button4").onClick = null;
            //        NGUITools.FindInChild<UISprite>(funcView, "button4/background").color = ColorConst.GRAY;
            //    }
            //    else
            //    {
            //        NGUITools.FindInChild<Button>(funcView, "button4").onClick = FuncTalk;
            //        NGUITools.FindInChild<UISprite>(funcView, "button4/background").color = ColorConst.Normal;
            //    }

            //}

            //if (currentUIToggle == blackList)
            //{
            //    if (NGUITools.FindInChild<UILabel>(currentRole, "name").color == ColorConst.GREY_NO)
            //    {
            //        NGUITools.FindInChild<Button>(funcView, "button3").onClick = null;
            //        NGUITools.FindInChild<UISprite>(funcView, "button3/background").color = ColorConst.GRAY;
            //    }
            //    else
            //    {
            //        NGUITools.FindInChild<Button>(funcView, "button3").onClick = FuncAddFriend;
            //        NGUITools.FindInChild<UISprite>(funcView, "button3/background").color = ColorConst.Normal;
            //    }

            //}

        }

	    private void SetRoleClickEffect(GameObject role,string icnName,int depth)
	    {
	        NGUITools.FindInChild<UISprite>(role,"icn1").spriteName = icnName;
            NGUITools.FindInChild<UISprite>(role,"icn1").depth += depth;
            NGUITools.FindInChild<UISprite>(role, "icn2").spriteName = icnName;
            NGUITools.FindInChild<UISprite>(role, "icn2").depth += depth;
            
	    }



	    private void AcceptClick(GameObject obj)
        {
            uint roleId = getRoleId(obj.transform.parent.transform.parent.gameObject);
            Singleton<FriendControl>.Instance.ReplyFriendAskInfo(roleId,true);
        }

	    private void RejectClick(GameObject obj)
	    {
            uint roleId = getRoleId(obj.transform.parent.transform.parent.gameObject);
            Singleton<FriendControl>.Instance.ReplyFriendAskInfo(roleId, false);
	    }

	    private uint getRoleId(GameObject obj)
	    {
	        string roleid = obj.name.Split('_')[1];
            Log.info(this, "roleid" + roleid);
	        return uint.Parse(roleid);
	    }


	    private GameObject GetNextRoleObject()
	    {
	        GameObject obj;
	        if (roleList.Count > roleInfoNum)
	        {   
	            obj =  roleList[roleInfoNum];
                obj.transform.parent = onCenterGrid.transform;
	        }
	        else
	        {
  
                obj =   AddRoleObjet();
	        }
            roleInfoNum++;
	        return obj;
	    }


	    private void ReSetRoleListActive()
        {
            roleInfoNum = 0;
            foreach (GameObject obj in roleList)
            {
               obj.SetActive(false);
               obj.transform.parent =  gameObject.transform;
               obj.name = obj.name.Split('_')[0]+'_';
            }
            //roleList.Clear();
	    }

	}
}
