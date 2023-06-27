using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D _rb;
    private bool playing = false;
    private Vector3 startPosition;

    void Awake()
    {
        playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Player.Space.performed += _ => StartGame();
    }

    private void StartGame()
    {
        playing = !playing;
        if (playing)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Static;
            transform.position = startPosition;
        }
    }
}
