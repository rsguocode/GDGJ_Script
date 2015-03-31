using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoPhoGames.USpeak.Core.Utils
{
	/// <summary>
	/// Helper class to avoid GC allocs when temporary arrays are needed
	/// </summary>
	public class USpeakPoolUtils
	{
		private static List<byte[]> BytePool = new List<byte[]>();
		private static List<short[]> ShortPool = new List<short[]>();
		private static List<float[]> FloatPool = new List<float[]>();

		/// <summary>
		/// Get a float array from the pool, or create a new one
		/// </summary>
		/// <param name="length">The length of the array</param>
		/// <returns>A float array of the given length</returns>
		public static float[] GetFloat( int length )
		{
			for( int i = 0; i < FloatPool.Count; i++ )
			{
				if( FloatPool[ i ].Length == length )
				{
					float[] f = FloatPool[ i ];
					FloatPool.RemoveAt( i );
					return f;
				}
			}

			return new float[ length ];
		}

		/// <summary>
		/// Get a short array from the pool, or create a new one
		/// </summary>
		/// <param name="length">The length of the array</param>
		/// <returns>A short array of the given length</returns>
		public static short[] GetShort( int length )
		{
			for( int i = 0; i < ShortPool.Count; i++ )
			{
				if( ShortPool[ i ].Length == length )
				{
					short[] s = ShortPool[ i ];
					ShortPool.RemoveAt( i );
					return s;
				}
			}

			return new short[ length ];
		}

		/// <summary>
		/// Get a byte array from the pool, or create a new one
		/// </summary>
		/// <param name="length">The length of the array</param>
		/// <returns>A byte array of the given length</returns>
		public static byte[] GetByte( int length )
		{
			for( int i = 0; i < BytePool.Count; i++ )
			{
				if( BytePool[ i ].Length == length )
				{
					byte[] b = BytePool[ i ];
					BytePool.RemoveAt( i );
					return b;
				}
			}

			return new byte[ length ];
		}

		/// <summary>
		/// Return a float array to the pool for reuse
		/// </summary>
		/// <param name="d">The float array to return</param>
		public static void Return( float[] d )
		{
			FloatPool.Add( d );
		}

		/// <summary>
		/// Return a byte array to the pool for reuse
		/// </summary>
		/// <param name="d">The byte array to return</param>
		public static void Return( byte[] d )
		{
			BytePool.Add( d );
		}

		/// <summary>
		/// Return a short array to the pool for reuse
		/// </summary>
		/// <param name="d">The short array to return</param>
		public static void Return( short[] d )
		{
			ShortPool.Add( d );
		}
	}
}