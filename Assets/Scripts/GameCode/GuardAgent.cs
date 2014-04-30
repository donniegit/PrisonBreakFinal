using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
 * Represents Guards that chase PlayerAgent
 */
public class GuardAgent : GameAgent {

	/* Guard speed */
	public int Speed { get; set; }

	public bool Tracking { get; set; }

	public int TrackingValue { get; set; }

	public bool MadeTurnChoice { get; set; }

	public bool OnTurningFloorcell { get; set; }

}

