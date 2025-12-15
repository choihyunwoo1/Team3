using UnityEngine;
using Choi;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private MonoBehaviour stateProvider;

    private IGameStateProvider gameState;
    private bool isConsumed;

    protected virtual void Awake()
    {
        gameState = stateProvider as IGameStateProvider;

        if (gameState == null)
        {
            Debug.LogError(
                $"{name} : stateProvider does not implement IGameStateProvider",
                this
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isConsumed)
            return;

        if (gameState != null &&
            gameState.CurrentState != GameState.Playing)
            return;

        Player player = collision.GetComponent<Player>();
        if (player == null)
            return;

        if (PickUp(player))
        {
            isConsumed = true;
            Destroy(gameObject);
        }
    }

    protected abstract bool PickUp(Player player);
}
