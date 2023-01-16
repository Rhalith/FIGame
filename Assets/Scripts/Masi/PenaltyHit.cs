using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Masi
{
    public class PenaltyHit : MonoBehaviour
    {
        [SerializeField] Penalty _penalty;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollision(collision);
        }

        private void CheckCollision(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CheckPenalty(collision);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void CheckPenalty(Collider2D collision)
        {
            if (_penalty.Equals(Penalty.FiveSeconds))
            {
                collision.GetComponent<PlayerSpecs>().DamagePlayer(5f);
            }
            else
            {
                collision.GetComponent<PlayerSpecs>().DamagePlayer(10f);
            }
        }
    }

    enum Penalty
    {
        FiveSeconds,
        TenSeconds
    }
}