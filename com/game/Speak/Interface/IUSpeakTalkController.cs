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
	/// Interface for custom talk controllers
	/// Determines when data should be sent (for example, send while key is held down)
	/// </summary>
	public interface IUSpeakTalkController
	{
		/// <summary>
		/// Draw the inspector for this data handler
		/// Custom data handlers should leave this function blank
		/// </summary>
		void OnInspectorGUI();

		/// <summary>
		/// Determine whether data should be sent
		/// </summary>
		/// <returns>True if audio data should be sent, false otherwise</returns>
		bool ShouldSend();
	}
}