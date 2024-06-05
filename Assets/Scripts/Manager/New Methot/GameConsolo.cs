using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Manager
{
	[System.Serializable]
	public struct SelectingLevel
	{
		public Button selectLevelButton;
		public int IntXInt;
		
	}
	
	public class GameConsolo : MonoBehaviour
	{
		#region instance

			public static GameConsolo instance { get; private set; }

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

		#region Datas

			[Header("LevelButon Prefab")]
			[SerializeField] private LevelManager LevelManager;
			[SerializeField] private GameObject prefab; // Prefab olarak kullanacağımız GameObject
			[SerializeField] private Transform parentTransform; // Prefabların ekleneceği ana GameObject'in transformu

			[Header("Select Level")] 
			[SerializeField] private List<SelectingLevel> selectLevelButtons;
			
			[Header("Canvas Text")] 
			[SerializeField] private TMP_Text Text_PlayerCoins;
			[SerializeField] private TMP_Text Text_PlayerPuans;
			[SerializeField] private TMP_Text Text_PlayerStamina;
			[SerializeField] private TMP_Text Text_LevelSelectLevelInt;
			[SerializeField] private TMP_Text Text_LevelSelectThemaName;
			
			[Header("Panels")]
			[SerializeField] private GameObject levelSelectPanel;
			[SerializeField] private GameObject  profilePanel;

			[Header("For Scene Move")]
			[field: SerializeField] public int nextLevelThemaInt;
			[field: SerializeField] public int nextLevelHowMuch;
			[field: SerializeField] public string nextLevelString = "";

			[Header("Do Tween and List")]
			[SerializeField] private RectTransform  LevelPanel;
			[SerializeField] private RectTransform  ListPanel;
			[SerializeField] private PlayerSC playerSc;

		#endregion
		
		#region Funks
		
			private void Start()
			{
				OpenLevelPanel();
				UpdateCanvas();
				for (int i = 0; i < LevelManager.Levels.Count; i++)
				{
					CreatePrefabInstance(i);
				}
				
				foreach (var selectLevelData in selectLevelButtons)
				{
					selectLevelData.selectLevelButton.onClick.AddListener(() => onLevelSelectButton(selectLevelData));
				}

				JustCloseSecentPanel();
			}

			private void CreatePrefabInstance(int index)
			{
				GameObject newPrefab = Instantiate(prefab, parentTransform);
				newPrefab.name = "Level_" + index;
				LevelCard currentLevel = LevelManager.Levels[index];
				
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
							if (currentLevel.isbought)
							{
								button.gameObject.SetActive(false);
							}
							else
							{
								button.gameObject.SetActive(true);
							}
		                
							TMP_Text priceText = button.GetComponentInChildren<TMP_Text>();
							if (priceText != null)
							{
								priceText.text = currentLevel.buyPrice.ToString(); 
							}
							
							// OnClick olayına thisFonksiyon'u bağlama
							button.onClick.AddListener(() => LevelBuy(currentLevel));
							break;
						}
					}
					
					Button[] buttons2 = newPrefab.GetComponentsInChildren<Button>();
					foreach (Button button in buttons2)
					{
						if (button.name == "B_LevelButton")
						{
							if (!currentLevel.isbought)
							{
								button.interactable = false ;
							}
							else
							{
								button.interactable = true ;
							}

							// OnClick olayına thisFonksiyon'u bağlama
							button.onClick.AddListener(() => OpenSelectLevel(currentLevel));
							break;
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
			
			private void onLevelSelectButton(SelectingLevel clickedLevel)
			{
				nextLevelMove(clickedLevel.IntXInt);
				playerSc.PlayerStamina--;
				SceneManager.LoadScene("T_CardList");

			}

			public void LevelBuy(LevelCard level)
			{
				if (playerSc.PlayerCoins >= level.buyPrice)
				{
					level.isbought = true;
					playerSc.PlayerCoins -= level.buyPrice;

					Debug.Log(level.themaName + " bölümü satın alındı!");
					DeletePrefab();
					UpdateCanvas();
				}
				else
				{
					//Para yok animasyonu
					print("Yetersiz Bakiye!");
				}
			}

			private void OpenSelectLevel(LevelCard currentLevel)
			{
				//Paneldeki text'leri yazdırma işlemi
				Text_LevelSelectThemaName.text = currentLevel.themaName;
				Text_LevelSelectLevelInt.text = "Level: " + currentLevel.levelIndex.ToString();
				
				nextLevelThemaInt = currentLevel.levelIndex; // Diğer sahnede çekeceğimiz thema int değeri
			
				levelSelectPanel.SetActive(true); // Paneli aktif etmek için 
			}
			
		#endregion

		#region Support Funk

			private void UpdateCanvas()
			{
				Text_PlayerCoins.text = playerSc.PlayerCoins.ToString();
				Text_PlayerPuans.text = playerSc.PlayerPuans.ToString();
				Text_PlayerStamina.text = playerSc.PlayerStamina.ToString();
			}
			
			public void DeletePrefab()
			{
				// Parent transformun altındaki tüm çocukları sil
				foreach (Transform child in parentTransform)
				{
					Destroy(child.gameObject);
				}

				// Parent altına tekrardan tüm çocukları oluşturmak için 
				for (int i = 0; i < LevelManager.Levels.Count; i++)
				{
					CreatePrefabInstance(i);
				}
			}

			private void nextLevelMove(int x)
			{
				switch (x)
				{
					case 22:
						nextLevelString = "TwoXTwo";
						nextLevelHowMuch = 4;
						break;
					case 44:
						nextLevelString = "FourXFour";
						nextLevelHowMuch = 16;
						break;
					case 34:
						nextLevelString = "ThreeXFour";
						nextLevelHowMuch = 12;
						break;
					case 46:
						nextLevelString = "FourXSix";
						nextLevelHowMuch = 24;
						break;
				
				}
			}
		
			public void JustCloseSecentPanel()
			{
				levelSelectPanel.SetActive(false);
				profilePanel.SetActive(false);
			}

			public void OpenProfilePanel()
			{
				profilePanel.SetActive(true);
			}

		#endregion

		#region DoTweeen

		public void OpenListPanel()
		{
			ListPanel.DOAnchorPos(new Vector2(0, 0), 0.5f);
			LevelPanel.DOAnchorPos(new Vector2(2000, 0), .5f);
					
		}

		public void OpenLevelPanel()
		{
			LevelPanel.DOAnchorPos(new Vector2(0, 0), 0.5f);
			ListPanel.DOAnchorPos(new Vector2(-2000, 0), .5f);
		}

		#endregion
	}
}
