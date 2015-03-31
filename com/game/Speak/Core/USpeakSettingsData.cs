/* Copyright (c) 2012 MoPho' Games
 * All Rights Reserved
 * 
 * Please see the included 'LICENSE.TXT' for usage rights
 * If this asset was downloaded from the Unity Asset Store,
 * you may instead refer to the Unity Asset Store Customer EULA
 * If the asset was NOT purchased or downloaded from the Unity
 * Asset Store and no such 'LICENSE.TXT' is present, you may
 * assume that the software has been pirated.
 * */

using UnityEngine;
using System.Collections;

namespace MoPhoGames.USpeak.Core
{
	/// <summary>
	/// Helper class for converting settings on a USpeaker to a single byte
	/// </summary>
	public class USpeakSettingsData
	{
		public BandMode bandMode;
		public int Codec;

		public USpeakSettingsData()
		{
			bandMode = BandMode.Narrow;
			Codec = 0;
		}

		/// <summary>
		/// Load settings from a byte
		/// </summary>
		public USpeakSettingsData( byte src )
		{
			if( ( src & 1 ) == 1 )
			{
				//Narrowband
				bandMode = BandMode.Narrow;
			}
			else if( ( src & 2 ) == 2 )
			{
				//Wideband
				bandMode = BandMode.Wide;
			}
			else
			{
				//Ultrawideband
				bandMode = BandMode.UltraWide;
			}

			Codec = src >> 2;
		}

		/// <summary>
		/// Convert the given settings to a byte
		/// </summary>
		public byte ToByte()
		{
			byte meta = 0;
			if( bandMode == BandMode.Narrow )
				meta |= 1;
			else if( bandMode == BandMode.Wide )
				meta |= 2;
			meta |= (byte)( ( (uint)Codec ) << 2 );
			return meta;
		}
	}
}