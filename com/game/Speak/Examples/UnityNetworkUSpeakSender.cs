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

using MoPhoGames.USpeak.Interface;

/// <summary>
/// Sends data received from USpeak over RPC with Unity Networking
/// </summary>
public class UnityNetworkUSpeakSender : MonoBehaviour, ISpeechDataHandler
{
	void Start()
	{
		if( !networkView.isMine )
			USpeaker.Get( this ).SpeakerMode = SpeakerMode.Remote;
	}

	void OnDisconnectedFromServer( NetworkDisconnection cause )
	{
		Destroy( gameObject );
	}

	#region ISpeechDataHandler Members
	
	/// <summary>
	/// Calls an RPC which passes the data back to USpeaker
	/// </summary>
	public void USpeakOnSerializeAudio( byte[] data )
	{
		networkView.RPC( "vc", RPCMode.All, data );
	}

	/// <summary>
	/// Calls a buffered RPC which passes the settings data to USpeaker
	/// </summary>
	public void USpeakInitializeSettings( int data )
	{
		networkView.RPC( "init", RPCMode.AllBuffered, data );
	}

	#endregion

	[RPC]
	void vc( byte[] data )
	{
		USpeaker.Get( this ).ReceiveAudio( data );
	}

	[RPC]
	void init( int data )
	{
		USpeaker.Get( this ).InitializeSettings( data );
	}
}