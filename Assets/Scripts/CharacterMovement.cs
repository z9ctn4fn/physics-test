using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    private bool jump = false;
    private Transform cam;
    private Transform player;
    private Camera camera;
    private Vector3 worldPosition;
    private GameObject selectedObject;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        player = gameObject.GetComponent<Transform>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        cam.position = new Vector3 (player.position.x, player.position.y, -10f);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        worldPosition = camera.ScreenToWorldPoint(mousePos);
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
        if (hitData && Input.GetMouseButtonDown(0))
        {
            selectedObject = hitData.transform.gameObject;
        }
        if (Input.GetMouseButtonUp(0))
        {
            selectedObject.GetComponent<BoxCollider2D>().enabled = true;
            selectedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            selectedObject = null;
            
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
        if (selectedObject.tag == "Grabbable")
        {
            Transform transform = selectedObject.GetComponent<Transform>();
            transform.position = worldPosition;
            BoxCollider2D collider = selectedObject.GetComponent<BoxCollider2D>();
            collider.enabled = false;
            Rigidbody2D rb = selectedObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;

        }
    }
}
