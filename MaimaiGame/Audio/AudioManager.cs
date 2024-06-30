using System;
using ManagedBass;
using Microsoft.Xna.Framework.Audio;

namespace MaimaiGame.Audio;

public static class AudioManager
{
	public static float Offset { get; private set; }

	public static float Volume
	{
		get => (float) Math.Round(Bass.GlobalStreamVolume / 10000.0f, 2);
		set
		{
			value = (float) Math.Clamp(Math.Round(value, 2), 0.0f, 1.0f);

			Bass.GlobalStreamVolume = (int) (value * 10000.0f);
			SoundEffect.MasterVolume = value;
		}
	}

	private static bool _initialised;

	public static void Initialise()
	{
		if (_initialised)
			return;

		_initialised = true;

		if (!Bass.Init())
			throw new Exception(Bass.LastError.ToString());

		Offset = Configuration.Instance.Audio.Offset;
		Volume = Configuration.Instance.Audio.Volume;
	}
}