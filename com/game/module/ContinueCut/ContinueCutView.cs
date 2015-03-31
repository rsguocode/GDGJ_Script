using com.game.consts;
using com.game.module.test;
using Holoville.HOTween;
using UnityEngine;

namespace Com.Game.Module.ContinueCut
{
    public class ContinueCutView : BaseView<ContinueCutView>
    {
        private int _cutNum;
        private UILabel _labCutNum; //连斩数量
        private float _leftTime;
        private UISprite _sprLeftTime; //剩余时间

        public override string url
        {
            get { return "UI/ContinueCut/ContinueCutView.assetbundle"; }
        }

        public override ViewLayer layerType
        {
            get { return ViewLayer.LowLayer; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }


        public override bool waiting
        {
            get { return false; }
        }

        protected override void Init()
        {
            firstOpen = false;
            _labCutNum = FindInChild<UILabel>("right/cutnum");
            _sprLeftTime = FindInChild<UISprite>("right/timeleft");
            StartScaleNum();
        }

        private void UpdateView()
        {
            if (null == gameObject || !gameObject.activeSelf)
            {
                OpenView();
            }
            else
            {
                UpdateInfo();
            }
        }

        private void StartScaleNum()
        {
            var sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));
            sequence.Append(HOTween.To(_labCutNum.gameObject.transform, 0.06f,
                new TweenParms().Prop("localScale", new Vector3(2f, 2f, 1), false)));
            sequence.Append(HOTween.To(_labCutNum.gameObject.transform, 0.12f,
                new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1), false).Delay(0.02f)));
            sequence.Insert(0, HOTween.To(_labCutNum, 0.06f, new TweenParms().Prop("color", ColorConst.Blood)));
            sequence.Insert(0.08F, HOTween.To(_labCutNum, 0.12f, new TweenParms().Prop("color", Color.white)));
            sequence.Play();
        }

        //显示数量
        public void ShowNum(int num)
        {
            _cutNum = num;
            ShowEffect();
            UpdateView();
        }

        private void ShowEffect()
        {
            StartScaleNum();
        }

        //显示剩余时间
        public void ShowLeftTime(float timeLeft)
        {
            _leftTime = timeLeft;
            UpdateView();
        }

        private void UpdateInfo()
        {
            _labCutNum.text = _cutNum.ToString();
            _sprLeftTime.fillAmount = _leftTime;
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            UpdateInfo();
        }
    }
}