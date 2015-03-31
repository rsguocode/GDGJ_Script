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
using System.Collections.Generic;
using System;
using System.IO;

using MoPhoGames.USpeak.Codec;
using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Core
{
	/// <summary>
	/// Helper class to aid in converting and compressing audio clips
	/// for sending over the network
	/// </summary>
	public class USpeakAudioClipCompressor : MonoBehaviour
	{
		#region Static Methods

		/// <summary>
		/// Compress the given audio data
		/// </summary>
		/// <param name="samples">The raw 32-bit audio data</param>
		/// <param name="channels">The number of channels (nearly always 1)</param>
		/// <param name="sample_count">The number of samples that were encoded</param>
		/// <param name="mode">The chosen bandwidth mode (recording frequency)</param>
		/// <param name="Codec">The codec to encode the audio with</param>
		/// <param name="gain">The gain to apply to the audio</param>
		/// <returns>An encoded byte array</returns>
		public static byte[] CompressAudioData( float[] samples, int channels, out int sample_count, BandMode mode, ICodec Codec, float gain = 1.0f )
		{
			data.Clear();
			sample_count = 0;

			short[] b = USpeakAudioClipConverter.AudioDataToShorts( samples, channels, gain );

			byte[] mlaw = Codec.Encode( b, mode );

			USpeakPoolUtils.Return( b );

			data.AddRange( mlaw );

			USpeakPoolUtils.Return( mlaw );

			//byte[] zipped = zip( data.ToArray() );

			return data.ToArray();
		}

		/// <summary>
		/// Decompress the given encoded audio data
		/// </summary>
		/// <param name="data">The encoded audio</param>
		/// <param name="samples">The number of encoded samples</param>
		/// <param name="channels">The number of channels</param>
		/// <param name="threeD">Whether the audio is 3D</param>
		/// <param name="mode">The bandwidth mode used to encode the data</param>
		/// <param name="Codec">The codec to decode the data with</param>
		/// <param name="gain">The gain to apply to the decoded audio</param>
		/// <returns>32bit raw audio data</returns>
		public static float[] DecompressAudio( byte[] data, int samples, int channels, bool threeD, BandMode mode, ICodec Codec, float gain )
		{
			int frequency = 4000;
			if( mode == BandMode.Narrow )
			{
				frequency = 8000;
			}
			else if( mode == BandMode.Wide )
			{
				frequency = 16000;
			}

			byte[] d;
			//d = unzip( data );
			d = data;

			short[] pcm = Codec.Decode( d, mode );

			tmp.Clear();
			tmp.AddRange( pcm );

			USpeakPoolUtils.Return( pcm );

			return USpeakAudioClipConverter.ShortsToAudioData( tmp.ToArray(), channels, frequency, threeD, gain );
		}

		#endregion

		#region Private Fields

		private static List<byte> data = new List<byte>();

		private static List<short> tmp = new List<short>();

		#endregion
	}
}