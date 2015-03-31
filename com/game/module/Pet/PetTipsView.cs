
using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using PCustomDataType;
using UnityEngine;

namespace Com.Game.Module.Pet
{
    class PetTipsView : Singleton<PetTipsView>
    {

        private GameObject gameObject;
        private UILabel name;
        private UILabel tips;

        public TweenPlay tipsPlay;

        private bool close;

        private GameObject light;

        private List<GameObject> stars;

        private uint  starNum;

        private bool newPet;
        public EventDelegate.Callback AfterOpenViewGuideDelegate;
        public Button CloseButton;

        public void Init(GameObject obj)
        {
            gameObject = obj;
            obj.SetActive(false);
            name = NGUITools.FindInChild<UILabel>(gameObject, "info/name");
            tips = NGUITools.FindInChild<UILabel>(gameObject, "info/tips");
            CloseButton = NGUITools.FindInChild<Button>(gameObject, "info/button");
            CloseButton.onClick = OnCloseClick;

            NGUITools.FindInChild<UIWidgetContainer>(gameObject, "background").onClick = OnCloseClick;


            light = NGUITools.FindChild(gameObject, "info/light");

            tipsPlay = NGUITools.FindInChild<TweenPlay>(gameObject, "info");
            EventDelegate.Add(tipsPlay.onFinished, CloseView);
            close = false;

            stars = new List<GameObject>();
            GameObject star1 = NGUITools.FindChild(gameObject, "info/stars/1");
            GameObject star2 = NGUITools.FindChild(gameObject, "info/stars/2");
            GameObject star3 = NGUITools.FindChild(gameObject, "info/stars/3");
            GameObject star4 = NGUITools.FindChild(gameObject, "info/stars/4");
            GameObject star5 = NGUITools.FindChild(gameObject, "info/stars/5");

            stars.Add(star1);
            stars.Add(star2);
            stars.Add(star3);
            stars.Add(star4);
            stars.Add(star5);
            starNum = 0;

        }

        private void SetStars(uint starnum)
        {
            starNum = starnum;
            for (int i = 0; i < stars.Count; i++)
            {
                if (i < starnum)
                {
                    NGUITools.FindInChild<UISprite>(stars[i],"").spriteName = "xingxing1";
                }
                else
                {
                    NGUITools.FindInChild<UISprite>(stars[i], "").spriteName = "kongxing";
                }
                NGUITools.FindInChild<UISprite>(stars[i], "").depth = 10;
                stars[i].SetActive(false);
            }
        }

        private void PlayStars()
        {
            for (int i = 0; i < stars.Count; i++)
            {   
                if (newPet && i < starNum)
                {
                    NGUITools.FindInChild<UISprite>(stars[i], "").depth = 10 + i+1;
                    stars[i].SetActive(true);
                    NGUITools.FindInChild<TweenPlay>(stars[i], "").PlayForward();
                }
                else if (!newPet && i == starNum - 1)
                {
                    NGUITools.FindInChild<UISprite>(stars[i], "").depth = 10 + i+1;
                    stars[i].SetActive(true);
                    NGUITools.FindInChild<TweenPlay>(stars[i], "").PlayForward();
                }
                else
                {
                    stars[i].SetActive(true);
                    stars[i].transform.localScale = new Vector3(1, 1, 1);
                }
               
                
            }
        }

        public void OpenViewForNew(uint petId)
        {
            newPet = true;
            Singleton<PetView>.Instance.LoadPet(petId,gameObject.transform,new Vector3(-100,-15,0));
            SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(petId);
            name.text = spet.name;
            NGUITools.FindInChild<UISprite>(gameObject, "info/gradeicn").spriteName = "hslv";
            gameObject.SetActive(true);
            close = false;
            SetStars((uint)spet.star);

            tips.text = "恭喜您获得新幻兽";
            PPet pet = Singleton<PetMode>.Instance.GetPetByPetId(petId);
            PetVo petVo = Singleton<PetMode>.Instance.PetVos[pet.id];
            NGUITools.FindInChild<UILabel>(gameObject, "info/fightvalue").text = petVo.fight.ToString();
            tipsPlay.PlayReverse();
            if (AfterOpenViewGuideDelegate != null)
            {
                EventDelegate.Add(tipsPlay.onFinished, AfterOpenViewGuideDelegate);
                AfterOpenViewGuideDelegate = null;
            }
        }

        public void OpenViewForEvolve(uint uid)
        {
            newPet = false;
            
            PPet pet = Singleton<PetMode>.Instance.GetPetById(uid);
            Singleton<PetView>.Instance.LoadPet(pet.petId, gameObject.transform, new Vector3(-100, -15, 0));
            NGUITools.FindInChild<UISprite>(gameObject, "info/gradeicn").spriteName = PetLogic.GetGradeIcn(pet.grade);
            SysPet sPet = BaseDataMgr.instance.GetDataById<SysPet>(pet.petId);
            name.text =  PetLogic.GetGradeDes(pet.grade, sPet.name);
            tips.text = "您的幻兽进化到了"+pet.star+"星";
            PetVo petVo = Singleton<PetMode>.Instance.PetVos[pet.id];
            NGUITools.FindInChild<UILabel>(gameObject, "info/fightvalue").text = petVo.fight.ToString();

            SetStars(pet.star);
            gameObject.SetActive(true);
            close = false;
            Singleton<PetView>.Instance.PetPlay(Status.Win);
            tipsPlay.PlayReverse();
        }

        private void OnCloseClick(GameObject obj)
        {
            close = true;
            starNum = 0;
            tipsPlay.PlayForward();
        }

        private void CloseView()
        {

            if (close)
            {
                gameObject.SetActive(false);
                if (!newPet)
                {
                    Singleton<PetInfoView>.Instance.ShowPet();
                }
                else
                {
                    if (Singleton<PetView>.Instance.OpenForStone)
                    {
                        Singleton<PetView>.Instance.CloseView();
                    }
                }
                
            }
            else
            {
                PlayStars();
            }
        }

    }
}
