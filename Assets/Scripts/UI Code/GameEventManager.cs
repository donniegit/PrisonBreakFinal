public static class GameEventManager {

	public delegate void GameEvent();

	public static event GameEvent GameStart, GameOver, GameUpdate;

	public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();
		}
	}

	public static void TriggerGameUpdate(){
		if(GameUpdate != null){
			GameUpdate();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
		}
	}
}