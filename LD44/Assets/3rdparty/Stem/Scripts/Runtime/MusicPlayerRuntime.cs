using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal class Fader
	{
		internal delegate void ActionHandler();
		internal event ActionHandler OnFadeEnd;
		internal event ActionHandler OnFadeBegin;

		private float duration = 0.0f;
		private float time = 0.0f;
		private bool finished = true;
		private float volume = 0.0f;
		private float oldVolume = 0.0f;
		private float targetVolume = 0.0f;

		internal float Volume
		{
			get { return volume; }
			set
			{
				targetVolume = value;
				volume = value;
				oldVolume = value;
				finished = true;
			}
		}

		internal void FadeIn(float d)
		{
			BeginFade(d, 1.0f);
		}

		internal void FadeOut(float d)
		{
			BeginFade(d, 0.0f);
		}

		internal void Update(float dt)
		{
			if (finished)
				return;

			time -= dt;

			float k = Mathf.Clamp01(time / duration);
			volume = Mathf.Lerp(targetVolume, oldVolume, k);

			if (time < 0.0f)
			{
				volume = targetVolume;
				finished = true;
				if (OnFadeEnd != null)
					OnFadeEnd();
			}
		}

		private void BeginFade(float d, float v)
		{
			duration = d;
			time = d;

			oldVolume = volume;
			targetVolume = v;

			finished = false;
			if (OnFadeBegin != null)
				OnFadeBegin();

			if (duration <= 0.0f)
			{
				volume = targetVolume;
				finished = true;
				if (OnFadeEnd != null)
					OnFadeEnd();
			}
		}
	}

	internal class PlaylistAdvancer
	{
		private Playlist playlist;
		private bool shuffle;
		private bool loop;
		private int shuffleIndex;
		private int[] shuffleOrder;

		internal PlaylistAdvancer(Playlist playlist_, bool shuffle_, bool loop_)
		{
			SetPlaylist(playlist_, shuffle_, loop_);
		}

		internal void SetPlaylist(Playlist playlist_, bool shuffle_, bool loop_)
		{
			playlist = playlist_;
			shuffle = shuffle_;
			loop = loop_;

			int numTracks = playlist.Tracks.Count;

			shuffleIndex = 0;
			shuffleOrder = new int[numTracks];

			for (int i = 0; i < numTracks; i++)
				shuffleOrder[i] = i;

			if (shuffle)
				Shuffle();
		}

		internal PlaylistTrack GetTrack()
		{
			if (shuffleIndex < 0 || shuffleIndex >= shuffleOrder.Length)
				return null;

			int index = shuffleOrder[shuffleIndex];
			return playlist.Tracks[index];
		}

		internal PlaylistTrack Next()
		{
			shuffleIndex++;
			Wrap();
			return GetTrack();
		}

		internal PlaylistTrack Prev()
		{
			shuffleIndex--;
			Wrap();
			return GetTrack();
		}

		internal PlaylistTrack Seek(string name)
		{
			int numTracks = playlist.Tracks.Count;
			for (int i = 0; i < numTracks; i++)
			{
				int index = shuffleOrder[i];
				PlaylistTrack track = playlist.Tracks[index];
				if (track.Name == name)
				{
					shuffleIndex = i;
					return GetTrack();
				}
			}
			return null;
		}

		private void Wrap()
		{
			if (!loop)
				return;

			int numTracks = playlist.Tracks.Count;
			if (shuffle && (shuffleIndex >= numTracks || shuffleIndex < 0))
				Shuffle();

			shuffleIndex %= numTracks;
			while (shuffleIndex < 0)
				shuffleIndex += numTracks;
		}

		private void Shuffle()
		{
			int numTracks = playlist.Tracks.Count;
			for(int i = numTracks - 1; i >= 0; i--)
			{
				int index = Random.Range(0, numTracks);
				int temp = shuffleOrder[i];
				shuffleOrder[i] = shuffleOrder[index];
				shuffleOrder[index] = temp;
			}
		}
	}

	internal class TrackMixer
	{
		internal Fader fader = new Fader();
		internal AudioSource source = null;
		internal PlaylistTrack track = null;
	}

	internal enum PlaybackAction
	{
		Play,
		Stop,
		Pause,
		UnPause,
	}

	internal class MusicPlayerRuntime
	{
		private GameObject root = null;
		private List<TrackMixer> usedMixers = new List<TrackMixer>();
		private List<TrackMixer> freeMixers = new List<TrackMixer>();

		private Fader playbackFader = new Fader();
		private PlaybackAction playbackAction = PlaybackAction.Stop;
		private bool isPlaying = false;

		private MusicPlayer player = null;
		private PlaylistAdvancer advancer = null;

		internal MusicPlayerRuntime(Transform transform_, MusicPlayer player_)
		{
			player = player_;

			root = new GameObject();
			root.transform.parent = transform_;
			root.name = player.Name;

			playbackFader.OnFadeBegin += OnPlaybackFadeBegin;
			playbackFader.OnFadeEnd += OnPlaybackFadeEnd;

			playbackFader.Volume = 1.0f;
		}

		internal void Play()
		{
			Play(player.Fade);
		}

		internal void Play(float fade)
		{
			if (isPlaying || advancer == null)
				return;

			playbackAction = PlaybackAction.Play;
			playbackFader.FadeIn(fade);
		}

		internal void Stop()
		{
			Stop(player.Fade);
		}

		internal void Stop(float fade)
		{
			if (!isPlaying || advancer == null)
				return;

			playbackAction = PlaybackAction.Stop;
			playbackFader.FadeOut(fade);
		}

		internal void Pause()
		{
			Pause(player.Fade);
		}

		internal void Pause(float fade)
		{
			if (!isPlaying || advancer == null)
				return;

			playbackAction = PlaybackAction.Pause;
			playbackFader.FadeOut(fade);
		}

		internal void UnPause()
		{
			UnPause(player.Fade);
		}

		internal void UnPause(float fade)
		{
			if (isPlaying || advancer == null)
				return;

			playbackAction = PlaybackAction.UnPause;
			playbackFader.FadeIn(fade);
		}

		internal void Next()
		{
			Next(player.Fade);
		}

		internal void Next(float fade)
		{
			if (advancer == null)
				return;

			Crossfade(advancer.Next(), fade);
		}

		internal void Prev()
		{
			Prev(player.Fade);
		}

		internal void Prev(float fade)
		{
			if (advancer == null)
				return;

			Crossfade(advancer.Prev(), fade);
		}

		internal void Seek(string track)
		{
			Seek(track, player.Fade);
		}

		internal void Seek(string track, float fade)
		{
			if (advancer == null)
				return;

			Crossfade(advancer.Seek(track), fade);
		}

		internal void SetPlaylist(Playlist playlist)
		{
			SetPlaylist(playlist, player.Fade);
		}

		internal void SetPlaylist(Playlist playlist, float fade)
		{
			if (playlist == null)
			{
				Stop(fade);
				advancer = null;
				return;
			}

			advancer = new PlaylistAdvancer(playlist, player.Shuffle, player.Loop);
			Crossfade(advancer.GetTrack(), fade);
		}

		internal void Update(float dt)
		{
			if (!isPlaying)
				return;

			playbackFader.Update(dt);

			bool audible = player.Audible;

			for (int i = usedMixers.Count - 1; i >= 0; i--)
			{
				TrackMixer mixer = usedMixers[i];
				AudioSource source = mixer.source;
				PlaylistTrack track = mixer.track;
				Fader fader = mixer.fader;

				fader.Update(dt);
				if (fader.Volume == 0.0f)
					ReleaseUsedMixer(i);

				source.volume = playbackFader.Volume * fader.Volume * player.Volume * track.Volume;
				source.mute = !audible;
			}

			if (advancer == null)
				return;

			TrackMixer currentMixer = GetUsedMixer(advancer.GetTrack());
			if (currentMixer == null)
				return;

			bool needToSwitch = (currentMixer.source.time + player.Fade >= currentMixer.source.clip.length);
			if (player.AutoAdvance && needToSwitch)
				Crossfade(advancer.Next(), player.Fade);
		}

		private void Crossfade(PlaylistTrack track, float fade)
		{
			for (int i = 0; i < usedMixers.Count; i++)
			{
				TrackMixer mixer = usedMixers[i];
				mixer.fader.FadeOut(fade);
			}

			TrackMixer targetMixer = FetchMixer(track);
			if (targetMixer != null)
				targetMixer.fader.FadeIn(fade);
		}

		private TrackMixer GetUsedMixer(PlaylistTrack track)
		{
			if (track == null)
				return null;

			for (int i = 0; i < usedMixers.Count; i++)
			{
				TrackMixer mixer = usedMixers[i];
				if (mixer.track == track)
					return mixer;
			}

			return null;
		}

		private TrackMixer FetchMixer(PlaylistTrack track)
		{
			if (track == null)
				return null;

			TrackMixer mixer = GetUsedMixer(track);
			if (mixer != null)
				return mixer;

			if (freeMixers.Count > 0)
			{
				mixer = freeMixers[freeMixers.Count - 1];
				freeMixers.RemoveAt(freeMixers.Count - 1);
			}

			if (mixer == null)
			{
				mixer = new TrackMixer();
				mixer.source = root.AddComponent<AudioSource>();
			}

			mixer.track = track;
			mixer.source.clip = track.Clip;
			mixer.source.timeSamples = 0;
			mixer.source.outputAudioMixerGroup = player.MixerGroup;
			mixer.source.loop = player.Loop;

			if (isPlaying)
				mixer.source.Play();

			usedMixers.Add(mixer);
			return mixer;
		}

		private void ReleaseUsedMixer(int index)
		{
			TrackMixer mixer = usedMixers[index];
			usedMixers.RemoveAt(index);
			freeMixers.Add(mixer);

			mixer.source.clip = null;
			mixer.track = null;
		}

		private void OnPlaybackFadeBegin()
		{
			switch (playbackAction)
			{
				case PlaybackAction.Play:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Play();
					}
					isPlaying = true;
				break;
				case PlaybackAction.UnPause:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.UnPause();
					}
					isPlaying = true;
				break;
			}
		}

		private void OnPlaybackFadeEnd()
		{
			switch (playbackAction)
			{
				case PlaybackAction.Stop:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Stop();
					}
					isPlaying = false;
				break;
				case PlaybackAction.Pause:
					for (int i = 0; i < usedMixers.Count; i++)
					{
						TrackMixer mixer = usedMixers[i];
						mixer.source.Pause();
					}
					isPlaying = false;
				break;
			}
		}
	}
}
