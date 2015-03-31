using System;
using System.Collections;

using com.game.module.effect;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.u3d.bases.consts;
using UnityEngine;
using com.game.module.login;
using com.game.utils;
using com.game.manager;
using com.game.consts;
using Com.Game.Module.ContinueCut;
using Com.Game.Module.Chat;
using com.u3d.bases.debug;
using com.game.Public.Message;

namespace com.game.module.rolecreate
{
    internal class RoleCreateView : BaseView<RoleCreateView>
    {
        public override string url
        {
            get { return "UI/RoleCreate/RoleCreateView.assetbundle"; }
        }

        public override bool isDestroy
        {
            get { return true; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        public override bool isUnloadDelay
        {
            get { return true; }
        }

        private const string RANDOM_NAME_URL = "Xml/RandomName.assetbundle";

        public override bool waiting
        {
            get { return false; }
        }

        //private UIWidgetContainer btn_back;
        private UIWidgetContainer btn_start;
        private Button btn_random;
        private UIInput input;

        private UIToggle yuansushi, mojianshi, qiangxieshi, current;

        private string singleFamilys;
        private string[] doubleFamilys, firstNames;

        // 特殊字符
        private string[] _specialName;
        // 男单字
        private string[] _manSingleName;
        // 男双字
        private string[] _manDoubleName;
        // 女单字
        private string[] _womanSingleName;
        // 女双字
        private string[] _womanDoubleName;
        //姓氏
        private string[] _familyName;

        private string[][] laynameLayout = { new[] { "s", "s", "d" }, new[] { "s", "t", "d" }, new[] { "d", "s", "s" }, new[] { "t", "s", "s", "d" }, new[] { "s", "s", "t", "d" }, new[] { "t", "s", "d", "t" }, new[] { "d", "t", "s" }, new[] { "t", "s", "s", "d", "t" } };



        //UI2DSprite yuansushiSprite, mojianshiSprite, qiangxieshiSprite;
	   // private UISprite yssInfoSprite, mjsInfoSprite, qxsInfoSprite;
	    private UILabel tipsLabel;

        private int job=1; // 职业
        
		//private TweenScale scale;
	    private Transform info;

	    private int topY = 188; //角色顶位置
	    private int gapY1 = 140; 
	    private int gapUp = 120;
        private int gapDown = 116;
        private SpinWithMouse modelSpin;


        protected override void Init()
        {
            //btn_back.
            btn_start = Tools.find(gameObject, "btn_start").GetComponent<UIWidgetContainer>();
            btn_start.FindInChild<UILabel>("label").text = LanguageManager.GetWord("RoleCreate.BtnStart");
            //btn_back = Tools.find(gameObject, "btn_fanhui").GetComponent<UIWidgetContainer>();
            btn_random = Tools.find(gameObject, "btn_random").GetComponent<Button>();
            input = Tools.find(gameObject, "inp_name").GetComponent<UIInput>();
            yuansushi = Tools.find(gameObject, "ckb_yss").GetComponent<UIToggle>();
            mojianshi = Tools.find(gameObject, "ckb_mjs").GetComponent<UIToggle>();
            qiangxieshi = Tools.find(gameObject, "ckb_qxs").GetComponent<UIToggle>();
			//yuansushiSprite = Tools.find(gameObject, "rw/yss").GetComponent<UI2DSprite>();
			//mojianshiSprite = Tools.find(gameObject, "rw/mjs").GetComponent<UI2DSprite>();
			//qiangxieshiSprite = Tools.find(gameObject, "rw/qxs").GetComponent<UI2DSprite>();

			//scale = FindInChild<TweenScale>("rw");
            //yssInfoSprite = FindInChild<UISprite>("info/yuansushi");
            //mjsInfoSprite = FindInChild<UISprite>("info/mojianshi");
            //qxsInfoSprite = FindInChild<UISprite>("info/qiangxieshi");
            tipsLabel = FindInChild<UILabel>("info/shuoming");
            info = FindChild("info").transform;
            info.localScale = Vector3.one;
            info.gameObject.SetActive(true);

            modelSpin = NGUITools.FindInChild<SpinWithMouse>(gameObject, "mode");

            btn_random.onClick += RandomName;
            btn_start.onClick += StartGame;
            //btn_back.onClick = BackOnClick;
            yuansushi.startsActive = false;
            mojianshi.startsActive = false;
            qiangxieshi.startsActive = false;
            //yuansushiSprite.gameObject.SetActive(false);
            //qiangxieshiSprite.gameObject.SetActive(false);
            //mojianshiSprite.gameObject.SetActive(false);
            yuansushi.onStateChange += CheckBoxStateChange;
            mojianshi.onStateChange += CheckBoxStateChange;
            qiangxieshi.onStateChange += CheckBoxStateChange;
            //input.onChange.Add(new EventDelegate(LimitNameShow));  //限制显示名字的长度
            input.characterLimit = 6; //限制最长为6个字符
           // LoadRandomNameData();
            gameObject.SetActive(false);
            NGUITools.SetLayer(gameObject,LayerMask.NameToLayer("Mode"));
            EffectMgr.Instance.CreateUIEffect(EffectId.UI_SelectRoleStage1,Vector3.zero , OnFirstEffectOver);
           // play = NGUITools.FindInChild<TweenPlay>(gameObject, "mode");
            FindInChild<UIWidgetContainer>("mode").onClick = OnRoleClick;


        }
            
        private GameObject roleChoseEffect;
        private void OnRoleChoseEffectCreated(GameObject obj)
        {
            roleChoseEffect = obj;
            obj.transform.localPosition = new Vector3(140, -115, 0);
            roleChoseEffect.SetActive(false);
            NGUITools.SetLayer(obj,LayerMask.NameToLayer("Viewport"));

        }

        private IEnumerator DelayShowRoleChoseEffect()
        {
            yield return new WaitForSeconds(0.4f);
            roleChoseEffect.SetActive(true);
        }

        private GameObject roleBustEffect;
        private void OnRoleBustEffectCreated(GameObject obj)
        {
            roleBustEffect = obj;
            obj.transform.localPosition = new Vector3(140, -115, 0);
            roleBustEffect.SetActive(false);
            NGUITools.SetLayer(obj, LayerMask.NameToLayer("Viewport"));
        }

        private GameObject SelectRoleStage2;
        private void OnSelectRoleStage2Created(GameObject obj)
        {
            SelectRoleStage2 = obj;

        }


		protected override void HandleAfterOpenView ()
        {
            gameObject.SetActive(false);
			base.HandleAfterOpenView ();
			Singleton<ContinueCutView>.Instance.CloseView();
		}

	    private void OnFirstEffectOver()
	    {
            EffectMgr.Instance.CreateUIEffect(EffectId.UI_SelectRoleStage2, Vector3.zero, null, true, OnSelectRoleStage2Created);
            EffectMgr.Instance.CreateUIEffect(EffectId.UI_SelectRoleFeedback, Vector3.zero, null, true, OnRoleChoseEffectCreated);
            EffectMgr.Instance.CreateUIEffect(EffectId.UI_SelectRoleBurst, Vector3.zero, null, true, OnRoleBustEffectCreated);
	        AtlasManager.Instance.LoadAtlas(AtlasUrl.HeaderIconURL,AtlasUrl.HeaderIcon,LoadAtlas);
	    }

        private void LoadAtlas(UIAtlas obj)
        {
            UISprite job1 = NGUITools.FindInChild<UISprite>(mojianshi.gameObject, "icn");
            UISprite job2 = NGUITools.FindInChild<UISprite>(yuansushi.gameObject, "icn");
            UISprite job3 = NGUITools.FindInChild<UISprite>(qiangxieshi.gameObject, "icn");

            job1.atlas = obj;
            job1.spriteName = "101";


            job2.atlas = obj;
            job2.spriteName = "201";


            job3.atlas = obj;
            job3.spriteName = "301";

            LoadRandomNameData();
            gameObject.SetActive(true);
	    }

	    private void RandomRole(GameObject go)
        {
            System.Random ran = new System.Random();
            int randomNum = ran.Next(1, 4);
            switch (randomNum)
			{
			    case GameConst.JOB_JIAN:
			        mojianshi.value = true;
				    break;
			    case GameConst.JOB_FASHI:
                    yuansushi.value = true;
				    break;
			    case GameConst.JOB_CHIKE:
                    qiangxieshi.value = true;
				    break;
			}
			RandomName(null);
			
        }
        private void CheckBoxStateChange(bool state)
        {

            if (UIToggle.current.Equals(yuansushi) && current != yuansushi) //元素师
            {
                if (state)
                {
                    job = GameConst.JOB_FASHI;
                    LoadRoleMode();
                    tipsLabel.text = LanguageManager.GetWord("RoleCreate.YSSInfo").Replace(@"\n", "\n"); ;
                    NGUITools.FindChild(yuansushi.gameObject,"job").SetActive(false);
                    NGUITools.FindChild(yuansushi.gameObject, "high").SetActive(true);
                    NGUITools.FindChild(qiangxieshi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(qiangxieshi.gameObject, "high").SetActive(false);
                    NGUITools.FindChild(mojianshi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(mojianshi.gameObject, "high").SetActive(false);

                    yuansushi.gameObject.GetComponent<TweenPosition>().from =
                        yuansushi.gameObject.transform.localPosition;
                    yuansushi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - gapY1, 0);
                    yuansushi.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
                    yuansushi.GetComponent<TweenScale>().to = new Vector3(1.1f, 1.1f, 1.1f);

                    qiangxieshi.gameObject.GetComponent<TweenPosition>().from = qiangxieshi.gameObject.transform.localPosition;
                    qiangxieshi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - gapY1 - gapUp - gapDown, 0);

                    info.localPosition = new Vector3(-295, topY -  gapY1 - gapUp, 0);
                    //info.gameObject.SetActive(true);
                    //info.GetComponent<TweenPlay>().PlayForward();
                    
                    if (current == mojianshi)
                    {
                        current.GetComponent<TweenScale>().PlayReverse();
                        qiangxieshi.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
                        qiangxieshi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }
                    else if (current == qiangxieshi)
                    {
                        qiangxieshi.GetComponent<TweenScale>().from = new Vector3(1.1f, 1.1f, 1.1f);
                        qiangxieshi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }

                    yuansushi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    qiangxieshi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    current = yuansushi;
                }

            }
            else if (UIToggle.current.Equals(qiangxieshi) && current != qiangxieshi)
            {
                if (state)
                {
                    job = GameConst.JOB_CHIKE;
                    LoadRoleMode();
                    tipsLabel.text = LanguageManager.GetWord("RoleCreate.QXSInfo").Replace(@"\n","\n");

                    NGUITools.FindChild(yuansushi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(yuansushi.gameObject, "high").SetActive(false);
                    NGUITools.FindChild(qiangxieshi.gameObject, "job").SetActive(false);
                    NGUITools.FindChild(qiangxieshi.gameObject, "high").SetActive(true);
                    NGUITools.FindChild(mojianshi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(mojianshi.gameObject, "high").SetActive(false);

                    yuansushi.gameObject.GetComponent<TweenPosition>().from = yuansushi.gameObject.transform.localPosition;
                    yuansushi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - gapY1, 0);

                    qiangxieshi.gameObject.GetComponent<TweenPosition>().from = qiangxieshi.gameObject.transform.localPosition;
                    qiangxieshi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - 2 * gapY1, 0);

                    qiangxieshi.GetComponent<TweenScale>().from = new Vector3(1, 1,1);
                    qiangxieshi.GetComponent<TweenScale>().to = new Vector3(1.1f, 1.1f, 1.1f);


                    info.localPosition = new Vector3(-295, topY - 2 * gapY1 - gapUp, 0);
                    //info.gameObject.SetActive(true);
                    //info.GetComponent<TweenPlay>().PlayForward();
                    if (current == mojianshi)
                    {
                        current.GetComponent<TweenScale>().PlayReverse();
                        yuansushi.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
                        yuansushi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }
                    else if (current == yuansushi)
                    {
                        yuansushi.GetComponent<TweenScale>().from = new Vector3(1.1f, 1.1f, 1.1f);
                        yuansushi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }
                    yuansushi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    qiangxieshi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    current = qiangxieshi;

                }
            }
            else if (UIToggle.current.Equals(mojianshi) && current != mojianshi)
            {
                if (state)
                {
                    job = GameConst.JOB_JIAN;
                    LoadRoleMode();
                    tipsLabel.text = LanguageManager.GetWord("RoleCreate.MJSInfo").Replace(@"\n", "\n"); ;


                    NGUITools.FindChild(yuansushi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(yuansushi.gameObject, "high").SetActive(false);
                    NGUITools.FindChild(qiangxieshi.gameObject, "job").SetActive(true);
                    NGUITools.FindChild(qiangxieshi.gameObject, "high").SetActive(false);
                    NGUITools.FindChild(mojianshi.gameObject, "job").SetActive(false);
                    NGUITools.FindChild(mojianshi.gameObject, "high").SetActive(true);

                    yuansushi.gameObject.GetComponent<TweenPosition>().from = yuansushi.gameObject.transform.localPosition;
                    yuansushi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - gapUp - gapDown, 0);

                    qiangxieshi.gameObject.GetComponent<TweenPosition>().from = qiangxieshi.gameObject.transform.localPosition;
                    qiangxieshi.gameObject.GetComponent<TweenPosition>().to = new Vector3(-400, topY - gapUp - gapDown - gapY1, 0);

                    info.localPosition = new Vector3(-295, topY - gapUp, 0);
                    //info.gameObject.SetActive(true);
                    //info.GetComponent<TweenPlay>().PlayForward();
                    if (current == yuansushi)
                    {
                        yuansushi.GetComponent<TweenScale>().from = new Vector3(1.1f, 1.1f, 1.1f);
                        yuansushi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);

                        qiangxieshi.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
                        qiangxieshi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }
                    else if (current == qiangxieshi)
                    {
                        qiangxieshi.GetComponent<TweenScale>().from = new Vector3(1.1f, 1.1f, 1.1f);
                        qiangxieshi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);

                        yuansushi.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
                        yuansushi.GetComponent<TweenScale>().to = new Vector3(1, 1, 1);
                    }
                    mojianshi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    yuansushi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    qiangxieshi.gameObject.GetComponent<TweenPlay>().PlayForward();
                    current = mojianshi;

                }
            }
			//scale.Begin();            
        }
        /// <summary>
        /// 读取字库数据和 模型的位置和方向
        /// </summary>
        private void LoadRandomNameData()
        {
            AssetManager.Instance.LoadAsset<TextAsset>(RANDOM_NAME_URL, LoadRandomNameCallback);
        }




