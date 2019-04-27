using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// Defines how new sounds will be created after the drag-drop event.
	/// </summary>
	public enum SoundBatchImportMode
	{
		/// <summary>A single sound will be created. Provided audio clips will be used as variations.</summary>
		SingleWithVariations,
		/// <summary>A sound with a single variation will be created per each provided audio clip.</summary>
		MultiplePerClip,
	}

	/// <summary>
	/// The persistent storage for sounds and sound buses.
	/// </summary>
	[CreateAssetMenu(fileName = "New Sound Bank", menuName = "Stem/Sound Bank")]
	public class SoundBank : ScriptableObject, ISerializationCallbackReceiver, IBank
	{
		[SerializeField]
		private SoundBatchImportMode soundBatchImportMode = SoundBatchImportMode.SingleWithVariations;

		[SerializeField]
		private bool showSounds = true;

		[SerializeField]
		private bool showSoundBuses = true;

		[SerializeField]
		private List<Sound> sounds = new List<Sound>();
		private ReadOnlyCollection<Sound> soundsRO = null;

		[SerializeField]
		private List<SoundBus> buses = new List<SoundBus>();
		private ReadOnlyCollection<SoundBus> busesRO = null;

		[SerializeField]
		private int[] busIndices = null;

		[NonSerialized]
		private SoundBankRuntime runtime = new SoundBankRuntime();

		internal SoundBank()
		{
			AddSoundBus("Master");
		}

		/// <summary>
		/// Determines whether the sound bank contains a sound with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// True, if the sound bank contains sound with a matching name. False otherwise.
		/// </returns>
		public bool ContainsAsset(string name)
		{
			return runtime.ContainsSound(name);
		}

		/// <summary>
		/// Prepares sound bank for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
			if (sounds.Count > 0)
			{
				busIndices = new int[sounds.Count];
				for (int i = 0; i < sounds.Count; i++)
				{
					SoundBus bus = sounds[i].Bus;
					busIndices[i] = buses.IndexOf(bus);
				}
			}
		}

		/// <summary>
		/// Prepares sound bank for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			for (int i = 0; i < sounds.Count; i++)
			{
				Sound sound = sounds[i];
				SoundBus bus = null;
				if (busIndices != null)
				{
					int index = busIndices[i];
					bus = (index != -1) ? buses[index] : null;
				}
				sound.Bus = bus;
				sound.Bank = this;
			}

			foreach (SoundBus bus in buses)
				bus.Bank = this;

			SoundManager.RegisterBank(this);
		}

		/// <summary>
		/// The collection of sounds.
		/// </summary>
		/// <value>A reference to a read-only collection of sounds.</value>
		public ReadOnlyCollection<Sound> Sounds
		{
			get
			{
				if (soundsRO == null)
					soundsRO = sounds.AsReadOnly();

				return soundsRO;
			}
		}

		/// <summary>
		/// The collection of sound buses.
		/// </summary>
		/// <value>A reference to a read-only collection of sound buses.</value>
		public ReadOnlyCollection<SoundBus> Buses
		{
			get
			{
				if (busesRO == null)
					busesRO = buses.AsReadOnly();

				return busesRO;
			}
		}

		/// <summary>
		/// The sound bus which will be used by default on newly created sounds.
		/// </summary>
		/// <value>A reference to a sound bus.</value>
		public SoundBus DefaultBus
		{
			get { return buses[0]; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show the 'Sounds' group.
		/// </summary>
		/// <value>True, if the 'Sounds' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowSounds
		{
			get { return showSounds; }
			set { showSounds = value; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show the 'Buses' group.
		/// </summary>
		/// <value>True, if the 'Buses' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowSoundBuses
		{
			get { return showSoundBuses; }
			set { showSoundBuses = value; }
		}

		/// <summary>
		/// The batch import mode defining how new sounds will be created after the drag-drop event.
		/// </summary>
		/// <value>An enum value.</value>
		public SoundBatchImportMode SoundBatchImportMode
		{
			get { return soundBatchImportMode; }
			set { soundBatchImportMode = value; }
		}

		internal SoundBankRuntime Runtime
		{
			get { return runtime; }
		}

		/// <summary>
		/// Searches for the specified sound with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a sound, if found. Null reference otherwise.
		/// </returns>
		public Sound GetSound(string name)
		{
			return runtime.GetSound(name);
		}

		/// <summary>
		/// Adds an empty sound to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;

			sounds.Add(sound);
			runtime.AddSound(sound);
			return sound;
		}

		/// <summary>
		/// Adds a sound with a single variation to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="clip">A reference to the audio clip with audio data.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name, AudioClip clip)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;

			sound.AddVariation(clip);

			sounds.Add(sound);
			runtime.AddSound(sound);
			return sound;
		}

		/// <summary>
		/// Adds a sound with multiple variations to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound.</param>
		/// <param name="clips">An array of audio clips with audio data.</param>
		/// <returns>
		/// A reference to a newly created sound.
		/// </returns>
		public Sound AddSound(string name, AudioClip[] clips)
		{
			Sound sound = new Sound(this, name);
			sound.Bus = DefaultBus;

			if (clips != null)
			{
				for (int i = 0; i < clips.Length; i++)
				{
					if (clips[i] == null)
						continue;

					sound.AddVariation(clips[i]);
				}
			}

			sounds.Add(sound);
			runtime.AddSound(sound);
			return sound;
		}

		/// <summary>
		/// Removes existing sound from the sound bank.
		/// </summary>
		/// <param name="sound">A reference to a sound.</param>
		/// <remarks>
		/// <para>This method does nothing if the sound was not found in the sound bank.</para>
		/// </remarks>
		public void RemoveSound(Sound sound)
		{
			int index = sounds.IndexOf(sound);
			if (index != -1)
				sounds.RemoveAt(index);

			runtime.RemoveSound(sound);
		}

		/// <summary>
		/// Searches for the specified sound bus with a matching name.
		/// </summary>
		/// <param name="name">Name of the sound bus.</param>
		/// <returns>
		/// A reference to a sound bus, if found. Null reference otherwise.
		/// </returns>
		public SoundBus GetSoundBus(string name)
		{
			return runtime.GetSoundBus(name);
		}

		/// <summary>
		/// Adds a new sound bus to the sound bank.
		/// </summary>
		/// <param name="name">Name of the sound bus.</param>
		/// <returns>
		/// A reference to a newly created sound bus.
		/// </returns>
		public SoundBus AddSoundBus(string name)
		{
			SoundBus bus = new SoundBus(this, name);

			buses.Add(bus);
			runtime.AddSoundBus(bus);
			return bus;
		}

		/// <summary>
		/// Removes existing sound bus from the sound bank.
		/// </summary>
		/// <param name="bus">A reference to a sound bus.</param>
		/// <remarks>
		/// <para>This method does nothing if the sound bus was not found in the sound bank.</para>
		/// </remarks>
		public void RemoveSoundBus(SoundBus bus)
		{
			if (buses.Count == 1)
				return;

			int index = buses.IndexOf(bus);
			if (index != -1)
				buses.RemoveAt(index);

			foreach (Sound sound in sounds)
				if (sound.Bus == bus)
					sound.Bus = DefaultBus;

			runtime.RemoveSoundBus(bus);
		}
	}
}
