using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsaAnim : MonoBehaviour
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    public bool flipped;
    public float speed;
    Vector3 direction;

    private void Start()
    {
        start = this.transform.position;
        if (flipped == true)
        {
            direction = Vector3.right;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            direction = Vector3.left;
        }
    }

    private void Update()
    {
        if (Mathf.Round(this.transform.position.x) != Mathf.Round(end.x))
        {

            this.transform.Translate(direction * Time.deltaTime * speed);

        }
        else
        {
            this.transform.position = start;
        }
    }

}
