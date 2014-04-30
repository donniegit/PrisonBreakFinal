using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Represents floor cells in prison that agents can turn on
 */
public class TurningFloorCell {
	/* probability of agent taking left corner */
	public double LeftCornerProbability { get; set; }

	/* probability of agent taking down corner */
	public double DownCornerProbability { get; set; }

	/* probability of agent taking right corner */
	public double RightCornerProbability { get; set; }

	/* probability of agent taking up corner */
	public double UpCornerProbability { get; set; }

	public double[] cornerProbabilities { get; set;}

	/* x coordinate in GameMap GameMapArray */
	public int LocationX { get; set; }

	/* y coordinate in GameMap GameMapArray */
	public int LocationY { get; set; }

	/* unique value that identifies floor cell */
	public int FloorCellIndex { get; set; }

	/* unique hash value that can be used to index List<> and Ditionary<> */
	public int HashValue { get; set; }

	public TurningFloorCell(int locationX, int locationY) {
		this.LocationX = locationX;
		this.LocationY = locationY;
		this.LeftCornerProbability = 0.25;
		this.RightCornerProbability = 0.25;
		this.UpCornerProbability = 0.25;
		this.DownCornerProbability = 0.25;
		this.cornerProbabilities = new double[4];
	}
}