        private void LoadRandomNameCallback(TextAsset nameText)
        {
            string text = nameText.ToString();
/*            text = text.Replace("\r\n", "").Replace("\t", "");
            int sin, dou, nam;
            string douStr, namStr;
            sin = text.IndexOf("//单姓");
            dou = text.IndexOf("//复姓");
            nam = text.IndexOf("//名字");
            singleFamilys = text.Substring(sin + 4, dou - sin - 4);
            douStr = text.Substring(dou + 4, nam - dou - 4);
            namStr = text.Substring(nam + 4);
            //sinStr=sinStr.Replace("\"", "").Replace(" +", "");
            //douStr=douStr.Replace("[\"","").Replace("\"];","");
            //namStr = namStr.Replace("[\"", "").Replace("\"];", "");
            //doubleFamilys = Regex.Split(douStr, "\",\"", RegexOptions.IgnoreCase);
            //firstNames = Regex.Split(namStr, "\",\"", RegexOptions.IgnoreCase);

            douStr = douStr.Replace("\",\"", "|");
            douStr = douStr.Replace("\"", "");
            doubleFamilys = douStr.Split('|');

            namStr = namStr.Replace("\",\"", "|");
            namStr = namStr.Replace("\"", "");
            firstNames = namStr.Split('|');*/

            XMLNode xml = XMLParser.Parse(text);
            string spec = xml.GetValue("root>0>special>0>_text");
            _specialName = spec.Split(',');

            string manS = xml.GetValue("root>0>man>0>single>0>_text");
            _manSingleName = manS.Split(',');

            string manD = xml.GetValue("root>0>man>0>double>0>_text");
            _manDoubleName = manD.Split(',');

            string womanS = xml.GetValue("root>0>women>0>single>0>_text");
            _womanSingleName = womanS.Split(',');

            string womanD = xml.GetValue("root>0>women>0>double>0>_text");
            _womanDoubleName = womanD.Split(',');

            RandomRole(null);
        }


