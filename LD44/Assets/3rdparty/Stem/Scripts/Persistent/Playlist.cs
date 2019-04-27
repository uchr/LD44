using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The persistent collection of playlist tracks.
	/// </summary>
	[Serializable]
	public class Playlist : ISerializationCallbackReceiver
	{
		[SerializeField]
		private string name = null;

		[SerializeField]
		private List<PlaylistTrack> tracks = new List<PlaylistTrack>();
		private ReadOnlyCollection<PlaylistTrack> tracksRO = null;

		[SerializeField]
		private bool unfolded = false;

		[NonSerialized]
		private MusicBank bank = null;

		internal Playlist(MusicBank bank_, string name_)
		{
			bank = bank_;
			name = name_;
		}

		/// <summary>
		/// Prepares playlist for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
		}

		/// <summary>
		/// Prepares playlist for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			foreach (PlaylistTrack track in tracks)
				track.Playlist = this;
		}

		/// <summary>
		/// The collection of playlist tracks.
		/// </summary>
		/// <value>A reference to a read-only collection of playlist tracks.</value>
		public ReadOnlyCollection<PlaylistTrack> Tracks
		{
			get
			{
				if (tracksRO == null)
					tracksRO = tracks.AsReadOnly();

				return tracksRO;
			}
		}

		/// <summary>
		/// The music bank the playlist belongs to.
		/// </summary>
		/// <value>A reference to a music bank.</value>
		public MusicBank Bank
		{
			get { return bank; }
			set
			{
				if (bank != null)
					bank.Runtime.RemovePlaylist(this);

				bank = value;

				if (bank != null)
					bank.Runtime.AddPlaylist(this);
			}
		}

		/// <summary>
		/// The name of the playlist. Used for fast search in corresponding music bank.
		/// </summary>
		/// <value>Name of the playlist.</value>
		public string Name
		{
			get { return name; }
			set
			{
				if (name == value)
					return;

				if (bank != null)
					bank.Runtime.RemovePlaylist(this);

				name = value;

				if (bank != null)
					bank.Runtime.AddPlaylist(this);
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show advanced settings for the playlist.
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
