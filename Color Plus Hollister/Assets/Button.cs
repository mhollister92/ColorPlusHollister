using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public void startGame (int index) 
	{
		Application.LoadLevel(index);
	}

	public void startGame(string levelName)
	{
		Application.LoadLevel(levelName);
	}
}
