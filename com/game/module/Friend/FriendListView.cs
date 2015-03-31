﻿﻿﻿using com.game.consts;
﻿﻿﻿using com.game.manager;
﻿﻿﻿using Com.Game.Module.Chat;
﻿﻿﻿using Com.Game.Module.Role;
﻿﻿﻿using com.game.Public.Message;
﻿﻿﻿using com.u3d.bases.debug;
﻿﻿﻿using PCustomDataType;
﻿﻿﻿using UnityEngine;
using com.game.module.test;
using System.Collections.Generic;


namespace Com.Game.Module.Friend
{
	/// <summary>
	/// 好友列表视图
	/// </summary>
    public class FriendListView : BaseView<FriendListView>
	{   
		/// <summary>
		/// 预设路径
		/// </summary>
        public override string url { get { return "UI/Friend/FriendListView.assetbundle"; } }

	    public override ViewLayer layerType
	    {
            get { return ViewLayer.MiddleLayer; }
	    }

	    private GameObject role;
	    private GameObject container;
	    private UIGrid onCenterGrid;

        private UISprite up;
        private UISprite down;

        private readonly int topY = -2;//最上位置
        private readonly int showNum = 6; //展示数量为5
        private readonly int infoY = 56;//记录高度56 
        private readonly int gapY = 10; //隐藏间隔10

	    private int roleInfoNum = 0;
		
	    private bool forTalk = false;

        private List<GameObject> roleList = new List<GameObject>(); //对象缓存

	    public void openViewForTalk()
	    {
            forTalk = true;
            base.OpenView();
	    }

	    protected override  void Init()
        {
            FindInChild<Button>("button_close").onClick = CloseClick;

            container = FindChild("body/container");
            container.GetComponent<UIScrollView>().onDragFinished += OnDragFinish;
            onCenterGrid = FindInChild<UIGrid>("body/container/oncenter");

            role = FindChild("body/container/oncenter/role");
            role.SetActive(false);

            up = FindInChild<UISprite>("body/up/icn");
            down = FindInChild<UISprite>("body/down/icn");

		    initLabel();
		}

	    private void initLabel()
        {
            FindInChild<UILabel>("head/friend/label").text = LanguageManager.GetWord("FriendView.Friend");
            FindInChild<UILabel>("num").text = LanguageManager.GetWord("FriendView.FriendNum");
        }

        private GameObject AddRoleObjet()
        {
            if (roleList.Count == 0)
            {
                role.name = "role" + (1000+roleList.Count + 1) + '_';
                roleList.Add(role);
                role.GetComponent<Button>().onClick = RoleInfoClick;
                return role;
            }
            else
            {
                GameObject roleInfo = (GameObject)GameObject.Instantiate(roleList[0]);
                roleInfo.name = "role" + (1000+roleList.Count + 1) + "_";
                roleInfo.transform.parent = onCenterGrid.transform;
                roleInfo.transform.localScale = new Vector3(1, 1, 1);
                roleInfo.GetComponent<Button>().onClick = RoleInfoClick;
                roleList.Add(roleInfo);
                return roleInfo;
            }
        }

        //关闭按钮
        private void CloseClick(GameObject obj)
        {
            base.CloseView();
        }

        //玩家信息面板点击
	    private void RoleInfoClick(GameObject obj)
	    {
            SetRoleClickEffect(obj, "exp", 1);
			if(forTalk)
	        {
                FuncTalk(obj);
	        }
            SetRoleClickEffect(obj, "fgx5656", -1);
            CloseClick(null);
	    }

        private void FuncTalk(GameObject obj)
        {

            if (NGUITools.FindInChild<UILabel>(obj, "name").color == ColorConst.FONT_GRAY)
            {
                MessageManager.Show("FriendView.OffLine");
                return;
            }
            string name = NGUITools.FindInChild<UILabel>(obj, "name").text;
            Singleton<ChatView>.Instance.OpenPrivateChatView(name, getRoleId(obj));
        }

