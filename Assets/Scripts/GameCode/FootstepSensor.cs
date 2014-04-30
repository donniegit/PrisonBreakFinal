using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* 
 * Gives PlayerAgent virtual sound sensor
 * 
 * TODO: make sure player direction is being set correctly
 * 
 * TODO: find out how xy values are indexed and fix these values here
 */
using System;


class FootstepSensor {
	/* 
	 * Figures out how close a GuardAgent is behind PlayerAgent
	 * 
	 * Returns -1 if there are no guards close. Returns 0 if guard is at PlayerAgent.
	 * Otherwise returns value up to GameConstants.FootstepSensorBehindPixels (distance of closest GuardAgent)
	 */
	public int getSoundBehindPlayer() {
		int pLocX = GameEngine.player.LocationX;
		int pLocY = GameEngine.player.LocationY;

		double cloestGuard = (GameConstants.FootstepSensorBehindPixels + 10);
		double distance = cloestGuard;
		for (int i = 0; i<GameEngine.guards.Count; i++) {
			if (GameEngine.player.MovingDirection == GameConstants.Up && GameEngine.player.LocationY > GameEngine.guards[i].LocationY) { // PlayerAgent facing up
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Down && GameEngine.player.LocationY < GameEngine.guards[i].LocationY) { // PlayerAgent facing down
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Left && GameEngine.player.LocationX < GameEngine.guards[i].LocationX) { // PlayerAgent facing left
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Right && GameEngine.player.LocationX > GameEngine.guards[i].LocationX) { // PlayerAgent facing right
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			}
		}

		if (distance > GameConstants.FootstepSensorBehindPixels) {
			return -1;
		}

		return (int)distance;
	}

	/* 
	 * Figures out how close a GuardAgent is in front of PlayerAgent
	 * 
	 * Returns -1 if there are no guards close. Returns 0 if guard is at PlayerAgent.
	 * Otherwise returns value up to GameConstants.FootstepSensorFrontPixels (distance of closest GuardAgent)
	 */
	public int getSoundInFrontOfPlayer() {
		int pLocX = GameEngine.player.LocationX;
		int pLocY = GameEngine.player.LocationY;
		
		double cloestGuard = (GameConstants.FootstepSensorFrontPixels + 10);
		double distance = cloestGuard;
		for (int i = 0; i<GameEngine.guards.Count; i++) {
			if (GameEngine.player.MovingDirection == GameConstants.Up && GameEngine.player.LocationY < GameEngine.guards[i].LocationY) { // PlayerAgent facing up
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Down && GameEngine.player.LocationY > GameEngine.guards[i].LocationY) { // PlayerAgent facing down
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Left && GameEngine.player.LocationX > GameEngine.guards[i].LocationX) { // PlayerAgent facing left
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			} else if (GameEngine.player.MovingDirection == GameConstants.Right && GameEngine.player.LocationX < GameEngine.guards[i].LocationX) { // PlayerAgent facing right
				distance = (Math.Pow(pLocX-GameEngine.guards[i].LocationX,2)+Math.Pow(pLocY-GameEngine.guards[i].LocationY,2));
				if (distance < cloestGuard) {
					cloestGuard = distance;
				}
			}
		}
		
		if (distance > GameConstants.FootstepSensorFrontPixels) {
			return -1;
		}
		
		return (int)distance;
	}
}

