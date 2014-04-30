using UnityEngine;
using System.Collections;

/*
 * Represents a partile used for the Particle Filter Estimator
 */
public class Particle {
	/* Direction particle is moving */
	public int MovingDirection { get; set; }

	/* x coordinate in GameMap GameMapArray */
	public int CurrentLocationX { get; set; }

	/* y coordinate in GameMap GameMapArray */
	public int CurrentLocationY { get; set; }

	/* Value given based on if PlayerAgent or RatAgent created particle */
	public int ParticleValue { get; set; }

	/* Number of game steps until partile dies */
	public int TimeToLive { get; set; }

	/* Checks whether particle was created during current game step */
	public bool JustCreated { get; set; }

	public ulong UniqueId { get; set; }

	public bool ParticleRemoved { get; set; }

	public Particle(int MovingDirection, int CurrentLocationX, int CurrentLocationY,
	                int ParticleValue, int TimeToLive, bool JustCreated, ulong id){

		this.MovingDirection = MovingDirection;
		this.CurrentLocationX = CurrentLocationX;
		this.CurrentLocationY = CurrentLocationY;
		this.ParticleValue = ParticleValue;
		this.TimeToLive = TimeToLive;
		this.JustCreated = JustCreated;
		this.UniqueId = id;
	}

}
