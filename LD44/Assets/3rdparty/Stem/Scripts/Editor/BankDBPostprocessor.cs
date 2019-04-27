using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Stem
{
	internal class BankDBPostprocessor
	{
		[PostProcessSceneAttribute(0)]
		internal static void OnPostprocessScene() {
			if (EditorApplication.isPlayingOrWillChangePlaymode)
				AssetDatabase.SaveAssets();

			SoundBank[] soundBanks = LoadBankAssets<SoundBank>();
			MusicBank[] musicBanks = LoadBankAssets<MusicBank>();

			GameObject db = GameObject.Find("BankDBRuntime");

			if (db == null)
			{
				db = new GameObject();
				db.name = "BankDBRuntime";
			}

			BankDBRuntime dbRuntime = db.AddComponent<BankDBRuntime>();
			dbRuntime.soundBanks = soundBanks;
			dbRuntime.musicBanks = musicBanks;

			EditorApplication.playModeStateChanged += UnloadBankAssets;
		}

		private static void UnloadBankAssets(PlayModeStateChange state)
		{
			if (state != PlayModeStateChange.ExitingPlayMode)
				return;

			for (int i = 0; i < SoundManager.Banks.Count; i++)
				Resources.UnloadAsset(SoundManager.Banks[i]);

			for (int i = 0; i < MusicManager.Banks.Count; i++)
				Resources.UnloadAsset(MusicManager.Banks[i]);
		}

		private static BankType[] LoadBankAssets<BankType>() where BankType : ScriptableObject
		{
			string[] guids = AssetDatabase.FindAssets("t:"+ typeof(BankType).Name);
			if (guids.Length == 0)
				return null;

			BankType[] banks = new BankType[guids.Length];
			for(int i =0; i < guids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				banks[i] = AssetDatabase.LoadAssetAtPath<BankType>(path);
			}

			return banks;
		}
	}
}
