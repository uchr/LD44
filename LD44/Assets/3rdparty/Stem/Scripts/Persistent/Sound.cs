using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// Defines how sound will play its variations.
	/// </summary>
	[Serializable]
	public enum RetriggerMode
	{
		/// <summary>Play variations in a sequence as they stored in the sound.</summary>
		Sequential,
		/// <summary>Play random variations with possible repetitions.</summary>
		Random,
		/// <summary>Play random variations without repetitions.</summary>
		RandomNoRepeat,
	}

	/// <summary>
	/// Defines how sound volume will be lowered over the distance.
	/// </summary>
	[Serializable]
	public enum AttenuationMode
	{
		/// <summary>A real-world rolloff.</summary>
		Logarithmic,
		/// <summary>A linear rolloff.</summary>
		Linear,
	}

	/// <summary>
	/// The persistent storage for sound variations and the most important audio source settings.
	/// </summary>
	[Serializable]
	public class Sound
	{
		[SerializeField]
		private string name = null;

		[SerializeField]
		private bool muted = false;

		[SerializeField]
		private bool soloed = false;

		[SerializeField]
		private bool unfolded = false;

		[SerializeField]
		private float panStereo = 0.0f;

		[SerializeField]
		private float spatialBlend = 0.0f;

		[SerializeField]
		private AttenuationMode attenuationMode = AttenuationMode.Logarithmic;

		[SerializeField]
		private float spread = 0.0f;

		[SerializeField]
		private float dopplerLevel = 1.0f;

		[SerializeField]
		private float minDistance = 1.0f;

		[SerializeField]
		private float maxDistance = 500.0f;

		[SerializeField]
		private RetriggerMode variationRetriggerMode = RetriggerMode.RandomNoRepeat;

		[SerializeField]
		private List<SoundVariation> variations = new List<SoundVariation>();
		private ReadOnlyCollection<SoundVariation> variationsRO = null;

		[NonSerialized]
		private SoundBank bank = null;

		[NonSerialized]
		private SoundBus bus = null;

		[NonSerialized]
		private int lastVariationIndex = -1;

		internal Sound(SoundBank bank_, string name_)
		{
			bank = bank_;
			name = name_;
		}

		/// <summary>
		/// The flag indicating if the sound can be heard.
		/// </summary>
		/// <value>True, if the sound can be heard. False otherwise.</value>
		/// <remarks>
		/// <para>If the <see cref="Sound.Bus"/> is inaudible, the sound will also be inaudible.</para>
		/// </remarks>
		public bool Audible
		{
			get
			{
				if (bus != null && !bus.Audible)
					return false;

				if (bank != null && bank.Runtime.SoloedSounds > 0)
					return soloed;

				return !muted;
			}
		}

		/// <summary>
		/// The collection of sound variations.
		/// </summary>
		/// <value>A reference to a read-only collection of sound variations.</value>
		public ReadOnlyCollection<SoundVariation> Variations
		{
			get
			{
				if (variationsRO == null)
					variationsRO = variations.AsReadOnly();

				return variationsRO;
			}
		}

		/// <summary>
		/// The sound bank the sound belongs to.
		/// </summary>
		/// <value>A reference to a sound bank.</value>
		public SoundBank Bank
		{
			get { return bank; }
			set
			{
				if (bank != null)
					bank.Runtime.RemoveSound(this);

				bank = value;

				if (bank != null)
					bank.Runtime.AddSound(this);
			}
		}

		/// <summary>
		/// The name of the sound. Used for fast search in corresponding sound bank.
		/// </summary>
		/// <value>Name of the sound.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.Runtime.RemoveSound(this);

				name = value;

				if (bank != null)
					bank.Runtime.AddSound(this);
			}
		}

		/// <summary>
		/// The reference to a sound bus which will manage the sound.
		/// </summary>
		/// <value>A reference to a sound bus.</value>
		/// <remarks>
		/// <para>If set to null, <see cref="SoundBank.DefaultBus"/> will be used.</para>
		/// </remarks>
		public SoundBus Bus
		{
			get { return bus; }
			set { bus = value; }
		}

		/// <summary>
		/// The flag indicating if the sound is muted and can't be heard.
		/// </summary>
		/// <value>True, if the sound is muted. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may be overridden by the <see cref="Sound.Soloed"/> flag, i.e. if the sound is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Muted
		{
			get { return muted; }
			set { muted = value; }
		}

		/// <summary>
		/// The flag indicating if the sound is soloed. If set to true, all other non-solo sounds won't be audible.
		/// </summary>
		/// <value>True, if the sound is soloed. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may override the <see cref="Sound.Muted"/> flag, i.e. if the sound is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Soloed
		{
			get { return soloed; }
			set
			{
				if (soloed == value)
					return;

				soloed = value;

				if (bank != null)
					bank.Runtime.SoloedSounds += (soloed) ? 1 : -1;
			}
		}

		/// <summary>
		/// The attenuation mode defining how sound volume will be lowered over the distance.
		/// </summary>
		/// <value>An enum value.</value>
		/// <remarks>
		/// <para>Note that attenuation rules applies only for 3D sounds. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public AttenuationMode AttenuationMode
		{
			get { return attenuationMode; }
			set { attenuationMode = value; }
		}

		/// <summary>
		/// The retrigger mode defining how the sound will play variations.
		/// </summary>
		/// <value>An enum value.</value>
		public RetriggerMode VariationRetriggerMode
		{
			get { return variationRetriggerMode; }
			set { variationRetriggerMode = value; }
		}

		/// <summary>
		/// The stereo panning parameter defining sound position in a stereo way (left or right).
		/// </summary>
		/// <value>Stereo pan of the sound. The value must be in [-1;1] range.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float PanStereo
		{
			get { return panStereo; }
			set { panStereo = value; }
		}

		/// <summary>
		/// The spatial blend parameter defining how much the sound is affected by 3d spatialisation calculations (attenuation, doppler etc).
		/// </summary>
		/// <value>Spatial blend of the sound. The value must be in [0;1] range.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float SpatialBlend
		{
			get { return spatialBlend; }
			set { spatialBlend = value; }
		}

		/// <summary>
		/// The parameter defining the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space.
		/// </summary>
		/// <value>Spread angle of the sound. The value must be in [0;360] range.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float Spread
		{
			get { return spread; }
			set { spread = value; }
		}

		/// <summary>
		/// The parameter defining the Doppler scale for the sound.
		/// </summary>
		/// <value>Doppler scale of the sound. The value must be greater or equal to zero.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float DopplerLevel
		{
			get { return dopplerLevel; }
			set { dopplerLevel = value; }
		}

		/// <summary>
		/// The parameter defining the boundary within which the sound won't get any louder.
		/// </summary>
		/// <value>Distance in units.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float MinDistance
		{
			get { return minDistance; }
			set { minDistance = value; }
		}

		/// <summary>
		/// The parameter defining the boundary outside which the sound will be inaudible or stop attenuating depending on AttenuationMode value.
		/// </summary>
		/// <value>Distance in units.</value>
		/// <remarks>
		/// <para>This parameter duplicates corresponding parameter from AudioSource. Please refer to Unity Scripting Reference for details.</para>
		/// </remarks>
		public float MaxDistance
		{
			get { return maxDistance; }
			set { maxDistance = value; }
		}

		/// <summary>
		/// The flag indicating whether the sound bank inspector should show advanced settings for the sound.
		/// </summary>
		/// <value>True, if advanced settings are shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the sound bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool Unfolded
		{
			get { return unfolded; }
			set { unfolded = value; }
		}

		internal SoundVariation AddVariation(AudioClip clip)
		{
			SoundVariation variation = new SoundVariation();
			variation.Clip = clip;
			variations.Add(variation);

			return variation;
		}

		internal SoundVariation FetchVariation()
		{
			if (variations.Count == 0)
				return null;

			if (variations.Count == 1)
				return variations[0];

			switch (variationRetriggerMode)
			{
				case RetriggerMode.Sequential: lastVariationIndex = (lastVariationIndex + 1) % variations.Count; break;
				case RetriggerMode.Random: lastVariationIndex = UnityEngine.Random.Range(0, variations.Count); break;
				case RetriggerMode.RandomNoRepeat:
				{
					int newVariation = lastVariationIndex;
					while(newVariation == lastVariationIndex)
						newVariation = UnityEngine.Random.Range(0, variations.Count);

					lastVariationIndex = newVariation;
				}
				break;
			}

			return variations[lastVariationIndex];
		}
	}
}
