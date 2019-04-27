using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	/// <summary>
	/// The persistent storage for playlists and music players.
	/// </summary>
	[CreateAssetMenu(fileName = "New Music Bank", menuName = "Stem/Music Bank")]
	public class MusicBank : ScriptableObject, ISerializationCallbackReceiver, IBank
	{
		[SerializeField]
		private bool showPlaylists = true;

		[SerializeField]
		private bool showPlayers = true;

		[SerializeField]
		private List<Playlist> playlists = new List<Playlist>();
		private ReadOnlyCollection<Playlist> playlistsRO = null;

		[SerializeField]
		private List<MusicPlayer> players = new List<MusicPlayer>();
		private ReadOnlyCollection<MusicPlayer> playersRO = null;

		[SerializeField]
		private int[] playlistIndices = null;

		[NonSerialized]
		private MusicBankRuntime runtime = new MusicBankRuntime();

		/// <summary>
		/// Determines whether the music bank contains a music player with a matching name.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// True, if the music bank contains music player with a matching name. False otherwise.
		/// </returns>
		public bool ContainsAsset(string name)
		{
			return runtime.ContainsMusicPlayer(name);
		}

		/// <summary>
		/// Prepares music bank for serialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during serialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnBeforeSerialize()
		{
			if (players.Count > 0)
			{
				playlistIndices = new int[players.Count];
				for (int i = 0; i < players.Count; i++)
				{
					Playlist playlist = players[i].Playlist;
					playlistIndices[i] = playlists.IndexOf(playlist);
				}
			}
		}

		/// <summary>
		/// Prepares music bank for runtime use after deserialization.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Unity during deserialization process. Don't call it manually.</para>
		/// </remarks>
		public void OnAfterDeserialize()
		{
			for (int i = 0; i < players.Count; i++)
			{
				MusicPlayer player = players[i];
				Playlist playlist = null;
				if (playlistIndices != null)
				{
					int index = playlistIndices[i];
					playlist = (index != -1) ? playlists[index] : null;
				}
				player.Playlist = playlist;
				player.Bank = this;
			}

			foreach (Playlist playlist in playlists)
				playlist.Bank = this;

			MusicManager.RegisterBank(this);
		}

		/// <summary>
		/// The collection of playlists.
		/// </summary>
		/// <value>A reference to a read-only collection of playlists.</value>
		public ReadOnlyCollection<Playlist> Playlists
		{
			get
			{
				if (playlistsRO == null)
					playlistsRO = playlists.AsReadOnly();

				return playlistsRO;
			}
		}

		/// <summary>
		/// The collection of music players.
		/// </summary>
		/// <value>A reference to a read-only collection of music players.</value>
		public ReadOnlyCollection<MusicPlayer> Players
		{
			get
			{
				if (playersRO == null)
					playersRO = players.AsReadOnly();

				return playersRO;
			}
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show the 'Playlists' group.
		/// </summary>
		/// <value>True, if the 'Playlists' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowPlaylists
		{
			get { return showPlaylists; }
			set { showPlaylists = value; }
		}

		/// <summary>
		/// The flag indicating whether the music bank inspector should show the 'Players' group.
		/// </summary>
		/// <value>True, if the 'Players' group is shown. False otherwise.</value>
		/// <remarks>
		/// <para>This property is used only by the music bank inspector and does nothing during runtime.</para>
		/// </remarks>
		public bool ShowPlayers
		{
			get { return showPlayers; }
			set { showPlayers = value; }
		}

		internal MusicBankRuntime Runtime
		{
			get { return runtime; }
		}

		/// <summary>
		/// Searches for the specified playlist with a matching name.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <returns>
		/// A reference to a playlist, if found. Null reference otherwise.
		/// </returns>
		public Playlist GetPlaylist(string name)
		{
			return runtime.GetPlaylist(name);
		}

		/// <summary>
		/// Adds a new playlist to the music bank.
		/// </summary>
		/// <param name="name">Name of the playlist.</param>
		/// <returns>
		/// A reference to a newly created playlist.
		/// </returns>
		public Playlist AddPlaylist(string name)
		{
			Playlist playlist = new Playlist(this, name);
			playlists.Add(playlist);

			runtime.AddPlaylist(playlist);
			return playlist;
		}

		/// <summary>
		/// Removes existing playlist from the music bank.
		/// </summary>
		/// <param name="playlist">A reference to a playlist.</param>
		/// <remarks>
		/// <para>This method does nothing if the playlist was not found in the music bank.</para>
		/// <para>All existing music players containing removed playlist will set their playlist reference to null.</para>
		/// </remarks>
		public void RemovePlaylist(Playlist playlist)
		{
			int index = playlists.IndexOf(playlist);
			if (index != -1)
				playlists.RemoveAt(index);

			foreach (MusicPlayer player in players)
				if (player.Playlist == playlist)
					player.Playlist = null;

			runtime.RemovePlaylist(playlist);
		}

		/// <summary>
		/// Searches for the specified music player with a matching name.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		public MusicPlayer GetMusicPlayer(string name)
		{
			return runtime.GetMusicPlayer(name);
		}

		/// <summary>
		/// Adds a new music player to the music bank.
		/// </summary>
		/// <param name="name">Name of the music player.</param>
		/// <returns>
		/// A reference to a newly created music player.
		/// </returns>
		public MusicPlayer AddMusicPlayer(string name)
		{
			MusicPlayer player = new MusicPlayer(this, name);

			players.Add(player);
			runtime.AddMusicPlayer(player);
			return player;
		}

		/// <summary>
		/// Removes existing music player from the music bank.
		/// </summary>
		/// <param name="player">A reference to a music player.</param>
		/// <remarks>
		/// <para>This method does nothing if the music player was not found in the music bank.</para>
		/// </remarks>
		public void RemoveMusicPlayer(MusicPlayer player)
		{
			int index = players.IndexOf(player);
			if (index != -1)
				players.RemoveAt(index);

			runtime.RemoveMusicPlayer(player);
		}
	}
}
