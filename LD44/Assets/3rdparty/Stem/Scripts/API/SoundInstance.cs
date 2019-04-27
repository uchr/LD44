using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The game object with audio source component. Used for manual playback and custom mixing logic.
	/// </summary>
	public class SoundInstance
	{
		private GameObject gameObject = null;
		private Transform transform = null;
		private AudioSource source = null;
		private Sound sound = null;
		private bool looped = false;
		private bool paused = false;
		private float volume = 0.0f;
		private float pitch = 1.0f;
		private float delay = 0.0f;

		internal SoundInstance(Sound sound_, string name, Transform root)
		{
			sound = sound_;

			gameObject = new GameObject();
			gameObject.name = name;
			gameObject.transform.parent = root;

			transform = gameObject.transform;

			source = gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
		}

		/// <summary>
		/// The transform component from the game object of the sound instance. Please refer to Unity Scripting Reference for details.
		/// </summary>
		/// <value>A reference to a transform component.</value>
		public Transform Transform
		{
			get { return transform; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is paused.
		/// </summary>
		/// <value>True, if sound instance is paused. False otherwise.</value>
		public bool Paused
		{
			get { return paused; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is playing.
		/// </summary>
		/// <value>True, if sound instance is playing. False otherwise.</value>
		public bool Playing
		{
			get { return source.isPlaying; }
		}

		/// <summary>
		/// The playback position in samples.
		/// </summary>
		/// <value>An offset in samples from the start of an audio clip.</value>
		public int TimeSamples
		{
			get { return source.timeSamples; }
		}

		/// <summary>
		/// The reference to a sound which will be used for playback. Changing this value allows playing different sounds.
		/// </summary>
		/// <value>A reference to a sound.</value>
		public Sound Sound
		{
			get { return sound; }
			set { sound = value; }
		}

		/// <summary>
		/// The flag indicating that the sound instance is looping. Set whether it should replay the audio clip after it finishes.
		/// </summary>
		/// <value>True, if sound instance is looping. False otherwise.</value>
		public bool Looped
		{
			get { return looped; }
			set { looped = value; }
		}

		/// <summary>
		/// The volume property allows controlling the overall level of sound coming to the audio source.
		/// </summary>
		/// <value>Volume value in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The pitch property allows controlling how high or low the tone of the audio source is.
		/// </summary>
		/// <value>Pitch value in [3;3] range.</value>
		public float Pitch
		{
			get { return pitch; }
			set { pitch = value; }
		}

		/// <summary>
		/// Plays sound in 3D space.
		/// </summary>
		/// <param name="position">Position of the sound.</param>
		public void Play3D(Vector3 position)
		{
			if (sound == null)
				return;

			SoundVariation variation = sound.FetchVariation();
			PlayInternal(position, variation.Clip, variation.Volume, variation.Pitch, variation.Delay);
		}

		/// <summary>
		/// Plays sound in 3D space.
		/// </summary>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// </remarks>
		public void Play3D(Vector3 position, float volume)
		{
			if (sound == null)
				return;

			SoundVariation variation = sound.FetchVariation();
			PlayInternal(position, variation.Clip, volume, variation.Pitch, variation.Delay);
		}

		/// <summary>
		/// Plays sound in 3D space.
		/// </summary>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// </remarks>
		public void Play3D(Vector3 position, float volume, float pitch)
		{
			if (sound == null)
				return;

			SoundVariation variation = sound.FetchVariation();
			PlayInternal(position, variation.Clip, volume, pitch, variation.Delay);
		}

		/// <summary>
		/// Plays sound in 3D space.
		/// </summary>
		/// <param name="position">Position of the sound.</param>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public void Play3D(Vector3 position, float volume, float pitch, float delay)
		{
			if (sound == null)
				return;

			SoundVariation variation = sound.FetchVariation();
			PlayInternal(position, variation.Clip, volume, pitch, delay);
		}

		/// <summary>
		/// Plays sound.
		/// </summary>
		public void Play()
		{
			Play3D(Vector3.zero);
		}

		/// <summary>
		/// Plays sound.
		/// </summary>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// </remarks>
		public void Play(float volume)
		{
			Play3D(Vector3.zero, volume);
		}

		/// <summary>
		/// Plays sound.
		/// </summary>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// </remarks>
		public void Play(float volume, float pitch)
		{
			Play3D(Vector3.zero, volume, pitch);
		}

		/// <summary>
		/// Plays sound.
		/// </summary>
		/// <param name="volume">Volume of the sound. Value must be in [0;1] range.</param>
		/// <param name="pitch">Pitch of the sound. Value must be in [-3;3] range.</param>
		/// <param name="delay">Delay of the sound. Value must be greater or equal to zero.</param>
		/// <remarks>
		/// <para>Volume parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Volume"/> value.</para>
		/// <para>Pitch parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Pitch"/> value.</para>
		/// <para>Delay parameter value will override <see cref="Stem.SoundVariation"/>.<see cref="Stem.SoundVariation.Delay"/> value.</para>
		/// </remarks>
		public void Play(float volume, float pitch, float delay)
		{
			Play3D(Vector3.zero, volume, pitch, delay);
		}

		/// <summary>
		/// Stops sound.
		/// </summary>
		public void Stop()
		{
			paused = false;
			source.Stop();
		}

		/// <summary>
		/// Pauses sound.
		/// </summary>
		public void Pause()
		{
			paused = true;
			source.Pause();
		}

		/// <summary>
		/// Resumes sound.
		/// </summary>
		public void UnPause()
		{
			paused = false;
			source.UnPause();
		}

		internal void Update()
		{
			if (sound == null)
				return;

			SoundBus bus = sound.Bus;

			source.mute = !sound.Audible;
			source.volume = volume * bus.Volume;
			source.pitch = pitch;
		}

		private void PlayInternal(Vector3 position, AudioClip clip, float volume_, float pitch_, float delay_)
		{
			paused = false;

			volume = volume_;
			pitch = pitch_;
			delay = delay_;

			SoundBus bus = sound.Bus;

			source.clip = clip;
			source.volume = volume * bus.Volume;
			source.pitch = pitch;
			source.loop = looped;

			source.spatialBlend = sound.SpatialBlend;
			source.panStereo = sound.PanStereo;
			source.dopplerLevel = sound.DopplerLevel;
			source.spread = sound.Spread;
			source.rolloffMode = (AudioRolloffMode)sound.AttenuationMode;
			source.minDistance = sound.MinDistance;
			source.maxDistance = sound.MaxDistance;

			source.PlayDelayed(delay);
			source.outputAudioMixerGroup = bus.MixerGroup;

			transform.localPosition = position;
		}
	}
}
