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
using System.Linq;

using MoPhoGames.USpeak.Codec;
using MoPhoGames.USpeak.Interface;
using MoPhoGames.USpeak.Core;
using MoPhoGames.USpeak.Core.Utils;

/// <summary>
/// Bandwidth mode used to record
/// </summary>
public enum BandMode
{
	/// <summary>
	/// Records at 8khz
	/// </summary>
	Narrow,

	/// <summary>
	/// Records at 16khz
	/// </summary>
	Wide,

	/// <summary>
	/// Records at 32khz
	/// </summary>
	UltraWide
}

/// <summary>
/// Mode the USpeaker operates in
/// </summary>
public enum SpeakerMode
{
	/// <summary>
	/// USpeaker belongs to local player
	/// Records and encodes microphone input
	/// </summary>
	Local,

	/// <summary>
	/// USpeaker belongs to remote player
	/// Will only play back received audio without recording/sending
	/// </summary>
	Remote
}

/// <summary>
/// How data is sent
/// </summary>
public enum SendBehavior
{
	/// <summary>
	/// Data is sent as soon as it's available
	/// </summary>
	Constant,

	/// <summary>
	/// Waits until recording is finished to send data all at once
	/// </summary>
	RecordThenSend
}

/// <summary>
/// Which 3D Features to use during playback
/// </summary>
public enum ThreeDMode
{
	/// <summary>
	/// No 3D effects are applied (sound is entirely 2D)
	/// </summary>
	None,

	/// <summary>
	/// Sound pans across speakers
	/// </summary>
	SpeakerPan,

	/// <summary>
	/// Sound utilizes all 3D features (panning, doppler, distance rolloff, reverb, etc)
	/// </summary>
	Full3D
}

/// <summary>
/// The core component of USpeak
/// Handles recording and encoding, and decoding and playing, audio data
/// </summary>
[AddComponentMenu( "USpeak/USpeaker" )]
public class USpeaker : MonoBehaviour
{
	#region Static Fields

	/// <summary>
	/// The gain to apply to incoming audio data
	/// </summary>
	public static float RemoteGain = 1.0f;

	/// <summary>
	/// The gain to apply to outgoing audio data
	/// </summary>
	public static float LocalGain = 1.0f;

	/// <summary>
	/// Mute all incoming audio data
	/// </summary>
	public static bool MuteAll = false;

	/// <summary>
	/// A list of all USpeakers in the scene
	/// </summary>
	public static List<USpeaker> USpeakerList = new List<USpeaker>();

	private static int InputDeviceID = 0;
	private static string InputDeviceName = "";

	#endregion

	#region Public Fields

	/// <summary>
	/// The mode this USpeaker operates in (local or remote)
	/// </summary>
	public SpeakerMode SpeakerMode;

	/// <summary>
	/// The bandwidth mode of this USpeaker (Narrow or Wide)
	/// </summary>
	public BandMode BandwidthMode = BandMode.Narrow;

	/// <summary>
	/// The rate to send audio data (number of times per second)
	/// </summary>
	public float SendRate = 16;

	/// <summary>
	/// The behavior of sent audio data (send ASAP or send after recording)
	/// </summary>
	public SendBehavior SendingMode = SendBehavior.Constant;

	/// <summary>
	/// Whether to only send if volume exceeds threshold
	/// </summary>
	public bool UseVAD = false;

	/// <summary>
	/// <para>Whether audio playback is directional (pans across speakers)</para>
	/// <para>Is3D is deprecated, please use _3DMode instead</para>
	/// </summary>
	[System.Obsolete( "Use USpeaker._3DMode instead" )]
	public bool Is3D
	{
		get
		{
			return _3DMode == ThreeDMode.SpeakerPan;
		}
		set
		{
			if( value )
				_3DMode = ThreeDMode.SpeakerPan;
			else
				_3DMode = ThreeDMode.None;
		}
	}

	/// <summary>
	/// Which 3D effects to apply to playback
	/// </summary>
	public ThreeDMode _3DMode = ThreeDMode.None;