        /// <summary>
        /// 随机取名
        /// </summary>
        /// <param name="go"></param>
        private void RandomName(GameObject go)
        {
            input.value = getRandomName();
        }


        		/** 根据性别返回一个随机名字 
		 * sex:1男，2女
		 * 名字包含2个单字，1个双字，0-2个特殊字符
		 */
		public string getRandomName()
		{
			string[] singleName;
			string[] doubleName;
			if (current == mojianshi)
			{
				singleName = _manSingleName;
				doubleName = _manDoubleName;
			}
			else
			{
				singleName = _womanSingleName;
				doubleName = _womanDoubleName;
			}
			// 随机名字格局
		    System.Random rand = new System.Random();
		    string[] layout = laynameLayout[rand.Next(0, laynameLayout.Length - 1)];
		    string name = "";
			foreach(string t in layout)
			{
				switch (t)
				{
					case "s":
                        name += singleName[rand.Next(0, singleName.Length - 1)];						
						break;
					case "d":
                        name += doubleName[rand.Next(0, doubleName.Length - 1)];						
						break;
					case "t":
                        name += _specialName[rand.Next(0, _specialName.Length - 1)];						
						break;
					default:
						break;
				}
			}
			return name;
		}

	    private void BackOnClick(GameObject go)
	    {
	        this.CloseView();
//            Singleton<SelectServerView>.Instance.OpenView();
			Singleton<LoginView>.Instance.OpenView ();
	    }

