namespace Choi
{
    public enum GameState
    {
        Ready,
        Playing,
        Paused,
        GameOverCutscene,
        GameOver,
        StageClearCutscene,   
        StageClear,
        Die
    }

    public enum DeathCause
    {
        None,
        Enemy,
        Fall,
        Obstacle
    }
}

