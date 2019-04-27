using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Stem
{
	internal interface IBank
	{
		bool ContainsAsset(string name);
	}

	internal interface IManagerRuntime<T> where T : ScriptableObject, IBank
	{
		void Init(T bank);
	}

	internal class BankManager<BankType, RuntimeType>
		where RuntimeType : MonoBehaviour, IManagerRuntime<BankType>
		where BankType : ScriptableObject, IBank
	{

		private List<BankType> banks = new List<BankType>();
		private ReadOnlyCollection<BankType> banksRO = null;

		private List<RuntimeType> bankRuntimes = new List<RuntimeType>();
		private ReadOnlyCollection<RuntimeType> bankRuntimesRO = null;

		private List<GameObject> bankGameObjects = new List<GameObject>();
		private int primaryIndex = -1;

		internal ReadOnlyCollection<BankType> Banks
		{
			get
			{
				if (banksRO == null)
					banksRO = banks.AsReadOnly();

				return banksRO;
			}
		}

		internal ReadOnlyCollection<RuntimeType> Runtimes
		{
			get
			{
				if (bankRuntimesRO == null)
					bankRuntimesRO = bankRuntimes.AsReadOnly();

				return bankRuntimesRO;
			}
		}

		internal BankType PrimaryBank
		{
			get
			{
				if (primaryIndex == -1)
					return null;

				return banks[primaryIndex];
			}
			set
			{
				primaryIndex = banks.IndexOf(value);
			}
		}

		internal bool RegisterBank(BankType bank)
		{
			if (banks.Contains(bank))
				return false;

			banks.Add(bank);
			bankRuntimes.Add(null);
			bankGameObjects.Add(null);

			if (primaryIndex == -1)
				primaryIndex = 0;

			return true;
		}

		internal RuntimeType FetchRuntime(string name)
		{
			// Check primary runtime first
			if (primaryIndex != -1)
			{
				BankType bank = banks[primaryIndex];

				if (bank.ContainsAsset(name))
					return FetchGameObject(primaryIndex);
			}

			// Check other runtimes
			for (int i = 0; i < banks.Count; i++)
			{
				// Skip primary runtime
				if (i == primaryIndex)
					continue;

				BankType bank = banks[i];

				if (bank.ContainsAsset(name))
					return FetchGameObject(i);
			}

			return null;
		}

		private RuntimeType FetchGameObject(int index)
		{
			BankType bank = banks[index];

			RuntimeType runtime = bankRuntimes[index];
			if (runtime != null)
				return runtime;

			GameObject gameObject = new GameObject();
			gameObject.name = bank.name;
			GameObject.DontDestroyOnLoad(gameObject);

			runtime = gameObject.AddComponent<RuntimeType>();
			runtime.Init(bank);

			bankRuntimes[index] = runtime;
			bankGameObjects[index] = gameObject;

			return runtime;
		}
	}
}
