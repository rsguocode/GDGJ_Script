using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;


public class PlaytList {


	public List<IPlay> playList = new List<IPlay>();

	private int index = 0;
	//private int delegateIndex;
	//private int taskIndex;

	//private bool isDelegate;
	//private int nextIndex;
	//private void GetNextIndex()
	//{

	//}
	/// <summary>
	/// Adds the play.
	/// </summary>
	/// <param name="play">Play.</param>
	/// <param name="index">Index. -1表示添加在末尾</param>
	public void AddPlay(IPlay play,int index = -1)
	{
		if(play == null)
			return;
		if(index == -1)
			playList.Add(play);
		else if(index < 0 || index > playList.Count)
			return;
		else
			playList.Insert(index,play);
	}
	public void AddTimeInterval(float seconds,int index = -1)
	{
		AddPlay(WaitForSeconds(seconds),index);
	}
	//提供类似invoke的方法
	public void AddTimeInterval(float seconds,EventDelegate.Callback callBack)
	{
		Task t = WaitForSeconds(seconds);
		EventDelegate.Add(t.onFinish,callBack);
		AddPlay(t,index);
	}
	public void AddCoroutine(IEnumerator coroutine,int index = -1)
	{
		AddPlay(new Task(coroutine,false),index);
	}
	public void ContatPlay()
	{
		for(int i =0,length = playList.Count-1;i<length;i++)
		{
			if(playList[i].OnEnd != null)
				EventDelegate.Add (playList[i].OnEnd,PlayNext);
		}
	}
	public void ClearPlay()
	{
		playList.Clear();
	}
	public void Start()
	{
		ContatPlay();
		index = 0;
		if(playList.Count>0)
			playList[0].Begin();
	}

	private void PlayNext()
	{
		index ++;
		if(index >= playList.Count)
			return;
		else
			playList[index].Begin();

	}

	public Task WaitForSeconds(float seconds)
	{
		return new Task(WaitForSecondsHandle(seconds),false);
	}
	IEnumerator WaitForSecondsHandle(float seconds)
	{
		yield return new WaitForSeconds(seconds);
	}
	//从事件中断地方重新调用
	public void Continue()
	{
		PlayNext();
	}
	public void AddDelegate(EventDelegate.Callback callBack,int index = -1)
	{
		AddPlay (new DelegatePlay(callBack),index);
	}
	class DelegatePlay : IPlay
	{
		private EventDelegate.Callback callBack;
		public DelegatePlay(EventDelegate.Callback callBack)
		{
			this.callBack = callBack;
		}
		public void Begin()
		{
			if(this.callBack !=null)
			{
				callBack();
			}
		}
		public List<EventDelegate> OnEnd
		{
			get{return null;}
		}
	}




}
