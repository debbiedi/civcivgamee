using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private StateController _stateController;

    [Header("Animation State Names (Animator İçindeki İsimler)")]
    [SerializeField] private string _idleAnimName = "Idle";
    [SerializeField] private string _moveAnimName = "Move";
    [SerializeField] private string _jumpAnimName = "PlayerJump";
    [SerializeField] private string _slideStartAnimName = "PlayerSlideStart";
    [SerializeField] private string _slideIdleAnimName = "PlayerSlideIdle";
    [SerializeField] private string _slideAnimName = "PlayerSlide";
    
    [Header("Settings")]
    [SerializeField] private float _transitionDuration = 0.15f; // Yumuşak geçiş süresi

    private PlayerState _lastState;

    private void Awake()
    {
        // Eğer Inspector'dan atanmamışsa otomatik bulmaya çalış
        if (_stateController == null) _stateController = GetComponent<StateController>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (_stateController != null)
        {
            _lastState = _stateController.GetCurrentState();
            PlayAnimationForState(_lastState);
        }
    }

    private void Update()
    {
        if (_stateController == null || _animator == null) return;

        // Karakterin şu anki durumunu al
        PlayerState currentState = _stateController.GetCurrentState();

        // Eğer durum değiştiyse yeni animasyona geç
        if (currentState != _lastState)
        {
            PlayAnimationForState(currentState);
            _lastState = currentState;
        }
    }

    private void PlayAnimationForState(PlayerState state)
    {
        string animToPlay = _idleAnimName;

        switch (state)
        {
            case PlayerState.Idle:
                animToPlay = _idleAnimName;
                break;
            case PlayerState.Move:
                animToPlay = _moveAnimName;
                break;
            case PlayerState.Jump:
                animToPlay = _jumpAnimName;
                break;
            case PlayerState.SlideStart:
                animToPlay = _slideStartAnimName;
                break;
            case PlayerState.SlideIdle:
                animToPlay = _slideIdleAnimName;
                break;
            case PlayerState.Slide:
                animToPlay = _slideAnimName;
                break;
        }

        // CrossFade kullanarak oklarla (transition lines) uğraşmadan yumuşak ve direkt geçiş yapıyoruz
        _animator.CrossFade(animToPlay, _transitionDuration);
    }
}