        //private void LimitNameShow()
        //{
        //    string roleName = input.label.text;
        //    if (roleName.Length > 6)
        //    {
        //        roleName = roleName.Substring(0, 6);
        //        roleName = roleName.Replace(" ", "");
        //        input.label.text = roleName;// "请输入角色名称";
        //    }
        //}

        /// <summary>
        /// 开始游戏 按钮 的点击响应处理函数
        /// </summary>
        /// <param name="go"></param>
        private void StartGame(GameObject go)
        {
            string roleName = input.value;
            //roleName = roleName.Substring(0, 6);
            roleName = roleName.Replace(" ", "");
            input.label.text = roleName;// "请输入角色名称";
            Log.debug(this, "角色名字： " + roleName);

            //有敏感词直接提示重新输入名字
            if (StringUtils.isEmpty(roleName) || roleName.Equals("请输入角色名称"))
            {
                input.label.text = "请输入角色名称";
                return;
            }
            if(Singleton<ChatView>.Instance.ContainsFilter(roleName))
            {
                MessageManager.Show("名字中包含敏感词汇！");
                input.label.text = "请输入角色名称";
                return;
            }
            //LoginControl con = AppFacde.instance.getControl(LoginControl.NAME) as LoginControl;
            //Log.info(this, "点击开始游戏按钮，创建角色！platformName:" + con.platformName + " 角色名称：" + roleName + " 角色类型：" + job + "性别ID：1" );
			Singleton<RoleMode>.Instance.roleName = roleName;
			Singleton<LoginMode>.Instance.createRole(job, 1, roleName);

            //LoginControl con = AppFacde.instance.getControl(LoginControl.NAME) as LoginControl;
            //(con.mode as com.game.module.login.LoginMode).LoginTest();
        }

