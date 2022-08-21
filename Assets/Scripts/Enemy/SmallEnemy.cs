using System;
using Game;
using UnityEngine;

namespace Enemy
{
    public class SmallEnemy : EnemyBase
    {
        protected override int InitialHealth => 3;

        private void Update()
        {
            if (!GameController.Instance.IsPlaying)
                return;
            
            var playerPos = Player.PlayerMovement.Instance.transform.position;
            playerPos.y = transform.position.y;

            transform.forward = (playerPos - transform.position).normalized;
        }
    }
}
