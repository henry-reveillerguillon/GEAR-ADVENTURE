﻿/* Author : Raphaël Marczak - 2018/2020, for MIAMI Teaching (IUT Tarbes) and MMI Teaching (IUT Bordeaux Montaigne)
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Represents the cardinal directions (South, North, West, East)
public enum CardinalDirections { CARDINAL_S, CARDINAL_N, CARDINAL_W, CARDINAL_E };

public class PlayerBehavior : MonoBehaviour
{
    public float m_speed = 1f; // Speed of the player when he moves
    private CardinalDirections m_direction; // Current facing direction of the player

    public Sprite m_frontSprite = null;
    public Sprite m_leftSprite = null;
    public Sprite m_rightSprite = null;
    public Sprite m_backSprite = null;

    private string currentAnswer = "000";
    private string codehacking = "false";
    private string winAnswer = "715";
    public GameObject teleportWhenWin = null;
    public GameObject m_fireBall = null; // Object the player can shoot
    
    public GameObject m_map = null;
    public GameObject m_tableau = null;
    public DialogManager m_dialogDisplayer;
    public GameObject m_compagnon = null;
    public GameObject doorToDestroy = null;
    public GameObject CompagnonAppear = null;
    public GameObject dialogcompagnon = null;
    public GameObject porte = null;
    private int nbCollectable = 0;


    public AudioClip m_mapSound = null;
    private Dialog m_closestNPCDialog;

    Rigidbody2D m_rb2D;
    SpriteRenderer m_renderer;

    void Awake()
    {
        m_rb2D = gameObject.GetComponent<Rigidbody2D>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();

        m_closestNPCDialog = null;
    }

    // This update is called at a very precise and constant FPS, and
    // must be used for physics modification
    // (i.e. anything related with a RigidBody)
    void FixedUpdate()
    {
        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_map.activeSelf)
        {
            return;
        }

        // Moves the player regarding the inputs
        Move();
        
        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_tableau.activeSelf)
        {
            return;
        }

        if (nbCollectable == 3)
        {
            m_compagnon.SetActive(true); 
            dialogcompagnon.SetActive(true);
            doorToDestroy.SetActive(false);
            porte.SetActive(false);
        }

        // Moves the player regarding the inputs
        Move();
    }
    
    private void Move()
    {
        float horizontalOffset = Input.GetAxis("Horizontal");
        float verticalOffset = Input.GetAxis("Vertical");

        // Translates the player to a new position, at a given speed.
        Vector2 newPos = new Vector2(transform.position.x + horizontalOffset * m_speed,
                                     transform.position.y + verticalOffset * m_speed);
        m_rb2D.MovePosition(newPos);

        // Computes the player main direction (North, Sound, East, West)
        if (Mathf.Abs(horizontalOffset) > Mathf.Abs(verticalOffset))
        {
            if (horizontalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_E;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_W;
            }
        }
        else if (Mathf.Abs(horizontalOffset) < Mathf.Abs(verticalOffset))
        {
            if (verticalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_N;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_S;
            }
        }
    }


    // This update is called at the FPS which can be fluctuating
    // and should be called for any regular actions not based on
    // physics (i.e. everything not related to RigidBody)
    private void Update()
    {

        // If the player presses M, the map will be activated if not on screen
        // or desactivated if already on screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.instance.PlaySound(m_mapSound);
            m_map.SetActive(!m_map.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            m_tableau.SetActive(!m_tableau.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated

        if (m_dialogDisplayer.IsOnScreen() || m_tableau.activeSelf)
        {
            return;
        }

        ChangeSpriteToMatchDirection();

        // If the player presses SPACE, then two solution
        // - If there is a dialog ready to be displayed (i.e. the player is closed to a NPC)
        //   then a dialog is set to the dialogManager
        // - If not, then the player will shoot a fireball
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_closestNPCDialog != null)
            {
                m_dialogDisplayer.SetDialog(m_closestNPCDialog.GetDialog());
            }
            else 
            {
                ShootFireball();
            }
        }
        if (codehacking == "false")
        {
            CheckAnswer();
        }
    }

      

// Changes the player sprite regarding it position
// (back when going North, front when going south, right when going east, left when going west)
private void ChangeSpriteToMatchDirection()
    {
        if (m_direction == CardinalDirections.CARDINAL_N)
        {
            m_renderer.sprite = m_backSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_S)
        {
            m_renderer.sprite = m_frontSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_E)
        {
            m_renderer.sprite = m_rightSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_W)
        {
            m_renderer.sprite = m_leftSprite;
        }
    }

    // Creates a fireball, and launches it
    private void ShootFireball()
    {
        GameObject newFireball = Instantiate(m_fireBall, this.transform) as GameObject;

        FireBehavior fireBallBehavior = newFireball.GetComponent<FireBehavior>();

        if (fireBallBehavior != null)
        {
            // Lauches the fireball upward
            // (Vector2 represents a direction in x and y ;
            // so Vector2(0f, 1f) is a direction of 0 in x and 1 in y (up)
            if (m_direction == CardinalDirections.CARDINAL_N)
            {
                fireBallBehavior.Launch(new Vector2(0f, 1f));
            }
            if (m_direction == CardinalDirections.CARDINAL_E)
            {
                fireBallBehavior.Launch(new Vector3(1f, 0f));
            }
            if (m_direction == CardinalDirections.CARDINAL_W)
            {
                fireBallBehavior.Launch(new Vector4(-1f, 0f));
            }
            if (m_direction == CardinalDirections.CARDINAL_S)
            {
                fireBallBehavior.Launch(new Vector2(0f, -1f));
            }
        }
    }


    // This is automatically called by Unity when the gameObject (here the player)
    // enters a trigger zone. Here, two solutions
    // - the player is in an NPC zone, then he grabs the dialog information ready to be
    //   displayed when SPACE will be pressed
    // - the player is in an instantDialog zone, then he grabs the dialog information and
    //   displays it instantaneously
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = collision.GetComponent<Dialog>();
        }
        else if (collision.tag == "InstantDialog")
        {
            Dialog instantDialog = collision.GetComponent<Dialog>();
            if (instantDialog != null)
            {
                m_dialogDisplayer.SetDialog(instantDialog.GetDialog());
            }
        }
        else if (collision.tag == "CompagnonAppear")
        {
            Dialog CompagnonAppear = collision.GetComponent<Dialog>();
            if (CompagnonAppear != null)
            {
                m_dialogDisplayer.SetDialog(CompagnonAppear.GetDialog());
            }
        }
    }

    // This is automatically called by Unity when the gameObject (here the player)
    // leaves a trigger zone. Here, two solutions
    // - the player was in an NPC zone, then the dialog information is removed
    // - the player was in an instantDialog zone, then the instantDialog is destroyed
    //   (as it has been displayed, and must only be displayed once)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = null;
        }
        else if (collision.tag == "InstantDialog")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            nbCollectable++;
        }
    }
    private void CheckAnswer()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentAnswer += "1";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentAnswer += "2";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentAnswer += "3";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            currentAnswer += "4";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            currentAnswer += "5";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            currentAnswer += "6";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            currentAnswer += "7";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            currentAnswer += "8";
            Debug.Log(currentAnswer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            currentAnswer += "9";
            Debug.Log(currentAnswer);
        }

        if (currentAnswer.Substring(currentAnswer.Length - winAnswer.Length, winAnswer.Length) == winAnswer)
        {
            codehacking = "true";
            Debug.Log("code bon");
            currentAnswer = "000";
            teleportWhenWin.transform.parent.gameObject.SetActive(true);

            this.transform.position = teleportWhenWin.transform.position;
        }
    }
}
