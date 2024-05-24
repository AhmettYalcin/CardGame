using System;
using System.Collections;
using System.Collections.Generic;
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
	
	public class MenuManager : MonoBehaviour
	{
		[Header("Menu Buttons")]
		[SerializeField] private List<LevelData> levelDataList;

		[Header("Select Level Panel")]
		[SerializeField] private List<SelectLevelData> selectLevelButtons;
		[SerializeField] private GameObject  levelSelectPanel;
		[SerializeField] private TMP_Text levelNameText;
		[SerializeField] private TMP_Text levelIndexText;
		

		private void Start()
		{
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

	}
}