using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float followSpeed;
    Vector3 lastMousePosition = Vector2.zero;
    Transform player;
    Vector3 prevPlayerPos;

    float limitX = 34f, limitY = 18.5f;

    bool isUsingController = false;

    void Update ()
    {
        Cursor.visible = false;

        // Controller
        if ((Input.GetAxisRaw("AimHorizontal") != 0 || Input.GetAxisRaw("AimVertical") != 0) && (!PauseMenu.isPaused || !Player.isDead))
        {
            isUsingController = true;
            // Follow bullet functionality
            if (ReadWriteSaveManager.Instance.GetData("BulletType", 0, false) == 2)
            {
                Vector2 moveVelocity = new Vector2(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical")) * followSpeed;
                transform.Translate(moveVelocity);
            }
            else
            {
                transform.position = player.position + new Vector3(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical"), 0f).normalized * offset;
            }
        }
        // When controller is used, but analog stick is not in use
        else if (isUsingController && !Player.isDead)
        {
            Vector3 playerOffset = player.position - prevPlayerPos;
            transform.position += playerOffset;
        }

        // Mouse and keyboard
        if (lastMousePosition != Input.mousePosition)
        {
            isUsingController = false;
            lastMousePosition = Input.mousePosition;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (player != null) prevPlayerPos = player.position;
        
        // Ensures crosshair does not go off-screen
        Vector2 pos = transform.position;
        if (pos.x > limitX) transform.position = new Vector2(limitX, pos.y);
        if (pos.x < -limitX) transform.position = new Vector2(-limitX, pos.y);
        if (pos.y > limitY) transform.position = new Vector2(pos.x, limitY);
        if (pos.y < -limitY) transform.position = new Vector2(pos.x, -limitY);

        transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
    }

    public void PlayerRef(Transform obj)
    {
        player = obj;
    }
}
