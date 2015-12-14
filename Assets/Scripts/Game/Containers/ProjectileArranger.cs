using UnityEngine;
using System.Collections.Generic;

public class ProjectileArranger : MonoBehaviour {

    public GameObject _projectileObj;

    public List<Projectile> List
    {
        get
        {
            return projectiles;
        }
    }

    private List<Projectile> projectiles;

    void Awake()
    {
        projectiles = new List<Projectile>();
    }

    public Projectile CreateProjectile(Projectile.ProjectileType type, int damage, float speed, bool right, bool isDangerous)
    {
        foreach (Projectile p in projectiles)
        {
            if (!p.gameObject.activeSelf)
            {
                p.Init(type, damage, speed, right, isDangerous);
                p.gameObject.SetActive(true);
                return p;
            }
        }
        GameObject newProjectileObj = Instantiate(_projectileObj);
        Projectile newProjectile = newProjectileObj.GetComponent<Projectile>();
        newProjectile.Init(type, damage, speed, right, isDangerous);
        projectiles.Add(newProjectile);
        return newProjectile;
    }
}
