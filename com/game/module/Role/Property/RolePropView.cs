using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.vo;
using com.game.consts;
using com.game.manager;
using Com.Game.Module.Role;
using com.game.Public.Message;
using com.u3d.bases.debug;

namespace Com.Game.Module.Role
{
    public class RolePropView: BaseView<RolePropView>
    {
        public override string url { get { return "UI/Property/Property.assetbundle"; } }
		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		//动态组件
		private UILabel lab_xm;  //名字
		private UILabel lab_dj;  //等级
		private UISprite spr_mjs;  //魔剑士
		private UISprite spr_qjs;  //枪械士
		private UISprite spr_yss;  //元素士
		//private UILabel lab_zdl; //战斗力
		
		private UISlider sld_jy;  //经验
		private UILabel lab_jy;
		private UISlider sld_sm; //生命
		private UILabel lab_sm;
		private UISlider sld_mf;  //魔法
		private UILabel lab_mfa;
		private UILabel lab_zy;  //阵营
		private Button btn_ch;  //称号
		
		private UILabel lab_ll; //力量
		private UILabel lab_tz; //体质
		private UILabel lab_mj; //敏捷
		private UILabel lab_zl; //智力
		
		private UILabel lab_gj; //攻击
		private UILabel lab_wf; //物防
		private UILabel lab_mf; //魔防
		private UILabel lab_mz; //命中
		private UILabel lab_sb; //闪避
		private UILabel lab_bj; //暴击
		private UILabel lab_rx; //韧性
		private UILabel lab_bs; //暴伤
		private UILabel lab_xy; //幸运
		private UILabel lab_js; //减伤

		//静态组件
		//private UILabel lab_zhanlitag;  //战力静态标签
		private UILabel lab_jytag;      //经验静态标签
		private UILabel lab_smtag;      //生命静态标签
		private UILabel lab_mofatag;    //魔法静态标签
		private UILabel lab_zytag;      //阵营静态标签

		private UILabel lab_lltag;      //力量静态标签
		private UILabel lab_tztag;      //体质静态标签
		private UILabel lab_mjtag;      //敏捷静态标签
		private UILabel lab_zltag;      //智力静态标签

		private UILabel lab_gjtag;      //攻击静态标签
		private UILabel lab_wftag;      //物防静态标签
		private UILabel lab_mftag;      //魔防静态标签
		private UILabel lab_mztag;      //命中静态标签
		private UILabel lab_sbtag;      //闪避静态标签
		private UILabel lab_bjtag;      //暴击静态标签
		private UILabel lab_rxtag;      //韧性静态标签
		private UILabel lab_bstag;      //暴伤静态标签
		private UILabel lab_xytag;      //幸运静态标签
		private UILabel lab_jstag;      //减伤静态标签

		public float fX = 727;  
		public float fY = 389;

        private UILabel jobName;  //职业名字
        private UILabel zhanLi;
		protected override void Init()
		{
			//设置父对象为角色主面板
			//transform.parent = Singleton<RoleView>.Instance.transform;
            

			//获取动态静态组件引用
			GetDynamicComponent();
			GetStaticComponent();

			//从资源包中获取静态组件文字
			LoadStaticComponentContent();
            gameObject.SetActive(false);
            zhanLi = FindInChild<UILabel>("zl");
            //Singleton<RoleView>.Instance.CloseCurrentTab();
            
            //gameObject.SetActive(true);
            //Singleton<RoleView>.Instance.currentTab = gameObject;
		}

