﻿﻿﻿using UnityEngine;
using System.Collections;
using com.game.sound;

public class Test : MonoBehaviour 
{
	public float sceneVolumn = 0.2f;
	public float effectVolumn = 1f;
	public bool mute = false;

	public float SceneVolumn
	{
		get
		{
			return SoundMgr.Instance.SceneVolumn;
		}

		set
		{
			SoundMgr.Instance.SceneVolumn = value;
		}
	}

	public float EffectVolumn
	{
		get
		{
			return SoundMgr.Instance.EffectVolumn;
		}

		set
		{
			SoundMgr.Instance.EffectVolumn = value;
		}
	}

	public bool Mute
	{
		get
		{
			return SoundMgr.Instance.Mute;
		}

		set
		{
			SoundMgr.Instance.Mute = value;
		}
	}

    private Animation ani;
	// Use this for initialization
	void Start0 () {

        ani = GetComponent<Animation>();
        AnimationClip clip = ani.GetClip("pao");
        Debug.Log(ani.GetClipCount());

        if (clip != null) {
            Debug.Log("ok");
        }

	}
	
	// Update is called once per frame
	void Update0 () {
        ani.Play("pao");
	}

	void Update()
	{
		if (sceneVolumn != SceneVolumn)
		{
			SceneVolumn = sceneVolumn;
		}

		if (effectVolumn != EffectVolumn)
		{
			EffectVolumn = effectVolumn;
		}

		if (mute != Mute)
		{
			Mute = mute;
		}
	}
}
