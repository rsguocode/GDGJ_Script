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

namespace MoPhoGames.USpeak.Codec
{
	/// <summary>
	/// Base interface for codecs in USpeak
	/// </summary>
	public interface ICodec
	{
		/// <summary>
		/// Encode the given audio data
		/// </summary>
		/// <param name="data">16 bit PCM data</param>
		/// <param name="bandMode">The chosen Bandwidth mode (Narrow = 8khz, Wide=16khz, Ultrawide=32khz)</param>
		/// <returns>A byte array</returns>
		byte[] Encode( short[] data, BandMode bandMode );

		/// <summary>
		/// Decode the given byte array
		/// </summary>
		/// <param name="data">The encoded byte array</param>
		/// <param name="bandMode">The chosen Bandwidth mode (Narrow = 8khz, Wide=16khz, Ultrawide=32khz)</param>
		/// <returns>A short array, representing 16-bit PCM data</returns>
		short[] Decode( byte[] data, BandMode bandMode );

		/// <summary>
		/// Get the number of samples each input array to Encode must be
		/// </summary>
		/// <param name="recordingFrequency">The audio frequency of the input data</param>
		/// <returns>How many samples the data must be (0 = don't care, in practice actually means 100 samples for purposes of array pooling)</returns>
		int GetSampleSize( int recordingFrequency );
	}
}