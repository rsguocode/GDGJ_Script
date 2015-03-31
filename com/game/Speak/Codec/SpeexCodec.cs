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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;

using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Codec
{
	/// <summary>
	/// Codec wrapper around the Speex encoding algorithm
	/// </summary>
	public class SpeexCodec : ICodec
	{
		private NSpeex.SpeexDecoder m_ultrawide_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.UltraWide );
		private NSpeex.SpeexEncoder m_ultrawide_enc = new NSpeex.SpeexEncoder( NSpeex.BandMode.UltraWide );

		private NSpeex.SpeexDecoder m_wide_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Wide );
		private NSpeex.SpeexEncoder m_wide_enc = new NSpeex.SpeexEncoder( NSpeex.BandMode.Wide );

		private NSpeex.SpeexDecoder m_narrow_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Narrow );
		private NSpeex.SpeexEncoder m_narrow_enc = new NSpeex.SpeexEncoder( NSpeex.BandMode.Narrow );

		/// <summary>
		/// Create an instance of the Speex codec (at Quality = 5)
		/// </summary>
		public SpeexCodec()
		{
			m_wide_enc.Quality = 5;
			m_narrow_enc.Quality = 5;
			m_ultrawide_enc.Quality = 5;
		}

		#region Private Methods

		private byte[] SpeexEncode( short[] input, BandMode mode )
		{
			NSpeex.SpeexEncoder speexEnc = null;
			int byteLen = 320;
			switch( mode )
			{
				case BandMode.Narrow:
					speexEnc = m_narrow_enc;
					byteLen = 320;
					break;
				case BandMode.Wide:
					speexEnc = m_wide_enc;
					byteLen = 640;
					break;
				case BandMode.UltraWide:
					speexEnc = m_ultrawide_enc;
					byteLen = 1280;
					break;
			}

			byte[] encoded = USpeakPoolUtils.GetByte( byteLen + 4 );

			int length = speexEnc.Encode( input, 0, input.Length, encoded, 4, encoded.Length );

			// first 4 bytes contains length
			byte[] len_bytes = BitConverter.GetBytes( length );

			System.Array.Copy( len_bytes, encoded, 4 );

			return encoded;
		}

		private short[] SpeexDecode( byte[] input, BandMode mode )
		{
			NSpeex.SpeexDecoder speexDec = null;
			int shortLen = 320;
			switch( mode )
			{
				case BandMode.Narrow:
					speexDec = m_narrow_dec;
					shortLen = 320;
					break;
				case BandMode.Wide:
					speexDec = m_wide_dec;
					shortLen = 640;
					break;
				case BandMode.UltraWide:
					speexDec = m_ultrawide_dec;
					shortLen = 1280;
					break;
			}

			byte[] len_bytes = USpeakPoolUtils.GetByte( 4 );
			System.Array.Copy( input, len_bytes, 4 );

			int dataLength = BitConverter.ToInt32( len_bytes, 0 );

			USpeakPoolUtils.Return( len_bytes );

			byte[] actual_bytes = USpeakPoolUtils.GetByte( input.Length - 4 );
			Buffer.BlockCopy( input, 4, actual_bytes, 0, input.Length - 4 );

			short[] decoded = USpeakPoolUtils.GetShort( shortLen );

			speexDec.Decode( actual_bytes, 0, dataLength, decoded, 0, false );

			USpeakPoolUtils.Return( actual_bytes );

			return decoded;
		}

		#endregion

		#region ICodec Members

		public byte[] Encode( short[] data, BandMode mode )
		{
			return SpeexEncode( data, mode );
		}

		public short[] Decode( byte[] data, BandMode mode )
		{
			return SpeexDecode( data, mode );
		}

		/// <summary>
		/// Returns either 320 (if frequency is 8khz), 640 (if frequency is 16khz), or 1280 (if frequency is 32khz)
		/// </summary>
		public int GetSampleSize( int recordingFrequency )
		{
			switch( recordingFrequency )
			{
				case 8000:
					return 320;
				case 16000:
					return 640;
				case 32000:
					return 1280;
				default:
					return 320;
			}
		}

		#endregion
	}
}