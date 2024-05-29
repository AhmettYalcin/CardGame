using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelList", menuName = "LevelMaanger/LevelList", order = 2)]
public class LevelManager : ScriptableObject
{
	public List<LevelCard> Levels;
	
}
