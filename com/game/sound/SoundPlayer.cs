using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.game.sound
{
	public class SoundPlayer
	{
		private static AudioListener audioListener;
		private AudioSource audioSource;

		private float delay = 0f;
		private bool isDelayPlaying = false;

		public float Delay
		{
			get {return delay;}
			set {delay = value;}
		}

		//当前音频剪辑
		public AudioClip Clip
		{
			get
			{
				return audioSource.clip;
			}

			set
			{
				audioSource.clip = value;
			}
		}

		//是否循环播放
		public bool Loop
		{
			get
			{
				return audioSource.loop;
			}

			set
			{
				audioSource.loop = value;
			}
		}

		//音量大小
		public float Volumn
		{
			get
			{
				return audioSource.volume;
			}
			
			set
			{
				audioSource.volume = value;
			}
		}

		//静音调节
		public bool Mute
		{
			get
			{
				return audioSource.mute;
			}
			
			set
			{
				audioSource.mute = value;
			}
		}

		//是否正在播放
		public bool IsPlaying
		{
			get
			{
				return (audioSource.isPlaying || isDelayPlaying);
			}	
		}

		public SoundPlayer(AudioClip clip)
		{
			//寻找全局AudioListener，一般绑定在主摄像机上
			if (null == audioListener)
			{
				audioListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
				
				if (null == audioListener)
				{
					Camera cam = Camera.main;

					if (null == cam) 
					{
						cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
					}

					if (null != cam) 
					{
						audioListener = cam.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			
			if (null != audioListener)
			{
				audioSource = audioListener.gameObject.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
			}

			audioSource.clip = clip;
		}

		public void Play()
		{
			if (delay > 0f)
			{
				isDelayPlaying = true;
				vp_Timer.In(delay, DelayPlay, 1, 0);
			}
			else
			{
				audioSource.Play();
			}
		}

		private void DelayPlay()
		{
			isDelayPlaying = false;
			audioSource.Play();
		}

		public void Pause()
		{
			audioSource.Pause();
		}

		public void Stop()
		{
			audioSource.Stop();
		}
	}
}

