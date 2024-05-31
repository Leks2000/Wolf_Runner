using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(70 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().PlaySound("Coin");
            PlayerManager.numberOfCoins += 1;
            Destroy(gameObject);
        }
    }
}
