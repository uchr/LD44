using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class SoundBusRuntime
	{
		private GameObject root = null;
		private List<SoundInstance> usedSounds = null;
		private List<SoundInstance> freeSounds = null;

		private SoundBus bus = null;

		internal SoundBusRuntime(Transform transform, SoundBus bus_)
		{
			bus = bus_;

			root = new GameObject();
			root.transform.parent = transform;
			root.name = bus.Name;

			freeSounds = new List<SoundInstance>(bus.Polyphony);
			usedSounds = new List<SoundInstance>(bus.Polyphony);

			for (int i = 0; i < bus.Polyphony; i++)
			{
				SoundInstance managedInstance = new SoundInstance(null, string.Format("Sound {0}", i + 1), root.transform);
				freeSounds.Add(managedInstance);
			}
		}

		internal void Stop()
		{
			for (int i = usedSounds.Count - 1; i >= 0; i--)
				ReleaseSound(i);
		}

		internal void Pause()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].Pause();
		}

		internal void UnPause()
		{
			for (int i = 0; i < usedSounds.Count; i++)
				usedSounds[i].UnPause();
		}

		internal void Update(SoundBank bank)
		{
			for (int i = usedSounds.Count - 1; i >= 0; i--)
			{
				SoundInstance instance = usedSounds[i];
				instance.Update();

				if (!instance.Paused && !instance.Playing)
					ReleaseSound(i);
			}
		}

		internal SoundInstance GrabSound(Sound sound)
		{
			if (bus.Polyphony < 1)
				return null;

			// get free sound
			if (freeSounds.Count > 0)
			{
				int last = freeSounds.Count - 1;

				SoundInstance instance = freeSounds[last];
				instance.Sound = sound;

				freeSounds.RemoveAt(last);
				usedSounds.Add(instance);

				return instance;
			}

			if (!bus.AllowVoiceStealing)
				return null;

			// steal oldest used sound
			int oldestSoundTimeSamples = -1;
			SoundInstance oldestSound = null;

			for (int i = 0; i < usedSounds.Count; i++)
			{
				SoundInstance instance = usedSounds[i];
				if (oldestSoundTimeSamples <= instance.TimeSamples)
				{
					oldestSound = instance;
					oldestSoundTimeSamples = instance.TimeSamples;
				}
			}

			oldestSound.Sound = sound;
			return oldestSound;
		}

		private void ReleaseSound(int index)
		{
			SoundInstance instance = usedSounds[index];

			instance.Stop();
			instance.Sound = null;

			int last = usedSounds.Count - 1;
			usedSounds[index] = usedSounds[last];
			usedSounds.RemoveAt(last);

			freeSounds.Add(instance);
		}
	}
}
