using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using com.game.manager;
using com.game.module.hud.stub;
using com.game.module.test;
using com.game.Public.Hud;
using UnityEngine;
using Com.Game.Module.Chat;

namespace com.game.module.hud
{
    /// <summary>
    ///     头部信息、伤害跟随
    /// </summary>
    public class HudView : MonoBehaviour
    {
        public enum Type
        {
            Player,
            Npc,
            Monster,
        }

        public const string Prefix = "UI/Hud";
        public const string HealthUrl = "UI/Hud/Widget/Health.assetbundle";
        public const string BloodUrl = "UI/Hud/Widget/Blood.assetbundle";
        public const string NicknameUrl = "UI/Hud/Widget/Nickname.assetbundle";
        public const string HeadInfo = "UI/Hud/Widget/HeadInfo.assetbundle";
        public const string BossMsgUrl = "UI/Hud/Widget/BossMsg.assetbundle";
        public const string HeadInfoItemUrl = "UI/Hud/Widget/HeadInfoItem.assetbundle";
        private const string FilterZhUrl = "Xml/filters_zh.assetbundle";  //敏感词汇

        public static HudView Instance;

        protected GameObject BloodGo;
        protected GameObject BossMsgGo;
        protected GameObject ExclamationGo;
        protected GameObject HealthGo;
        protected GameObject InfoGo;

        protected Dictionary<string, UIFont> Maps = new Dictionary<string, UIFont>();
        public GameObject MountPoint;
        protected GameObject NicknameGo;
        protected GameObject QuestionGo;
        protected GameObject QuestionGo2;

        /// <summary>
        ///     添加伤害
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害值</param>
        public void AddDamage(GameObject target, int damage)
        {
            AddDamage(target, damage, Color.red);
        }

        /// <summary>
        ///     添加伤害
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害值</param>
        /// <param name="color"></param>
        public void AddDamage(GameObject target, int damage, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(String.Format("-{0}", damage), color, 0f);
        }

        /// <summary>
        ///     添加金币奖励
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="count">伤害值</param>
        /// <param name="color"></param>
        public void AddGold(GameObject target, int count, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.fontSize = 30;
            hudText.Add(String.Format("金币+{0}", count), color, 0f);
        }

        /// <summary>
        ///     在若干秒后添加伤害
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害</param>
        /// <param name="color">颜色</param>
        /// <param name="secs">秒</param>
        /// <returns>返回迭代器</returns>
        protected IEnumerator AddDamageAfter(GameObject target, int damage, Color color, float secs)
        {
            float endTime = Time.time + secs;
            while (Time.time < endTime)
            {
                yield return 0;
            }
            AddDamage(target, damage, color);
        }

        /// <summary>
        ///     添加增益
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="benefit">增益值</param>
        public void AddBenefit(GameObject target, int benefit)
        {
            AddBenefit(target, benefit, ColorPreds.GREEN);
        }

        /// <summary>
        ///     添加增益
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="benefit">增益值</param>
        /// <param name="color">字体颜色</param>
        public void AddBenefit(GameObject target, int benefit, string color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(String.Format("+{0}", benefit), Color.white, 0f);
        }

        /// <summary>
        ///     添加暴击
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害</param>
        /// <param name="color">字体颜色</param>
        public void AddCrit(GameObject target, int damage, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(String.Format("暴击-{0}", damage), color, 0f);
        }

        /// <summary>
        ///     添加闪避
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="color">字体颜色</param>
        public void AddDodge(GameObject target, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(String.Format("闪避"), color, 0f);
        }

        /// <summary>
        ///     添加格挡
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="damage">掉血</param>
        /// <param name="color">字体颜色</param>
        public void AddParray(GameObject target, int damage, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.bitmapFont = Maps[ColorPreds.OTHERS];
            hudText.Add("格挡", color, 0f);
            CoroutineManager.StartCoroutine(AddDamageAfter(target, damage, color, 0.3f));
        }

        /// <summary>
        ///     添加文本
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">文本</param>
        /// <param name="color">颜色</param>
        public void AddText(GameObject target, string text, Color color)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(text, color, 0f);
        }

        /// <summary>
        ///     添加文本
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">文本</param>
        /// <param name="color">颜色</param>
        /// <param name="duration">持续时间</param>
        public void AddText(GameObject target, string text, Color color, float duration)
        {
            var hudText = target.GetComponent<HUDText>();
            hudText.Add(text, color, duration - 1f);
        }

        /// <summary>
        ///     添加感叹号
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>游戏对象</returns>
        public GameObject AddExclamation(Transform target)
        {
            if (!target) return null;
            GameObject child = NGUITools.AddChild(MountPoint, ExclamationGo);
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            return child;
        }

        /// <summary>
        ///     添加问号
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>游戏对象</returns>
        public GameObject AddQuestion(Transform target)
        {
            if (!target) return null;
            GameObject child = NGUITools.AddChild(MountPoint, QuestionGo);
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            return child;
        }

        /// <summary>
        ///     添加问号
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="color">问号颜色</param>
        /// <returns>游戏对象</returns>
        public GameObject AddQuestion(Transform target, Color color)
        {
            if (!target) return null;
            GameObject child;
            if (color == Color.grey)
            {
                child = NGUITools.AddChild(MountPoint, QuestionGo2);
            }
            else
            {
                child = NGUITools.AddChild(MountPoint, QuestionGo);
            }
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            return child;
        }