        public override void CloseView()
        {
            if (ReferenceEquals(gameObject, null))
                return;
            //base.CloseView();
            StartPlayModelAnim(Status.CreateRoleIdleTwo);
            if (roleBustEffect != null)
            {
                roleBustEffect.SetActive(false);
                roleBustEffect.SetActive(true);
            }
            FindChild("btn_random").SetActive(false);
            FindChild("btn_start").SetActive(false);
            FindChild("ckb_mjs").SetActive(false);
            FindChild("ckb_qxs").SetActive(false);
            FindChild("ckb_yss").SetActive(false);
            FindChild("info").SetActive(false);
            FindChild("inp_name").SetActive(false);

            CoroutineManager.StartCoroutine(DelayClose());
        }

	    private IEnumerator DelayClose( )
	    {
	        yield return new WaitForSeconds(1.5f);
            Singleton<LoginControl>.Instance.ChangeToFirstScene();
            CoroutineManager.StopAllCoroutine();
            EffectMgr.Instance.RemoveEffect(SelectRoleStage2.name);
            EffectMgr.Instance.RemoveEffect(roleChoseEffect.name);
            EffectMgr.Instance.RemoveEffect(roleBustEffect.name);
            base.CloseView();
	    }


	    private RoleDisplay[] roles = new RoleDisplay[3]; //角色持有，避免重复加载
	    private test.Task[] tasks = new test.Task[3];
        protected  void LoadRoleMode()
        {
            if (roles[job - 1] == null)
            {
                roles[job - 1] = new RoleDisplay();
                roles[job - 1].CreateRole(job, LoadCallBack);
            }
            else
            {
                ShowMode(job);
            }
        }

