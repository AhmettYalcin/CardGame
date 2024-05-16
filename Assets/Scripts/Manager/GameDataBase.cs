using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
	public class GameDataBase : MonoBehaviour
	{
		#region instance

			public static GameDataBase instance { get; private set; }

			private void Awake()
			{
				if (instance != null && instance != this)
				{
					Destroy(this);
				}
				else
				{
					instance = this;
				}
			}

		#endregion

		public int PlayerCoins;
		public int PlayerPuans;
		public int PlayerStamina;
	}
}
