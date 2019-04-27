using System.Collections.ObjectModel;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The main class for sound playback and bank management.
	/// </summary>
	public static class SoundManager
	{
		private static BankManager<SoundBank, SoundManagerRuntime> bankManager = new BankManager<SoundBank, SoundManagerRuntime>();
		private static SoundInstanceManagerRuntime instanceManagerRuntime = null;
		private static GameObject instanceManagerGameObject = null;
		private static bool shutdown = false;

		/// <summary>
		/// The collection of all registered sound banks.
		/// </summary>
		/// <value>A reference to a read-only collection of sound banks.</value>
		public static ReadOnlyCollection<SoundBank> Banks
		{
			get { return bankManager.Banks; }
		}

		/// <summary>
		/// The primary sound bank that will be searched first in case of name collisions.
		/// </summary>
		/// <value>A reference to a primary sound bank.</value>
		public static SoundBank PrimaryBank
		{
			get { return bankManager.PrimaryBank; }
			set { bankManager.PrimaryBank = value; }
		}

		/// <summary>
		/// Registers new sound bank.
		/// </summary>
		/// <param name="bank">A reference to a sound bank to register.</param>
		/// <returns>
		/// True, if sound bank was succesfully registered. False otherwise.
		/// </returns>
		public static bool RegisterBank(SoundBank bank)
		{
			if (shutdown)
				return false;

			return bankManager.RegisterBank(bank);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="position">Position of the sound.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// </remarks>
		public static void Play3D(string name, Vector3 position)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play3D(name, position);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// </remarks>
		public static void Play3D(string name, Vector3 position, float volume)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play3D(name, position, volume);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// </remarks>
		public static void Play3D(string name, Vector3 position, float volume, float pitch)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play3D(name, position, volume, pitch);
		}

		/// <summary>
		/// Plays one-shot sound in 3D space.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public static void Play3D(string name, Vector3 position, float volume, float pitch, float delay)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play3D(name, position, volume, pitch, delay);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// </remarks>
		public static void Play(string name)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play(name);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// </remarks>
		public static void Play(string name, float volume)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play(name, volume);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// </remarks>
		public static void Play(string name, float volume, float pitch)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play(name, volume, pitch);
		}

		/// <summary>
		/// Plays one-shot sound.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public static void Play(string name, float volume, float pitch, float delay)
		{
			if (shutdown)
				return;

			SoundManagerRuntime runtime = bankManager.FetchRuntime(name);
			if (runtime != null)
				runtime.Play(name, volume, pitch, delay);
		}

		/// <summary>
		/// Searches for the specified sound with a matching name.
		/// </summary>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// </remarks>
		public static Sound GetSound(string name)
		{
			if (shutdown)
				return null;

			// Check primary bank first
			SoundBank primaryBank = bankManager.PrimaryBank;
			if (primaryBank != null && primaryBank.ContainsAsset(name))
				return primaryBank.GetSound(name);

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				SoundBank bank = bankManager.Banks[i];

				// Skip primary bank
				if (bank == primaryBank)
					continue;

				if (bank.ContainsAsset(name))
					return bank.GetSound(name);
			}

			return null;
		}

		/// <summary>
		/// Grabs an empty sound instance from the sound pool. Used for manual playback and custom mixing logic.
		/// </summary>
		/// <returns>
		/// A reference to an empty sound instance.
		/// </returns>
		/// <remarks>
		/// <para>This method may increase the size of the sound pool causing additional memory allocations.</para>
		/// <para>When a sound instance is not needed anymore, use <see cref="ReleaseSound(SoundInstance)"/> to return it back to the sound pool.</para>
		/// </remarks>
		public static SoundInstance GrabSound()
		{
			if (shutdown)
				return null;

			SoundInstanceManagerRuntime runtime = FetchSoundManager();
			if (runtime == null)
				return null;

			return runtime.GrabSound();
		}

		/// <summary>
		/// Grabs a sound instance from the sound pool. Used for manual playback and custom mixing logic.
		/// </summary>
		/// <returns>
		/// A reference to a sound instance.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have sounds with a matching name, primary sound bank will be checked first.
		/// Within a bank, the first occurrence of found sound will be used.</para>
		/// <para>This method may increase the size of the sound pool causing additional memory allocations.</para>
		/// <para>When a sound instance is not needed anymore, use <see cref="ReleaseSound(SoundInstance)"/> to return it back to the sound pool.</para>
		/// </remarks>
		public static SoundInstance GrabSound(string name)
		{
			if (shutdown)
				return null;

			Sound sound = GetSound(name);
			if (sound == null)
				return null;

			SoundInstanceManagerRuntime runtime = FetchSoundManager();
			if (runtime == null)
				return null;

			return runtime.GrabSound(sound);
		}

		/// <summary>
		/// Releases sound instance and return it back to the sound pool.
		/// </summary>
		/// <param name="instance">A reference to a sound instance.</param>
		/// <returns>
		/// True, if the sound instance was successfully returned to sound pool. False otherwise.
		/// </returns>
		/// <remarks>
		/// <para>Once the sound instance is returned back to a sound pool, it's possible to reuse it again by calling <see cref="GrabSound()"/> or <see cref="GrabSound(string)"/>.</para>
		/// </remarks>
		public static bool ReleaseSound(SoundInstance instance)
		{
			if (shutdown)
				return false;

			SoundInstanceManagerRuntime runtime = FetchSoundManager();
			if (runtime == null)
				return false;

			return runtime.ReleaseSound(instance);
		}

		/// <summary>
		/// Stops all playing sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also stop all sounds instances returned from <see cref="GrabSound()"/> or <see cref="GrabSound(string)"/>.</para>
		/// </remarks>
		public static void Stop()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].Stop();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.Stop();
		}

		/// <summary>
		/// Pauses all playing sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also stop all sounds instances returned from <see cref="Stem.SoundManager.GrabSound()"/> or <see cref="Stem.SoundManager.GrabSound(string)"/>.</para>
		/// </remarks>
		public static void Pause()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].Pause();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.Pause();
		}

		/// <summary>
		/// Resumes all paused sounds.
		/// </summary>
		/// <remarks>
		/// <para>This method will also resume all sounds instances returned from <see cref="Stem.SoundManager.GrabSound()"/> or <see cref="Stem.SoundManager.GrabSound(string)"/>.</para>
		/// </remarks>
		public static void UnPause()
		{
			if (shutdown)
				return;

			ReadOnlyCollection<SoundManagerRuntime> runtimes = bankManager.Runtimes;
			for (int i = 0; i < runtimes.Count; i++)
				runtimes[i].UnPause();

			if (instanceManagerRuntime != null)
				instanceManagerRuntime.UnPause();
		}

		internal static void Shutdown()
		{
			shutdown = true;
		}

		private static SoundInstanceManagerRuntime FetchSoundManager()
		{
			if (shutdown)
				return null;

			if (instanceManagerRuntime != null)
				return instanceManagerRuntime;

			instanceManagerGameObject = new GameObject();
			instanceManagerGameObject.name = "Sound Instance Pool";
			GameObject.DontDestroyOnLoad(instanceManagerGameObject);

			instanceManagerRuntime = instanceManagerGameObject.AddComponent<SoundInstanceManagerRuntime>();
			return instanceManagerRuntime;
		}
	}
}
