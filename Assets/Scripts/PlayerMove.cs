using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float shootDelay = 0.5f;
    private float lastShootTime;
    public bool canShoot = true;

    private Rigidbody2D rb;

    public GameObject posGun;
    public GameObject gun;
    public GameObject bulletPrefab;

    private Vector2 move;
    private Vector3 originalGunPosition;

    public float gunRecoilDistance = 0.2f;
    public float recoilSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastShootTime = -shootDelay;
        originalGunPosition = gun.transform.localPosition;
    }

    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        RotateTowardsMouse();

        if (Input.GetButton("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

        if (Time.time - lastShootTime > shootDelay)
        {
            canShoot = true;
        }

        // Lerp pour le recul du pistolet
        gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, originalGunPosition - new Vector3(0f, 0f, gunRecoilDistance), recoilSpeed * Time.deltaTime);
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, posGun.transform.position, transform.rotation);

        // Ne pas oublier de réinitialiser canShoot et lastShootTime ici
        canShoot = false;
        lastShootTime = Time.time;
    }
}
