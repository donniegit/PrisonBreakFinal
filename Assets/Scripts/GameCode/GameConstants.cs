using UnityEngine;
using System.Collections;

/*
 * Game constants
 * 
 * TODO: update and tweak constants
 */
public class GameConstants {
	/* Left direction */
	public const int Left = 0;

	/* Down direction */
	public const int Down = 1;

	/* Right direction */
	public const int Right = 2;

	/* Up direction */
	public const int Up = 3;

	/* Id that uniquely identifies CellmateAgent */
	public const int CellmateAgentId = 4;

	/* Id that uniquely identifies PlayerAgent */
	public const int PlayerAgentId = 5;

	/* Id that uniquely identifies GuardAgent */
	public const int GuardAgentId = 6;

	/* Id that uniquely identifies RatAgent */
	public const int RatAgentId = 7;

	/* Distance that guard can move from their current location */
	public const int GuardSearchDistancePixels = 10;

	/* Distance that guard can catch PlayerAgent */
	public const int PlayerCaughtDistancePixels = 5;

	/* Number of pixels on 3D map for every one pixel in 2D GameMap */
	public const int MapScalePixels = 10;

	/* Size of 2D GameMap */
	public const int SizeOfMapArrayPixels = 10000;

	/* Width of 2D GameMap */
	public const int MapWidthPixels = 100;

	/* Height of 2D GameMap */
	public const int MapHeightPixels = 100;

	/* Width of 2D GameMap turning floor cell */
	public const int TurningFloorCellWidthPixels = 7;

	/* Height of 2D GameMap turning floor cell */
	public const int TurningFloorCellHeightPixels = 7;

	/* Initial number of time stemps for particle to live */
	public const int ParticleLifeSpan = 50;

	/* Particle value when RatAgent steps on floor sensor */
	public const int RatAgentParticleValue = 5;

	/* IParticle value when PlayerAgent steps on floor sensor */
	public const int PlayerAgentParticleValue = 50;

	/* Floor value from Map Array */
	public const int MapHallwayFloorcell = 0;

	/* Wall value from Map Array */
	public const int MapWallFloorcell = 1;

	/* Exit value from Map Array */
	public const int MapExitFloorcell = 2;

	/* Cell Entrance value from Map Array */
	public const int MapEntranceFloorcell = 3;

	/* Turning cell value from Map Array */
	public const int MapTurningFloorcell = 4;

	/* Cell Space value from Map Array */
	public const int MapPrisonFloorcell = 5;

	/* Distance guards start from player */
	public const int GuardStartDistanceFromPlayer = 20;

	/* Number of hallway floorcell pixels in GameMap */
	public const int NumberOfHallwayFloorTiles = 4930;

	/* Number of game steps to skip before printing GameDebugger */
	public const int GameDebuggerGameSteps = 5;

	/* Turns GameDebugger on and off */
	public const bool GameDebuggerOn = true;

	/* GuardAgent is same speed as PlayerAgent */
	public const int GuardSpeedMedium = 0;

	/* GuardAgent speed is faster than PlayerAgent */
	public const int GuardSpeedFast = 1;

	/* GuardAgent speed is slower than PlayerAgent */
	public const int GuardSpeedSlow = -1;

	/*  starting range to listen for footsteps behind PlayerAgent */
	public const int FootstepSensorBehindPixels = 10;

	/*  starting range to listen for footsteps in front of PlayerAgent */
	public const int FootstepSensorFrontPixels = 10;	

	/*XLocation for the exit.*/
	public const int exitX = 1;

	/*Y Location for the exit*/
	public const int exitY = 54;

	/*It is this rate at which the code explores.*/
	public const double explorationRate = 0.7;

	/* Radius of particle filter area */
	public const int ParticleFilterRadius = 15;
}
