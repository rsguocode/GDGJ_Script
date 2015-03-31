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
/// Sends received data from USpeaker back, with faked network conditions (includes sliders for ping, ping jitter, and packet loss)
/// </summary>
public class LocalUSpeakSender : MonoBehaviour, ISpeechDataHandler, IUSpeakTalkController
{
	bool recording = false;

	float jitter = 0f;
	float ping = 0f;
	float packetLoss = 0f;

	void OnGUI()
	{
		GUILayout.Label( "Ping - " + Mathf.Round( ping ) + "ms" );
		ping = GUILayout.HorizontalSlider( ping, 0f, 100f, GUILayout.Width( 200f ) );
		GUILayout.Label( "Ping Jitter - " + Mathf.Round( jitter ) + "ms" );
		jitter = GUILayout.HorizontalSlider( jitter, 0f, 100f, GUILayout.Width( 200f ) );
		GUILayout.Label( "Packet Loss - " + Mathf.Round( packetLoss ) + "%" );
		packetLoss = GUILayout.HorizontalSlider( packetLoss, 0f, 10f, GUILayout.Width( 200f ) );

		GUILayout.Label( "Using Microphone: " + Microphone.devices[ 0 ] );

		GUILayout.Space( 10f );

		if( recording )
		{
			if( GUILayout.Button( "Stop Recording" ) )
			{
				recording = false;
			}
		}
		else
		{
			if( GUILayout.Button( "Start Recording" ) )
			{
				//Debug.Log( "Preparing to start sending at time: " + Time.time );
				recording = true;
			}
		}
	}

	#region IUSpeakTalkController Members

	public void OnInspectorGUI()
	{
		return;
	}

	public bool ShouldSend()
	{
		return recording;
	}

	#endregion

	#region ISpeechDataHandler Members

	public void USpeakOnSerializeAudio( byte[] data )
	{
		StartCoroutine( fakeSendPacket( data ) );
		//USpeaker.Get( this ).ReceiveAudio( data );
	}

	public void USpeakInitializeSettings( int data )
	{
		USpeaker.Get( this ).InitializeSettings( data );
	}

	IEnumerator fakeSendPacket( byte[] data )
	{
		float rand = Random.Range( 0f, 100f );

		if( rand < packetLoss )
			yield break;

		float delay = Random.Range( ping, ping + jitter ) / 1000f;
		yield return new WaitForSeconds( delay );
		USpeaker.Get( this ).ReceiveAudio( data );
	}

	#endregion
}