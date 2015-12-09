using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject cubePrefab;
	private GameObject [,] allCubes;
	public GameObject nextCube;
	public CubeClass [,] cubeArray;
	public CubeClass activeCube;

	int gridWidth = 8;
	int gridHeight = 5;
	int multiScore = 5;
	int sameScore = 10;
	float turn = 2.0f;
	float gameLength = 120.0f;
	float timeToAct;
	int row;
	public int score = 0;
	float timer;
	float seconds;
	int loseLevel = 2;
	int winLevel = 3;

	Color [] colors = {Color.blue, Color.yellow, Color.green, Color.magenta, Color.red};
	public Color nextColor;
	public Color cubeColor;

	public bool cubeSpawned = true;
	public bool gotInput = false;

	public Text scoreUI;
	public Text nextCubeUI;
	public Text timerUI;


	public void ProcessClickedCube (GameObject clickedCube, int x, int y)
	{

		if ((cubeArray [x, y].x == activeCube.x && (cubeArray [x, y].y == activeCube.y + 1 || cubeArray [x, y].y == activeCube.y - 1))
			|| ((cubeArray [x, y].x == activeCube.x + 1 || cubeArray [x, y].x == activeCube.x - 1) && cubeArray [x, y].y == activeCube.y)
			&& cubeArray [x, y].cubeColor == Color.white && activeCube.active) {

			//set new color
			cubeArray [x, y].cubeColor = activeCube.cubeColor;
			allCubes [x, y].GetComponent<Renderer> ().material.color = activeCube.cubeColor;
			int tempx = activeCube.x;
			int tempy = activeCube.y;
			//set previous cube white
			allCubes [tempx, tempy].GetComponent<Renderer> ().material.color = Color.white;
			cubeArray [tempx, tempy].cubeColor = Color.white;
			cubeArray [tempx, tempy].active = false;
			allCubes [tempx, tempy].GetComponent<CubeBehavior> ().active = false;
			//set new cube to active cube
			activeCube.x = cubeArray [x, y].x;
			activeCube.y = cubeArray [x, y].y;
			activeCube.cubeColor = cubeArray [x, y].cubeColor;
			activeCube.active = true;
			cubeArray [x, y].active = true;
			//helps check if correct cube is active
			allCubes [x, y].GetComponent<CubeBehavior> ().active = true;
			allCubes[tempx,tempy].transform.localScale = new Vector3(1f, 1f, 1f);
			allCubes[x,y].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
			//check for score
			checkMultiPlus ();
			checkSamePlus ();
		} 
		//if clicked adjacent cube is colored
		else if ((cubeArray [x, y].x == activeCube.x && (cubeArray [x, y].y == activeCube.y + 1 || cubeArray [x, y].y == activeCube.y - 1))
			|| ((cubeArray [x, y].x == activeCube.x + 1 || cubeArray [x, y].x == activeCube.x - 1) && cubeArray [x, y].y == activeCube.y)
		           && cubeArray [x, y].cubeColor != Color.white && cubeArray [x, y].cubeColor != Color.black
		           && cubeArray [x, y].cubeColor != Color.grey && activeCube.active) {

			//sets previous active cube to false
			activeCube.active = false;
			cubeArray[activeCube.x, activeCube.y].active = false;
			//this helps me check that the correct cubes are active
			allCubes[activeCube.x, activeCube.y].GetComponent<CubeBehavior>().active = false;
			allCubes[activeCube.x, activeCube.y].transform.localScale = new Vector3 (1f, 1f, 1f);
			//sets new active cube
			activeCube.x = x;
			activeCube.y = y;
			activeCube.cubeColor = cubeArray[x,y].cubeColor;
			activeCube.active = true;
			cubeArray[x,y].active = true;
			//helps me check correct cube is active
			allCubes[x,y].GetComponent<CubeBehavior>().active = true;
			allCubes[x, y].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		}

		//makes clicked cube active
		else if ((cubeArray [x, y].cubeColor != Color.white ||
		    	 cubeArray[x, y].cubeColor != Color.black || 
		          cubeArray[x,y].cubeColor != Color.grey)&&
				 cubeArray [x, y].active == false) {

			cubeArray [x, y].active = true;
			activeCube.x = x;
			activeCube.y = y;
			activeCube.cubeColor = cubeArray[x,y].cubeColor;
			activeCube.active = true;
			//helps me check that correct cube is active
			allCubes[x,y].GetComponent<CubeBehavior>().active = true;
			allCubes[x,y].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

		} 

		//deactivates clicked cube
		else if ((cubeArray [x, y].cubeColor != Color.white||
		         cubeArray[x, y].cubeColor != Color.black || 
		          cubeArray[x,y].cubeColor != Color.grey) &&
				 cubeArray [x, y].active && activeCube.active) {

			cubeArray [x, y].active = false;
			activeCube.active = false;
			//lets me check if correct cube is deactivated
			allCubes[x,y].transform.localScale = new Vector3(1f, 1f, 1f);
			allCubes[x,y].GetComponent<CubeBehavior>().active = false;
		}


	}
	
	public Color spawnRandom()
	{
		nextCube = (GameObject) Instantiate(cubePrefab, new Vector3(5, 0, 10), 
		                                    Quaternion.identity);
		cubeColor = nextCube.GetComponent<Renderer> ().material.color = colors [Random.Range (0, colors.Length)];

		return cubeColor;
	}

	public void placeCube(int row, Color cubeColor)
	{
		bool cubePlaced = false;
		bool rowFull = true;
		while (cubePlaced == false) 
		{
			for(int count = 0; count < gridWidth; count++)
			{
				if(cubeArray[count, row].cubeColor == Color.white)
				{
					rowFull = false;
				}
			}
			if (rowFull)
			{
				//end game
				Application.LoadLevel (loseLevel);
				cubePlaced = true;
			}
			int xCoord = Random.Range (0, gridWidth);
			if (cubeArray [xCoord, row].cubeColor == Color.white && rowFull == false) {
				allCubes [xCoord, row].GetComponent<Renderer>().material.color = cubeColor;
				cubeArray [xCoord, row].cubeColor = cubeColor;
				cubePlaced = true;
			}
		}
	}

	//checks the cubes not on the perimeter of the grid for win conditions
	public void checkMultiPlus()
	{
		for (int xcount = 1; xcount < gridWidth-1; xcount++) {
			for (int ycount = 1; ycount < gridHeight-1; ycount++) {
				if (cubeArray [xcount, ycount].cubeColor != Color.white && cubeArray [xcount, ycount].cubeColor != Color.black &&
					cubeArray [xcount, ycount].cubeColor != Color.grey) {
					if(cubeArray[xcount, ycount].cubeColor != cubeArray[xcount, ycount+1].cubeColor &&
					   cubeArray[xcount, ycount+1].cubeColor != Color.white && 
					   cubeArray[xcount, ycount+1].cubeColor != Color.black &&
					   cubeArray[xcount, ycount+1].cubeColor != Color.grey)
					{
						if(cubeArray[xcount, ycount].cubeColor != cubeArray[xcount, ycount-1].cubeColor &&
						   cubeArray[xcount, ycount+1].cubeColor != cubeArray[xcount, ycount-1].cubeColor &&
						   cubeArray[xcount, ycount-1].cubeColor != Color.white &&
						   cubeArray[xcount, ycount-1].cubeColor != Color.black &&
						   cubeArray[xcount, ycount-1].cubeColor != Color.grey)
						{
							if(cubeArray[xcount, ycount].cubeColor != cubeArray[xcount+1, ycount].cubeColor &&
							   cubeArray[xcount, ycount+1].cubeColor != cubeArray[xcount+1, ycount].cubeColor &&
							   cubeArray[xcount, ycount-1].cubeColor != cubeArray[xcount+1, ycount].cubeColor &&
							   cubeArray[xcount+1, ycount].cubeColor != Color.white &&
							   cubeArray[xcount+1, ycount].cubeColor != Color.black &&
							   cubeArray[xcount+1, ycount].cubeColor != Color.grey)
							{
								if(cubeArray[xcount, ycount].cubeColor != cubeArray[xcount-1, ycount].cubeColor &&
								   cubeArray[xcount, ycount+1].cubeColor != cubeArray[xcount-1, ycount].cubeColor &&
								   cubeArray[xcount, ycount-1].cubeColor != cubeArray[xcount-1, ycount].cubeColor &&
								   cubeArray[xcount+1, ycount].cubeColor != cubeArray[xcount-1, ycount].cubeColor &&
								   cubeArray[xcount-1, ycount].cubeColor != Color.white &&
								   cubeArray[xcount-1, ycount].cubeColor != Color.black &&
								   cubeArray[xcount-1, ycount].cubeColor != Color.grey)
								{
									score += multiScore;
									cubeArray[xcount, ycount].cubeColor = Color.grey;
									cubeArray[xcount+1, ycount].cubeColor = Color.grey;
									cubeArray[xcount-1, ycount].cubeColor = Color.grey;
									cubeArray[xcount, ycount+1].cubeColor = Color.grey;
									cubeArray[xcount, ycount-1].cubeColor = Color.grey;
									
									allCubes[xcount, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount+1, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount-1, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount, ycount+1].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount, ycount-1].GetComponent<Renderer>().material.color = Color.grey;
								}
							}
						}
					}
				}
			}
		}
	}

	//checks the cubes that are not on the perimeter of the grid for win conditions
	public void checkSamePlus()
	{
		for (int xcount = 1; xcount < gridWidth-1; xcount++) {
			for (int ycount = 1; ycount < gridHeight-1; ycount++) {
				if (cubeArray [xcount, ycount].cubeColor != Color.white && cubeArray [xcount, ycount].cubeColor != Color.black &&
				    cubeArray [xcount, ycount].cubeColor != Color.grey){
					if(cubeArray[xcount, ycount].cubeColor == cubeArray[xcount, ycount-1].cubeColor)
					{
						if(cubeArray[xcount, ycount].cubeColor == cubeArray[xcount, ycount+1].cubeColor)
						{
							if(cubeArray[xcount, ycount].cubeColor == cubeArray[xcount+1, ycount].cubeColor)
							{
								if(cubeArray[xcount, ycount].cubeColor == cubeArray[xcount-1, ycount].cubeColor)
								{
									score += sameScore;
									cubeArray[xcount, ycount].cubeColor = Color.grey;
									cubeArray[xcount+1, ycount].cubeColor = Color.grey;
									cubeArray[xcount-1, ycount].cubeColor = Color.grey;
									cubeArray[xcount, ycount+1].cubeColor = Color.grey;
									cubeArray[xcount, ycount-1].cubeColor = Color.grey;

									allCubes[xcount, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount+1, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount-1, ycount].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount, ycount+1].GetComponent<Renderer>().material.color = Color.grey;
									allCubes[xcount, ycount-1].GetComponent<Renderer>().material.color = Color.grey;
								}
							}
						}
					}

				}
			}
		}
	}

	public int getInput ()
	{
		int rowNum = 5;

		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
			rowNum = 4;
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
		{
			rowNum = 3;
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
		{
			rowNum = 2;
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
		{
			rowNum = 1;
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
		{
			rowNum = 0;
		}
		return rowNum;
	}

	// Use this for initialization
	void Start () {

		allCubes = new GameObject[gridWidth, gridHeight];
		cubeArray = new CubeClass [gridWidth, gridHeight];
		activeCube = new CubeClass ();

		for (int xcount = 0; xcount < gridWidth; xcount++) {
			for (int ycount = 0; ycount < gridHeight; ycount++)
			{
				allCubes [xcount,ycount] = (GameObject) Instantiate (cubePrefab, new Vector3(xcount*2 - 14, ycount*2 - 8, 10), 
				                                           Quaternion.identity);
				allCubes[xcount,ycount].GetComponent<CubeBehavior>().x = xcount;
				allCubes[xcount,ycount].GetComponent<CubeBehavior>().y = ycount;
				cubeArray[xcount,ycount] = new CubeClass();
				cubeArray[xcount, ycount].x = xcount;
				cubeArray[xcount, ycount].y = ycount;
			}
		}

		timeToAct = 0f;
	}

	// Update is called once per frame
	void Update () {
	
		timer = Time.timeSinceLevelLoad;
		seconds = timer % 60;
		seconds = 60 - (int)seconds;
		if (seconds < 10) 
		{
			timerUI.color = Color.red;
			timerUI.text = "0" + seconds.ToString ();
		} 
		else if (seconds >= 10)
		{
			timerUI.text = seconds.ToString ();
		}

		//UI
		scoreUI.text = "Score: " + score;
		nextCubeUI.text = "Next Cube";
		//Checks score
		checkMultiPlus ();
		checkSamePlus ();
		//gets row number
		if (gotInput == false) {
			row = getInput ();
			if(row >=0 && row < 5)
			{
				placeCube (row, nextColor);
				//destroys the next cube after it's been placed
				Destroy (nextCube);
				gotInput = true;
				cubeSpawned = true;
			}
			else
			{
				gotInput = false;
			}
		}
		if (Time.timeSinceLevelLoad >= gameLength) {
			// end game
			if(score >0)
			{
				Application.LoadLevel (winLevel);
			}
			else
			{
				Application.LoadLevel (loseLevel);
			}
		} 

		//check for turn
		else if (Time.timeSinceLevelLoad >= timeToAct) {
			if(cubeSpawned == false)
			{
				bool cubePlaced = false;
				bool gridFull = true;
				//checks for an open random cube to color black
				while (cubePlaced == false) 
				{
					for(int countx = 0; countx < gridWidth; countx++)
					{
						for(int county = 0; county < gridHeight; county++)

						{
							if(cubeArray[countx, county].cubeColor == Color.white)
							{
								//tells the loop that the grid is not full
								gridFull = false;
							}
						}
					}
					if (gridFull)
					{
						//end game
						Application.LoadLevel (loseLevel);
						cubePlaced = true;
					}
					//sets random ints to an x coordinate and a y corrdinate and then checks if the cube is white. Will continually set new coords until it finds a white cube.
				int xCoord = Random.Range (0, gridWidth);
				int yCoord = Random.Range (0, gridHeight);
				if (cubeArray [xCoord, yCoord].cubeColor == Color.white && gridFull == false) 
					{
					allCubes [xCoord, yCoord].GetComponent<Renderer>().material.color = 
							Color.black;
						//destroys the next cube if a black cube was placed
						Destroy (nextCube);
						cubeArray [xCoord, yCoord].cubeColor = Color.black;
						if(score > 0)
						{
							score--;
						}
						cubePlaced = true;
					}
				}
			}
			nextColor = spawnRandom ();
			gotInput = false;
			cubeSpawned = false;
			timeToAct += turn;
		}
	
	}
}
