using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    Rigidbody rb;

    [Header("Movement")]
    //[SerializeField] private float speed = 7f;
    [SerializeField] private Joystick joystick;
    private float horiInput, verInput;
    private Vector3 movement;

    [Header("Camera")]
    [SerializeField] private Transform cameraPivot; // Them reference den pivot camera

    [Header("Shooting")]
    [SerializeField] private Image crosshair;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float damage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //crosshair.rectTransform.position = new Vector3(Screen.width / 2 + 75f, Screen.height / 2 + 25f, 0);
        InitialHP();
    }

    // Update is called once per frame
    void Update()
    {
        HandlerMove();
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    private void HandlerMove()
    {
        /*horiInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");*/
        horiInput = joystick.Horizontal;
        verInput = joystick.Vertical;
        movement = new Vector3(horiInput, 0, verInput);

        if (movement.magnitude > 1) movement.Normalize();

        bool verMore = Mathf.Abs(verInput) > Mathf.Abs(horiInput);
        animator.SetBool("Up", verInput > 0 && verMore);
        animator.SetBool("Down", verInput < 0 && verMore);
        animator.SetBool("Left", horiInput < 0 && !verMore);
        animator.SetBool("Right", horiInput > 0 && !verMore);

        // move player theo dir camera (chi truc XZ)
        if (movement.magnitude > 0)
        {
            Vector3 moveDirection = cameraPivot.TransformDirection(movement);
            moveDirection.y = 0;
            rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

            // Tu dong xoay player theo dir move
            //transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    public void Shoot()
    {
        SoundManager17.Instance.PlaySound(5);
        if (muzzleFlash) muzzleFlash.Play();

        // Get shoot direction from camera center
        Ray ray = Camera.main.ScreenPointToRay(crosshair.rectTransform.position);
        RaycastHit hit;
        int ignoreLayerMask = ~LayerMask.GetMask("TriggerZone");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreLayerMask))
        {
            //Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(damage);
                Debug.DrawRay(firePoint.position, hit.point - firePoint.position, Color.red, 2f);
                Debug.DrawRay(Camera.main.transform.position, hit.normal, Color.blue, 20f);
            }

            GameObject effectObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effectObj, 2f);
        }
    }

    protected override void Die()
    {
        base.Die();
        tag = "Untagged";
        GameManager17.Instance.GameLose();
    }
}
