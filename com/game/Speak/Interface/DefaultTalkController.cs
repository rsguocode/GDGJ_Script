
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

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

using MoPhoGames.USpeak.Interface;

/// <summary>
/// Default talk controller. Can either send while a key is held, or toggle sending on key press
/// </summary>
[AddComponentMenu( "USpeak/Default Talk Controller" )]
public class DefaultTalkController : MonoBehaviour, IUSpeakTalkController
{
	/// <summary>
	/// The key, either held down to send or pressed to toggle sending
	/// </summary>
	[HideInInspector]
	[SerializeField]
	public KeyCode TriggerKey;

	/// <summary>
	/// The toggle mode. 0 for Push To Talk, 1 for Toggle Talk
	/// </summary>
	[HideInInspector]
	[SerializeField]
	public int ToggleMode = 0; // PushToTalk

	private bool val = false;

	#region IUSpeakTalkController Members

	public void OnInspectorGUI()
	{
		#if UNITY_EDITOR
		EditorGUI.BeginChangeCheck();
		KeyCode newKeyCode = (KeyCode)EditorGUILayout.EnumPopup( "Trigger Key", TriggerKey );
		if( EditorGUI.EndChangeCheck() )
		{
			Undo.RegisterSceneUndo( "Changed Trigger Key" );
			EditorUtility.SetDirty( this );

			TriggerKey = newKeyCode;
		}
		EditorGUI.BeginChangeCheck();
		int newToggleMode = EditorGUILayout.Popup( "Key Mode", ToggleMode, new string[] { "Push To Talk", "Toggle Talk" } );
		if( EditorGUI.EndChangeCheck() )
		{
			Undo.RegisterSceneUndo( "Changed Key Mode" );
			EditorUtility.SetDirty( this );

			ToggleMode = newToggleMode;
		}
		#endif
	}

	public bool ShouldSend()
	{
		if( ToggleMode == 0 )
		{
			val = Input.GetKey( TriggerKey );
		}
		else
		{
			if( Input.GetKeyDown( TriggerKey ) )
				val = !val;
		}
		return val;
	}

	#endregion
}