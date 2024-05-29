using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "LevelManager/Level", order = 1)]
public class LevelCard : ScriptableObject
{
	public string themaName;
	
	public float buyPrice;
	
	public int playingCount;
	public int MaxPlayingSliderCount;
	
	public bool isbought;
	
	public Button LevelButton;
	public Button OpenButton;
	
	public Slider LevelSlider;
	
	public TMP_Text levelButtonText;
	public TMP_Text levelPriceText;
	
}