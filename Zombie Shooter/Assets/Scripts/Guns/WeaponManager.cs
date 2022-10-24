using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Player player;

    public Camera playerCamera;

    public GameObject bulletHolePrefab;
    public GameObject bulletContainer;
    public ParticleSystem muzzleFlash;

    public int currentTemp = 0;
    public bool overheated = false;

    public int selectedWeaponIndex = 0;

    public List<Weapon> weapons = new List<Weapon>();

    private Weapon currentWeapon
    {
        get => weapons[selectedWeaponIndex];
    }

    private float nextFire = 0.0f;
    private float nextCooldown = 0.0f;

    private float horizontalRecoilRandomModifier = 0.5f;
    private float verticalRecoilRandomModifier = 1.0f;

    public void Update()
    {
        if (!GameManager.Started || GameManager.Paused)
            return;

        if (Time.time > nextFire && Input.GetMouseButton(0) && !overheated)
        {
            Shoot();
            nextFire = Time.time + (100f / currentWeapon.fireRate);
        }

        if (Time.time > nextCooldown && !Input.GetMouseButton(0) || Time.time > nextCooldown && overheated)
        {
            Cooldown();
            nextCooldown = Time.time + (100f / currentWeapon.cooldownRate);
        }

        if (Input.GetMouseButtonDown(1))
            playerCamera.fieldOfView = 10f;

        if (Input.GetMouseButtonUp(1))
            playerCamera.fieldOfView = 45f;

        if (Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(0) && !overheated)
            NextWeapon();

        if (Input.GetKeyDown(KeyCode.Q) && !Input.GetMouseButton(0) && !overheated)
            PreviousWeapon();

        if (currentTemp > currentWeapon.maxTemp)
            overheated = true;

        HudHelper.SetHeatBar((float)currentTemp / (float)currentWeapon.maxTemp);
        HudHelper.SetGunName(currentWeapon.Name);
    }

    public void ResetManager()
    {
        currentTemp = 0;
        overheated = false;

        selectedWeaponIndex = 0;

        foreach (Transform bulletHole in bulletContainer.transform)
            Destroy(bulletHole.gameObject);

        nextFire = 0.0f;
        nextCooldown = 0.0f;
    }

    public void Shoot()
    {
        currentTemp++;

        Recoil();

        muzzleFlash.Play();

        RaycastHit hitInfo;

        if (Physics.Raycast(player.transform.position, transform.forward, out hitInfo))
        {
            if (hitInfo.collider.tag == "Enemy")
            {
                var enemy = hitInfo.transform.GetComponent<Enemy>();
                
                if (enemy.alive)
                    enemy.Damage(currentWeapon.damage);
            }
            else
            {
                var bulletHole = Instantiate(bulletHolePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal), bulletContainer.transform);
                bulletHole.transform.position += bulletHole.transform.forward / 1000;
            }
        }
    }

    public void Recoil()
    {
        float recoil = (float)currentWeapon.recoil / 100f;

        float horizontalRecoil = Random.Range((recoil - horizontalRecoilRandomModifier) / 5 * -1, 
                                              (recoil + horizontalRecoilRandomModifier) / 5);
        float verticalRecoil = Random.Range(0, recoil + verticalRecoilRandomModifier);

        player.UpdatePosition(horizontalRecoil, verticalRecoil);
    }

    public void Cooldown()
    {
        if (currentTemp != 0)
            currentTemp--;
        else
            overheated = false;
    }

    public void NextWeapon()
    {
        currentTemp = 0;

        selectedWeaponIndex++;

        if (selectedWeaponIndex > weapons.Count - 1)
            selectedWeaponIndex = 0;
    }

    public void PreviousWeapon() 
    {
        currentTemp = 0;

        selectedWeaponIndex--;

        if (selectedWeaponIndex < 0)
            selectedWeaponIndex = weapons.Count - 1;
    }
}

[System.Serializable]
public class Weapon
{
    public string Name = "Gun";
    public int recoil = 50;
    public int fireRate = 1000;
    public int cooldownRate = 2000;
    public int maxTemp = 100;
    public int damage = 100;
}
