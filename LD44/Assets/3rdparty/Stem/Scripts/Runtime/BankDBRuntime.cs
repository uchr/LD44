using UnityEngine;

namespace Stem
{
	public class BankDBRuntime : MonoBehaviour
	{
		public SoundBank[] soundBanks = null;
		public MusicBank[] musicBanks = null;

		private void Start()
		{
			if (soundBanks != null)
				for (int i = 0; i < soundBanks.Length; i++)
					SoundManager.RegisterBank(soundBanks[i]);

			if (musicBanks != null)
				for (int i = 0; i < musicBanks.Length; i++)
					MusicManager.RegisterBank(musicBanks[i]);
		}
	}
}
