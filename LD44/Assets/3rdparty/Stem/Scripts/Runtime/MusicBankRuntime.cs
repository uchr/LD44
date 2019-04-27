using System;
using System.Collections.Generic;

namespace Stem
{
	internal class MusicBankRuntime
	{
		[NonSerialized]
		private Dictionary<string, List<Playlist> > playlistByName = new Dictionary<string, List<Playlist> >();

		[NonSerialized]
		private Dictionary<string, List<MusicPlayer> > playerByName = new Dictionary<string, List<MusicPlayer> >();

		internal int SoloedMusicPlayers { get; set; }

		internal Playlist GetPlaylist(string name)
		{
			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(name, out playlistList))
				return null;

			if (playlistList.Count == 0)
				return null;

			return playlistList[0];
		}

		internal MusicPlayer GetMusicPlayer(string name)
		{
			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(name, out playerList))
				return null;

			if (playerList.Count == 0)
				return null;

			return playerList[0];
		}

		internal bool ContainsMusicPlayer(string name)
		{
			return playerByName.ContainsKey(name);
		}

		internal void AddPlaylist(Playlist playlist)
		{
			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(playlist.Name, out playlistList))
			{
				playlistList = new List<Playlist>();
				playlistByName.Add(playlist.Name, playlistList);
			}

			playlistList.Add(playlist);
		}

		internal void RemovePlaylist(Playlist playlist)
		{
			List<Playlist> playlistList = null;
			if (!playlistByName.TryGetValue(playlist.Name, out playlistList))
				return;

			int index = playlistList.IndexOf(playlist);
			if (index != -1)
				playlistList.RemoveAt(index);
		}

		internal void AddMusicPlayer(MusicPlayer player)
		{
			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(player.Name, out playerList))
			{
				playerList = new List<MusicPlayer>();
				playerByName.Add(player.Name, playerList);
			}

			playerList.Add(player);

			if (player.Soloed)
				SoloedMusicPlayers++;
		}

		internal void RemoveMusicPlayer(MusicPlayer player)
		{
			List<MusicPlayer> playerList = null;
			if (!playerByName.TryGetValue(player.Name, out playerList))
				return;

			int index = playerList.IndexOf(player);
			if (index != -1)
				playerList.RemoveAt(index);

			if (player.Soloed)
				SoloedMusicPlayers--;
		}
	}
}
