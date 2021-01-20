using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponProjectile : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject muzzlePrefab;
    public GameObject impactEffectPrefab;

    [Header("Transforms")]
    public Transform firePoint;

    [Header("Camera")]
    public Camera fpsMainCamera;

    public bool isAutoFireEnabled = false;

    [Header("Floats")]
    public float fireRate = 0.05f;
    public float singleFireRate = 0.3f;
    public float reloadTime = 2.0f;
    public float range = 100.0f;
    public float bulletSpeed = 50.0f;
    public float spread = 0.2f;
    public float bulletForce = 80f;
    public float upwardForce = 2f;

    [Header("Integers")]
    public int magzineAmmo = 30;
    public int maxAmmo = 180;
    public int damage = 38;

    [Header("UI Details")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI autoFireText;
    public TextMeshProUGUI reloadingText;

    [Header("Aniamtions")]
    public Animator gunAnimator;

    private bool isReloading;
    private bool canShootNext;
    
    private int currentMaxAmmo;
    private int currentAmmo;

    private int isShootingHash;
    
    private float someTime;

    private Rigidbody weaponHolderRBody;

    private void Start()
    {
        Initilization();
        isShootingHash = Animator.StringToHash("isShooting");
    }

    private void Initilization()
    {
        //Rigidbodiy
        weaponHolderRBody = GetComponent<Rigidbody>();

        //Numbers
        currentAmmo = magzineAmmo;
        currentMaxAmmo = maxAmmo;
        someTime = 0.0f;

        //bools
        isReloading = false;
        canShootNext = true;
        ammoText.text = currentAmmo.ToString() + '/' + currentMaxAmmo.ToString();
        autoFireText.text = "Auto Fire Enabled";
        reloadingText.text = "Not Reloading";
    }

    private void Update()
    {
        InputFromUser();
        UpdatingUIDetails();
    }

    private void InputFromUser()
    {
        if (!isAutoFireEnabled && Input.GetButtonDown("Fire1") && currentAmmo != 0 && !isReloading)
        {
            Shoot();
        }
        if (isAutoFireEnabled && Input.GetButton("Fire1") && currentAmmo != 0 && canShootNext && !isReloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != magzineAmmo && !isReloading && currentMaxAmmo >= 0)
        {
            StartCoroutine(Reload());
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            isAutoFireEnabled = !isAutoFireEnabled;
        }
    }

    private void Shoot()
    {
        //Debug.Log("Shooting");

        Ray ray = fpsMainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // From the middle of viewport
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }

        Vector3 directionWithoutSpread = targetPoint - firePoint.position;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 directionWithSpead = directionWithoutSpread + new Vector3(xSpread, ySpread, 0.0f);

        GameObject currentBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpead.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpead.normalized * bulletForce, ForceMode.Impulse);
        muzzlePrefab.SetActive(true);

        gunAnimator.SetBool(isShootingHash, true);

        if (isAutoFireEnabled)
        {
            StartCoroutine(WaitBeforeNextShot(fireRate));
        }
        else
        {
            StartCoroutine(WaitBeforeNextShot(singleFireRate));
        }
        
        currentAmmo--;
    }

    IEnumerator WaitBeforeNextShot(float fireRateAdjusted)
    {
        //Debug.Log("Waiting");
        canShootNext = false;
        
        yield return new WaitForSeconds(fireRateAdjusted);

        gunAnimator.SetBool(isShootingHash, false);
        muzzlePrefab.SetActive(false);
        //Debug.Log("CanShoot");
        canShootNext = true;
    }

    IEnumerator Reload()
    {
        //Debug.Log("Reloading");
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);
        
        //Debug.Log("Reloaded");
        isReloading = false;
        currentAmmo = magzineAmmo;
        currentMaxAmmo -= magzineAmmo;
    }

    private void UpdatingUIDetails()
    {
        //Updating UI 
        ammoText.text = currentAmmo.ToString() + '/' + currentMaxAmmo.ToString();

        if(isAutoFireEnabled)
        {
            autoFireText.text = "Auto Fire Enabled";
        }
        else
        {
            autoFireText.text = "Auto Fire Disabled";
        }
     
        //Reloading
        if(isReloading)
        {
            reloadingText.text = "Reloading";
        }
        else
        {
            reloadingText.text = "Not Reloading";
        }
    }
}
