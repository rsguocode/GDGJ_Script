using com.game.utils;
using com.game.module.test;
using System.Collections.Generic;
using com.game.manager;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.vo;
using UnityEngine;
using Com.Game.Module.Copy;
using Com.Game.Speech;

namespace com.game.module.loading
{
        /// <summary>
        /// 初始化加载视图
        /// </summary>
        public class StartLoadingView : BaseView<StartLoadingView>
        {
            public List<AssetBundleLoader>  loaderList= new List<AssetBundleLoader>();

            public  int PreLoadNum { get; set; } //需预加载的资源
            public  int PreLoadedNum { get; set; } //已经预加载的资源
            

            /// <summary>
            /// 预设路径
            /// </summary>
            public override string url { get { return "UI/Loading/LoadingView.assetbundle"; } }
			public override bool playClosedSound { get { return false; } }

            private GameObject _loadAnimationModel;
            private Button _leftChestButton;
            private Button _rightChestButton;
            private GameObject _hand;
            private GameObject _bottomGameObject;

			private int interal = 30;
			private int index;
			private int lastComplete ;
            private int _currentStatu;
            //private GameObject effect;

            public override bool IsFullUI
            {
                get { return true; }
            }

            public override ViewLayer layerType
            {
                get { return ViewLayer.HighLayer; }
            }

            public override bool waiting{get{return false;}}
            public override bool isDestroy
            {
                get
                {
                    return false;
                }
            }


            private UILabel labelLoading;
            private UISlider slider;

            protected override void Init()
            {
                slider = Tools.find(gameObject, "center/offset/Bottom/loading/silder").GetComponent<UISlider>();
                labelLoading = Tools.find(gameObject, "center/offset/Bottom/LoadingInfo/loadinglabel").GetComponent<UILabel>();
                _leftChestButton = FindInChild<Button>("center/offset/Buttons/LeftChestBtn");
                _rightChestButton = FindInChild<Button>("center/offset/Buttons/RightChestBtn");
                _leftChestButton.onClick = OnLeftChestButtonClick;
                _rightChestButton.onClick = OnRightChestButtonClick;
                _hand = FindChild("center/offset/Bottom/Hand");
                _hand.SetActive(false);
                _bottomGameObject = FindChild("center/offset/Bottom");
                NGUITools.SetLayer(_bottomGameObject,LayerMask.NameToLayer("TopUI"));
                //effect = Tools.find(gameObject, "center/offset/loading/silder/light/30020/effect");
                //effect.SetActive(false);
                _loadAnimationModel = FindChild("center/offset/Model");
                NGUITools.SetLayer(_loadAnimationModel, LayerMask.NameToLayer("TopUI"));
                slider.value = 0;
            }

            private void OnLeftChestButtonClick(GameObject go)
            {
                if (_loadAnimationModel == null)
                {
                    return;
                }
                if (_currentStatu != Status.ATTACK1)
                {
                    _loadAnimationModel.GetComponentInChildren<Animator>().SetInteger(Status.STATU, Status.ATTACK1);
                    _currentStatu = Status.ATTACK1;
                }
                else
                {
                    _loadAnimationModel.GetComponentInChildren<Animator>().SetInteger(Status.STATU, Status.ATTACK2);
                    _currentStatu = Status.ATTACK2;
                }
                vp_Timer.In(0.1f, ResetModelStatu);
				//女神，你尖叫吧
				SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicBeautyCry);
            }


            private void ResetModelStatu()
            {
                if (_loadAnimationModel != null && _loadAnimationModel.GetComponentInChildren<Animator>()!=null)
                {
                    _loadAnimationModel.GetComponentInChildren<Animator>().SetInteger(Status.STATU, Status.IDLE);
                }
            }

