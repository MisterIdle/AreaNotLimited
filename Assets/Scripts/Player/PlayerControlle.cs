using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum WeaponType
    {
        Default,
        Red,
        Yellow,
        Green
    }

    public float speedPlayer = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;

    private int[] ammunitionCounts = new int[4];
    private bool isReloading = false;
    public float reloadTime = 1.5f;

    private float shootDelay = 0.5f;
    private float lastShootTime;

    private int inventoryNum = 0;

    public GameObject posGun;
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject dashArmor;

    private Rigidbody2D rb;

    private Vector2 move;

    private WeaponType currentWeapon = WeaponType.Default;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastShootTime = -shootDelay;

        for (int i = 0; i < ammunitionCounts.Length; i++)
        {
            ammunitionCounts[i] = 10;
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        LookAtMouse();
        HandleWeaponChange();

        if (Input.GetButton("Fire1") && Time.time > lastShootTime + shootDelay)
        {
            if (ammunitionCounts[(int)currentWeapon] > 0)
            {
                Shoot();
                StartCoroutine(ScreenShake(0.20f, 0.03f));
                lastShootTime = Time.time;
                ammunitionCounts[(int)currentWeapon]--;
                Debug.Log("Mun: " + ammunitionCounts[(int)currentWeapon]);
            }
            else
            {
                Debug.Log("R");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + move.normalized * (isDashing ? dashSpeed : speedPlayer) * Time.fixedDeltaTime);
        dashArmor.GetComponent<SpriteRenderer>().color = isDashing ? Color.red : Color.white;
    }

    private void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, posGun.transform.position, transform.rotation);
    }

    private void HandleWeaponChange()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel > 0f)
        {
            inventoryNum++;
            Debug.Log(inventoryNum);
        }
        else if (mouseWheel < 0f)
        {
            inventoryNum--;
            Debug.Log(inventoryNum);
        }

        if (inventoryNum < 0)
        {
            inventoryNum = 3;
        }

        if (inventoryNum > 3)
        {
            inventoryNum = 0;
        }

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        switch (inventoryNum)
        {
            case 0:
                SetWeapon(WeaponType.Default, Color.white, 0.5f);
                break;
            case 1:
                SetWeapon(WeaponType.Red, Color.red, 0.2f);
                break;
            case 2:
                SetWeapon(WeaponType.Yellow, Color.yellow, 0.5f);
                break;
            case 3:
                SetWeapon(WeaponType.Green, Color.green, 1f);
                break;
        }
    }

    private void SetWeapon(WeaponType newWeapon, Color color, float newShootDelay)
    {
        currentWeapon = newWeapon;
        gun.GetComponent<SpriteRenderer>().color = color;
        shootDelay = newShootDelay;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Rechargement");

        yield return new WaitForSeconds(reloadTime);

        ammunitionCounts[inventoryNum] = 10;

        Debug.Log("Mun: " + ammunitionCounts[inventoryNum]);

        isReloading = false;
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}
