using UnityEngine;
using System.Collections;

/*
 * 暂时用作全屏UI动画
 */
public class TweenUtils  {

	public static TweenPlay AddClickTween(GameObject go,out TweenPosition tweenPosition)
	{
		tweenPosition = NGUITools.AddMissingComponent<TweenPosition>(go);
		tweenPosition.duration = 0.3f;
		tweenPosition.enabled = false;
		TweenAlphas tweenAlphas = NGUITools.AddMissingComponent<TweenAlphas>(go);
		tweenAlphas.duration = 0.3f;
		tweenAlphas.from = 0f;
		tweenAlphas.enabled  = false;
		TweenScale tweenScale = NGUITools.AddMissingComponent<TweenScale>(go);
		tweenScale.duration = 0.3f;
		tweenScale.from = new Vector3(0.2f,0.2f,0.2f);
		tweenScale.enabled = false;
		return NGUITools.AddMissingComponent<TweenPlay>(go);
	}

	public static void AdjustTransformToClick(TweenPosition tweenPosition)
	{
		NGUITools.SetLocalPositionToRayHit(tweenPosition.transform);// 调整动画启动位置为当前鼠标点击的位置
		tweenPosition.from = tweenPosition.value;
	}

}
