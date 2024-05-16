using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Manager
{
	[System.Serializable]
	public class LevelClass
	{
		public Button LevelButton;
		public Button OpenButton;
		public string ThemaName;
		public int levelIndex;
		public int Price;
		public bool IsUnlocked;
		public TMP_Text levelButtonText;
		public TMP_Text levelPriceText;
	}

	public class LevelSelect : MonoBehaviour
	{
		public LevelClass[] levels;

		[Header("Select Level Panel")]
		[SerializeField] private List<SelectLevelData> selectLevelButtons;
		[SerializeField] private GameObject  profilePanel;
		[SerializeField] private GameObject  levelSelectPanel;
		[SerializeField] private TMP_Text levelNameText;
		[SerializeField] private TMP_Text levelIndexText;
		
		[Header("Do Tween ")]
		[SerializeField] private RectTransform  LevelPanel;
		[SerializeField] private RectTransform  ListPanel;

		[Header("Up Nav Bar ")]
		[SerializeField] private TMP_Text  playerCoinText;
		[SerializeField] private TMP_Text  playerPuanText;
		[SerializeField] private TMP_Text  playerStaminaText;
		
		private void Start()
		{
			JustCloseSecentPanel(); // Başlangıçta ikinci ekranları kapatmak için
			UpdateUI(); // UI'yi güncellemek için Start'ta çağırıyoruz
			OpenLevelPanel(); // Başlangıçta Level sahnesini açmak için 
			textUpdate(); // Başlangıçta textleri güncellemek için 
			//Levels içersindeki butonlara metot atamak için 
			foreach (var levelData in levels)
			{
				levelData.LevelButton.onClick.AddListener(() => OnLevelButtonClick(levelData));
				levelData.levelButtonText.text = levelData.ThemaName.ToString();
				levelData.levelPriceText.text = levelData.Price.ToString();
			}
			foreach (var selectLevelData in selectLevelButtons)
			{
				selectLevelData.selectLevelButton.onClick.AddListener(() => OnSelectLevelButtonClick(selectLevelData));
			}
		}
		private void OnLevelButtonClick(LevelClass clickedLevel)
		{
			// Seçilen seviye bilgilerini güncelle
			levelNameText.text = clickedLevel.ThemaName;
			levelIndexText.text = "Level " + clickedLevel.levelIndex.ToString();

			// Paneli aktif hale getir
			levelSelectPanel.SetActive(true);
		}
		private void OnSelectLevelButtonClick(SelectLevelData clickedLevel)
		{
			//ToDo Yeni mape geçiş 
			print(clickedLevel.IntXInt);
		}

		public void OpenProfilePanel()
		{
			profilePanel.SetActive(true);
		}

		public void UpdateUI()
		{
			foreach (LevelClass levelData in levels)
			{
				// Eğer level kilitli ise
				if (levelData.IsUnlocked)
				{
					// Butonu devre dışı bırak
					levelData.LevelButton.interactable = false;
					levelData.OpenButton.gameObject.SetActive(true);
				}
				else
				{
					// Eğer level kilitli değilse, butonu etkinleştir
					levelData.LevelButton.interactable = true;
					levelData.OpenButton.gameObject.SetActive(false);
				}
			}
		}

		public void OpenLevel()
		{
			// OpenButton'a tıklandığında, LevelButton'u açmak için işlem yapılacak
			foreach (LevelClass levelData in levels)
			{
				// OpenButton'un olduğu LevelClass'ın LevelButton'u tespit edilir ve bu butona tıklanır
				if (levelData.OpenButton == UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
				{
					if (GameDataBase.instance.PlayerCoins < levelData.Price)
					{
						print("Paran yok Köle");
					}
					else
					{
						GameDataBase.instance.PlayerCoins -= levelData.Price;
						levelData.LevelButton.interactable = true;
						levelData.OpenButton.gameObject.SetActive(false);
						textUpdate();
						
						break; // İlgili OpenButton bulunduğunda döngüyü sonlandırırız
					}
				}
			}
		}

		private void textUpdate()
		{
			playerCoinText.text = GameDataBase.instance.PlayerCoins.ToString();
			playerPuanText.text = GameDataBase.instance.PlayerPuans.ToString();
			playerStaminaText.text = GameDataBase.instance.PlayerStamina.ToString();

		}
		
		public void JustCloseSecentPanel()
		{
			levelSelectPanel.SetActive(false);
			profilePanel.SetActive(false);
		}

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