using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditorInternal;

namespace Stem
{
	[CustomEditor(typeof(SoundBank))]
	internal class SoundBankInspector : Editor
	{
		private GUILayoutOption defaultWidth = GUILayout.Width(22);
		private const float dropAreaHeight = 50.0f;

		private Rect dropArea;

		private SoundBank bank = null;
		private Dictionary<Sound, ReorderableList> cachedLists = new Dictionary<Sound, ReorderableList>();

		public void OnEnable()
		{
			bank = target as SoundBank;
		}

		public override void OnInspectorGUI()
		{
			OnSoundDropAreaGUI();

			bank.ShowSounds = EditorGUILayout.Foldout(bank.ShowSounds, "Sounds");
			if (bank.ShowSounds)
			{
				List<Sound> removedSounds = new List<Sound>();
				foreach (Sound sound in bank.Sounds)
					OnSoundPanelGUI(sound, removedSounds);

				foreach (Sound sound in removedSounds)
					bank.RemoveSound(sound);

				if (GUILayout.Button("Add Sound"))
					bank.AddSound("New Sound");
			}

			bank.ShowSoundBuses = EditorGUILayout.Foldout(bank.ShowSoundBuses, "Buses");
			if (bank.ShowSoundBuses)
			{
				List<SoundBus> removedBuses = new List<SoundBus>();
				foreach (SoundBus bus in bank.Buses)
					OnSoundBusPanelGUI(bus, removedBuses);

				foreach (SoundBus bus in removedBuses)
					bank.RemoveSoundBus(bus);

				if (GUILayout.Button("Add Bus"))
					bank.AddSoundBus("New Bus");
			}

			OnSoundDrop();
			EditorUtility.SetDirty(bank);
		}

		private void OnSoundDropAreaGUI()
		{
			EditorGUILayout.Space();
			bank.SoundBatchImportMode = (SoundBatchImportMode)EditorGUILayout.EnumPopup("Batch Import Mode", bank.SoundBatchImportMode);

			dropArea = GUILayoutUtility.GetRect(0.0f, dropAreaHeight, GUILayout.ExpandWidth(true));

			GUIStyle style = new GUIStyle("Box");
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Box(dropArea, "Drag AudioClips here to add sounds", style);

			EditorGUILayout.Space();
		}

		private void OnSoundDrop()
		{
			Event evt = Event.current;
			if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform)
				return;

			if (!dropArea.Contains(evt.mousePosition))
				return;

			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

			if (evt.type == EventType.DragPerform)
			{
				DragAndDrop.AcceptDrag();
				switch (bank.SoundBatchImportMode)
				{
					case SoundBatchImportMode.SingleWithVariations:
					{
						List<AudioClip> clips = new List<AudioClip>();
						foreach (Object obj in DragAndDrop.objectReferences)
						{
							AudioClip clip = obj as AudioClip;
							if (clip == null)
								continue;

							clips.Add(clip);
						}

						if (clips.Count > 0)
							bank.AddSound(clips[0].name, clips.ToArray());
					}
					break;
					case SoundBatchImportMode.MultiplePerClip:
					{
						foreach (Object obj in DragAndDrop.objectReferences)
						{
							AudioClip clip = obj as AudioClip;
							if (clip == null)
								continue;

							bank.AddSound(clip.name, clip);
						}
					}
					break;
				}
			}
			evt.Use();
		}

		private float SoundVariationElementHeight(ReorderableList list, int index)
		{
			SoundVariation variation = (SoundVariation)list.list[index];

			int numLines = 8;

			if (variation.RandomizeVolume)
				numLines += 1;

			if (variation.RandomizePitch)
				numLines += 1;

			if (variation.RandomizeDelay)
				numLines += 1;

			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
		}

