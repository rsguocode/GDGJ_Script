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

namespace MoPhoGames.USpeak.Interface
{
	/// <summary>
	/// Interface for custom data handlers
	/// </summary>
	public interface ISpeechDataHandler
	{
		/// <summary>
		/// Serialize the audio data to the network
		/// </summary>
		/// <param name="data">The raw data to send</param>
		void USpeakOnSerializeAudio( byte[] data );

		/// <summary>
		/// Serialize the settings data to the network.
		/// Data handler is responsible for ensuring this data is buffered and sent to new players
		/// </summary>
		/// <param name="data">The settings data to send</param>
		void USpeakInitializeSettings( int data );
	}
}