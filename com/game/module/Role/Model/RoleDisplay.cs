using com.u3d.bases.consts;
using UnityEngine;

using com.u3d.bases.display;
using com.u3d.bases.display.vo;
using System;
using com.game.consts;
using com.game;
using com.game.module;
using com.game.manager;


namespace Com.Game.Module.Role
{
	public delegate void ModelControl(GameObject go);

    public enum ModelType
    {
        Pet,
        Role,
        Monster,
        Npc,
        Trap,
        Teleport,
        Load,
    }

    public class RoleDisplay : BaseDisplay 
	{
		private static RoleDisplay instance = new RoleDisplay();
		public static RoleDisplay Instance{get { return instance;}}
		private string url;//模型assetBundle的地址
		private DisplayVo vo = new DisplayVo();
		public ModelControl modelControl;

		private Transform parent;

		private int roleType = -1;  //先暂时限制下

		public void CreateRole(int roleType,ModelControl loadInitCallBack)
		{
			this.roleType = roleType;
			vo = new DisplayVo();
		    vo.Type = DisplayType.ROLE_MODE;
			var _SysRoleBaseInfoVo = BaseDataMgr.instance.GetSysRoleBaseInfo((uint)roleType);
			vo.ClothUrl = "Model/Role/" + _SysRoleBaseInfoVo.Model + "/Model/"+_SysRoleBaseInfoVo.Model+".assetbundle";
            this.modelControl = loadInitCallBack;
			SetVo(vo);
		}


        /// <summary>
        /// 创建类型
        /// </summary>
        public void CreateModel(ModelType type, string id, LoadAssetFinish<GameObject> loadInitCallBack)
        {
            if (id.Equals("0"))
            {
                id = "10004";
            }
            string path = "Model/" + type + "/"+id+"/Model/"+id + ".assetbundle";
            AssetManager.Instance.LoadAsset<GameObject>(path, loadInitCallBack);
        }


		
		protected override void AddScript(GameObject go)
		{
			NGUITools.SetLayer(GoBase,LayerMask.NameToLayer("Mode"));
			SpriteRenderer [] renders  =   go.GetComponentsInChildren<SpriteRenderer>();
			if(renders == null)
			{
				//Debug.Log("*****************************找不到组件！");
			}
			foreach(SpriteRenderer render in renders)
			{
				render.sortingLayerName  = "Default";
				render.sortingOrder += 100; //调成 正 值
			}
			//this.model = go;
			//this.model.transform.localScale  =comTest.transform.localScale*0.63f;
			if(parent != null )
			{
				go.transform.parent = parent;
				parent = null;
			}
			else
				go.transform.parent = ViewTree.go.transform;
			go.transform.localRotation = new Quaternion(0,0,0,0);
			go.transform.localScale = new Vector3(170,170,170);
			go.transform.localPosition = new Vector3(vo.X,vo.Y,0);
			if(modelControl!=null)
			{
				modelControl(go);
			}

		}


		
		public void ChangePosition(float x,float y,int roleType = 0,ModelControl modelControl = null)
		{
			vo.X = x;
			vo.Y = y;
            this.roleType = roleType;
			parent = null;
			if(this.roleType == 0)
			{
				GoBase.transform.localPosition = new Vector3(x,y,0);
				if(modelControl != null)
				{
                    //GameObject.()
					modelControl(GoBase);
				}
			}
			else{
				CreateRole(roleType,modelControl);
			}
		}

		

		public void RemoveModel()
		{
			GoBase.transform.localPosition = new Vector3(2000f,2000f,0f);  //移出照相机视野范围外
		}

	}
}
