using UnityEngine;
using System.Collections;

public class DisplayMaze : MonoBehaviour {
	public GameObject[] shapes;
	private MazeGenerator MazeGenerator;
	private TransformFinish TransformFinish;
	public int distance = 5;
	public int torchPos = 15;
	public float minimumMazePercentage = 0.8f;
	public int scale = 12;

	void Start () {
		MazeGenerator = GetComponent<MazeGenerator> ();
		TransformFinish = GetComponent<TransformFinish> ();

		int visitedCellCount = 0;
		bool[,] visitedCells = new bool[MazeGenerator.mapRows, MazeGenerator.mapColumns];

		//Build Map until it's greater than the given percentage
		int minimumMazeCells = Mathf.FloorToInt((MazeGenerator.mapRows - 2) * (MazeGenerator.mapColumns - 2) * minimumMazePercentage);
		while (visitedCellCount < minimumMazeCells) {
			Debug.Log ("Current dungeon size = " + visitedCellCount + " which is less than the required " + minimumMazeCells + ". Retrying");
			MazeGenerator.InitializeMap ();
			visitedCells = MazeGenerator.findUnreachableParts ();
			visitedCellCount = getVisitedCellsCount (visitedCells);
			Debug.Log ("visited cell count = " + visitedCellCount);
		}

		MazeGenerator.displayMap ();

		for (int r = 1; r < MazeGenerator.mapRows - 1; r++) {
			bool setTorch = false; 
			for (int c = 1; c < MazeGenerator.mapColumns - 1; c++) {
				string ch = MazeGenerator.map [r, c].ToString();
				string boxCharacters = MazeGenerator.boxCharacters + "RLUB"; //add the missing deadends
				int charPos = boxCharacters.IndexOf (ch);

				//setting the shapes
				if (charPos < 0 || !visitedCells[r,c]) { //charPos < 0 -> there is no index found with this symbol and the cell can be visited -> no island
					continue;
				}

                if (c == 1 && r == 1)
                {
                    continue;
                }
                Instantiate(shapes [charPos], new Vector3 (r * scale, 0, c * scale), shapes[charPos].transform.rotation);

				//Setting the floating torches
				if (r%distance != 0 || setTorch) { //not every *distance*th row or if a torch already set in this column
					continue;
				}

				//if((charPos == 2 || charPos == 3 || charPos == 4 || charPos == 5)) { //Only choose corners for setting torches
				//	Instantiate (shapes [torchPos], new Vector3 (r * scale, 0, c * scale), shapes[torchPos].transform.rotation);
				//	setTorch = true;
				//}

			}
		}
		// TransformFinish.setPositionFinish(visitedCells, MazeGenerator.mapRows, MazeGenerator.mapColumns, scale);
	}

	private int getVisitedCellsCount(bool[,] visitedCells) {
		int visitedCellsCount = 0;

		for (int r = 1; r < MazeGenerator.mapRows - 1; r++) {
			for (int c = 1; c < MazeGenerator.mapColumns - 1; c++) {
				if (visitedCells [r, c]) {
					visitedCellsCount++;
				}
			}
		}

		return visitedCellsCount;
	}
}
