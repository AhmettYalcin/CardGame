using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Manager
{
	[System.Serializable]
	public class LevelClass
	{
		public string ThemaName;
		
		public int LevelIndex;
		public int Price;
		public int LevelSliderCount;
		public int MaxLevelSliderCount;

		public bool IsUnlocked;
		
		public Button LevelButton;
		public Button OpenButton;

		public Slider LevelSlider;

		public TMP_Text levelButtonText;
		public TMP_Text levelPriceText;
	}
	
	[System.Serializable]
	public struct SelectLevelData
	{
		public Button selectLevelButton;
		public int IntXInt;
		public string SecButtonThemaName;
	}
	
	public class LevelSelect : MonoBehaviour
	{
		public static LevelSelect instance { get; private set; }

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
		
		[Header("Scenes")]
		[field: SerializeField] public string levelStiringMenu = "";
		[field: SerializeField] public int levelIntMenu;
		[field: SerializeField] public int levelThemeNameInt;
		
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
		
		private void Start()
		{
			UPdateDatats(); // proje datalarını güncellemek için 
			JustCloseSecentPanel(); // Başlangıçta ikinci ekranları kapatmak için
			UpdateUI(); // UI'yi güncellemek için Start'ta çağırıyoruz
			OpenLevelPanel(); // Başlangıçta Level sahnesini açmak için 
			textUpdate(); // Başlangıçta textleri güncellemek için 

		}

		private void UPdateDatats()
		{
			//Levels içersindeki butonlara metot atamak için 
			foreach (var levelData in levels)
			{
				levelData.LevelButton.onClick.AddListener(() => OnLevelButtonClick(levelData));
				levelData.levelButtonText.text = levelData.ThemaName.ToString();
				levelData.levelPriceText.text = levelData.Price.ToString();

				levelData.LevelSlider.maxValue = levelData.MaxLevelSliderCount;
				levelData.LevelSlider.value = levelData.LevelSliderCount;
				
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
			
			
			print(clickedLevel.ThemaName);
			levelIndexText.text = "Level " + clickedLevel.LevelIndex.ToString();
			nextLevelTheme(clickedLevel.ThemaName);
			// Paneli aktif hale getir
			levelSelectPanel.SetActive(true);
		}
		private void OnSelectLevelButtonClick(SelectLevelData clickedLevel)
		{
			nextLevelMove(clickedLevel.IntXInt);
			SceneManager.LoadScene("T_CardList");

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

		private void nextLevelMove(int x)
		{
			switch (x)
			{
				case 22:
					levelStiringMenu = "TwoXTwo";
					levelIntMenu = 4;
					break;
				case 44:
					levelStiringMenu = "FourXFour";
					levelIntMenu = 16;
					break;
				case 34:
					levelStiringMenu = "ThreeXFour";
					levelIntMenu = 12;
					break;
				case 46:
					levelStiringMenu = "FourXSix";
					levelIntMenu = 24;
					break;
				
			}
			
		}

		private void nextLevelTheme(string theme)
		{
			switch (theme)
			{
				case "Animal":
					levelThemeNameInt = 2;
					break;
				case "Sport":
					levelThemeNameInt = 5; 
					break;
				case "Colors":
					levelThemeNameInt = 3;
					break;
				case "Fruit":
					levelThemeNameInt = 4;
					break;
				case "Music":
					levelThemeNameInt = 6;
					break;
			}			
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