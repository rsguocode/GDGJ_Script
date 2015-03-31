
using System;
using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.module.Guide.GuideLogic;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.UICommon;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.debug;

using PCustomDataType;
using UnityEngine;
using com.game.module.effect;

namespace Com.Game.Module.Pet
{
    class PetView : BaseView<PetView>
	{
        /// </summary>
        public override string url { get { return "UI/Pet/PetView.assetbundle"; } }

        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }

        public override ViewType viewType
        {
            get { return ViewType.CityView; }
        }

        /// <summary>
        /// 关闭时是否销毁
        /// </summary>
        public override bool isDestroy { get { return true; } }

        public override bool IsFullUI
        {
            get { return true; }
        }


        public override bool waiting {
            get { return true; }
        }

        private UIToggle current; //当前激活
        private UIToggle all;//全部
        private UIToggle gj;//攻击
        private UIToggle kz;//控制
        private UIToggle fz;//辅助

        //public UIAtlas SkillAltas { get; private set; }
        public UIAtlas EquipAltas { get; private set; }
        public UIAtlas EquipAltas_Gray { get; private set; }

        public UIAtlas PropAltas { get; private set; } //道具表

        private bool subViewInit; //子面板是否已经初始化

        private GameObject _petImage;

        private Animator petAnimator;

        private int currentState;

        private UIScrollView scrollView;

        public GameObject PetImage {
            get { return _petImage; } 
            set
            {
                if (_petImage != null && _petImage != value)
                {
                    Log.info(this, "destory _petImage");
                    GameObject.Destroy(_petImage);
                }
                _petImage = value;

            }
        }

        private string imageId;
        private SysPet imagePet;
        private Vector3 imagePos;
        private Transform imageParent;

        //坐标值处理
        private int petX = 150; //宠物排列的X坐标

        private int topRow = -40; //宠物行最高位置
        private int topFg = 6; //分隔线最高位置
        private int fzGap = 78; //分割线的间隔
        private int rowGap = 126;//行间隔
        
        private int petNum = 0;
        private int showNum = 0;
      
        private GameObject fg;

        public List<GameObject> petsObj;

        private int imageDepth;

        private GameObject tips;
        public GuidePetOpenDelegate GuidePetOpenDelegate;
        public Button CloseButton;

        public bool OpenForStone = false;
        private int petId;

        private Action closeCallBack;

		protected override void Init()
		{
		    all = NGUITools.FindInChild<UIToggle>(gameObject,"petlist/function/all");
            gj = NGUITools.FindInChild<UIToggle>(gameObject, "petlist/function/gj");
            kz = NGUITools.FindInChild<UIToggle>(gameObject, "petlist/function/kz");
            fz = NGUITools.FindInChild<UIToggle>(gameObject, "petlist/function/fz");

            all.onStateChange = OnToggleChange;
            gj.onStateChange = OnToggleChange;
            kz.onStateChange = OnToggleChange;
            fz.onStateChange = OnToggleChange;

            CloseButton = NGUITools.FindInChild<Button>(gameObject, "petlist/close");
            CloseButton.onClick = OnCloseClick;

            petsObj = new List<GameObject>();
            petsObj.Add(NGUITools.FindChild(gameObject, "petlist/pets/container/1"));
            petsObj[0].SetActive(false);
		    petsObj[0].GetComponent<Button>().onClick = OnPetClick;

            imageDepth = NGUITools.FindInChild<UISprite>(petsObj[0], "image").depth ;

            fg = NGUITools.FindChild(gameObject, "petlist/pets/container/fg");
            fg.SetActive(false);
            scrollView = NGUITools.FindInChild<UIScrollView>(gameObject, "petlist/pets/container");

            tips = NGUITools.FindChild(gameObject, "petlist/tips");

		    NGUITools.SetLayer(FindChild("tips"),LayerMask.NameToLayer("TopUI"));

		}



        private void SetLabels()
        {
        }


