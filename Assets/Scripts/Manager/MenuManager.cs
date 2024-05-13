using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
	[System.Serializable]
	public struct LevelData
	{
		public Button levelButton;
		public int levelIndex;
		public string levelName;
		public TMP_Text levelButtonText;
	}
	
	[System.Serializable]
	public struct SelectLevelData
	{
		public Button selectLevelButton;
		public int IntXInt;
	}
	public class MenuManager : MonoBehaviour
	{
		[Header("Menu Buttons")]
		[SerializeField] private List<LevelData> levelDataList;
		[SerializeField] private RectTransform MenuLevel;
		[SerializeField] private RectTransform MainScoreTable;

		[Header("Select Level Panel")]
		[SerializeField] private List<SelectLevelData> selectLevelButtons;
		[SerializeField] private GameObject  levelSelectPanel;
		[SerializeField] private TMP_Text levelNameText;
		[SerializeField] private TMP_Text levelIndexText;
		
		[Header("Profile Panel")]
		[SerializeField] private GameObject profilePanel;
		[SerializeField] private Button profileButton;


		private void Start()
		{
			OpenMenuLevelPanel();
				
			profilePanel.SetActive(false);
			levelSelectPanel.SetActive(false);
			
			// Her bir butona click event listener ekleniyor
			foreach (var levelData in levelDataList)
			{
				levelData.levelButton.onClick.AddListener(() => OnLevelButtonClick(levelData));
				levelData.levelButtonText.text = levelData.levelName.ToString();
			}
			foreach (var selectLevelData in selectLevelButtons)
			{
				selectLevelData.selectLevelButton.onClick.AddListener(() => OnSelectLevelButtonClick(selectLevelData));
			}
		}
		
		private void OnLevelButtonClick(LevelData clickedLevel)
		{
			// Seçilen seviye bilgilerini güncelle
			levelNameText.text = clickedLevel.levelName;
			levelIndexText.text = "Level " + clickedLevel.levelIndex.ToString();

			// Paneli aktif hale getir
			levelSelectPanel.SetActive(true);
		}
		
		private void OnSelectLevelButtonClick(SelectLevelData clickedLevel)
		{
			//ToDo Yeni mape geçiş 
			print(clickedLevel.IntXInt);
		}

		public void JustCloseLevelSelectPanel()
		{
			levelSelectPanel.SetActive(false);
		}

		public void OpenTabelListPanel()
		{
			MenuLevel.DOAnchorPos(new Vector2(+2000, 0), 0.5f);
			MainScoreTable.DOAnchorPos(new Vector2(0, 0), 0.5f);
		}
		
		public void OpenMenuLevelPanel()
		{
			MainScoreTable.DOAnchorPos(new Vector2(-2000, 0), 0.5f);
			MenuLevel.DOAnchorPos(new Vector2(0, 0), 0.5f);
		}

		public void JustOpenProfilePanel()
		{
			profilePanel.SetActive(true);
		}
		public void JustCloseProfilePanel()
		{
			profilePanel.SetActive(false);
		}

	}
}