using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using com.game.utils;
using com.u3d.bases.loader;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.preloader;
using com.game.data;
using com.game.consts;

namespace com.game.sound
{
	public class AudioSetting
	{
		public bool Loop;
		public float Volumn;
		public float Delay;
		public bool IsLoading;
	}

	public class SoundMgr : IPreloader
    {
        public static SoundMgr Instance = ((null == Instance) ? new SoundMgr() : Instance);

        //音频文件对象池
		private IDictionary<string, AudioClip> clipDictionary;
		private IDictionary<string, AudioSetting> clipSettingDictionary;
        //播放对象列表
        private IList<SoundPlayer> soundList;    

		private float sceneVolumn = GameConst.DefaultSceneVolumn/100f;
		private float effectVolumn = GameConst.DefaultEffectVolumn/100f;

		private bool mute = false;

		private IList<SysReadyLoadVo> preLoadList;	
		private IList<string> preladAudioNameList = new List<string>();
		private int preloadIndex;

		public IEnumerator PreloadResourceList(IList<SysReadyLoadVo> preLoadList)
		{
			this.preLoadList = preLoadList;
			this.preloadIndex = 0;
			
			PreloadResource();
			
			while (preloadIndex < this.preLoadList.Count)
			{
				yield return 0;
			}
		}

		private void PreloadResource()
		{
			try
			{
				while (preloadIndex < preLoadList.Count)
				{
					string soundId = preLoadList[preloadIndex].subid;
					string soundUrl = GetSkillSoundAssetPath(preLoadList[preloadIndex].subid);
					
					if (!clipDictionary.ContainsKey(soundId))
					{
						preladAudioNameList.Add(soundId);
						AssetManager.Instance.LoadAsset<AudioClip>(soundUrl, SoundPreLoaded);
						break;
					}
					else
					{
						preloadIndex++;
					}
				}
			}
			catch (Exception e)
			{
				Log.warin(this, "preloadResource error, exception is: " + e.Message);
			}
		}

		public void PreloadSceneAudio(string soundId)
		{
			if (!clipDictionary.ContainsKey(soundId))
			{
				string soundUrl = GetSceneSoundAssetPath(soundId);
				AssetManager.Instance.LoadAsset<AudioClip>(soundUrl, SceneSoundPreLoaded);
			}
		}

		private void SceneSoundPreLoaded(AudioClip clip)
		{
			if (null != clip && !clipDictionary.ContainsKey(clip.name))
			{
				clipDictionary.Add(clip.name, clip);
			}
		}

		//声音预加载后处理
		private void SoundPreLoaded(AudioClip clip)
		{
			try
			{
				//将声音对象缓存
				if (null != clip && !clipDictionary.ContainsKey(clip.name))
				{
					clipDictionary.Add(clip.name, clip);
				}
			}
			finally
			{
				preloadIndex++;
				PreloadResource();
			}
		}

		//场景音乐音量调节[0~1]
		public float SceneVolumn
		{
			get
			{
				return sceneVolumn;
			}

			set
			{
				sceneVolumn = Mathf.Clamp01(value);

				foreach (SoundPlayer player in soundList)
				{
					if (player.Loop)
					{
						player.Volumn = value;
					}
				}
			}
		}

		//音效音量调节[0~1]
		public float EffectVolumn
		{
			get
			{
				return effectVolumn;
			}
			
			set
			{
				effectVolumn = Mathf.Clamp01(value);

				foreach (SoundPlayer player in soundList)
				{
					if (!player.Loop)
					{
						player.Volumn = value;
					}
				}
			}
		}

		//静音设置
		public bool Mute
		{
			get
			{
				return mute;
			}

			set
			{
				if (mute == value)
				{
					return;
				}

				mute = value;

				foreach (SoundPlayer player in soundList)
				{
					player.Mute = value;
				}
			}
		}

		//确保不能外部实例化
        private SoundMgr()
        {
			clipDictionary = new Dictionary<string, AudioClip>();
			clipSettingDictionary = new Dictionary<string, AudioSetting>();
			soundList = new List<SoundPlayer>();
        }

		/// 音频播放控制，支持多文件同时播放
		/// audioFileName不能带后缀
		private void PlayClip(string soundId, string assetBundleName, bool loop, float volumn, float delay = 0f)
        {   
            try
            {
				if (StringUtils.isEmpty(soundId))
                {
                    return;
                }

				AudioSetting audioSetting = new AudioSetting();
				audioSetting.Loop = loop;
				audioSetting.Volumn = volumn;
				audioSetting.Delay = delay;
				audioSetting.IsLoading = true;
				clipSettingDictionary[soundId] = audioSetting;

                //将音频文件转换为AudioClip
				if (!clipDictionary.ContainsKey(soundId))
                {
					AssetManager.Instance.LoadAsset<AudioClip>(assetBundleName, PlayAfterLoad, soundId);					                   
                }
				else
				{
					PlayAfterLoad(clipDictionary[soundId]);
				}
            }
            catch (Exception e)
            {
                Log.warin(this, e.StackTrace);
            }
        }