	/// <summary>
	/// Whether to play back audio data locally (for testing)
	/// </summary>
	public bool DebugPlayback = false;

	/// <summary>
	/// Whether to ask the user for permission to use the microphone in browser environments
	/// </summary>
	public bool AskPermission = true;

	/// <summary>
	/// Whether this USpeaker is actively playing back audio data
	/// </summary>
	public bool IsTalking
	{
		get
		{
			return talkTimer > 0.0f;
		}
	}

	/// <summary>
	/// Whether to mute this specific USpeaker
	/// </summary>
	public bool Mute = false;

	/// <summary>
	/// The audio volume of this USpeaker
	/// </summary>
	public float SpeakerVolume = 1.0f;

	/// <summary>
	/// The volume threshold used for Volume Activation
	/// </summary>
	public float VolumeThreshold = 0.01f;

	/// <summary>
	/// The selected codec for this USpeaker (index into codec list as defined in Codec Manager)
	/// </summary>
	public int Codec = 0;

	#endregion

	#region Private Fields

	private USpeakCodecManager codecMgr;

	private AudioClip recording;

	private int recFreq;

	private int lastReadPos = 0;

	private float sendTimer = 0.0f;
	private float sendt = 1.0f;

	// cached microphone device list
	private string[] micDeviceList;
	private float lastDeviceUpdate = 0f;

	private List<USpeakFrameContainer> sendBuffer = new List<USpeakFrameContainer>();

	private List<byte> tempSendBytes = new List<byte>();

	private ISpeechDataHandler audioHandler;

	private IUSpeakTalkController talkController;

	private int overlap = 0;

	private USpeakSettingsData settings;

	private string currentDeviceName = "";

	private float talkTimer = 0.0f;

	private float vadHangover = 0.5f; // after detecting silence, USpeak waits for 500ms before disabling sending.

	private float lastVTime = 0.0f;

	//private List<float> rmsFrames = new List<float>();

	private List<float[]> pendingEncode = new List<float[]>();

	// how much have we played so far?
	private double played = 0;
	// what is the current write index in the audio clip?
	private int index = 0;
	// how much audio have we received so far?
	private double received = 0;
	// received data so far
	private float[] receivedData = null;
	// how long to wait before playing audio
	private float playDelay = 0;
	// whether we should start playing audio
	private bool shouldPlay = false;
	// keep track of the last play position within the audio clip
	private float lastTime = 0f;

	private BandMode lastBandMode;
	private int lastCodec;

	private ThreeDMode last3DMode;

	private int recordedChunkCount = 0;

	// new mic was found, delay recording by this many frames
	// for some reason you can't immediately start a new recording with a device that was just plugged in
	// Unity bug?
	private int micFoundDelay = 0;

	private bool waitingToStartRec = false;

	#endregion

	#region Private Properties

	private int audioFrequency
	{
		get
		{
			if( recFreq == 0 )
			{
				switch( BandwidthMode )
				{
					case BandMode.Narrow:
						recFreq = 8000;
						break;
					case BandMode.Wide:
						recFreq = 16000;
						break;
					case BandMode.UltraWide:
						recFreq = 32000;
						break;
					default:
						recFreq = 8000;
						break;
				}
			}
			return recFreq;
		}
	}

	#endregion

	#region Static Methods

	/// <summary>
	/// Forces uSpeak to use a particular device, and restarts the recording process as necessary
	/// As an example, you can iterate Microphone.devices and display microphone options to the user,
	/// then pass the index of the selected device to USpeaker.SetInputDevice
	/// </summary>
	/// <param name="deviceID"></param>
	public static void SetInputDevice( int deviceID )
	{
		InputDeviceID = deviceID;
		InputDeviceName = Microphone.devices[ InputDeviceID ];
	}

