using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFinish : MonoBehaviour
{

    public GameObject finishLine;
    public void setPositionFinish(bool[,] visitedCells, int mapRows, int mapColumns, int scale) {
        Vector3 temp = new Vector3(0, 0, 0);
		//search a cell (starting at the bottom right) which is able to be visited
        for (int r = mapRows - 1; r > 0 ; r--) {
			for (int c = mapColumns - 1; c > 0 ; c--) {
				if (visitedCells[r,c]){
                    temp = new Vector3(r * scale, 0, c * scale);
                    finishLine.transform.position = temp;
                    return;
                }
			}
		}

    }
}
