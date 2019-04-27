using System;
using System.Collections.Generic;

namespace Stem
{
	internal class SoundBankRuntime
	{
		[NonSerialized]
		private Dictionary<string, List<Sound> > soundByName = new Dictionary<string, List<Sound> >();

		[NonSerialized]
		private Dictionary<string, List<SoundBus> > busByName = new Dictionary<string, List<SoundBus> >();

		internal int SoloedSounds { get; set; }
		internal int SoloedSoundBuses { get; set; }

		internal bool ContainsSound(string name)
		{
			return soundByName.ContainsKey(name);
		}

		internal Sound GetSound(string name)
		{
			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(name, out soundList))
				return null;

			if (soundList.Count == 0)
				return null;

			return soundList[0];
		}

		internal void AddSound(Sound sound)
		{
			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(sound.Name, out soundList))
			{
				soundList = new List<Sound>();
				soundByName.Add(sound.Name, soundList);
			}

			soundList.Add(sound);

			if (sound.Soloed)
				SoloedSounds++;
		}

		internal void RemoveSound(Sound sound)
		{
			List<Sound> soundList = null;
			if (!soundByName.TryGetValue(sound.Name, out soundList))
				return;

			int index = soundList.IndexOf(sound);
			if (index != -1)
				soundList.RemoveAt(index);

			if (sound.Soloed)
				SoloedSounds--;
		}

		internal SoundBus GetSoundBus(string name)
		{
			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(name, out busList))
				return null;

			if (busList.Count == 0)
				return null;

			return busList[0];
		}

		internal void AddSoundBus(SoundBus bus)
		{
			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(bus.Name, out busList))
			{
				busList = new List<SoundBus>();
				busByName.Add(bus.Name, busList);
			}

			busList.Add(bus);

			if (bus.Soloed)
				SoloedSoundBuses++;
		}

		internal void RemoveSoundBus(SoundBus bus)
		{
			List<SoundBus> busList = null;
			if (!busByName.TryGetValue(bus.Name, out busList))
				return;

			int index = busList.IndexOf(bus);
			if (index != -1)
				busList.RemoveAt(index);

			if (bus.Soloed)
				SoloedSoundBuses--;
		}
	}
}
