using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class SlingShot : MonoBehaviour 
{
    public LineRenderer[] lineRenderers;
   // [SerializeField] LineRenderer line;
    public  Transform[] stripPosition;
    public Transform centre;
    public Transform idlePosition;
    [SerializeField] Transform slingShot;
    Vector3 currentPosition;
    public float maxLength;
    public float bottomBoundary;
    public GameObject prefab;
    Rigidbody2D ball;
    Collider2D ballCollider;
    public float ballPositionOffset;
    public float force;
    public static SlingShot instance;
    public GameObject ballRespawn;
    CinemachineVirtualCamera cvc;
    public GameObject pointsPreFab;
    public GameObject[] points;
    public int numberOfPoints;
    Vector3 ballForce;
   [SerializeField] LineRenderer line;
    bool isBallCreated;
    [SerializeField] Animator sling;
    private DottedLine.DottedLine DottedLineCont;




    Color aimColor = Color.white;





    bool isMouseDown;

    private void Awake()
    {
        line.GetComponent<LineRenderer>().enabled = false;
    }
    private void Start()
    {

        cvc = FindObjectOfType<CinemachineVirtualCamera>();
        line.SetPosition(0, idlePosition.position);
        instance = this.GetComponent<SlingShot>();
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPosition[0].position);
        lineRenderers[1].SetPosition(0, stripPosition[1].position);
        DottedLineCont = GetComponent<DottedLine.DottedLine>();
        DottedLine.DottedLine.Instance.DrawDottedLine(this.transform.position, this.transform.position);
        // line.SetPosition(0,centre.position);
        CreateBall();
       //points = new GameObject[numberOfPoints];
       // for (int i = 0; i < (numberOfPoints-10); i++)
       // {
       //     points[i] = Instantiate(pointsPreFab, centre.transform.position, Quaternion.identity);
       //     int newI = (numberOfPoints) - i;
       //     Color col = new Color();
       //     col = aimColor;
       //     col.a = col.a - (i * 0.1f);
       //     points[i].GetComponent<SpriteRenderer>().color = col;
       //     points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * (newI * 0.1f);
       // }
    }

    public Rigidbody2D GetBall() {
        if(ball == null)
        {
            CreateBall();
        }
        return ball;
    }

   public void CreateBall()
    {
        if(ball != null) Destroy(ball);
        isBallCreated = true;
        ball = Instantiate(prefab).GetComponent<Rigidbody2D>();
        ball.transform.GetChild(0).transform.GetComponent<TrailRenderer>().enabled = false;
        ball.transform.position = centre.position;
        ballCollider = ball.GetComponent<Collider2D>();
        ballCollider.enabled = false;
        ball.isKinematic = true;
        cvc.Follow = ball.transform;
    }

    private void Update()
    {

        if (isMouseDown)
        {
            ballForce = (currentPosition - centre.position) * force * -1;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = centre.position + Vector3.ClampMagnitude(currentPosition - centre.position, maxLength);
            //currentPosition = clampBoundary(currentPosition);
            line.SetPosition(0, currentPosition);
            line.SetPosition(1, (centre.position +Vector3.Normalize(currentPosition-centre.position) * -Vector3.Distance(currentPosition, centre.position)*1.5f));
            DottedLine.DottedLine.Instance.DrawDottedLine(line.GetPosition(0), line.GetPosition(1));
            SetStrips(currentPosition);
            if (ballCollider)
            {
                ballCollider.enabled = true;
            }
            // line.SetPosition(1,-1*(currentPosition));

        }
        else
        {
            
            ResetStrips();
        }
        //for (int i = 0; i < (numberOfPoints-10); i++)
        //{
        //    points[i].transform.localPosition = pointPosition(i * 0.1f);
        //}
       
    }



    public void shoot()
    {
        if (ball == null) return;
        lineRenderers[1].SetPosition(0, stripPosition[1].position);
        
      
        //Debug.Log(ball);
        //Debug.Log(ball.transform.GetChild(0));
        ball.transform.GetChild(0).transform.GetComponent<TrailRenderer>().enabled = true;
        StartCoroutine(ActivateElectric(ball));
       // ball.transform.GetChild(1).transform.GetComponent<ParticleSystem>().Play();
        FindObjectOfType<AudioManager>().Play("OnShoot");
        sling.Play("SlingShot");
       // if(GameManager.instance.isRoundUiIsDisplayed==true){ballForce=Vector3.zero;}
        Trail.instance.setTrail();
        //Debug.Log(ball);
       
        ball.isKinematic = false;

            ballForce = (currentPosition - centre.position) * force * -1;
            
       // Vector3 ballForce = (currentPosition - centre.position) * force * -1 *1.4f;
        ball.velocity = ballForce;
        // ball.gravityScale=3;
        
        ball = null;
        ballCollider=null;
        //GroundSound.instance.GetComponent<Collider2D>().enabled = false;


        StartCoroutine(BallReload());
    }

    IEnumerator ActivateElectric(Rigidbody2D localBall)
    {
        yield return new WaitForSecondsRealtime(0.25f);
        localBall.transform.GetChild(1).transform.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator BallReload()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ballRespawn.GetComponent<Animator>().Play("Reload");
        //GroundSound.instance.GetComponent<BoxCollider2D>().enabled = false;
    }
     public void ResetStrips()
    {
        if (Respawnball.instance.isReady != true)
        {
            currentPosition = stripPosition[3].position;
        }
        else
        {

             currentPosition = idlePosition.position;
        }
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (ball)
        {

            Vector3 dir = position - centre.position;
            ball.transform.position =position+ dir.normalized * ballPositionOffset;
            ball.transform.right = -dir.normalized;
        }
    }

    Vector3 clampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }

    //Vector2 pointPosition(float t)
    //{
      
    //    //This is from an equation of kinematics of motion.
    //    Vector2 currentPosition = (Vector2)centre.transform.position + ((Vector2)ballForce* t) + (0.5f * Physics2D.gravity * (t * t));
    //    return currentPosition;
    //}

    private void OnMouseDown() {
        Debug.Log(GameManager.instance.isRoundUiIsDisplayed);
        if ( Respawnball.instance.isReady==true)
        {
            sling.Play("MoveBackWard");
            lineRenderers[1].SetPosition(0, stripPosition[2].position);
            line.GetComponent<LineRenderer>().enabled = true;


             isMouseDown = true;
            FindObjectOfType<AudioManager>().Play("Whip");
            Debug.Log("u have pressed");
          //  Respawnball.instance.isReady = false;
        }
            if (currentPosition.x < 0)
            {
                slingShot.Rotate(Vector3.forward * 18);
            }
            else 
            {
                slingShot.Rotate(Vector3.back * 18);
            }


        
    }

    public void OnMouseUp() {

        if (Respawnball.instance.isReady == true)
        {
            Invoke("onGroundCollision", 0.25f);
            slingShot.Rotate(Vector3.back * 18);
            line.GetComponent<LineRenderer>().enabled = false;
            Debug.Log(GameManager.instance.isRoundUiIsDisplayed);

            isMouseDown = false;

            //Invoke("ActivateElectric", 0.5f);
            FindObjectOfType<AudioManager>().Play("OnShoot");
            shoot();
            Respawnball.instance.isReady = false;
        }
       
        //for(int i=0;i<(numberOfPoints-10);i++){
        //    points[i].GetComponent<SpriteRenderer>().enabled=false;
        //}

        //else{
        //    Trail.instance.setTrail();
        //    ball.isKinematic = false;
        //    ballForce = Vector3.down;

        // Vector3 ballForce = (currentPosition - centre.position) * force * -1 *1.4f;
        //Debug.Log(ball);
        //    ball.velocity = ballForce;
        //// ball.gravityScale=3;

        //     ball = null;
        //    ballCollider=null;

       // ballRespawn.GetComponent<Animator>().Play("Reload");
        }
        //StartCoroutine(ActiveProjectionPoints());
        void onGroundCollision()
        {
        GroundSound.instance.GetComponent<BoxCollider2D>().enabled = true;
        }
    }



    //IEnumerator ActiveProjectionPoints(){
    //    yield return new WaitForSecondsRealtime(1);
    //    //for(int i=0;i<(numberOfPoints-10);i++){
    //    //    points[i].GetComponent<SpriteRenderer>().enabled=true;
    //    //}
    //}
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
      
    //}