		//获得动态组件，这些组件内容会根据协议数据更新而变化
		private void GetDynamicComponent()
		{
            jobName = FindInChild<UILabel>("left/role_type");
			lab_xm = FindInChild<UILabel>("left/xm");
			lab_dj = FindInChild<UILabel>("left/dj");
			spr_mjs = FindInChild<UISprite>("left/mjs");
			spr_qjs = FindInChild<UISprite>("left/qjs");
			spr_yss = FindInChild<UISprite>("left/yss");
			//lab_zdl = FindInChild<UILabel>("left/zhanli/shuzi");
			
			sld_jy = FindInChild<UISlider>("right/sx/sld_jy");
			lab_jy = FindInChild<UILabel>("right/sx/sld_jy/label");
			sld_sm = FindInChild<UISlider>("right/sx/sld_sm");
			lab_sm = FindInChild<UILabel>("right/sx/sld_sm/label");
			sld_mf = FindInChild<UISlider>("right/sx/sld_mf");
			lab_mfa = FindInChild<UILabel>("right/sx/sld_mf/label");
			lab_zy = FindInChild<UILabel>("right/sx/zy");
			btn_ch = FindInChild<Button>("right/sx/btn_ch");
			
			lab_ll = FindInChild<UILabel>("right/liliang/liliang/ll");
			lab_tz = FindInChild<UILabel>("right/liliang/liliang/tz");
			lab_mj = FindInChild<UILabel>("right/liliang/liliang/mj");
			lab_zl = FindInChild<UILabel>("right/liliang/liliang/zl");
			
			lab_gj = FindInChild<UILabel>("right/liliang/gj");
			lab_wf = FindInChild<UILabel>("right/liliang/wf");
			lab_mf = FindInChild<UILabel>("right/liliang/mf");
			lab_mz = FindInChild<UILabel>("right/liliang/mz");
			lab_sb = FindInChild<UILabel>("right/liliang/sb");
			lab_bj = FindInChild<UILabel>("right/liliang/bj");
			lab_rx = FindInChild<UILabel>("right/liliang/rx");
			lab_bs = FindInChild<UILabel>("right/liliang/bs");
			lab_xy = FindInChild<UILabel>("right/liliang/xy");
			lab_js = FindInChild<UILabel>("right/liliang/js");
            btn_ch.onClick = OnClickChoose; //称号选择
		}

		//获得静态组件，这些组件内容从配置获取，支持多国语言
		private void GetStaticComponent()
		{
			//lab_zhanlitag = FindInChild<UILabel>("left/zhanli/label");
			lab_jytag = FindInChild<UILabel>("right/sx/dj");
			lab_smtag = FindInChild<UILabel>("right/sx/sm");
			lab_mofatag = FindInChild<UILabel>("right/sx/mf");
			lab_zytag = FindInChild<UILabel>("right/sx/labzy");

			lab_lltag = FindInChild<UILabel>("right/liliang/liliang/labll");
			lab_tztag = FindInChild<UILabel>("right/liliang/liliang/labtz");
			lab_mjtag = FindInChild<UILabel>("right/liliang/liliang/labmj");
			lab_zltag = FindInChild<UILabel>("right/liliang/liliang/labzl");

			lab_gjtag = FindInChild<UILabel>("right/liliang/labgj");
			lab_wftag = FindInChild<UILabel>("right/liliang/labwf");
			lab_mftag = FindInChild<UILabel>("right/liliang/labmf");
			lab_mztag = FindInChild<UILabel>("right/liliang/labmz");
			lab_sbtag = FindInChild<UILabel>("right/liliang/labsb");
			lab_bjtag = FindInChild<UILabel>("right/liliang/labbj");
			lab_rxtag = FindInChild<UILabel>("right/liliang/labrx");
			lab_bstag = FindInChild<UILabel>("right/liliang/labbs");
			lab_xytag = FindInChild<UILabel>("right/liliang/labxy");
			lab_jstag = FindInChild<UILabel>("right/liliang/labjs");
		}

		//加载静态组件文字
		private void LoadStaticComponentContent()
		{
			//lab_zhanlitag.text = LanguageManager.GetWord("RolePropView.zhanli");
			lab_jytag.text = LanguageManager.GetWord("RolePropView.jingyan");
			lab_smtag.text = LanguageManager.GetWord("RolePropView.shengming");
			lab_mofatag.text = LanguageManager.GetWord("RolePropView.mofa");
			lab_zytag.text = LanguageManager.GetWord("RolePropView.zhenyin");
			
			lab_lltag.text = LanguageManager.GetWord("RolePropView.liliang");
			lab_tztag.text = LanguageManager.GetWord("RolePropView.tizhi");
			lab_mjtag.text = LanguageManager.GetWord("RolePropView.minjie");
			lab_zltag.text = LanguageManager.GetWord("RolePropView.zhili");

			if (GameConst.JOB_FASHI == MeVo.instance.job)
			{
				lab_gjtag.text = LanguageManager.GetWord("RolePropView.mogongji");
			}
			else
			{
				lab_gjtag.text = LanguageManager.GetWord("RolePropView.wugongji");
			}

			lab_wftag.text = LanguageManager.GetWord("RolePropView.wufang");
			lab_mftag.text = LanguageManager.GetWord("RolePropView.mofang");
			lab_mztag.text = LanguageManager.GetWord("RolePropView.mingzhong");
			lab_sbtag.text = LanguageManager.GetWord("RolePropView.shanbi");
			lab_bjtag.text = LanguageManager.GetWord("RolePropView.baoji");
			lab_rxtag.text = LanguageManager.GetWord("RolePropView.renxing");
			lab_bstag.text = LanguageManager.GetWord("RolePropView.baoshang");
			lab_xytag.text = LanguageManager.GetWord("RolePropView.xingyun");
			lab_jstag.text = LanguageManager.GetWord("RolePropView.jianshang");
		}
        /*public override void OpenView()
        {
         	gameObject.SetActive(true);
            RegisterUpdateHandler();
            HandleAfterOpenView();
			//GameObject role = GameObject.Find("RoleDisplay");
			//role.transform.localPosition = new Vector3(-1.13f*(Screen.width/fX), -0.6934785f*(Screen.height/fY), 0);
			//role.transform.localScale  = new Vector3((Screen.width/fX), (Screen.height/fY), 0)*0.63f;
		}*/