	    private void OnRoleClick(GameObject obj)
	    {
            //int rolejob = job;
            //if (roles[rolejob - 1] != null && !ReferenceEquals(roles[rolejob - 1].GoBase, null))
            //{
            //    Animator animator = roles[rolejob - 1].GoBase.GetComponentInChildren<Animator>();
            //    if (!ReferenceEquals(animator, null))
            //    {
            //        animator.SetInteger(Status.STATU, Status.IDLE);
            //        animator.SetInteger(Status.STATU,Status.Win);
            //        CoroutineManager.StartCoroutine(SetRoleIdle(animator));
            //    }
            //}
            StartPlayModelAnim(Status.CreateRoleIdleThree);

	    }

	    public void LoadCallBack(GameObject go)
        {
            SetModelPosition(go);
            ShowMode(job);
        }

        private void SetModelPosition(GameObject go)
        {
            go.SetActive(false);
            go.transform.parent = NGUITools.FindChild(gameObject,"mode").transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
        }

	    private void ShowMode(int rolejob)
	    {
           // play.PlayForward();
            if (roleChoseEffect != null)
            {
                roleChoseEffect.SetActive(false);
            }
            for(int i=0;i<roles.Length;i++)
	        {
                if (roles[i] != null && !ReferenceEquals(roles[rolejob - 1].GoBase, null))
	            {
                    NGUITools.SetActive(roles[i].GoBase, i==(job-1));
	            }
	        }
	        if (roles[rolejob - 1] != null &&  !ReferenceEquals(roles[rolejob - 1].GoBase,null) )
	        {
	            modelSpin.target = roles[rolejob - 1].GoBase.transform.FindChild("101_0");
	            modelSpin.speed = 3f;
	        }
	        OnRoleModePlayEnd();
	    }

	    private void OnRoleModePlayEnd()
	    {
            StartPlayModelAnim(Status.CreateRoleIdleOne);
            CoroutineManager.StartCoroutine(DelayShowRoleChoseEffect());
	    }

	    private void StartPlayModelAnim(float type ,bool idle = true)
	    {
	        int rolejob = job;
            if (roles[rolejob - 1] != null && !ReferenceEquals(roles[rolejob - 1].GoBase, null))
            {
                Animator animator = roles[rolejob - 1].GoBase.GetComponentInChildren<Animator>();
                if (!ReferenceEquals(animator, null))
                {
                    //animator.SetInteger(Status.STATU, Status.IDLE);
                    animator.SetFloat(Status.IDLE_TYPE, type);
                    if (idle)
                    {
                        if (tasks[rolejob-1] != null)
                        {
                            CoroutineManager.StopCoroutine(tasks[rolejob-1]);
                        }
                        tasks[rolejob-1] = CoroutineManager.StartCoroutine(SetRoleIdle(animator));
                    }
                }
            }
        }

	    private IEnumerator SetRoleIdle(Animator animator)
	    {
	        if (!ReferenceEquals(animator, null))
	        {
	            while (!ReferenceEquals(animator, null) &&
	                   !(animator.IsInTransition(0) &&
	                     (animator.GetCurrentAnimatorStateInfo(0).nameHash == Status.NAME_HASH_Special ||
	                      animator.GetCurrentAnimatorStateInfo(0).nameHash == Status.NAME_HASH_Win)))
	            {
	                yield return 1;
	            }

	            if (!ReferenceEquals(animator, null))
	            {
	                animator.SetInteger(Status.STATU, Status.IDLE);
	                animator.SetFloat(Status.IDLE_TYPE, Status.FrontIdle);
	            }

	        }
	    }
	}
}