        /// <summary>
        ///     添加项目
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>游戏对象</returns>
        public GameObject AddItem(Transform target)
        {
            if (!target) return null;
            GameObject child = NGUITools.AddChild(MountPoint, BloodGo);
            child.transform.localScale = new Vector3(1.0f, 1.3f, 1.0f); //使数字变高
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            return child;
        }

        /// <summary>
        ///     增加头顶信息显示
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public GameObject AddHeadInfo(Transform target)
        {
            if (!target) return null;
            GameObject child = NGUITools.AddChild(MountPoint, InfoGo);
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            return child;
        }

        /// <summary>
        ///     添加项目
        /// </summary>
        /// <param name="itemName">名称</param>
        /// <param name="target">目标</param>
        /// <param name="type">类型</param>
        /// <returns>游戏对象</returns>
        public GameObject AddItem(string itemName, Transform target, Type type)
        {
            if (!target) return null;

            GameObject child = null;
            UILabel label;
            if (type == Type.Player)
            {
                child = NGUITools.AddChild(MountPoint, NicknameGo);
                label = child.GetComponentInChildren<UILabel>();
                label.color = Color.white;
                label.text = itemName;
            }
            else if (type == Type.Npc)
            {
                child = NGUITools.AddChild(MountPoint, NicknameGo);
                label = child.GetComponentInChildren<UILabel>();
                label.color = Color.yellow;
                label.text = itemName;
            }
            else if (type == Type.Monster && itemName != "msg")
            {
                child = NGUITools.AddChild(MountPoint, HealthGo);
            }
            else if (type == Type.Monster && itemName == "msg")
            {
                child = NGUITools.AddChild(MountPoint, BossMsgGo);
            }
            var followTarget = child.AddComponent<UIFollowTarget>();
            followTarget.target = target;
            //设置child的初始位置在相机外，防止child刚出现的时候在屏幕中间闪烁
            var tempPosition = new Vector3(-1000, 0, 0);
            child.transform.position = tempPosition;
            return child;
        }

        /// <summary>
        ///     更新血条
        /// </summary>
        /// <param name="go">游戏对象</param>
        /// <param name="hp">总血量</param>
        /// <param name="curHp">剩余血量</param>
        public void UpdateHealthBar(GameObject go, float hp, float curHp)
        {
            if (go == null) return;
            var slider = go.GetComponentInChildren<UISlider>();
            if (slider != null) slider.value = curHp/hp;
        }

        /// <summary>
        ///     更新血条
        /// </summary>
        /// <param name="slider">进度条</param>
        /// <param name="hp">总血量</param>
        /// <param name="curHp">剩余血量</param>
        public void UpdateHealthBar(UISlider slider, int hp, int curHp)
        {
            if (!slider) return;
            slider.value = curHp/(float) hp;
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        protected void Init()
        {
            Instance = this;
            //CreateMap();
        }

        /// <summary>
        ///     开始加载相关头顶显示资源，但是需要字体加载完后再加载
        /// </summary>
        public void StartLoadAsset()
        {
            AssetManager.Instance.LoadAsset<GameObject>(NicknameUrl, LoadNickNameCallback);
            AssetManager.Instance.LoadAsset<GameObject>(HealthUrl, LoadHealthCallback);
            AssetManager.Instance.LoadAsset<GameObject>(BloodUrl, LoadBloodCallback);
            AssetManager.Instance.LoadAsset<GameObject>(HeadInfo, LoadHeadInfoCallback);
            AssetManager.Instance.LoadAsset<GameObject>(BossMsgUrl, LoadBossMsgCallback);
            AssetManager.Instance.LoadAsset<GameObject>(HeadInfoItemUrl, LoadHeadInfoItemCallback);
            AssetManager.Instance.LoadAsset<TextAsset>(FilterZhUrl, LoadFilterZhCallback);
        }

        private void LoadFilterZhCallback(TextAsset filterText)
        {
            Singleton<ChatMode>.Instance.FilterString = filterText.ToString().Split('、');
        }

        private void LoadHeadInfoItemCallback(GameObject gObject)
        {
            HeadInfoItemPool.Instance.Init(gObject.transform);
        }

        private void LoadHeadInfoCallback(GameObject gObject)
        {
            InfoGo = gObject;
        }

        private void LoadNickNameCallback(GameObject gObject)
        {
            NicknameGo = gObject;
        }

        private void LoadHealthCallback(GameObject gObject)
        {
            HealthGo = gObject;
        }

        private void LoadBloodCallback(GameObject gObject)
        {
            BloodGo = gObject;
        }

        private void LoadBossMsgCallback(GameObject gObject)
        {
            BossMsgGo = gObject;
        }

        /// <summary>
        ///     创建字体映射
        /// </summary>
        protected void CreateMap()
        {
            FieldInfo[] fields = typeof (ColorPreds).GetFields();
            foreach (FieldInfo field in fields)
            {
                string color = field.GetValue(null).ToString();
                string url = String.Format("{0}/Widget/Typeface/{1}/{1}.assetbundle", Prefix, color);
                AssetManager.Instance.LoadAsset<GameObject>(url, LoadBitmapFontBack);
            }
        }

        private void LoadBitmapFontBack(GameObject gObject)
        {
            Maps.Add(gObject.name, gObject.GetComponent<UIFont>());
        }
    }
}