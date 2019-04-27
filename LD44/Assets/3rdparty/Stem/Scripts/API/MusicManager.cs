using System.Collections.ObjectModel;

namespace Stem
{
	/// <summary>
	/// The main class for music playback and bank management.
	/// </summary>
	public static class MusicManager
	{
		private static BankManager<MusicBank, MusicManagerRuntime> bankManager = new BankManager<MusicBank, MusicManagerRuntime>();
		private static bool shutdown = false;

		/// <summary>
		/// The collection of all registered music banks.
		/// </summary>
		/// <value>A reference to a read-only collection of music banks.</value>
		public static ReadOnlyCollection<MusicBank> Banks
		{
			get { return bankManager.Banks; }
		}

		/// <summary>
		/// The primary music bank that will be searched first in case of name collisions.
		/// </summary>
		/// <value>A reference to a primary music bank.</value>
		public static MusicBank PrimaryBank
		{
			get { return bankManager.PrimaryBank; }
			set { bankManager.PrimaryBank = value; }
		}

		/// <summary>
		/// Registers new music bank.
		/// </summary>
		/// <param name="bank">A reference to a music bank to register.</param>
		/// <returns>
		/// True, if music bank was succesfully registered. False otherwise.
		/// </returns>
		public static bool RegisterBank(MusicBank bank)
		{
			if (shutdown)
				return false;

			return bankManager.RegisterBank(bank);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="playlist">Name of the playlist.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>If music player was playing another track it'll automatically crossfade to the first track of the new playlist.</para>
		/// </remarks>
		public static void SetPlaylist(string player, string playlist)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.SetPlaylist(player, playlist);
		}

		/// <summary>
		/// Sets a playlist to a music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="playlist">Name of the playlist.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>If music player was playing another track it'll automatically crossfade to first track of the new playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void SetPlaylist(string player, string playlist, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.SetPlaylist(player, playlist, fade);
		}

		/// <summary>
		/// Searches for the specified music player with a matching name.
		/// </summary>
		/// <returns>
		/// A reference to a music player, if found. Null reference otherwise.
		/// </returns>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// </remarks>
		public static MusicPlayer GetMusicPlayer(string name)
		{
			if (shutdown)
				return null;

			// Check primary bank first
			MusicBank primaryBank = bankManager.PrimaryBank;
			if (primaryBank != null && primaryBank.ContainsAsset(name))
				return primaryBank.GetMusicPlayer(name);

			// Check other banks
			for (int i = 0; i < bankManager.Banks.Count; i++)
			{
				MusicBank bank = bankManager.Banks[i];

				// Skip primary bank
				if (bank == primaryBank)
					continue;

				if (bank.ContainsAsset(name))
					return bank.GetMusicPlayer(name);
			}

			return null;
		}

		/// <summary>
		/// Advances music player to next track.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Next(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Next(player);
		}

		/// <summary>
		/// Advances music player to next track.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Next(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Next(player, fade);
		}

		/// <summary>
		/// Advances music player to previous track.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Prev(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Prev(player);
		}

		/// <summary>
		/// Advances music player to previous track.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Prev(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Prev(player, fade);
		}

		/// <summary>
		/// Advances music player to a track with a matching name.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="track">Name of the track.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Seek(string player, string track)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Seek(player, track);
		}

		/// <summary>
		/// Advances music player to a track with a matching name.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="track">Name of the track.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>Target track must be one of current playlist tracks.</para>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Seek(string player, string track, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Seek(player, track, fade);
		}

		/// <summary>
		/// Plays music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Play(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Play(player);
		}

		/// <summary>
		/// Plays music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Play(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Play(player, fade);
		}

		/// <summary>
		/// Stops music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Stop(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Stop(player);
		}

		/// <summary>
		/// Stops music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Stop(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Stop(player, fade);
		}

		/// <summary>
		/// Pauses music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void Pause(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Pause(player);
		}

		/// <summary>
		/// Pauses music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void Pause(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.Pause(player, fade);
		}

		/// <summary>
		/// Resumes music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// </remarks>
		public static void UnPause(string player)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.UnPause(player);
		}

		/// <summary>
		/// Resumes music player.
		/// </summary>
		/// <param name="player">Name of the music player.</param>
		/// <param name="fade">Crossfade duration in seconds.</param>
		/// <remarks>
		/// <para>If multiple banks have music players/playlists with a matching name, primary music bank will be checked first.
		/// Within a bank, the first occurrence of found music player/playlist will be used.</para>
		/// <para>This method does nothing if no playlist was assigned to the music player. Use <see cref="SetPlaylist(string, string)"/> or <see cref="SetPlaylist(string, string, float)"/> to assign a playlist.</para>
		/// <para>Crossfade parameter value will override <see cref="Stem.MusicPlayer"/>.<see cref="Stem.MusicPlayer.Fade"/> value.</para>
		/// </remarks>
		public static void UnPause(string player, float fade)
		{
			if (shutdown)
				return;

			MusicManagerRuntime runtime = bankManager.FetchRuntime(player);
			if (runtime != null)
				runtime.UnPause(player, fade);
		}

		internal static void Shutdown()
		{
			shutdown = true;
		}
	}
}