	/// <summary>
	/// Get the USpeaker attached to the given object
	/// </summary>
	/// <param name="source">A game object, transform, or component</param>
	/// <returns>A USpeaker instance</returns>
	public static USpeaker Get( Object source )
	{
		if( source is GameObject )
		{
			return ( source as GameObject ).GetComponent<USpeaker>();
		}
		else if( source is Transform ) //<-- not sure if Transform counts as a Component or not...
		{
			return ( source as Transform ).GetComponent<USpeaker>();
		}
		else if( source is Component )
		{
			return ( source as Component ).GetComponent<USpeaker>();
		}
		return null;
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Looks for a component which implements the IUSpeakTalkController interface
	/// This component will be in charge of controlling when data is sent
	/// If no such component is present, data is sent unconditionally
	/// Included is the DefaultTalkController component, which uses keypress to control whether data is sent
	/// </summary>
	public void GetInputHandler()
	{
		talkController = (IUSpeakTalkController)FindInputHandler();
	}

	/// <summary>
	/// Draws the inspector for the talk controller
	/// If no such controller is present, simply draw a label stating so
	/// </summary>
	public void DrawTalkControllerUI()
	{
		if( talkController != null )
			talkController.OnInspectorGUI();
		else
			GUILayout.Label( "No component available which implements IUSpeakTalkController\nReverting to default behavior - data is always sent" );
	}

	/// <summary>
	/// Decode and buffer audio data to be played
	/// </summary>
	/// <param name="data">The data passed to USpeakOnSerializeAudio()</param>
	public void ReceiveAudio( byte[] data )
	{
		if( settings == null )
		{
			Debug.LogWarning( "Trying to receive remote audio data without calling InitializeSettings!\nIncoming packet will be ignored" );
			return;
		}

		if( MuteAll || Mute || ( SpeakerMode == SpeakerMode.Local && !DebugPlayback ) )
			return;

		if( SpeakerMode == SpeakerMode.Remote )
			talkTimer = 1.0f;

		int offset = 0;
		while( offset < data.Length )
		{
			int len = System.BitConverter.ToInt32( data, offset );
			byte[] frame = USpeakPoolUtils.GetByte( len + 6 );
			System.Array.Copy( data, offset, frame, 0, frame.Length );

			USpeakFrameContainer cont = default( USpeakFrameContainer );
			cont.LoadFrom( frame );

			USpeakPoolUtils.Return( frame );

			float[] sample = USpeakAudioClipCompressor.DecompressAudio( cont.encodedData, (int)cont.Samples, 1, false, settings.bandMode, codecMgr.Codecs[ Codec ], RemoteGain );

			float sampleTime = ( (float)sample.Length / (float)audioFrequency );
			received += sampleTime;

			System.Array.Copy( sample, 0, receivedData, index, sample.Length );

			USpeakPoolUtils.Return( sample );

			// advance the write position into the audio clip
			index += sample.Length;

			// if the write position extends beyond the clip length, wrap around
			if( index >= audio.clip.samples )
				index = 0;

			// write received data to audio clip
			audio.clip.SetData( receivedData, 0 );

			// not already playing audio, schedule audio to be played
			if( !audio.isPlaying )
			{
				shouldPlay = true;

				//Debug.Log( "Started receiving at time: " + Time.time );

				// no play delay set, advance play delay to allow more data to arrive (deal with network latency)
				if( playDelay <= 0 )
				{
					playDelay = sampleTime * 5f;
				}
			}

			offset += frame.Length;
		}
	}

	/// <summary>
	/// Initialize the settings of a USpeaker
	/// </summary>
	/// <param name="data">The data passed to USpeakOnInitializeSettings</param>
	public void InitializeSettings( int data )
	{
		print( "Settings changed" );
		settings = new USpeakSettingsData( (byte)data );

		Codec = settings.Codec;
	}

	#endregion

	#region Unity Callbacks

	void Awake()
	{
		USpeakerList.Add( this );

		if( audio == null )
			gameObject.AddComponent<AudioSource>();

		audio.clip = AudioClip.Create( "vc", audioFrequency * 10, 1, audioFrequency, ( _3DMode == ThreeDMode.Full3D ), false );
		audio.loop = true;
		receivedData = new float[ audioFrequency * 10 ];

		codecMgr = USpeakCodecManager.Instance;

		lastBandMode = BandwidthMode;
		lastCodec = Codec;
		last3DMode = _3DMode;
	}

	void OnDestroy()
	{
		USpeakerList.Remove( this );
	}

	IEnumerator Start()
	{
		yield return null;

		audioHandler = (ISpeechDataHandler)FindSpeechHandler();
		talkController = (IUSpeakTalkController)FindInputHandler();

		if( audioHandler == null )
		{
			Debug.LogError( "USpeaker requires a component which implements the ISpeechDataHandler interface" );
			yield break;
		}

		if( SpeakerMode == SpeakerMode.Remote )
		{
			yield break;
		}

		if( AskPermission )
		{
			if( !Application.HasUserAuthorization( UserAuthorization.Microphone ) )
			{
				yield return Application.RequestUserAuthorization( UserAuthorization.Microphone );
			}
		}

		if( !Application.HasUserAuthorization( UserAuthorization.Microphone ) )
		{
			Debug.LogError( "Failed to start recording - user has denied microphone access" );
			yield break;
		}

		if( Microphone.devices.Length == 0 )
		{
			Debug.LogWarning( "Failed to find a recording device" );
			yield break;
		}

		UpdateSettings();

		sendt = 1.0f / (float)SendRate;

		recording = Microphone.Start( currentDeviceName, true, 5, audioFrequency );

		print( Microphone.devices[ InputDeviceID ] );
		currentDeviceName = Microphone.devices[ InputDeviceID ];

		micDeviceList = Microphone.devices;
	}

	void Update()
	{
		// only update device list if this USpeaker is going to be recording
		if( SpeakerMode == SpeakerMode.Local )
		{
			// update microphone device list
			if( Time.time >= lastDeviceUpdate )
			{
				lastDeviceUpdate = Time.time + 2f;
				micDeviceList = Microphone.devices;
			}
		}

		talkTimer -= Time.deltaTime;

		audio.volume = SpeakerVolume;

		if( last3DMode != _3DMode )
		{
			last3DMode = _3DMode;

			StopPlaying();
			audio.clip = AudioClip.Create( "vc", audioFrequency * 10, 1, audioFrequency, ( _3DMode == ThreeDMode.Full3D ), false );
			audio.loop = true;
		}

		//speaker pan mode? Calculate it
		if( _3DMode == ThreeDMode.SpeakerPan )
		{
			Transform listener = Camera.main.transform;
			Vector3 side = Vector3.Cross( listener.up, listener.forward );
			side.Normalize();

			float x = Vector3.Dot( transform.position - listener.position, side );
			float z = Vector3.Dot( transform.position - listener.position, listener.forward );

			float angle = Mathf.Atan2( x, z );

			float pan = Mathf.Sin( angle );

			audio.pan = pan;
		}

		// currently playing audio
		if( audio.isPlaying )
		{
			// last played time exceeded audio length - add play time
			if( lastTime > audio.time )
			{
				played += audio.clip.length;
			}

			// update last played time
			lastTime = audio.time;

			// we've played past the audio we received - stop playing and wait for more data
			if( played + audio.time >= received )
			{
				StopPlaying();
				shouldPlay = false;
			}
		}
		else
		{
			// should play audio? Play audio after countdown
			if( shouldPlay )
			{
				playDelay -= Time.deltaTime;

				if( playDelay <= 0 )
				{
					audio.Play();
					// Debug.Log( "started playing at time: " + Time.time );
				}
			}
		}

		if( SpeakerMode == SpeakerMode.Remote )
			return;

		if( audioHandler == null )
			return;

		if( micDeviceList.Length == 0 )
		{
			return;
		}
		else
		{
			if( string.IsNullOrEmpty( InputDeviceName ) )
				InputDeviceName = currentDeviceName;

			if( string.IsNullOrEmpty( currentDeviceName ) )
			{
				if( waitingToStartRec )
				{
					micFoundDelay--;
					if( micFoundDelay <= 0 )
					{
						micFoundDelay = 0;
						waitingToStartRec = false;

						print( "New device found: " + currentDeviceName );
						InputDeviceID = 0;
						InputDeviceName = micDeviceList[ 0 ];
						currentDeviceName = micDeviceList[ 0 ];

						recording = Microphone.Start( currentDeviceName, true, 5, audioFrequency );

						lastReadPos = 0;
						sendBuffer.Clear();
						recordedChunkCount = 0;

						UpdateSettings();
					}
				}
				else
				{
					waitingToStartRec = true;
					micFoundDelay = 5;
				}
			}
			else
			{
				// switch to new device
				if( InputDeviceName != currentDeviceName )
				{
					Microphone.End( currentDeviceName );
					print( "Using input device: " + InputDeviceName );
					currentDeviceName = InputDeviceName;

					recording = Microphone.Start( currentDeviceName, true, 5, audioFrequency );

					lastReadPos = 0;
					sendBuffer.Clear();
					recordedChunkCount = 0;
				}

				// the device list changed
				if( micDeviceList[ Mathf.Min( InputDeviceID, micDeviceList.Length - 1 ) ] != currentDeviceName )
				{
					// attempt to find the existing device
					bool found = false;
					for( int i = 0; i < Microphone.devices.Length; i++ )
					{
						if( micDeviceList[ i ] == currentDeviceName )
						{
							InputDeviceID = i;
							found = true;
						}
					}

					// existing device must have been unplugged, switch to the default audio device
					if( !found )
					{
						InputDeviceID = 0;
						InputDeviceName = micDeviceList[ 0 ];
						currentDeviceName = micDeviceList[ 0 ];

						print( "Device unplugged, switching to: " + currentDeviceName );

						recording = Microphone.Start( currentDeviceName, true, 5, audioFrequency );

						lastReadPos = 0;
						sendBuffer.Clear();
						recordedChunkCount = 0;
					}
				}
			}
		}

		if( lastBandMode != BandwidthMode || lastCodec != Codec )
		{
			UpdateSettings();

			lastBandMode = BandwidthMode;
			lastCodec = Codec;
		}

		if( recording == null )
			return;

		int readPos = Microphone.GetPosition( currentDeviceName );

		int realReadPos = readPos + recording.samples * recordedChunkCount;

		if( realReadPos < lastReadPos )
			recordedChunkCount++;

		readPos += recording.samples * recordedChunkCount;

		if( readPos <= overlap )
			return;

		bool talkController_shouldSend = ( talkController == null || talkController.ShouldSend() );

		//read in the latest chunk(s) of audio
		try
		{
			int sz = readPos - lastReadPos;
			int minSize = codecMgr.Codecs[ Codec ].GetSampleSize( audioFrequency );

			if( minSize == 0 )
				minSize = 100;

			int currentIDX = lastReadPos;
			int numClips = Mathf.FloorToInt( sz / minSize );

			for( int i = 0; i < numClips; i++ )
			{
				float[] d = USpeakPoolUtils.GetFloat( minSize );

				recording.GetData( d, currentIDX % recording.samples );
				if( talkController_shouldSend )
				{
					talkTimer = 1f;
					OnAudioAvailable( d );
				}

				USpeakPoolUtils.Return( d );

				currentIDX += minSize;
			}

			lastReadPos = currentIDX;
		}
		catch( System.Exception ) { }

		ProcessPendingEncodeBuffer();

		bool allowSend = true;
		if( SendingMode == SendBehavior.RecordThenSend && talkController != null )
		{
			allowSend = !talkController_shouldSend;
		}

		sendTimer += Time.deltaTime;
		if( sendTimer >= sendt && allowSend )
		{
			sendTimer = 0.0f;

			//flush the send buffer
			tempSendBytes.Clear();
			foreach( USpeakFrameContainer frame in sendBuffer )
			{
				tempSendBytes.AddRange( frame.ToByteArray() );
			}
			sendBuffer.Clear();

			if( tempSendBytes.Count > 0 )
			{
				// Debug.Log( "Sending at time: " + Time.time );
				audioHandler.USpeakOnSerializeAudio( tempSendBytes.ToArray() );
			}
		}

	}

	#endregion

	#region Private Methods

	void StopPlaying()
	{
		audio.Stop();
		audio.time = 0;
		index = 0;
		played = 0;
		received = 0;
		lastTime = 0;
	}

	void UpdateSettings()
	{
		if( !Application.isPlaying )
			return;

		settings = new USpeakSettingsData();

		settings.bandMode = BandwidthMode;
		settings.Codec = Codec;

		audioHandler.USpeakInitializeSettings( (short)settings.ToByte() );
	}

	Component FindSpeechHandler()
	{
		Component[] comp = GetComponents<Component>();
		foreach( Component c in comp )
		{
			if( c is ISpeechDataHandler )
				return c;
		}
		return null;
	}

	Component FindInputHandler()
	{
		Component[] comp = GetComponents<Component>();
		foreach( Component c in comp )
		{
			if( c is IUSpeakTalkController )
				return c;
		}
		return null;
	}

	//Called when new audio data is available from the microphone
	void OnAudioAvailable( float[] pcmData )
	{
		if( UseVAD && !CheckVAD( pcmData ) )
			return;

		int chunkSize = 1280;
		List<float[]> audio_chunks = SplitArray( pcmData, chunkSize );

		foreach( float[] chunk in audio_chunks )
			pendingEncode.Add( chunk );
	}

	List<float[]> SplitArray( float[] array, int size )
	{
		List<float[]> chunksList = new List<float[]>();
		int skipCounter = 0;

		while( skipCounter < array.Length )
		{
			float[] chunk = array.Skip( skipCounter ).Take( size ).ToArray<float>();
			chunksList.Add( chunk );
			skipCounter += chunk.Length;
		}
		return chunksList;
	}

	void ProcessPendingEncodeBuffer()
	{
		int budget_ms = 100; //if time spent encoding exceeds this many milliseconds, abort and wait till next frame
		float budget_s = (float)budget_ms / 1000.0f;

		float t = Time.realtimeSinceStartup;
		while( Time.realtimeSinceStartup <= t + budget_s && pendingEncode.Count > 0 )
		{
			float[] pcm = pendingEncode[ 0 ];
			pendingEncode.RemoveAt( 0 );
			ProcessPendingEncode( pcm );
		}
	}

	void ProcessPendingEncode( float[] pcm )
	{
		// encode data and add it to the send buffer

		int s;
		byte[] b = USpeakAudioClipCompressor.CompressAudioData( pcm, 1, out s, lastBandMode, codecMgr.Codecs[ lastCodec ], LocalGain );

		USpeakFrameContainer cont = default( USpeakFrameContainer );
		cont.Samples = (ushort)s;
		cont.encodedData = b;

		sendBuffer.Add( cont );
	}

	int CalculateSamplesRead( int readPos )
	{
		if( readPos >= lastReadPos )
		{
			return readPos - lastReadPos;
		}
		else
		{
			return ( ( audioFrequency * 10 ) - lastReadPos ) + readPos;
		}
	}

	private float[] normalize( float[] samples, float magnitude )
	{
		//float max = 0.0f;
		//for( int i = 0; i < samples.Length; i++ )
		//max = Mathf.Max( max, Mathf.Abs( samples[ i ] ) );

		float[] newSamples = new float[ samples.Length ];
		for( int i = 0; i < samples.Length; i++ )
			newSamples[ i ] = samples[ i ] / magnitude;

		return newSamples;
	}

	private float amplitude( float[] x )
	{
		float sum = 0.0f;
		for( int i = 0; i < x.Length; i++ )
		{
			sum = Mathf.Max( sum, Mathf.Abs( x[ i ] ) );
		}
		return sum;
	}

	bool CheckVAD( float[] samples )
	{
		if( Time.realtimeSinceStartup < lastVTime + vadHangover )
			return true;

		float max = 0.0f;
		foreach( float f in samples )
			max = Mathf.Max( max, Mathf.Abs( f ) );
		bool val = ( max >= VolumeThreshold );
		if( val )
			lastVTime = Time.realtimeSinceStartup;
		return val;
	}

	#endregion
}