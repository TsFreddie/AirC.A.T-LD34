using UnityEngine;
using System.Collections.Generic;

public class EnemyArranger : MonoBehaviour {

    public GameObject _enemyObj;

    public List<Enemy> List
    {
        get
        {
            return enemies;
        }
    }

    private List<Enemy> enemies;

    void Awake () {
        enemies = new List<Enemy>();
    }
	
	public Enemy CreateEnemy(Enemy.EnemyType type, int health, float speed, bool downward, bool facingRight, int score)
    {
        foreach (Enemy e in enemies)
        {
            if (!e.gameObject.GetComponent<Enemy>().enabled)
            {
                e.Init(type, health, speed, downward, facingRight, score);
                e.gameObject.GetComponent<Enemy>().enabled = true;
                return e;
            }
        }
        GameObject newEnemyObj = Instantiate(_enemyObj);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
        newEnemy.Init(type, health, speed, downward, facingRight, score);
        enemies.Add(newEnemy);
        return newEnemy;
    }
}