        private void PlayAfterLoad(AudioClip clip) 
        {
            if (null != clip)
            {   
				//必须保证音效ID唯一
				string soundId = clip.name;

				if (!clipDictionary.ContainsKey(soundId))
                {
					clipDictionary.Add(soundId, clip);
                }

                //获得可以播放的player
                SoundPlayer player = GetUnusedPlayer();
                if (null == player)
                {
                    player = new SoundPlayer(clip);
                    soundList.Add(player);
                }
                else
                {
                    player.Clip = clip;
                }

				AudioSetting audioSetting = clipSettingDictionary[soundId];
				audioSetting.IsLoading = false;
				player.Loop = audioSetting.Loop;
				player.Volumn = audioSetting.Volumn;
				player.Delay = audioSetting.Delay;
				player.Mute = mute;
                player.Play();
            }
        }

		//释放无用的音频
		private void FreeUnusedAudio()
		{
			IList<string> freeList = new List<string>();
			foreach (KeyValuePair<string, AudioClip> pair in clipDictionary)
			{
				if (!preladAudioNameList.Contains(pair.Key))
				{
					freeList.Add(pair.Key);
				}
			}

			foreach (string item in freeList)
			{
				clipDictionary.Remove(item);
			}
		}

		//场景音乐播放
		public void PlaySceneAudio(string soundId)
		{
			string assetBundleName = GetSceneSoundAssetPath(soundId);	
			PlayClip(soundId, assetBundleName, true, sceneVolumn);
		}

		//技能音效播放
		public void PlaySkillAudio(string soundId, float delay = 0f)
		{
			string assetBundleName = GetSkillSoundAssetPath(soundId);			
			PlayClip(soundId, assetBundleName, false, effectVolumn, delay);
		}

		//UI音效播放
		public void PlayUIAudio(string soundId, float delay = 0f)
		{
			string assetBundleName = GetUISoundAssetPath(soundId);			
			PlayClip(soundId, assetBundleName, false, effectVolumn, delay);
		}

		//语音音效播放
		public void PlaySpeechAudio(string soundId, string roleType, float delay = 0f)
		{
			string assetBundleName = GetSpeechSoundAssetPath(soundId, roleType);			
			PlayClip(soundId, assetBundleName, false, effectVolumn, delay);
		}

		private string GetSceneSoundAssetPath(string soundId)
		{
			return "Music/Scene/" + soundId + ".assetbundle";
		}

		private string GetSkillSoundAssetPath(string soundId)
		{
			return "Music/Skill/" + soundId + ".assetbundle";
		}

		private string GetUISoundAssetPath(string soundId)
		{
			return "Music/UI/" + soundId + ".assetbundle";
		}

		private string GetSpeechSoundAssetPath(string soundId, string roleType)
		{
			return "Music/Speech/" + roleType + "/" + soundId + ".assetbundle";
		}


		//停止播放所有文件
		public void StopAll()
		{
			foreach (SoundPlayer player in soundList)
			{
				if (player.IsPlaying)
				{
					player.Stop();
				}
			}

			FreeUnusedAudio();
		}

		//停止播放某个文件
		public void Stop(string soundId)
		{
			AudioClip clip = clipDictionary[soundId];

			if (null == clip)
			{
				return;
			}

			foreach (SoundPlayer player in soundList)
			{
				if ((clip == player.Clip) && player.IsPlaying)
				{
					player.Stop();
					return;
				}
			}
		}

		//音频文件是否正在播放
		public bool IsPlaying(string soundId)
		{
			//如果音效正在加载，也视为正在播放
			if (clipSettingDictionary.ContainsKey(soundId))
			{
				if (clipSettingDictionary[soundId].IsLoading)
				{
					return true;
				}
			}

			if (!clipDictionary.ContainsKey(soundId))
			{
				return false;
			}

			AudioClip clip = clipDictionary[soundId];

			foreach (SoundPlayer player in soundList)
			{
				if ((clip == player.Clip) && player.IsPlaying)
				{
					return true;
				}
			}

			return false;
		}

		//获得可用的播放器
		private SoundPlayer GetUnusedPlayer()
		{
			foreach (SoundPlayer player in soundList)
			{
				if (!player.IsPlaying)
				{
					return player;
				}
			}

			return null;
		}

    }
}

