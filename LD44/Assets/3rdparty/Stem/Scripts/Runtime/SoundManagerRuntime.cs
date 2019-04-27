using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class SoundManagerRuntime : MonoBehaviour, IManagerRuntime<SoundBank>
	{
		private SoundBank bank = null;
		private Dictionary<SoundBus, SoundBusRuntime> busRuntimes = new Dictionary<SoundBus, SoundBusRuntime>();

		public void Init(SoundBank bank_)
		{
			busRuntimes.Clear();
			bank = bank_;

			foreach (SoundBus bus in bank.Buses)
			{
				SoundBusRuntime busRuntime = new SoundBusRuntime(transform, bus);
				busRuntimes.Add(bus, busRuntime);
			}
		}

		internal void Play3D(string name, Vector3 position)
		{
			Sound sound = bank.GetSound(name);
			if (sound == null)
				return;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
				return;

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return;

			instance.Play3D(position);
		}

		internal void Play3D(string name, Vector3 position, float volume)
		{
			Sound sound = bank.GetSound(name);
			if (sound == null)
				return;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
				return;

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return;

			instance.Play3D(position, volume);
		}

		internal void Play3D(string name, Vector3 position, float volume, float pitch)
		{
			Sound sound = bank.GetSound(name);
			if (sound == null)
				return;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
				return;

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return;

			instance.Play3D(position, volume, pitch);
		}

		internal void Play3D(string name, Vector3 position, float volume, float pitch, float delay)
		{
			Sound sound = bank.GetSound(name);
			if (sound == null)
				return ;

			SoundBus bus = sound.Bus;
			if (bus == null)
				bus = bank.DefaultBus;

			SoundBusRuntime runtime = null;
			if (!busRuntimes.TryGetValue(bus, out runtime))
				return;

			SoundInstance instance = runtime.GrabSound(sound);
			if (instance == null)
				return;

			instance.Play3D(position, volume, pitch, delay);
		}

		internal void Play(string name)
		{
			Play3D(name, Vector3.zero);
		}

		internal void Play(string name, float volume)
		{
			Play3D(name, Vector3.zero, volume);
		}

		internal void Play(string name, float volume, float pitch)
		{
			Play3D(name, Vector3.zero, volume, pitch);
		}

		internal void Play(string name, float volume, float pitch, float delay)
		{
			Play3D(name, Vector3.zero, volume, pitch, delay);
		}

		internal void Stop()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Stop();
			}
		}

		internal void Pause()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Pause();
			}
		}

		internal void UnPause()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.UnPause();
			}
		}

		private void Update()
		{
			Dictionary<SoundBus, SoundBusRuntime>.Enumerator enumerator = busRuntimes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoundBusRuntime runtime = enumerator.Current.Value;
				runtime.Update(bank);
			}
		}

		private void OnDestroy()
		{
			SoundManager.Shutdown();
		}
	}
}