		//虽然该节目没有动态数据变更，但是还是要注册数据更新，放在网络延时没有更新
		public override void RegisterUpdateHandler()
		{
			Singleton<RoleMode>.Instance.dataUpdated += UpdateRoleInfoHander;
		}
		public override void CancelUpdateHandler()
		{
			Singleton<RoleMode>.Instance.dataUpdated -= UpdateRoleInfoHander;
		}
		private void UpdateRoleInfoHander(object sender,int code)
		{
			if(code == Singleton<RoleMode>.Instance.UPDATE_ROLE_ATTR)
			{
				ShowProperty();
			}
		}
        public override void CloseView()
        {
            gameObject.SetActive(false);
            HandleBeforeCloseView();
            CancelUpdateHandler();
        }

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			vp_Timer.In(0.2f, ShowProperty, 1, 0f);            
		}
        
		private void ShowJobIcon(int job)
		{
			spr_mjs.gameObject.SetActive(false);
			spr_qjs.gameObject.SetActive(false);
			spr_yss.gameObject.SetActive(false);

			if (GameConst.JOB_JIAN == job)
            {
                jobName.text = "魔剑士";
                spr_yss.gameObject.SetActive(true);
			}
            else if (GameConst.JOB_FASHI == job)
            {
                jobName.text = "元素师";
                spr_qjs.gameObject.SetActive(true);
			}
			else
            {
                jobName.text = "潜行者";
                spr_mjs.gameObject.SetActive(true);
			}	            
		}
		
		///显示角色属性
		private void ShowProperty()
		{
            zhanLi.text = LanguageManager.GetWord("Goods.FightPoint") + ": " + MeVo.instance.fightPoint;
			lab_xm.text = MeVo.instance.Name;
			lab_dj.text = "Lv." + MeVo.instance.Level.ToString();
			//lab_zdl.text = MeVo.instance.fightPoint.ToString();
			ShowJobIcon(MeVo.instance.job);
			
			sld_jy.value = (float)MeVo.instance.exp / MeVo.instance.expFull;
			lab_jy.text = MeVo.instance.exp.ToString() + "/" + MeVo.instance.expFull.ToString();
			
			sld_sm.value = (float)MeVo.instance.CurHp / MeVo.instance.Hp;
			lab_sm.text = MeVo.instance.CurHp.ToString() + "/" + MeVo.instance.Hp.ToString();
			
			sld_mf.value = (float)MeVo.instance.CurMp / MeVo.instance.Mp;
			lab_mfa.text = MeVo.instance.CurMp.ToString() + "/" + MeVo.instance.Mp.ToString();
			
			lab_zy.text = MeVo.instance.guildName;
			
			lab_ll.text = MeVo.instance.Str.ToString();
			lab_tz.text = MeVo.instance.Phy.ToString();
			lab_mj.text = MeVo.instance.Agi.ToString();
			lab_zl.text = MeVo.instance.Wit.ToString();			

			if (GameConst.JOB_FASHI == MeVo.instance.job)
			{
				lab_gj.text = MeVo.instance.AttMMin.ToString() + "-" + MeVo.instance.AttMMax.ToString();
			}
			else
			{
				lab_gj.text = MeVo.instance.AttPMin.ToString() + "-" + MeVo.instance.AttPMax.ToString();
			}

			lab_wf.text = MeVo.instance.DefP.ToString();
			lab_mf.text = MeVo.instance.DefM.ToString();
			lab_mz.text = MeVo.instance.Hit.ToString();
			lab_sb.text = MeVo.instance.Dodge.ToString();
			lab_bj.text = MeVo.instance.Crit.ToString();
			lab_rx.text = MeVo.instance.Flex.ToString();
			lab_bs.text = MeVo.instance.CritRatio.ToString();
			lab_xy.text = MeVo.instance.Luck.ToString();
			lab_js.text = MeVo.instance.HurtRe.ToString();
		}

        //称号选择
        private void OnClickChoose(GameObject go)
        {
            MessageManager.Show("点击了称号选择！");
        }
		
	}
}

