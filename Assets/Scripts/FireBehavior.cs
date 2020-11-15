/* Author : Raphaël Marczak - 2018/2020, for MIAMI Teaching (IUT Tarbes) and MMI Teaching (IUT Bordeaux Montaigne)
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FireBehavior : MonoBehaviour {
    Rigidbody2D m_rb2D;

    float m_launchedTime; // Absolute time (in sec.) when the fireball has been launched
    float m_fireDuration = 2f; // Time (in sec.) after which the fireball should be destroyed

    float m_speed = 100f; // Speed of the fireball

    void Awake () {
        m_rb2D = gameObject.GetComponent<Rigidbody2D>();
        m_launchedTime = Time.realtimeSinceStartup;
    }

    public void Launch(Vector2 direction)
    {
        m_rb2D.AddForce(direction.normalized * m_speed, ForceMode2D.Impulse);
    }
	
	void Update () {
        // Checks if the fireball should remain on screen
        // or if the life time has been reached
		if (Time.realtimeSinceStartup > m_launchedTime + m_fireDuration)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Porte")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        // Destroys the fireball when it hits something, except the player or another fireball
        // (to prevent the fireball to be destroyed as soon as it is created)
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Fireball")
        {
            Destroy(gameObject);
        } else
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
 
        Debug.Log("titi");
        if (collision.gameObject.tag == "coeur")
        {
            Debug.Log("toto");
            collision.gameObject.GetComponent<coeurBehavior>().lives--;
            if (collision.gameObject.GetComponent<coeurBehavior>().lives == 0)
            {
                Destroy(collision.gameObject);
                SceneManager.LoadScene("Fin");  //la scene de fin
            }
            Destroy(this.gameObject);
        }
        // Destroys the fireball when it hits something, except the player or another fireball
        // (to prevent the fireball to be destroyed as soon as it is created)
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Fireball")
        {
            Destroy(gameObject);
        }
        else
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
