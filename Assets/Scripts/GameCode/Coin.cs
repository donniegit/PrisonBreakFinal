using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
 * Represents a Coin that a PlayerAgent can collect
 */
public class Coin {
	/* x coordinate in GameMap GameMapArray */
	public int LocationX { get; set; }

	/* y coordinate in GameMap GameMapArray */
	public int LocationY { get; set; }

	/* Tracks whether or not coin has been collected by PlayerAgent */
	public bool Collected { get; set; }

	/* Tracks whether or not coin has been removed from 3D world */
	public bool RemovedFromMap { get; set; }
}

