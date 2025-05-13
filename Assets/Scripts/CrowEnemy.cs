using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEnemy :EnemyBase
{
    private void Start()
    {
        moveSpeed = 3f;
        damage = 1;
        maxHealth = 2;
    }
}
