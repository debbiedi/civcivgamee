using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerVisuals;
    [SerializeField] private Rigidbody _rb;

    private void Update()
    {
        if (_orientation == null || _player == null) return;

        // Kameranın baktığı yönü hesapla
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        
        // Orientation objesini kameranın baktığı yöne eşitliyoruz (Sadece Y ekseninde döndürür)
        _orientation.forward = viewDir.normalized;
    }
}

