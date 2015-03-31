
using UnityEngine;
using System.Collections.Generic;
using Com.Game.Module.Manager;
using com.game.module.test;
using com.u3d.bases.debug;

public class PageIndex :MonoBehaviour{
	//private UIGrid grid;
	//private List<UISprite> spriteList ;
	public Vector2 size = new Vector2(22,22);
	public float padding = 36f;
	public int depth = 100;
	private Transform mTrans;

	public string hightligthSprite = "fanye_hightlight";
	public string backgroundSprite = "fanye_background";
	public string atlasName = "common";
	private  List<UISprite> spriteList = new List<UISprite>(); 
	public int current;
    //public static int previousPage = 1;  //之前的页号, 作为centerOnchild中也可以引用的一个参数
	private int total;
	public UICenterOnChild centerOnChild;
	void Awake()
	{
		//grid = gameObject.AddMissingComponent<UIGrid>();
		//这句代码把自己坑了，哎
		//spriteList = new List<UISprite>();
		mTrans = transform ;
		if(spriteList.Count == 0)
		{
			GameObject go = new GameObject("page");
			UISprite sprite = go.AddComponent<UISprite>();
			go.layer = mTrans.gameObject.layer;
			sprite.width = (int)size.x;
			sprite.height = (int)size.y;
			sprite.depth = 100;
			sprite.atlas = Singleton<AtlasManager>.Instance.GetAtlas(atlasName);
			spriteList.Add(sprite);
			if(centerOnChild != null)
				RegisterOnCenter(centerOnChild);
		}
		//mTrans = transform;
		//spriteList = mTrans.GetComponentsInChildren<UISprite>();
	}
	/// <summary>
	/// 更新页数
	/// </summary>
	/// <param name="current">当前是第几页，第一页为 1</param>
	/// <param name="total">总共页数</param>
	public void InitPage(int current,int total)
	{
	    Awake();
		this.current = current;
		this.total = total;
		Vector3 localPosition = Vector3.zero;
		float startX;
		if (total % 2 == 1)  //计算起始位置
			startX = padding * (total / 2);
		else
			startX = padding * (total / 2) - size.x/2;
		GameObject temp = spriteList[0].gameObject;
		while(spriteList.Count < total)
		{
			spriteList.Add((GameObject.Instantiate(temp) as GameObject).GetComponent<UISprite>());
		}
		UISprite sprite ;
		localPosition.x -= startX;
		for(int i =0,length = spriteList.Count ;i < length; i++)
		{
			sprite = spriteList[i];

			if(i<total)
			{
				if(current == i+1)
                {
                    sprite.spriteName = hightligthSprite;  //希望能将这个资源改进一下。。
                    sprite.transform.localScale = Vector3.one*1.1f;  
                }
				else
                {   
                    sprite.spriteName = backgroundSprite;
                    sprite.transform.localScale = Vector3.one;  
                }
				sprite.transform.parent = mTrans;
				sprite.transform.localPosition = localPosition;
				localPosition.x += padding;
				sprite.SetActive(true);
			    sprite.depth = 100 + i;
				sprite.MakePixelPerfect();
				//spriteDic.Add(i, sprite);
			}
			else
			{
				sprite.gameObject.SetActive(false);
			}
		}
	}
	public void TurnTo(int num)
	{
		if(num<1)
			num = 1;
		else if(num >total)
			num = total;
		InitPage(num,total);
	}

	public void TurnLeft()
	{
		current --;
		TurnTo(current);
	}
	public void TurnRight()
	{
		current ++;
		TurnTo(current);
	}

	/// <summary>
	/// 注册到UICenterOnChild，让UICenterOnChild触发滑页变化
	/// </summary>
	/// <param name="center">Center.</param>
	public void RegisterOnCenter(UICenterOnChild center)
	{
		centerOnChild = center;
		centerOnChild.onCenterFinish = PageIndexCallBack;
		//if (center != null)
		//{
			//this.centerOnChild = center;
			//NGUITools.FindInParents<UIScrollView>(center.gameObject).onDragFinished+=PageIndexCallBack;
		//}
	}
	private void PageIndexCallBack()
	{
        Log.info(this, "Child Count : " + NGUITools.GetActiveChildrenCount(centerOnChild.gameObject));
		this.InitPage(int.Parse(centerOnChild.centeredObject.name),NGUITools.GetActiveChildrenCount(centerOnChild.gameObject));
	}

}