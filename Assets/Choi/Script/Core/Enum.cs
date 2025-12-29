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
        StageClear            
    }

    public enum DeathCause
    {
        None,
        EnemyA,
        EnemyB,
        Fall,
        Trap,
        Laser,
    }
}

