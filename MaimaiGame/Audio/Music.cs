using System;
using System.IO;
using ManagedBass;

namespace MaimaiGame.Audio;

public class Music : IDisposable
{
	public float CurrentTime
	{
		get
		{
			long bytes = Bass.ChannelGetPosition(_handle);
			double seconds = Bass.ChannelBytes2Seconds(_handle, bytes);
			return (float) Math.Round(seconds, 4) + AudioManager.Offset;
		}
	}

	public bool IsPlaying => Bass.ChannelIsActive(_handle) == PlaybackState.Playing;

	public float Volume
	{
		get
		{
			return (float) Math.Clamp(
				Math.Round(Bass.ChannelGetAttribute(_handle, ChannelAttribute.Volume), 4),
				0.0f,
				1.0f
			);
		}
		set
		{
			value = (float) Math.Clamp(Math.Round(value, 4), 0.0f, 1.0f);
			Bass.ChannelSetAttribute(_handle, ChannelAttribute.Volume, value);
		}
	}

	private readonly int _handle;

	private Music(int handle)
	{
		_handle = handle;
	}

	public void Play(bool restart = false)
	{
		bool result = Bass.ChannelPlay(_handle, restart);

		if (!result)
			throw new Exception(Bass.LastError.ToString());
	}

	public void Pause()
	{
		bool result = Bass.ChannelPause(_handle);

		if (!result)
			throw new Exception(Bass.LastError.ToString());
	}

	public static Music Load(string path)
	{
		if (!File.Exists(path))
			throw new Exception($"File \"{path}\" doesn't exist.");

		int handle = Bass.CreateStream(path);

		if (handle == 0)
			throw new Exception(Bass.LastError.ToString());

		return new Music(handle);
	}

	public void Dispose()
	{
		Bass.StreamFree(_handle);
		GC.SuppressFinalize(this);
	}
}