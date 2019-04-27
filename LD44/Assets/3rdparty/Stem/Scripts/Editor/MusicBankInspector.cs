using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditorInternal;

namespace Stem
{
	[CustomEditor(typeof(MusicBank))]
	internal class MusicBankInspector : Editor
	{
		private GUILayoutOption defaultWidth = GUILayout.Width(22);

		private MusicBank bank = null;
		private Dictionary<Playlist, ReorderableList> cachedLists = new Dictionary<Playlist, ReorderableList>();

		public void OnEnable()
		{
			bank = target as MusicBank;
		}

		public override void OnInspectorGUI()
		{
			bank.ShowPlaylists = EditorGUILayout.Foldout(bank.ShowPlaylists, "Playlists");
			if (bank.ShowPlaylists)
			{
				List<Playlist> removedPlaylists = new List<Playlist>();
				foreach (Playlist playlist in bank.Playlists)
					OnPlaylistGUI(playlist, removedPlaylists);

				foreach (Playlist playlist in removedPlaylists)
					bank.RemovePlaylist(playlist);

				if (GUILayout.Button("Add Playlist"))
					bank.AddPlaylist("New Playlist");
			}

			bank.ShowPlayers = EditorGUILayout.Foldout(bank.ShowPlayers, "Players");
			if (bank.ShowPlayers)
			{
				List<MusicPlayer> removedPlayers = new List<MusicPlayer>();
				foreach (MusicPlayer player in bank.Players)
					OnMusicPlayerGUI(player, removedPlayers);

				foreach (MusicPlayer player in removedPlayers)
					bank.RemoveMusicPlayer(player);

				if (GUILayout.Button("Add Player"))
					bank.AddMusicPlayer("New Music Player");
			}

			EditorUtility.SetDirty(bank);
		}

		private float GetPlaylistTrackHeight(int index)
		{
			int numLines = 1;

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		private void DrawPlaylistTrackHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Tracks");
		}

		private void DrawPlaylistTrack(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			PlaylistTrack track = (PlaylistTrack)list.list[index];

			rect.height = EditorGUIUtility.singleLineHeight;
			rect.width /= 2;
			track.Clip = (AudioClip)EditorGUI.ObjectField(rect, track.Clip, typeof(AudioClip), false);

			rect.x += rect.width + 4;
			rect.width -= 4;
			track.Volume = EditorGUI.Slider(rect, track.Volume, 0.0f, 1.0f);
		}

		private ReorderableList FetchReorderableList(Playlist playlist)
		{
			ReorderableList list = null;
			if (!cachedLists.TryGetValue(playlist, out list))
			{
				FieldInfo field = typeof(Playlist).GetField("tracks", BindingFlags.Instance | BindingFlags.NonPublic);
				List<PlaylistTrack> tracks = (List<PlaylistTrack>)field.GetValue(playlist);

				list = new ReorderableList(tracks, typeof(PlaylistTrack), true, true, true, true);
				list.elementHeightCallback = GetPlaylistTrackHeight;
				list.drawElementCallback = (_1, _2, _3, _4) => { DrawPlaylistTrack(list, _1, _2, _3, _4); };
				list.drawHeaderCallback = DrawPlaylistTrackHeader;

				cachedLists.Add(playlist, list);
			}

			return list;
		}

		private void OnPlaylistGUI(Playlist playlist, List<Playlist> removedPlaylists)
		{
			EditorGUILayout.BeginHorizontal();

			playlist.Name = EditorGUILayout.TextField(playlist.Name, GUILayout.ExpandWidth(true));
			playlist.Unfolded = GUILayout.Toggle(playlist.Unfolded, "Edit", "button", GUILayout.ExpandWidth(false));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedPlaylists.Add(playlist);

			EditorGUILayout.EndHorizontal();

			if (playlist.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				ReorderableList list = FetchReorderableList(playlist);
				list.DoLayoutList();

				EditorGUILayout.EndVertical();
			}
		}

		private string[] GetPlaylistNames()
		{
			string[] result = new string[bank.Playlists.Count + 1];
			result[0] = "None";
			for (int i = 1; i <= bank.Playlists.Count; i++)
				result[i] = string.Format("[{0}]", bank.Playlists[i - 1].Name);

			return result;
		}

		private int GetPlaylistIndex(Playlist playlist)
		{
			for (int i = 0; i < bank.Playlists.Count; i++)
				if (bank.Playlists[i] == playlist)
					return i + 1;

			return 0;
		}

		private void OnMusicPlayerGUI(MusicPlayer player, List<MusicPlayer> removedPlayers)
		{
			EditorGUILayout.BeginHorizontal();

			player.Name = EditorGUILayout.TextField(player.Name, GUILayout.ExpandWidth(true));
			player.Volume = EditorGUILayout.Slider(player.Volume, 0.0f, 1.0f);

			player.Muted = GUILayout.Toggle(player.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			player.Soloed = GUILayout.Toggle(player.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));
			player.Unfolded = GUILayout.Toggle(player.Unfolded, "Edit", "button", GUILayout.ExpandWidth(false));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedPlayers.Add(player);

			EditorGUILayout.EndHorizontal();

			if (player.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				int index = GetPlaylistIndex(player.Playlist);
				index = EditorGUILayout.Popup("Playlist", index, GetPlaylistNames(), GUILayout.ExpandWidth(true));

				Playlist newPlaylist = (index != 0) ? bank.Playlists[index - 1] : null;
				player.Playlist = newPlaylist;

				player.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Output", player.MixerGroup, typeof(AudioMixerGroup), false);
				player.Fade = EditorGUILayout.FloatField("Fade", Mathf.Max(0.0f, player.Fade));
				player.Shuffle = EditorGUILayout.Toggle("Shuffle", player.Shuffle);
				player.AutoAdvance = EditorGUILayout.Toggle("Auto Advance", player.AutoAdvance);
				player.Loop = EditorGUILayout.Toggle("Loop", player.Loop);

				EditorGUILayout.EndVertical();
			}
		}
	}
}