        private uint getRoleId(GameObject obj)
        {
            string roleid = obj.name.Split('_')[1];
            Log.info(this, "roleid" + roleid);
            return uint.Parse(roleid);
        }

        private void SetRoleClickEffect(GameObject role, string icnName, int depth)
        {
            NGUITools.FindInChild<UISprite>(role, "icn1").spriteName = icnName;
            NGUITools.FindInChild<UISprite>(role, "icn1").depth += depth;
            NGUITools.FindInChild<UISprite>(role, "icn2").spriteName = icnName;
            NGUITools.FindInChild<UISprite>(role, "icn2").depth += depth;

        }

	    protected override void HandleAfterOpenView()
	    {
            Singleton<FriendControl>.Instance.SendRequestForFriendInfo();
	    }


	    void OnDragFinish()
        {
            Log.info(this, "container.transform.localPosition.y:" + container.transform.localPosition.y);
            SetUpDownIcn();
        }

        private void SetUpDownIcn()
        {
            if (roleInfoNum <= showNum)
            {
                down.gameObject.SetActive(false);
                up.gameObject.SetActive(false);

            }
            else
            {
                if (IsTop())
                {

                    down.spriteName = "banyuan1";
                    up.gameObject.SetActive(false);
                    down.gameObject.SetActive(true);

                }
                else if (IsBottom())
                {

                    up.spriteName = "banyuan1";
                    down.gameObject.SetActive(false);
                    up.gameObject.SetActive(true);
                }
                else
                {
                    down.gameObject.SetActive(false);
                    up.gameObject.SetActive(false);
                }

            }

        }


        private bool IsTop()
        {
            return container.transform.localPosition.y <= topY || roleInfoNum < showNum;

        }

        private bool IsBottom()
        {
            int bottomY = -topY + (roleInfoNum - showNum) * infoY - gapY;
            return container.transform.localPosition.y >= bottomY;
        }

        private void SetFriendNumInfo()
        {
            int num = Singleton<FriendMode>.Instance.friendNum;
            int maxNum = Singleton<FriendMode>.Instance.maxNum;
            FindInChild<UILabel>("numvalue").text = num + @"/" + maxNum;
        }

                //注册数据更新器
        public override void RegisterUpdateHandler()
        {
            Singleton<FriendMode>.Instance.dataUpdated += RoleDataUpdated;
        }


        //数据更新响应
	    public void RoleDataUpdated(object sender, int code)
	    {

	        if (code == FriendMode.FRIEND)
	        {
                List<PRelationInfo> friendList = Singleton<FriendMode>.Instance.friendsList;
                ShowAllPRelationInfo(friendList);
	        }
	        else if(code == FriendMode.FRIENDNUM)
	        {
	            SetFriendNumInfo();
	        }
	    }


        private void ShowAllPRelationInfo(List<PRelationInfo> relationList)
        {
            roleInfoNum = 0;
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
            onCenterGrid.repositionNow = true;
            SetUpDownIcn();
        }



        private void SetRelationRoleInfo(GameObject obj, PRelationInfo relation)
        {
            obj.name = obj.name + relation.roleId.ToString();
            NGUITools.FindInChild<UILabel>(obj, "name").text = relation.name;
            if (relation.isOnline == 1)
            {
                NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_LIGHT;
            }
            else
            {
                NGUITools.FindInChild<UILabel>(obj, "name").color = ColorConst.FONT_GRAY;

            }
            obj.SetActive(true);
        }

        private GameObject GetNextRoleObject()
        {
            GameObject obj;
            if (roleList.Count > roleInfoNum)
            {
                obj = roleList[roleInfoNum];
                obj.transform.parent = onCenterGrid.transform;
            }
            else
            {

                obj = AddRoleObjet();
            }
            roleInfoNum++;
            return obj;
        }
	    //取消数据更新器
        public override void CancelUpdateHandler()
        {
            Singleton<FriendMode>.Instance.dataUpdated -= RoleDataUpdated;
        }
	}
}
