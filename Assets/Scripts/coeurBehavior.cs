using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class coeurBehavior : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float speed = 1f;
    public int lives  = 15;
    public SpriteRenderer currentSprite;
    public Sprite firstskin;
    public Sprite secondskin;
    public Sprite thirdskin;
    public Sprite fourthskin;
    public Sprite deadskin;

    // Start is called before the first frame update
    void Start()
    {
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        currentSprite.sprite = firstskin;
    }

    // Update is called once per frame
    void Update()
    {
     if (lives <= 3)
     {
         currentSprite.sprite = deadskin;
     }   
     else if (lives <= 6)
     {
         currentSprite.sprite = fourthskin;
     }
     else if (lives <= 9)
     {
         currentSprite.sprite = thirdskin;
     }
     else if (lives <= 12)
     {
         currentSprite.sprite = secondskin;
     }     
     else
     {
         currentSprite.sprite = firstskin;
     }

     Debug.Log(currentSprite);
    }
}
