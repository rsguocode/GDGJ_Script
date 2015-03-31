using com.game.module.test;
using UnityEngine;
using com.game.consts;
using System.IO;
using Com.Game.Module.Copy;
using com.game.manager;
using Com.Game.Module.Copy;
using com.game.vo;
using Com.Game.Module.GoldHit;

namespace com.game.module.GameSetting
{
    public class GameSettingView : BaseView<GameSettingView>
    {
        public override string url { get { return "UI/GameSetting/GameSettingView.assetbundle"; } }
        public override ViewLayer layerType		
        {
            get { return ViewLayer.HighLayer; }
        }
		public override bool playClosedSound { get { return false; } }
        private Button _btnJx;
        private Button _btnSz;
        private Button _btnTc;
        private UILabel _jxLabel;
        private UILabel _szLabel;
        private UILabel _tcLabel;

        protected override void Init()
        {
            _btnJx = FindInChild<Button>("btn_jx");
            _btnSz = FindInChild<Button>("btn_sz");
            _btnTc = FindInChild<Button>("btn_tc");
            _jxLabel = FindInChild<UILabel>("btn_jx/label");
            _szLabel = FindInChild<UILabel>("btn_sz/label");
            _tcLabel = FindInChild<UILabel>("btn_tc/label");
            _btnJx.onClick += GoOn;
            _btnTc.onClick += QuiteCopy;
            InitLabel();
        }

        private void InitLabel() {
            _jxLabel.text = LanguageManager.GetWord("GameSettingView.GoOn");
            _szLabel.text = LanguageManager.GetWord("GameSettingView.Setting");
            _tcLabel.text = LanguageManager.GetWord("GameSettingView.EndCopy");
        }

        protected override void HandleAfterOpenView()
        {
            Time.timeScale = 0;
			Singleton<CopyMode>.Instance.PauseCopy();            
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        /// <param name="go"></param>
        private void GoOn(GameObject go)
        {
            CloseView();
            Time.timeScale = 1;
			Singleton<CopyMode>.Instance.ResumeCopy(); 
        }

        /// <summary>
        /// 退出副本,请求回到主城
        /// </summary>
        /// <param name="go"></param>
        private void QuiteCopy(GameObject go)
        {
            CloseView();
            Time.timeScale = 1;
            Singleton<CopyMode>.Instance.ApplyQuitCopy();
        }
    }
}
