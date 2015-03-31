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

using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Core
{

	/// <summary>
	/// Helper class, used to convert audio clips to short arrays and back
	/// </summary>
	public class USpeakAudioClipConverter
	{
		/// <summary>
		/// Convert audio data to a short array
		/// </summary>
		/// <param name="clip">The audio clip to convert</param>
		/// <returns>A short array</returns>
		public static short[] AudioDataToShorts( float[] samples, int channels, float gain = 1.0f )
		{
			short[] data = USpeakPoolUtils.GetShort( samples.Length * channels );
			for( int i = 0; i < samples.Length; i++ )
			{
				//convert to the -3267 to +3267 range
				float g = samples[ i ] * gain;
				if( Mathf.Abs( g ) > 1.0f )
				{
					if( g > 0 )
						g = 1.0f;
					else
						g = -1.0f;
				}
				float conv = g * 3267.0f;
				//int c = Mathf.RoundToInt( conv );

				data[ i ] = (short)conv;
			}

			return data;
		}

		/// <summary>
		/// Convert a short array to a PCM array
		/// </summary>
		/// <param name="data">The short array representing an audio clip</param>
		/// <param name="channels">How many channels in the audio data</param>
		/// <param name="frequency">The recording frequency of the audio data</param>
		/// <param name="threedimensional">Whether the audio clip should be 3D</param>
		/// <param name="gain">How much to boost the volume (1.0 = unchanged)</param>
		/// <returns>An array of float samples</returns>
		public static float[] ShortsToAudioData( short[] data, int channels, int frequency, bool threedimensional, float gain )
		{
			float[] samples = USpeakPoolUtils.GetFloat( data.Length );

			for( int i = 0; i < samples.Length; i++ )
			{
				//convert to float in the -1 to 1 range
				int c = (int)data[ i ];
				samples[ i ] = ( (float)c / 3267.0f ) * gain;
			}

			return samples;
		}
	}

}