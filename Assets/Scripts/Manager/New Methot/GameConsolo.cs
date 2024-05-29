using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
	public class GameConsolo : MonoBehaviour
	{
		public LevelManager LevelManager;
		public GameObject prefab; // Prefab olarak kullanacağımız GameObject
		public Transform parentTransform; // Prefabların ekleneceği ana GameObject'in transformu

		
		private void Start()
		{
			for (int i = 0; i < LevelManager.Levels.Count; i++)
			{
				CreatePrefabInstance(i);
			}
		}
		
		private void CreatePrefabInstance(int index)
		{
			GameObject newPrefab = Instantiate(prefab, parentTransform);
			newPrefab.name = "Level_" + index;
        
			LevelCard currentLevel = LevelManager.Levels[index];
			
			// Burada prefab içindeki bileşenleri güncelleyebilirsiniz
			// Örneğin, prefab içindeki bir text bileşenine level adını yazdırabilirsiniz
			// newPrefab.GetComponentInChildren<Text>().text = levelManager.levels[index].levelAd;

			#region Buttons&Text

				TMP_Text levelButtonText = newPrefab.GetComponentInChildren<TMP_Text>();
				if (levelButtonText != null)
				{
					levelButtonText.text = LevelManager.Levels[index].themaName;
				}

				Button[] buttons = newPrefab.GetComponentsInChildren<Button>();
				foreach (Button button in buttons)
				{
					if (button.name == "B_OpenButton")
					{
						button.gameObject.SetActive(currentLevel.isbought);
	                
						TMP_Text priceText = button.GetComponentInChildren<TMP_Text>();
						if (priceText != null)
						{
							priceText.text = currentLevel.buyPrice.ToString(); 
						}
						break;
					}

					if (button.name == "B_LevelButton")
					{
						//Current theme değiştir 
					}
				}

			#endregion

			#region Slider

				Slider slider = newPrefab.GetComponentInChildren<Slider>();
				if (slider != null)
				{
					slider.maxValue = currentLevel.MaxPlayingSliderCount; // Örnek olarak maksimum değer 100
					slider.value = currentLevel.playingCount;
				}

			#endregion
			
		}

		public void DeletePrefab()
		{
			// Parent transformun altındaki tüm çocukları sil
			foreach (Transform child in parentTransform)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < LevelManager.Levels.Count; i++)
			{
				CreatePrefabInstance(i);
			}
		}
		
		public void LevelBuy(int levelIndex)
		{
			print("La La La");
			LevelCard level = LevelManager.Levels[levelIndex];
			print(levelIndex);

			if (levelIndex < 0 || levelIndex >= LevelManager.Levels.Count)
			{
				Debug.LogError("Geçersiz bölüm indexi!");
				return;
			}

			if (level.isbought)
			{
				Debug.Log("Bu bölüm zaten satın alındı!");
				return;
			}

			// Satın alma işlemleri
			// Örneğin, kullanıcı bakiyesi kontrolü ve düşümü yapılabilir

			if (GameDataBase.instance.PlayerCoins >= level.buyPrice)
			{
				level.isbought = true;
				Debug.Log(level.themaName + " bölümü satın alındı!");
			
			}
        

		}
	}

}
