using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 3;
    private bool canDamage = true;
    private float immuneTimer = 0.5f;

    void Update()
    {
        if(playerHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.CompareTag("Trap") && canDamage == true)
        {
            playerHealth = playerHealth - 1;
            canDamage = false;
            StartCoroutine(DamageImmune());
            Debug.Log("You were hit!");
        }
        if(hit.gameObject.CompareTag("Coin"))
        {
            Debug.Log("You won!");
        }
    }

    private IEnumerator DamageImmune()
    {
        yield return new WaitForSeconds(immuneTimer);
        canDamage = true;
    }
}
