using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState =PlayerState.Idle;

    private void Start()
    {
        ChangeState(PlayerState.Idle); // Başlangıç durumunu Idle olarak ayarla
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState) { return; } // Aynı durumdaysa değişiklik yapma
        {
            _currentPlayerState = newPlayerState; // Eski durumu devre dışı bırak
        }

    }


    public PlayerState GetCurrentState()
    {
        return _currentPlayerState;
    }
}