		private void SoundVariationHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Variations");
		}

		private void SoundVariationElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SoundVariation v = (SoundVariation)list.list[index];
			float offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(rect, v.Name, EditorStyles.boldLabel);

			rect.y += offset;
			v.Clip = (AudioClip)EditorGUI.ObjectField(rect, "Clip", v.Clip, typeof(AudioClip), false);

			float fixedValue = v.FixedVolume;
			bool randomize = v.RandomizeVolume;
			Vector2 randomValueRange = v.RandomVolume;

			offset = RandomRangeField("Volume", 0.0f, 1.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedVolume = fixedValue;
			v.RandomizeVolume = randomize;
			v.RandomVolume = randomValueRange;

			fixedValue = v.FixedPitch;
			randomize = v.RandomizePitch;
			randomValueRange = v.RandomPitch;

			offset = RandomRangeField("Pitch", -3.0f, 3.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedPitch = fixedValue;
			v.RandomizePitch = randomize;
			v.RandomPitch = randomValueRange;

			fixedValue = v.FixedDelay;
			randomize = v.RandomizeDelay;
			randomValueRange = v.RandomDelay;

			offset = RandomRangeField("Delay", 0.0f, 10.0f, rect, ref fixedValue, ref randomize, ref randomValueRange);
			rect.y += offset;

			v.FixedDelay = fixedValue;
			v.RandomizeDelay = randomize;
			v.RandomDelay = randomValueRange;
		}

		private float RandomRangeField(string name, float min, float max, Rect rect, ref float fixedValue, ref bool randomize, ref Vector2 randomRange)
		{
			float offset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			int numLines = (randomize) ? 3 : 2;

			rect.y += offset;
			randomize = EditorGUI.Toggle(rect, string.Format("Randomize {0}", name), randomize);

			if (randomize)
			{
				rect.y += offset;
				EditorGUI.MinMaxSlider(rect, string.Format("{0} Range", name), ref randomRange.x, ref randomRange.y, min, max);

				GUI.enabled = false;
				string range = string.Format("{0:N3} - {1:N3}", randomRange.x, randomRange.y);

				rect.y += offset;
				EditorGUI.LabelField(rect, null, range);

				GUI.enabled = true;
			}
			else
			{
				rect.y += offset;
				fixedValue = EditorGUI.Slider(rect, name, fixedValue, min, max);
			}

			return numLines * offset;
		}

		private float SliderWithLabels(string name, float value, string leftLabel, string rightLabel, float min, float max)
		{
			Rect position = EditorGUILayout.GetControlRect(false, 1.5f * EditorGUIUtility.singleLineHeight);
			position.height = EditorGUIUtility.singleLineHeight;

			float result = EditorGUI.Slider(position, name, value, min, max);

			position.height = EditorGUIUtility.singleLineHeight;
			position.y += position.height * 0.75f;
			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth + 54;

			GUIStyle style = GUI.skin.label;
			bool oldEnabled = GUI.enabled;
			GUI.enabled = false;
			style.fontSize = 9;
			style.alignment = TextAnchor.UpperLeft;
			EditorGUI.LabelField (position, leftLabel, style);
			style.alignment = TextAnchor.UpperRight;
			EditorGUI.LabelField (position, rightLabel, style);
			GUI.enabled = oldEnabled;

			return result;
		}

		private ReorderableList FetchReorderableList(Sound sound)
		{
			ReorderableList list = null;
			if (!cachedLists.TryGetValue(sound, out list))
			{
				FieldInfo field = typeof(Sound).GetField("variations", BindingFlags.Instance | BindingFlags.NonPublic);
				List<SoundVariation> variations = (List<SoundVariation>)field.GetValue(sound);

				list = new ReorderableList(variations, typeof(SoundVariation), true, true, true, true);
				list.elementHeightCallback = (_1) => { return SoundVariationElementHeight(list, _1); };
				list.drawElementCallback = (_1, _2, _3, _4) => { SoundVariationElement(list, _1, _2, _3, _4); };
				list.drawHeaderCallback = SoundVariationHeader;

				cachedLists.Add(sound, list);
			}

			return list;
		}

		private string[] GetBusNames()
		{
			string[] result = new string[bank.Buses.Count];
			for (int i = 0; i < bank.Buses.Count; i++)
				result[i] = bank.Buses[i].Name;

			return result;
		}

		private int GetBusIndex(SoundBus bus)
		{
			for (int i = 0; i < bank.Buses.Count; i++)
				if (bank.Buses[i] == bus)
					return i;

			return -1;
		}

		private void OnSoundPanelGUI(Sound sound, List<Sound> removedSounds)
		{
			EditorGUILayout.BeginHorizontal();

			sound.Name = EditorGUILayout.TextField(sound.Name, GUILayout.ExpandWidth(true));

			int index = GetBusIndex(sound.Bus);
			index = EditorGUILayout.Popup(index, GetBusNames(), GUILayout.ExpandWidth(true));

			SoundBus newBus = (index != -1) ? bank.Buses[index] : bank.DefaultBus;
			sound.Bus = newBus;

			sound.Muted = GUILayout.Toggle(sound.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			sound.Soloed = GUILayout.Toggle(sound.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));
			sound.Unfolded = GUILayout.Toggle(sound.Unfolded, "Edit", "button", GUILayout.ExpandWidth(false));

			if (GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedSounds.Add(sound);

			EditorGUILayout.EndHorizontal();

			if (sound.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				sound.PanStereo = SliderWithLabels("Stereo Pan", sound.PanStereo, "Left", "Right", -1.0f, 1.0f);
				sound.SpatialBlend = SliderWithLabels("Spatial Blend", sound.SpatialBlend, "2D", "3D", 0.0f, 1.0f);

				sound.DopplerLevel = EditorGUILayout.Slider("Doppler Level", sound.DopplerLevel, 0.0f, 5.0f);
				sound.Spread = EditorGUILayout.Slider("Spread", sound.Spread, 0.0f, 360.0f);
				sound.AttenuationMode = (AttenuationMode)EditorGUILayout.EnumPopup("Volume Rolloff", (System.Enum)sound.AttenuationMode);
				sound.MinDistance = EditorGUILayout.FloatField("Min Distance", Mathf.Max(0.0f, sound.MinDistance));
				sound.MaxDistance = EditorGUILayout.FloatField("Max Distance", Mathf.Max(0.0f, sound.MaxDistance));

				sound.VariationRetriggerMode = (RetriggerMode)EditorGUILayout.EnumPopup("Retrigger Mode", (System.Enum)sound.VariationRetriggerMode);

				ReorderableList list = FetchReorderableList(sound);
				list.DoLayoutList();

				EditorGUILayout.EndVertical();
			}
		}

		private void OnSoundBusPanelGUI(SoundBus bus, List<SoundBus> removedBuses)
		{
			EditorGUILayout.BeginHorizontal();

			bus.Name = EditorGUILayout.TextField(bus.Name, GUILayout.ExpandWidth(true));
			bus.Volume = EditorGUILayout.Slider(bus.Volume, 0.0f, 1.0f);

			bus.Muted = GUILayout.Toggle(bus.Muted, "M", "button", defaultWidth, GUILayout.ExpandWidth(false));
			bus.Soloed = GUILayout.Toggle(bus.Soloed, "S", "button", defaultWidth, GUILayout.ExpandWidth(false));
			bus.Unfolded = GUILayout.Toggle(bus.Unfolded, "Edit", "button", GUILayout.ExpandWidth(false));

			bool canRemove = (bus != bank.DefaultBus);
			if (canRemove && GUILayout.Button("-", defaultWidth, GUILayout.ExpandWidth(false)))
				removedBuses.Add(bus);

			EditorGUILayout.EndHorizontal();

			if (bus.Unfolded)
			{
				EditorGUILayout.BeginVertical("groupbox");

				bus.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Output", bus.MixerGroup, typeof(AudioMixerGroup), false);
				bus.Polyphony = (byte)EditorGUILayout.IntSlider("Polyphony", bus.Polyphony, 1, 32);
				bus.AllowVoiceStealing = EditorGUILayout.Toggle("Allow Voice Stealing", bus.AllowVoiceStealing);

				EditorGUILayout.EndVertical();
			}
		}
	}
}