        public bool OpenViewForStone(uint stoneId, Action callback)
        {
            SysPet spet = PetLogic.GetSysPetByStoneId(stoneId);
            if (spet == null)
            {
				callback();
                return false;
            }

            PPet pet = Singleton<PetMode>.Instance.GetPetByPetId((uint)spet.id);
            if (pet != null)
            {
				callback();
                return false;
            }

            int own = Singleton<GoodsMode>.Instance.GetCountByGoodsId(stoneId);
            int need = PetLogic.GetNeedStone((uint)spet.star, false);
            if (own < need)
            {
				callback();
                return false;
            }

            OpenForStone = true;
            closeCallBack = callback;
            petId = spet.id;
            OpenView();

			//隐藏萌宠献礼相关特效
			EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_PetAnimation).SetActive(false);
			EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_PetLight).SetActive(false);

            return true;
        }

        private void OnPetClick(GameObject obj)
        {
            uint petId = uint.Parse(obj.name);
            SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>(petId);

            if (PetLogic.CanOwn(pet))
            {
                Singleton<PetControl>.Instance.ActiveAPet(petId);
            }
            else
            {
                PPet p = Singleton<PetMode>.Instance.GetPetByPetId(petId);
                if (p == null)
                {
                    Singleton<PetStoneTipsView>.Instance.OpenView(petId);
                }
                else
                {
                    Singleton<PetInfoView>.Instance.OpenView(petId);
                }
                
            }
           
        }

        //标签页触发
        private void OnToggleChange(bool state)
        {
            if (state)
            {
                if (UIToggle.current == all && current != all)
                {
                    current = all;
                    showNum = PetLogic.AllNum;
                    SetPetsInfo(PetLogic.Type_All);
                }
                else if (UIToggle.current == gj && current != gj)
                {
                    current = gj;
                    showNum = PetLogic.AttackNum;
                    SetPetsInfo(PetLogic.Type_Attack);
                }
                else if (UIToggle.current == kz && current != kz)
                {
                    current = kz;
                    showNum = PetLogic.ControlNum;
                    SetPetsInfo(PetLogic.Type_Control);
                }
                else if (UIToggle.current == fz && current != fz)
                {
                    current = fz;
                    showNum = PetLogic.HelpNum;
                    SetPetsInfo(PetLogic.Type_Help);
                }
                if (GuidePetOpenDelegate != null)
                {
                    GuidePetOpenDelegate();
                    GuidePetOpenDelegate = null;
                }
            }

        }

        private  void SetPetsInfo(int petType)
        {
            ClearPetsInfo();
            int num = 0;

            //先显示可以召唤的宠物数据
            List<SysPet> unOwnPet = new List<SysPet>();

            foreach (SysPet pet in PetLogic.SysPets)
            {
                if (petType == 0 || pet.type == petType)
                {
                    if (PetLogic.CanOwn(pet))
                    {
                        num++;
                        SetPetInfo(pet, num, true);
                    }
                    else if (!PetLogic.IsOwn(pet))
                    {
                        unOwnPet.Add(pet);
                    }

                }
            }
            
            //再显示已经拥有的宠物
            foreach (PPet pet in Singleton<PetMode>.Instance.AllPets)
            {
                SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(pet.petId);
                if (petType == 0 || spet.type == petType)
                {
                    num++;
                    SetPetInfo(spet, num, true);
                }
            }

            //已拥有和可召唤的宠物数
            petNum = num;

            //再显示未拥有的宠物
            foreach (SysPet pet in unOwnPet)
            {
                num++;
                SetPetInfo(pet, num,false);
            }
            
            SetFGTitlePos();

            scrollView.ResetPosition();

            SetPetPublicInfo();
        }

        //设置分割线的位置
        private void SetFGTitlePos()
        {
            //计算分割线的位置
            if (petNum != showNum)
            {
                fg.transform.localPosition = new Vector3(0, GetFgPosY(), 0);
                fg.SetActive(true);
            }
            else
            {
                fg.SetActive(false);
            }

        }

        private int GetFgPosY()
        {
            int posy = 0;
            if (petNum == 0)
            {
                posy = topFg;
            }
            else
            {
                posy = topRow;
                posy = posy - ((petNum - 1) / 2) * rowGap - fzGap;
            }
            return posy;
        }

        private void SetPetObjPos(GameObject obj,int num,bool own)
        {
            //计算x坐标
            int posx = 0;
            if (own)
            {
                posx = petX - (num % 2) * petX * 2;
            }
            else
            {
                posx = petX - ((num - petNum) % 2) * petX * 2;
            }

            //计算y坐标
            int posy = 0;
            int fgy = 0;

            if (own)
            {
                posy = topRow;
                posy = posy - ((num - 1) / 2) * rowGap;
            }
            else
            {
                fgy = GetFgPosY();
                posy = fgy - fzGap - ((num - petNum - 1) / 2) * rowGap;
            }

            obj.transform.localPosition = new Vector3(posx, posy, 0);
        }

        private void ClearPetsInfo()
        {
            foreach (var obj in petsObj)
            {
                obj.SetActive(false);
            }
        }

        private void SetStar(GameObject pet,uint starnum)
        {
            GameObject star1 = NGUITools.FindChild(pet, "stars/1");
            GameObject star2 = NGUITools.FindChild(pet, "stars/2");
            GameObject star3 = NGUITools.FindChild(pet, "stars/3");
            GameObject star4 = NGUITools.FindChild(pet, "stars/4");
            GameObject star5 = NGUITools.FindChild(pet, "stars/5");

            if (starnum == 1)
            {
                star1.SetActive(true);
                star1.transform.localPosition = new Vector3(0, 0, 0);

                star2.SetActive(false);
                star3.SetActive(false);
                star4.SetActive(false);
                star5.SetActive(false);
            }
            else if (starnum == 2)
            {
                star1.SetActive(true);
                star1.transform.localPosition = new Vector3(-9, 0, 0);

                star2.SetActive(true);
                star2.transform.localPosition = new Vector3(9, 0, 0);

                star3.SetActive(false);
                star4.SetActive(false);
                star5.SetActive(false);
            }else if (starnum == 3)
            {
                star1.SetActive(true);
                star1.transform.localPosition = new Vector3(-18, 0, 0);

                star2.SetActive(true);
                star2.transform.localPosition = new Vector3(0, 0, 0);

                star3.SetActive(true);
                star3.transform.localPosition = new Vector3(18, 0, 0);

                star4.SetActive(false);
                star5.SetActive(false);
            }
            else if (starnum == 4)
            {
                star1.SetActive(true);
                star1.transform.localPosition = new Vector3(27, 0, 0);

                star2.SetActive(true);
                star2.transform.localPosition = new Vector3(9, 0, 0);

                star3.SetActive(true);
                star3.transform.localPosition = new Vector3(-9, 0, 0);

                star4.SetActive(true);
                star4.transform.localPosition = new Vector3(-27, 0, 0);

                star5.SetActive(false);
            }
            else if(starnum == 5)
            {
                star1.SetActive(true);
                star1.transform.localPosition = new Vector3(36, 0, 0);

                star2.SetActive(true);
                star2.transform.localPosition = new Vector3(18, 0, 0);

                star3.SetActive(true);
                star3.transform.localPosition = new Vector3(0, 0, 0);

                star4.SetActive(true);
                star4.transform.localPosition = new Vector3(-18, 0, 0);

                star5.SetActive(true);
                star5.transform.localPosition = new Vector3(-36, 0, 0);
            }

        }

        private void SetEquipInfo(GameObject obj,uint petid)
        {
            NGUITools.FindInChild<UISprite>(obj, "equips/1icn").spriteName = GetEquipIcn(obj,petid, 1);
            NGUITools.FindInChild<UISprite>(obj, "equips/2icn").spriteName = GetEquipIcn(obj,petid, 2);
            NGUITools.FindInChild<UISprite>(obj, "equips/3icn").spriteName = GetEquipIcn(obj,petid, 3);
            NGUITools.FindInChild<UISprite>(obj, "equips/4icn").spriteName = GetEquipIcn(obj,petid, 4);
            NGUITools.FindInChild<UISprite>(obj, "equips/5icn").spriteName = GetEquipIcn(obj,petid, 5);
            NGUITools.FindInChild<UISprite>(obj, "equips/6icn").spriteName = GetEquipIcn(obj, petid, 6);
        }


        private string GetEquipIcn(GameObject obj,uint petId, int num)
        {
            int result = PetLogic.CheckEquip(petId, num);
            if (result == 0)
            {
                return "";
            }
            else if (result == 1 || result == 3) //满足等级-可装备或者可以合成
            {
                NGUITools.FindChild(obj, "tips").SetActive(true);
                return "add2";
            }
            else if (result == 2 || result == 4)
            {
                return "add";
            }
            else
            {
                SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint)result);
                return equipVo.icon.ToString();
            }
        }

        //设置宠物信息
        private void SetPetInfo(SysPet pet,int num,bool own)
        {
   
            GameObject petobj;
            if (num > petsObj.Count)
            {
                petobj = GameObject.Instantiate(petsObj[0]) as GameObject;
                petsObj.Add(petobj);
                petobj.transform.parent = petsObj[0].transform.parent;
                petobj.transform.localScale = new Vector3(1,1,1);
                petobj.SetActive(false);
                petobj.GetComponent<UIWidgetContainer>().onClick = OnPetClick;
                petobj.name = pet.id.ToString();
            }
            else
            {
                petobj = petsObj[num - 1];
                petobj.name = pet.id.ToString();
            }

            SetPetObjPos(petobj, num, own);

            NGUITools.FindInChild<UISprite>(petobj, "image").spriteName = pet.icon.ToString();
           
            petobj.SetActive(true);
            NGUITools.FindChild(petobj, "tips").SetActive(false); //提示默认关闭

            if (!own || PetLogic.CanOwn(pet))
            {
                NGUITools.FindInChild<UILabel>(petobj, "name").text = pet.name;
                UIUtils.ChangeGrayShader(NGUITools.FindInChild<UISprite>(petobj, "image"), imageDepth-1);
                NGUITools.FindChild(petobj, "equips").SetActive(false);
                NGUITools.FindChild(petobj, "level").SetActive(false);
                NGUITools.FindChild(petobj, "stars").SetActive(false);
                NGUITools.FindChild(petobj, "stone").SetActive(true);
                int stone = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)pet.stone_id);
                int need = PetLogic.GetNeedStone((uint) pet.star, false);
                
                NGUITools.FindInChild<UISprite>(petobj, "gradeicn").spriteName = "";
                NGUITools.FindInChild<UISlider>(petobj, "stone").value = stone/(float) need;
                
                NGUITools.FindInChild<UILabel>(petobj, "stone/num").text = stone +"/" + need;

                if (PetLogic.CanOwn(pet))
                {
                    NGUITools.FindChild(petobj, "active").SetActive(true);
                    NGUITools.FindChild(petobj, "tips").SetActive(true);
                    NGUITools.FindChild(petobj, "gradeicn").SetActive(false);
                }
                else
                {
                    NGUITools.FindChild(petobj, "gradeicn").SetActive(false);
                    NGUITools.FindChild(petobj, "active").SetActive(false);
                }
            }
            else
            {
                PPet ppet = Singleton<PetMode>.Instance.GetPetByPetId((uint)pet.id);
                NGUITools.FindChild(petobj, "gradeicn").SetActive(true);
                NGUITools.FindInChild<UISprite>(petobj, "gradeicn").spriteName = PetLogic.GetGradeIcn(ppet.grade);
                NGUITools.FindInChild<UILabel>(petobj, "name").text =PetLogic.GetGradeDes(ppet.grade,pet.name);
                UIUtils.ChangeNormalShader(NGUITools.FindInChild<UISprite>(petobj, "image"), imageDepth);
                NGUITools.FindChild(petobj, "equips").SetActive(true);
                NGUITools.FindChild(petobj, "level").SetActive(true);
                NGUITools.FindInChild<UILabel>(petobj, "level/label").text = ppet.lvl.ToString();
                NGUITools.FindChild(petobj, "stars").SetActive(true);
                SetStar(petobj, ppet.star);
                SetEquipInfo(petobj,ppet.petId);
                NGUITools.FindChild(petobj, "stone").SetActive(false);
                NGUITools.FindChild(petobj, "active").SetActive(false);
                //检查是否可以升阶-升星
                if (PetLogic.CanUpgrade(ppet) || PetLogic.CanEvolve(ppet))
                {
                    NGUITools.FindChild(petobj, "tips").SetActive(true);
                }
            }

            NGUITools.FindInChild<UISprite>(petobj, "typeicn").spriteName = PetLogic.GetTypeIcnName(pet.type);

           

        }


        protected override void HandleAfterOpenView()
        {
            gameObject.SetActive(false);

            if (EquipAltas == null)
            {
                Singleton<AtlasManager>.Instance.LoadAtlasHold(AtlasUrl.EquipIconHold, AtlasUrl.EquipIconNormal, LoadAtlas, true);
            }
            else
            {
                DelayShow();
            }
            
        }

        private void LoadAtlas(UIAtlas atlas)
        {
            if (EquipAltas == null)
            {
                EquipAltas = atlas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/1icn").atlas = EquipAltas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/2icn").atlas = EquipAltas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/3icn").atlas = EquipAltas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/4icn").atlas = EquipAltas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/5icn").atlas = EquipAltas;
                NGUITools.FindInChild<UISprite>(petsObj[0], "equips/6icn").atlas = EquipAltas;

                EquipAltas_Gray = Singleton<AtlasManager>.Instance.GetAtlas(AtlasUrl.EquipIconGray);

                Singleton<AtlasManager>.Instance.LoadAtlas(AtlasUrl.PropIconURL,AtlasUrl.PropIcon,LoadAtlas,true);
            }else if (PropAltas == null)
            {
                PropAltas = atlas;
                DelayShow();
            }
        }

        private void DelayShow()
        {
            //初始化子view 应该在之后
            InitSubView();
            if (OpenForStone)
            {
                Singleton<PetControl>.Instance.ActiveAPet((uint) petId);
                FindChild("petlist").SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
            gameObject.SetActive(true);
            

            //Singleton<PetControl>.Instance.SendRequestForPetInfo();
            //Singleton<PetControl>.Instance.SendRequestPetPublicInfo();
        }

        //初始化子面板
        private void InitSubView()
        {
            if (!subViewInit)
            {
                Singleton<PetInfoView>.Instance.Init(NGUITools.FindChild(gameObject,"petinfo"));
                Singleton<PetEquipView>.Instance.Init(NGUITools.FindChild(gameObject, "equip"));
                NGUITools.SetLayer(NGUITools.FindChild(gameObject, "equip"), LayerMask.NameToLayer("Viewport"));
                NGUITools.SetLayer(NGUITools.FindChild(gameObject, "stonetips"), LayerMask.NameToLayer("Viewport"));
                Singleton<PetTipsView>.Instance.Init(NGUITools.FindChild(gameObject, "pettips"));
                Singleton<PetStoneTipsView>.Instance.Init(NGUITools.FindChild(gameObject, "stonetips"));
                NGUITools.FindChild(gameObject,"tips").SetActive(true);
            }
            subViewInit = true;
        }


        public override void RegisterUpdateHandler()
        {
            Singleton<PetMode>.Instance.dataUpdated += DataUpdated;
            Singleton<GoodsMode>.Instance.dataUpdated += DataUpdated;
        }

        public override void DataUpdated(object sender, int code)
        {
                if ((sender == Singleton<PetMode>.Instance && code == PetMode.PetList) ||  (sender == Singleton<GoodsMode>.Instance&&code == Singleton<GoodsMode>.Instance.UPDATE_PET_GOODS) )
                {
                    if (current == all)
                    {
                        current = all;
                        showNum = PetLogic.AllNum;
                        SetPetsInfo(PetLogic.Type_All);
                    }
                    else if (current == gj)
                    {
                        showNum = PetLogic.AttackNum;
                        SetPetsInfo(PetLogic.Type_Attack);
                    }
                    else if (current == kz)
                    {
                        current = kz;
                        showNum = PetLogic.ControlNum;
                        SetPetsInfo(PetLogic.Type_Control);
                    }
                    else if (current == fz)
                    {
                        current = fz;
                        showNum = PetLogic.HelpNum;
                        SetPetsInfo(PetLogic.Type_Help);
                    }
                }
                else if (sender == Singleton<PetMode>.Instance && code == PetMode.NewPet)
                {
                    Singleton<PetTipsView>.Instance.OpenViewForNew(Singleton<PetMode>.Instance.CurrentPet.petId);
                }
        }

        public override void CancelUpdateHandler()
        {
            Singleton<PetMode>.Instance.dataUpdated -= DataUpdated;
            Singleton<GoodsMode>.Instance.dataUpdated -= DataUpdated;
        }

        private void OnCloseClick(GameObject obj)
        {
            CloseView();
        }

        public override void CloseView()
        {
            OpenForStone = false;
            if (closeCallBack != null)
            {
                closeCallBack();
                closeCallBack = null;
            }
            base.CloseView();
        }

        public void LoadPetImage(GameObject image)
        {
            image = GameObject.Instantiate(image) as GameObject;
            
            GameObject.Destroy(image.GetComponent<AnimationEventController>());
            image.AddComponent<AnimationEventEmptyController>();

            image.transform.parent = imageParent;
            image.transform.localRotation = new Quaternion(0, 0, 0, 0);
            float scale = imagePet.size/10f;
            image.transform.localScale = new Vector3(scale*240,scale*240,scale*240);
            image.transform.localPosition = imagePos;
            NGUITools.SetLayer(image, LayerMask.NameToLayer("Mode"));
            PetImage = image;
            PetImage.SetActive(true);

            //初次载入即播放胜利动作
            petAnimator = image.GetComponent<Animator>();
            PetPlay(Status.Win);
        }

        public void LoadPet(uint petId,Transform parent , Vector3 pos)
        {
             SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>(petId);
             pos = new Vector3(pos.x,pos.y-20*pet.fly,pos.z);
             imagePet = pet;
             string resId =  "10004";
             if (!pet.res.Equals("0"))
             {
                 resId = pet.res;
             }
             if (resId == imageId)
            {
                PetImage.transform.parent = parent;
                PetImage.transform.localPosition = pos;
            }
            else
            {
                if (!ReferenceEquals(PetImage, null))
                {
                    PetImage.SetActive(false);
                }

                RoleDisplay.Instance.CreateModel(ModelType.Pet, resId, LoadPetImage);
                imageId = resId;
                imageParent = parent;
                imagePos = pos;

            }   
        }

        public void PetPlay(int statu)
        {
            if (!ReferenceEquals(petAnimator, null))
            {
                petAnimator.SetInteger(Status.STATU, statu);
                currentState = statu;
            }
        }

        //展示对应的选中的宠物信息
        public void ShowSelectedPetInfo(uint petid)
        {
            
        }

        private void SetPetPublicInfo()
        {
            NGUITools.FindInChild<UILabel>(tips, "num").text = "("+Singleton<PetMode>.Instance.AllPets.Count + "/" +
                                                          PetLogic.SysPets.Count
            +")";
            NGUITools.FindInChild<UILabel>(tips, "fightvalue").text =
                Singleton<PetMode>.Instance.totalAdd.fight.ToString();
            NGUITools.FindInChild<UILabel>(tips, "attackvalue").text = Singleton<PetMode>.Instance.totalAdd.AttPMax.ToString();
            NGUITools.FindInChild<UILabel>(tips, "hpvalue").text = Singleton<PetMode>.Instance.totalAdd.Hp.ToString();
            NGUITools.FindInChild<UILabel>(tips, "pdvalue").text = Singleton<PetMode>.Instance.totalAdd.DefP.ToString();
            NGUITools.FindInChild<UILabel>(tips, "mdvalue").text = Singleton<PetMode>.Instance.totalAdd.DefM.ToString();
            NGUITools.FindInChild<UILabel>(tips, "hurtrvalue").text = Singleton<PetMode>.Instance.totalAdd.HurtRe.ToString();

        }

        public override void Update()
        {
           Singleton<PetInfoView>.Instance.UpdateTimeInfo();

           if (!ReferenceEquals(petAnimator, null) && !ReferenceEquals(_petImage, null) && currentState != Status.IDLE )
           {
               if (currentState == Status.ATTACK1) //攻击动作
               {
                   AnimatorStateInfo stateInfo = petAnimator.GetCurrentAnimatorStateInfo(0);
                   int curStatuNameHash = stateInfo.nameHash;
                   if (curStatuNameHash == Status.NAME_HASH_ATTACK1)
                   {
                       if (stateInfo.normalizedTime > 0.9)
                       {
                           PetPlay(Status.IDLE);
                       }
                   }
               }else if (currentState == Status.Win && petAnimator.IsInTransition(0)) //胜利动作
               {
                   PetPlay(Status.IDLE);
               }

           }
        }

        public override void Destroy()
        {
            EquipAltas = null;
            EquipAltas_Gray = null;
            PropAltas = null;
            if (!ReferenceEquals(_petImage,null))
            {
                GameObject.Destroy(_petImage);
                _petImage = null;
            }
            imageParent = null;
            petAnimator = null;
            imageId = null;

            subViewInit = false;
            base.Destroy();
        }
	}
}