            private void OnRightChestButtonClick(GameObject go)
            {
                if (_loadAnimationModel == null)
                {
                    return;
                }
                if (_currentStatu != Status.ATTACK1)
                {
                    _currentStatu = Status.ATTACK1;
                    _loadAnimationModel.GetComponentInChildren<Animator>().SetInteger(Status.STATU, Status.ATTACK1);
                }
                else
                {
                    _currentStatu = Status.ATTACK2;
                    _loadAnimationModel.GetComponentInChildren<Animator>().SetInteger(Status.STATU, Status.ATTACK2);
                }
                vp_Timer.In(0.1f, ResetModelStatu);
				//女神，你尖叫吧
				SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicBeautyCry);
            }

            protected override void HandleAfterOpenView()
            {
                Singleton<CopyView>.Instance.CloseView();
                slider.value = 0;
                labelLoading.text = "游戏资源正在加载：0%";
                /*if (loadAnimationModel == null)
                {
                    var displayVo = new DisplayVo();
                    displayVo.Type = DisplayType.ANIMATION;
                    var display = new BaseDisplay(); //(PlayerDisplay)ObjectPoolMgr.get(PlayerDisplay.NAME);
                    displayVo.ClothUrl = "Model/Load/10001/Model/BIP.assetbundle";
                    displayVo.ModelLoadCallBack = LoadAnimationModelBack;
                    display.SetVo(displayVo);
                }
                else
                {
                    loadAnimationModel.SetActive(true);
                }*/
                //NGUITools.SetLayer(effect, LayerMask.NameToLayer("TopUI"));
                //effect.SetActive(true);
            }

            /*private void LoadAnimationModelBack(BaseDisplay display)
            {
                loadAnimationModel = display.GoBase;
                loadAnimationModel.transform.localScale = new Vector3(0.52f, 0.52f, 1);
                loadAnimationModel.transform.localPosition = new Vector3(-0.58f, -1.1f, 0);
                NGUITools.SetLayer(loadAnimationModel, LayerMask.NameToLayer("LoadingModel"));
                Object.DontDestroyOnLoad(loadAnimationModel);
                if (gameObject==null||!gameObject.activeInHierarchy)
                {
                    loadAnimationModel.SetActive(false);
                }
                //_hand.SetActive(true);
            }*/

            protected override void HandleBeforeCloseView()
            {
                base.HandleBeforeCloseView();
                if (_loadAnimationModel != null)
                {
                    ResetModelStatu();
                    vp_Timer.CancelAll("ResetModelStatu");
                    //loadAnimationModel.SetActive(false);
                }
            }

            public override void Update()
            {
				//之前的版本实现
                /*float current = getProgress();
                float last = slider.value;

                if (last != current)
                {
                    slider.value = last + 0.01f;
                }
                if (current > 0.999f)
                {
                    slider.value = 1;
                }*/
			    //end 
				
				float value = getProgress();
                value += PreLoadedNum*0.01f;

				slider.value = value;
                labelLoading.text = "游戏资源正在加载："+(int)(slider.value*100)+"%";
            }

            public override void CloseView()
            {
                //effect.SetActive(false);
                slider.value = 0;
                loaderList.Clear();
                PreLoadNum = 0;
                PreLoadedNum = 0;
                base.CloseView();

            }

            public override void Destroy()
            {
                slider = null;
                labelLoading = null;
                loaderList.Clear();
                base.Destroy();

            }

            /// <summary>
            /// 获取当前的加载进度
            /// </summary>
            /// <returns>加载进度</returns>
            private float getProgress() {
                if (loaderList != null)
                {   
                    int size = loaderList.Count;
                    //Log.info(this, " loaderList Size : " + size + " " + PreLoadNum + "  " + PreLoadedNum);
                    float progress = 0f;
					int temp = 0;
                    if (size != 0)
                    {
                        foreach (AssetBundleLoader loader in loaderList)
                        {
							if(loader.progress == 1f)
								temp++;

                            //progress += loader.progress;
                        }
						if(temp !=lastComplete)
						{
							index = 0;
							lastComplete = temp;
						}
						else{
							index ++;
							if(index == interal)
							{
								index = interal -1;
							}
						}
					    progress = lastComplete /(float)size + index /(float) interal /(float)size;
                        if (progress > 1)
                        {
                            progress = 1f;
                        }
                        //progress = progress / size;
                    }
					//Log.info(this," loaderList Size : " + size + " progress : "+ progress);
                    //Log.info(this, " loaderList progress : " + (progress * (1f - PreLoadNum * 0.01f)));
                    return progress * (1f - PreLoadNum * 0.01f);
                    
                }
                else {
                    return 1f - PreLoadNum * 0.01f;
                }
            }

        }

}
