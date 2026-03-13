using UnityEngine;

public class PlayerInteractionsController : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        // PlayerController scriptini başlangıçta yakalıyoruz ki sonradan özellikleri değiştirebilelim
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tag (Etiket) kontrolü ile objeleri yakalıyoruz
        if (other.CompareTag("GoldWheat"))
        {
            Debug.Log("Altın Buğday toplandı! 5 saniyeliğine hızlandın!");
            // Speed'i 2 katına çıkarıyoruz, etki 5 saniye sürüyor
            if (_playerController != null) _playerController.MultiplySpeed(2f, 5f);
            
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HolyWheat"))
        {
            Debug.Log("Kutsal Buğday toplandı! 5 saniyeliğine yükseğe zıplayabilirsin!");
            // JumpForce'u 1.5 katına çıkarıyoruz, etki 5 saniye sürüyor
            if (_playerController != null) _playerController.MultiplyJumpForce(1.5f, 5f);
            
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RottenWheat"))
        {
            Debug.Log("Çürük Buğday toplandı! 5 saniyeliğine yavaşladın!");
            // Speed'i yarıya (0.5) indiriyoruz, etki 5 saniye sürüyor
            if (_playerController != null) _playerController.MultiplySpeed(0.5f, 5f);
            
            Destroy(other.gameObject);
        }
    }
}
