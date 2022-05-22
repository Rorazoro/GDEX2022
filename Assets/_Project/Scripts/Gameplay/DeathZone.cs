﻿using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            Debug.Log($"Player died");
            other.GetComponent<PlayerObject>().TriggerDeath();
        }
    }
}