﻿﻿using System;
using com.game.consts;
using com.game.preloader;
using com.game.data;
using com.game.module.effect;
using com.game.sound;

//资源预加载工厂类
namespace com.game.preloader
{
	public class PreloaderFactory
	{
		public static PreloaderFactory Instance = (Instance == null ? new PreloaderFactory() : Instance);

		private PreloaderFactory()
		{
		}

		public IPreloader GetPreLoader(SysReadyLoadVo preLoadVo)
		{
			switch (preLoadVo.subtype)
			{
				case PRTypeConst.ST_SKILL:
					return EffectMgr.Instance;

				case PRTypeConst.ST_SOUND:
					return SoundMgr.Instance;

				default:
					return null;
			}
		}
	}
}

