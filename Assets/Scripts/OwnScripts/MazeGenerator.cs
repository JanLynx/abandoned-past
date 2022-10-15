using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour {
	public int mapRows = 5;
	public int mapColumns = 10;

	public char[,] map;

	public string boxCharacters;
	private string[] boxCharacterUpFriends;
	private string[] boxCharacterDownFriends;
	private string[] boxCharacterLeftFriends;
	private string[] boxCharacterRightFriends;

	void Awake () {
		InitializeBoxCharacters ();
	}

	public void displayMap() {
		string output = "";
		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapColumns; c++) {
				output += map [r, c];
			}
			output += "\n";
		}
		Debug.Log (output);
	}

	public void InitializeMap() {
		map = new char[mapRows, mapColumns];

		//fill the rowborders of the array with X
		for (int c = 0; c < mapColumns; c++) {
			map [0, c] = 'X';
			map [mapRows - 1, c] = 'X';
		}
		
		//fill the columnborders of the array with X
		for (int r = 0; r < mapRows; r++) {
			map [r, 0] = 'X';
			map [r, mapColumns - 1] = 'X';
		}
		
		//fill the rest of the array with O
		for (int r = 1; r < mapRows - 1; r++) {
			for (int c = 1; c < mapColumns - 1; c++) {
				map [r, c] = 'O';
			}
		}

		Random.InitState(System.DateTime.Now.Millisecond);
		
		//placing valid shapes inside the array
		for (int r = 1; r < mapRows - 1; r++) {
			for (int c = 1; c < mapColumns - 1; c++) {
				string validCharacters = getValidBoxCharacters (r, c);
				map [r, c] = validCharacters [Random.Range (0, validCharacters.Length)];
			}
		}

		findDeadends();
	}

	private void findDeadends() {
		bool[,] visitedCells = findUnreachableParts();
		for (int r = 1; r < mapRows - 1; r++) {
			for (int c = 1; c < mapColumns - 1; c++) {
				if (map [r, c] == 'O') { //Deadend?
					if ("─┐┘┤┬┴┼".Contains(map [r, c + 1].ToString()) && visitedCells [r, c + 1]) { //│─┐┘┌└┤├┬┴┼
						map [r, c] = 'R';
					} else if ("─┌└├┬┴┼".Contains(map [r, c - 1].ToString()) && visitedCells [r, c - 1]) { //│─┐┘┌└┤├┬┴┼
						map [r, c] = 'L';
					} else if ("│┐┌┤├┬┼".Contains(map [r - 1, c].ToString()) && visitedCells [r - 1, c]) { //│─┐┘┌└┤├┬┴┼
						map [r, c] = 'U';
					} else if ("│─┘└┤├┴┼".Contains(map [r + 1, c].ToString()) && visitedCells [r + 1, c]) { //│─┐┘┌└┤├┬┴┼
						map [r, c] = 'B';
					}
				}
			}
		}

	}

	private string getValidBoxCharacters(int row, int column) {
		string validCharacters = "";

		for (int i = 0; i < boxCharacters.Length; i++) {
			char ch = boxCharacters [i];
			
			
			if (
				boxCharacterLeftFriends [i].Contains (map [row, column - 1].ToString ()) &&
				boxCharacterRightFriends [i].Contains (map [row, column + 1].ToString ()) &&
				boxCharacterUpFriends [i].Contains (map [row - 1, column].ToString ()) &&
				boxCharacterDownFriends [i].Contains (map [row + 1, column].ToString ())){
				validCharacters += ch.ToString ();
			}
		}

		if (validCharacters.Length == 0) {
			validCharacters = "O";
		}

		return validCharacters;
	}


	private void InitializeBoxCharacters() {
		boxCharacters = "─│┌┐└┘├┤┬┴┼";
		boxCharacterUpFriends = new string[boxCharacters.Length];
		boxCharacterDownFriends = new string[boxCharacters.Length];
		boxCharacterLeftFriends = new string[boxCharacters.Length];
		boxCharacterRightFriends = new string[boxCharacters.Length];

		boxCharacterLeftFriends [0] = "O─┌└├┬┴┼"; //    ─
		boxCharacterLeftFriends [1] = "O│┐┘┤X"; //     │
		boxCharacterLeftFriends [2] = "O│┐┘┤X"; //     ┌
		boxCharacterLeftFriends [3] = "O─┌└├┬┴┼"; //    ┐
		boxCharacterLeftFriends [4] = "O│┐┘┤X"; //     └
		boxCharacterLeftFriends [5] = "O─┌└├┬┴┼"; //    ┘
		boxCharacterLeftFriends [6] = "O│┐┘┤X"; //      ├
		boxCharacterLeftFriends [7] = "O─┌└├┬┴┼"; //   ┤
		boxCharacterLeftFriends [8] = "O─┌└├┬┴┼"; //    ┬
		boxCharacterLeftFriends [9] = "O─┌└├┬┴┼"; //    ┴
		boxCharacterLeftFriends [10] = "O─┌└├┬┴┼"; //   ┼

		boxCharacterRightFriends [0] = "O─┐┘┤┬┴┼"; //    ─
		boxCharacterRightFriends [1] = "O│┌└├X"; //     │
		boxCharacterRightFriends [2] = "O─┐┘┤┬┴┼"; //   ┌
		boxCharacterRightFriends [3] = "O│┌└├X"; //      ┐
		boxCharacterRightFriends [4] = "O─┐┘┤┬┴┼"; //   └
		boxCharacterRightFriends [5] = "O│┌└├X"; //      ┘
		boxCharacterRightFriends [6] = "O─┐┘┤┬┴┼"; //   ├
		boxCharacterRightFriends [7] = "O│┌└├X"; //      ┤
		boxCharacterRightFriends [8] = "O─┐┘┤┬┴┼"; //    ┬
		boxCharacterRightFriends [9] = "O─┐┘┤┬┴┼"; //    ┴
		boxCharacterRightFriends [10] = "O─┐┘┤┬┴┼"; //   ┼

		boxCharacterUpFriends [0] = "O─└┘┴X"; //       ─
		boxCharacterUpFriends [1] = "O│┌┐├┤┬┼"; //      │
		boxCharacterUpFriends [2] = "O─└┘┴X"; //        ┌
		boxCharacterUpFriends [3] = "O─└┘┴X"; //        ┐
		boxCharacterUpFriends [4] = "O│┌┐├┤┬┼"; //     └
		boxCharacterUpFriends [5] = "O│┌┐├┤┬┼"; //     ┘
		boxCharacterUpFriends [6] = "O│┌┐├┤┬┼"; //      ├
		boxCharacterUpFriends [7] = "O│┌┐├┤┬┼"; //      ┤
		boxCharacterUpFriends [8] = "O─└┘┴X"; //        ┬
		boxCharacterUpFriends [9] = "O│┌┐├┤┬┼"; //     ┴
		boxCharacterUpFriends [10] = "O│┌┐├┤┬┼"; //     ┼

		boxCharacterDownFriends [0] = "O─┌┐┬X"; //       ─
		boxCharacterDownFriends [1] = "O│└┘├┤┴┼"; //      │
		boxCharacterDownFriends [2] = "O│└┘├┤┴┼"; //     ┌
		boxCharacterDownFriends [3] = "O│└┘├┤┴┼"; //     ┐
		boxCharacterDownFriends [4] = "O─┌┐┬X"; //        └
		boxCharacterDownFriends [5] = "O─┌┐┬X"; //        ┘
		boxCharacterDownFriends [6] = "O│└┘├┤┴┼"; //      ├
		boxCharacterDownFriends [7] = "O│└┘├┤┴┼"; //      ┤
		boxCharacterDownFriends [8] = "O│└┘├┤┴┼"; //     ┬
		boxCharacterDownFriends [9] = "O─┌┐┬X"; //        ┴
		boxCharacterDownFriends [10] = "O│└┘├┤┴┼"; //     ┼
	}	

	public bool[,] findUnreachableParts() {
		bool[,] visitedCells = new bool[mapRows, mapColumns];
		int currentRow = 1;
		int currentColumn = 1;

		traverseCells(visitedCells, currentRow, currentColumn);

		return visitedCells;
	}

	private void traverseCells(bool[,] visited, int currentRow, int currentColumn) {
		if (visited [currentRow, currentColumn]) {
			return;
		}

		visited [currentRow, currentColumn] = true;

		switch (map [currentRow, currentColumn]) {
		case '┌':
			traverseCells (visited, currentRow, currentColumn + 1);
			traverseCells (visited, currentRow + 1, currentColumn);
			break;
		case '┐':
			traverseCells (visited, currentRow + 1, currentColumn);
			traverseCells (visited, currentRow, currentColumn - 1);
			break;
		case '─':
			traverseCells (visited, currentRow, currentColumn - 1);
			traverseCells (visited, currentRow, currentColumn + 1);
			break;
		case '│':
			traverseCells (visited, currentRow - 1, currentColumn);
			traverseCells (visited, currentRow + 1, currentColumn);
			break;
		case '└':
			traverseCells (visited, currentRow, currentColumn + 1);
			traverseCells (visited, currentRow - 1, currentColumn);
			break;
		case '┘':
			traverseCells (visited, currentRow - 1, currentColumn);
			traverseCells (visited, currentRow, currentColumn - 1);
			break;
		case '├':
			traverseCells (visited, currentRow - 1, currentColumn);
			traverseCells (visited, currentRow + 1, currentColumn);
			traverseCells (visited, currentRow, currentColumn + 1);
			break;
		case '┤':
			traverseCells (visited, currentRow - 1, currentColumn);
			traverseCells (visited, currentRow + 1, currentColumn);
			traverseCells (visited, currentRow, currentColumn - 1);
			break;
		case '┬':
			traverseCells (visited, currentRow, currentColumn - 1);
			traverseCells (visited, currentRow, currentColumn + 1);
			traverseCells (visited, currentRow + 1, currentColumn);
			break;
		case '┴':
			traverseCells (visited, currentRow, currentColumn - 1);
			traverseCells (visited, currentRow, currentColumn + 1);
			traverseCells (visited, currentRow - 1, currentColumn);
			break;
		case '┼':
			traverseCells (visited, currentRow, currentColumn - 1);
			traverseCells (visited, currentRow, currentColumn + 1);
			traverseCells (visited, currentRow - 1, currentColumn);
			traverseCells (visited, currentRow + 1, currentColumn);
			break;
		case 'O': case 'R': case 'L': case 'U': case 'B': 
			return;
		default:
			Debug.LogError ("Error:(" + currentRow + "," + currentColumn + ") '" + map[currentRow,currentColumn]);
			return;
		}

	}

}
