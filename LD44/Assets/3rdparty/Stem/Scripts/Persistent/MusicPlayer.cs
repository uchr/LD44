using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Stem
{
	/// <summary>
	/// The persistent storage for playback rules of a particular playlist.
	/// </summary>
	[Serializable]
	public class MusicPlayer
	{
		[NonSerialized]
		private Playlist playlist = null;

		[SerializeField]
		private string name = null;

		[SerializeField]
		private AudioMixerGroup mixerGroup = null;

		[SerializeField]
		private bool autoAdvance = true;

		[SerializeField]
		private bool shuffle = false;

		[SerializeField]
		private bool loop = false;

		[SerializeField]
		private float fade = 0.5f;

		[SerializeField]
		private float volume = 1.0f;

		[SerializeField]
		private bool muted = false;

		[SerializeField]
		private bool soloed = false;

		[SerializeField]
		private bool unfolded = false;

		[NonSerialized]
		private MusicBank bank = null;

		internal MusicPlayer(MusicBank bank_, string name_)
		{
			bank = bank_;
			name = name_;
		}

		/// <summary>
		/// The flag indicating if the music player can be heard.
		/// </summary>
		/// <value>True, if the music player can be heard. False otherwise.</value>
		public bool Audible
		{
			get
			{
				if (bank != null && bank.Runtime.SoloedMusicPlayers > 0)
					return soloed;

				return !muted;
			}
		}

		/// <summary>
		/// The music bank the music player belongs to.
		/// </summary>
		/// <value>A reference to a music bank.</value>
		public MusicBank Bank
		{
			get { return bank; }
			set
			{
				if (bank != null)
					bank.Runtime.RemoveMusicPlayer(this);

				bank = value;

				if (bank != null)
					bank.Runtime.AddMusicPlayer(this);
			}
		}

		/// <summary>
		/// The name of the music player. Used for fast search in corresponding music bank.
		/// </summary>
		/// <value>Name of the music player.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.Runtime.RemoveMusicPlayer(this);

				name = value;

				if (bank != null)
					bank.Runtime.AddMusicPlayer(this);
			}
		}

		/// <summary>
		/// The reference to a playlist which will be played.
		/// </summary>
		/// <value>A reference to a playlist.</value>
		public Playlist Playlist
		{
			get { return playlist; }
			set { playlist = value; }
		}

		/// <summary>
		/// The reference to an audio mixer group. Please refer to Unity Scripting Reference for details.
		/// </summary>
		/// <value>A reference to a mixer group.</value>
		public AudioMixerGroup MixerGroup
		{
			get { return mixerGroup; }
			set { mixerGroup = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should auto advance to next playlist track it finishes.
		/// </summary>
		/// <value>True, if music player is auto advancing playlist tracks. False otherwise.</value>
		/// <remarks>
		/// <para>This flag works in pair with <see cref="MusicPlayer.Loop"/> flag. There are four possible combinations:
		/// <list type="number">
		/// <item><description>Both flags false — play the current track once and then stop.</description></item>
		/// <item><description>Both flags true — play all playlist tracks in a loop and never stop.</description></item>
		/// <item><description><see cref="MusicPlayer.Loop"/> is false, <see cref="MusicPlayer.AutoAdvance"/> is true — play all playlist tracks once and then stop.</description></item>
		/// <item><description><see cref="MusicPlayer.Loop"/> is true, <see cref="MusicPlayer.AutoAdvance"/> is false — play the current track in a loop and never stop.</description></item>
		/// </list>
		/// </para>
		/// </remarks>
		public bool AutoAdvance
		{
			get { return autoAdvance; }
			set { autoAdvance = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should play tracks in random order.
		/// </summary>
		/// <value>True, if the music player is playing playlist tracks in random order. False if it's playing sequentially.</value>
		/// <remarks>
		/// <para>Note that tracks will be reshuffled again after the player will finish playing all the tracks.</para>
		/// </remarks>
		public bool Shuffle
		{
			get { return shuffle; }
			set { shuffle = value; }
		}

		/// <summary>
		/// The flag indicating whether the music player should repeat playlist tracks after they finish.
		/// </summary>
		/// <value>True, if the music player is looping playlist tracks. False otherwise.</value>
		/// <remarks>
		/// <para>This flag works in pair with <see cref="MusicPlayer.AutoAdvance"/> flag. There are four possible combinations:
		/// <list type="number">
		/// <item><description>Both flags false — play the current track once and then stop.</description></item>
		/// <item><description>Both flags true — play all playlist tracks in a loop and never stop.</description></item>
		/// <item><description><see cref="MusicPlayer.Loop"/> is false, <see cref="MusicPlayer.AutoAdvance"/> is true — play all playlist tracks once and then stop.</description></item>
		/// <item><description><see cref="MusicPlayer.Loop"/> is true, <see cref="MusicPlayer.AutoAdvance"/> is false — play the current track in a loop and never stop.</description></item>
		/// </list>
		/// </para>
		/// </remarks>
		public bool Loop
		{
			get { return loop; }
			set { loop = value; }
		}

		/// <summary>
		/// The crossfade parameter that is used when the music player transitions between tracks or playback states.
		/// </summary>
		/// <value>Crossfade duration in seconds.</value>
		public float Fade
		{
			get { return fade; }
			set { fade = value; }
		}

		/// <summary>
		/// The volume of the music player.
		/// </summary>
		/// <value>Volume of the music player. Value must be in [0;1] range.</value>
		public float Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		/// <summary>
		/// The flag indicating if the music player is muted and can't be heard.
		/// </summary>
		/// <value>True, if the music player is muted. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may be overridden by the <see cref="MusicPlayer.Soloed"/> flag, i.e. if the music player is simultaneously muted and soloed it'll be audible.</para>
		/// </remarks>
		public bool Muted
		{
			get { return muted; }
			set { muted = value; }
		}

		/// <summary>
		/// The flag indicating if the music player is soloed. If set to true, all other non-solo music players won't be audible.
		/// </summary>
		/// <value>True, if the music player is soloed. False otherwise.</value>
		/// <remarks>
		/// <para>This flag may override the <see cref="MusicPlayer.Muted"/> flag, i.e. if the music player is simultaneously muted and soloed it'll be audible.</para>
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
					bank.Runtime.SoloedMusicPlayers += (soloed) ? 1 : -1;
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show advanced settings for the music player.
		/// </summary>
		/// <value>True, if advanced settings are shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool Unfolded
		{
			get { return unfolded; }
			set { unfolded = value; }
		}
	}
}
