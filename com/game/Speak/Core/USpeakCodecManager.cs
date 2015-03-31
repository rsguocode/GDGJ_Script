using UnityEngine;
using System.Collections;

using MoPhoGames.USpeak.Codec;

/// <summary>
/// Contains data about which codecs are usable by USpeak
/// To use the Codec Manager, open Window/USpeak/Codec Manager
/// </summary>
public class USpeakCodecManager : ScriptableObject
{
	/// <summary>
	/// Get an instance of USpeakCodecManager (must exist in a Resources folder as 'CodecManager')
	/// </summary>
	public static USpeakCodecManager Instance
	{
		get
		{
			if( instance == null )
			{
				instance = (USpeakCodecManager)Resources.Load( "CodecManager" );

				if( Application.isPlaying )
				{
					instance.Codecs = new ICodec[ instance.CodecNames.Length ];

					for( int i = 0; i < instance.Codecs.Length; i++ )
					{
						instance.Codecs[ i ] = (ICodec)System.Activator.CreateInstance( System.Type.GetType( instance.CodecNames[ i ] ) );
					}
				}
			}

			return instance;
		}
	}
	private static USpeakCodecManager instance;

	/// <summary>
	/// A list of available codecs
	/// Populated at runtime upon accessing 'Instance'
	/// </summary>
	public ICodec[] Codecs;

	/// <summary>
	/// A list of assembly-qualified names of chosen codecs
	/// </summary>
	public string[] CodecNames = new string[ 0 ];

	/// <summary>
	/// A list of class names of chosen codecs (used for display)
	/// </summary>
	public string[] FriendlyNames = new string[ 0 ];
}