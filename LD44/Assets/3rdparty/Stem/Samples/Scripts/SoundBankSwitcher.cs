using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBankSwitcher : MonoBehaviour
{
	public Stem.SoundBank[] skins = null;
	public Dropdown dropdown = null;

	private void Start()
	{
		if (skins.Length > 0)
			Stem.SoundManager.PrimaryBank = skins[0];

		if (dropdown != null)
		{
			List<string> names = new List<string>();
			for (int i = 0; i < skins.Length; i++)
				names.Add(skins[i].name);

			dropdown.ClearOptions();
			dropdown.AddOptions(names);
		}
	}

	public void SetBank(int index)
	{
		if (dropdown == null)
			return;

		if (skins == null)
			return;

		Stem.SoundManager.PrimaryBank = skins[index];
	}
}
