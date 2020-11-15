/* Author : Raphaël Marczak - 2018/2020, for MIAMI Teaching (IUT Tarbes) and MMI Teaching (IUT Bordeaux Montaigne)
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is used to teleport the player from one scene to another
// If the scene is different, then the previous scene is disabled and the new
// one is enabled ; and the map indication is updated accordingly
public class Teleport : MonoBehaviour
{
    public Image m_map = null;
    public Sprite m_newMap = null;
    public GameObject m_teleportTo = null;

    private GameObject m_player = null;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            TeleportPlayer();
        }

        if (m_newMap != null)
        {
            m_map.sprite = m_newMap;
        }

    }


    private void TeleportPlayer()
    {
        if (m_teleportTo != null)
        {
            if (this.transform.parent != null)
            {
                this.transform.parent.gameObject.SetActive(false);
                m_teleportTo.transform.parent.gameObject.SetActive(true);

                m_player.transform.position = m_teleportTo.transform.position;
            }
        }
    }
}
