using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour {

	GameController aGameController;
	public int x;
	public int y;

	public Color cubeColor = Color.white;
	public bool active = false;

	// Use this for initialization
	void Start () {
		aGameController = GameObject.Find ("GameControllerObject").GetComponent<GameController> ();
		
	}
	
	void OnMouseDown ()
	{
		aGameController.ProcessClickedCube (this.gameObject, x, y);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
