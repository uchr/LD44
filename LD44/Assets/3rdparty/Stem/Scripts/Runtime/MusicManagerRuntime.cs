using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class MusicManagerRuntime : MonoBehaviour, IManagerRuntime<MusicBank>
	{
		private MusicBank bank = null;
		private Dictionary<string, MusicPlayerRuntime> playerRuntimes = new Dictionary<string, MusicPlayerRuntime>();

		public void Init(MusicBank bank_)
		{
			playerRuntimes.Clear();
			bank = bank_;

			foreach (MusicPlayer player in bank.Players)
			{
				MusicPlayerRuntime runtime = new MusicPlayerRuntime(transform, player);
				if (player.Playlist != null)
					runtime.SetPlaylist(player.Playlist);

				playerRuntimes.Add(player.Name, runtime);
			}
		}

		internal void SetPlaylist(string player, string playlistName)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			Playlist playlist = bank.GetPlaylist(playlistName);
			runtime.SetPlaylist(playlist);
		}

		internal void SetPlaylist(string player, string playlistName, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			Playlist playlist = bank.GetPlaylist(playlistName);
			runtime.SetPlaylist(playlist, fade);
		}

		internal void Next(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Next();
		}

		internal void Next(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Next(fade);
		}

		internal void Prev(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Prev();
		}

		internal void Prev(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Prev(fade);
		}

		internal void Seek(string player, string track)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Seek(track);
		}

		internal void Seek(string player, string track, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Seek(track, fade);
		}

		internal void Play(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Play();
		}

		internal void Play(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Play(fade);
		}

		internal void Stop(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Stop();
		}

		internal void Stop(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Stop(fade);
		}

		internal void Pause(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Pause();
		}

		internal void Pause(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.Pause(fade);
		}

		internal void UnPause(string player)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.UnPause();
		}

		internal void UnPause(string player, float fade)
		{
			MusicPlayerRuntime runtime = playerRuntimes[player];
			runtime.UnPause(fade);
		}

		private void Update()
		{
			float dt = Time.unscaledDeltaTime;

			Dictionary<string, MusicPlayerRuntime>.Enumerator enumerator = playerRuntimes.GetEnumerator();
			while(enumerator.MoveNext())
			{
				MusicPlayerRuntime runtime = enumerator.Current.Value;
				runtime.Update(dt);
			}
		}

		private void OnDestroy()
		{
			MusicManager.Shutdown();
		}
	}
}
