using com.game.module.test;
using UnityEngine;
using Com.Game.Module.GoldSilverIsland;
using com.game.Public.Message;
using PCustomDataType;
using com.game.Public.Confirm;
using com.game.manager;
using Com.Game.Module.Chat;

namespace com.game.module.main
{
    public class MainView : BaseView<MainView>
    {
        private GameObject topright;

		private GameObject noticeObj;
		private UILabel labNotice;

        public override string url
        {
            get { return "UI/Main/MainView.assetbundle"; }
        }

		public override bool playClosedSound { get { return false; } }

        public override ViewLayer layerType
        {
            get { return ViewLayer.LowLayer; }
        }

        public override bool waiting
        {
            get { return false; }
        }


        protected override void Init()
        {
            topright = FindChild("topright");
            Singleton<MainLeftView>.Instance.gameObject = FindChild("Left");
            //Singleton<MainLeftView>.Instance.Init();

            Singleton<MainBottomLeftView>.Instance.gameObject = FindChild("bottomleft");
            //Singleton<MainBottomLeftView>.Instance.Init();

            Singleton<MainBottomRightView>.Instance.gameObject = FindChild("bottomright");
            //Singleton<MainBottomRightView>.Instance.Init();

            Singleton<MainTopLeftView>.Instance.gameObject = FindChild("topleft");
            //Singleton<MainTopLeftView>.Instance.Init();

            Singleton<MainTopRightView>.Instance.gameObject = FindChild("topright");
            //Singleton<MainTopRightView>.Instance.Init();

			noticeObj = FindInChild<Transform>("notice").gameObject;
			labNotice = FindInChild<UILabel>("notice/label");
			noticeObj.SetActive(false);
        }

        public void OpenViewBy()
        {
            OpenView();
            //topright.SetActive(false);
        }

        public void CloseViewByCopy()
        {
            topright.SetActive(false);
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<MainLeftView>.Instance.OpenView();
            Singleton<MainBottomLeftView>.Instance.OpenView();
            Singleton<MainBottomRightView>.Instance.OpenView();
            Singleton<MainTopLeftView>.Instance.OpenView();
            Singleton<MainTopRightView>.Instance.OpenView();
            //topright.SetActive(false);
        }

        protected override void HandleBeforeCloseView()
        {
            Singleton<MainLeftView>.Instance.CloseView();
            Singleton<MainBottomLeftView>.Instance.CloseView();
            Singleton<MainTopLeftView>.Instance.CloseView();
            Singleton<MainTopRightView>.Instance.CloseView();
            Singleton<MainBottomRightView>.Instance.CloseView();
            //topright.SetActive(false);
        }

        public override void Update()
        {
            Singleton<MainBottomLeftView>.Instance.Update();
        }

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateFriendReplyHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateInviteRequestHandle;	
			Singleton<ChatMode>.Instance.dataUpdated += UpdateNoticeHandle;
			Singleton<ChatMode>.Instance.dataUpdated += UpdateRumorHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateFriendReplyHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateInviteRequestHandle;	
			Singleton<ChatMode>.Instance.dataUpdated -= UpdateNoticeHandle;
			Singleton<ChatMode>.Instance.dataUpdated -= UpdateRumorHandle;
		}

		private void UpdateFriendReplyHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_FRIEND_REPLY == type)
			{
				UpdateFriendReply();
			}
		}

		private void UpdateInviteRequestHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_INVITE_REQUEST == type)
			{
				UpdateInviteRequest();
			}
		}

		private void UpdateNoticeHandle(object sender, int type)
		{
			if (Singleton<ChatMode>.Instance.UPDATE_NOTICE == type)
			{
				UpdateNoticeRequest(Singleton<ChatMode>.Instance.Notice);
			}
		}

		private void UpdateRumorHandle(object sender, int type)
		{
			if (Singleton<ChatMode>.Instance.UPDATE_RUMOR == type)
			{
				UpdateNoticeRequest(Singleton<ChatMode>.Instance.Rumor);
			}
		}

		private void UpdateNoticeRequest(string notice)
		{
			float screenHalfSize = 512f;
			float moveSpeed = 100f;

			labNotice.text = notice;

			Vector3 from = labNotice.transform.localPosition;
			from.x = screenHalfSize;

			Vector3 to  = from;
			to.x = -NGUIText.CalculatePrintedSize(labNotice.text).x - screenHalfSize;

			noticeObj.SetActive(true);
			float moveTime = (from.x - to.x) / moveSpeed;

			ShowNoticeMoveEffect(labNotice, from, to, moveTime);
			vp_Timer.In(moveTime, FinishNoticeMove, 1, 0);
		}

		private void ShowNoticeMoveEffect(UILabel labNotice, Vector3 from, Vector3 to, float moveTime)
		{
			TweenPosition tweenPosition = labNotice.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}
			
			tweenPosition = labNotice.gameObject.AddComponent<TweenPosition>();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.None;
			tweenPosition.duration = moveTime;
		}

		private void FinishNoticeMove()
		{
			noticeObj.SetActive(false);
		}

		private void UpdateFriendReply()
		{
			uint assistId = Singleton<GoldSilverIslandMode>.Instance.SelectAssistId;
			byte friendReply = Singleton<GoldSilverIslandMode>.Instance.FriendReply;
			PWoodsFriendInfo item = Singleton<GoldSilverIslandMode>.Instance.GetAssist(assistId);

			if ((byte)AssistReply.Accept == friendReply)
			{
				string[] param = {item.name};
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.FriendAcceptInvite", param));
			}
			else
			{
				string[] param = {item.name};
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.FriendDenyInvite", param));
			}

			Singleton<GoldSilverIslandMode>.Instance.GetCurAssistInfo();
		}

		private void UpdateInviteRequest()
		{
			if (0 == Singleton<GoldSilverIslandMode>.Instance.AssistRemainTimes)
			{
				return; 
			}

			string name = Singleton<GoldSilverIslandMode>.Instance.InviterName;
			string[] param = {name};
			ConfirmMgr.Instance.ShowCommonAlert(LanguageManager.GetWord("GoldSilverIsland.FriendInvite", param), ConfirmCommands.OK_CANCEL, 
			                                    AcceptInvite, LanguageManager.GetWord("GoldSilverIsland.Accept"), 
			                                    DenyInvite, LanguageManager.GetWord("GoldSilverIsland.Deny"));
		}

		private void AcceptInvite()
		{
			uint id = Singleton<GoldSilverIslandMode>.Instance.InviterId;
			Singleton<GoldSilverIslandMode>.Instance.ReplyInvite(0, id); 
		}

		private void DenyInvite()
		{
			uint id = Singleton<GoldSilverIslandMode>.Instance.InviterId;
			Singleton<GoldSilverIslandMode>.Instance.ReplyInvite(1, id); 
		}
    }
}