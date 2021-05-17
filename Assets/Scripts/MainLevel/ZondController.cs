using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZondController : MonoBehaviour
{
    // Editor variables
    public Sprite NoLeftPanelSphere;
    public Sprite NoRightPanelSphere;
    public Sprite NoPanelsSphere;
    public Sprite NoSpherePanels;
    public Sprite NoLeftPanelNoSphere;
    public Sprite NoRightPanelNoSphere;
    public Sprite Bolvanka;

    public GameObject Bar;
    
    public int HealthPoints;
    public int BaseHealthPoints;

    // Private variables
    private GameObject HealthBar;
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private bool IsVisibled = false;
    // Public variables
    public Vector2 force;
    public float Torque; 
    public float speed;
    public float hitTime = 2.5f;

    public GameObject Sphere;
    public GameObject leftPanel;
    public GameObject rightPanel;

    private bool isLeftPanel = true;
    private bool isRightPanel = true;
    private bool isSphere = true;

    void Start()
    {
        BaseHealthPoints = HealthPoints;
        rb2D = GetComponent<Rigidbody2D>();

        //
        // HealthBar INIT
        //
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Bar.GetComponent<HealthBarController>().Target = gameObject;
        Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
        HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        HealthBar.transform.SetParent(Canvas.transform, false);

        //
        // HealthBar INIT
        //
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;
        hitTime += Time.deltaTime;

        //
        // HealthBar changes
        //
        if (HealthBar && hitTime > 2.5f)
        {
            HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
        }
        else if (HealthBar)
        {
            HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
            HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
        }
        //
        // HealthBar changes
        //

        //
        // object forcing
        //
        if (LifeTime < 0.5f)
        {
            rb2D.velocity = force * speed;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(force * speed);
            rb2D.AddTorque(Torque);
        }
        //
        // object forcing
        //
        if (isLeftPanel && leftPanel.GetComponent<PanelController>().PreviousHP <= 0)
        {
            leftPanel.GetComponent<PanelController>().ActivateObject(0.5f);
            isLeftPanel = false;
            ChangeSprite();
        }
        if (isRightPanel && rightPanel.GetComponent<PanelController>().PreviousHP <= 0)
        {
            rightPanel.GetComponent<PanelController>().ActivateObject(0.5f);
            isRightPanel = false;
            ChangeSprite();
        }
        if (isSphere && Sphere && Sphere.GetComponent<SphereController>().PreviousHP <= 0)
        {
            Sphere.GetComponent<SphereController>().ActivateObject(0.5f);
            isSphere = false;
            ChangeSprite();
        }
        //
        // Destroying
        //
        if (HealthPoints <= 0)
           DestroyController.DestroyZond(gameObject);
        //
        // Destroying
        //

        //
        // Teleporting
        //
        if (gameObject.GetComponent<Renderer>().isVisible)
            IsVisibled = true;
        if (!gameObject.GetComponent<Renderer>().isVisible && IsVisibled)
            GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TeleportObject(gameObject);
        //
        // Teleporting
        //
    }
    private void ChangeSprite()
    {
        if (!isLeftPanel && isRightPanel && isSphere)
            GetComponent<SpriteRenderer>().sprite = NoLeftPanelSphere;
        else if (isLeftPanel && !isRightPanel && isSphere)
            GetComponent<SpriteRenderer>().sprite = NoRightPanelSphere;           
        else if (!isLeftPanel && !isRightPanel && isSphere)
            GetComponent<SpriteRenderer>().sprite = NoPanelsSphere;
        else if (isLeftPanel && isRightPanel && !isSphere)
            GetComponent<SpriteRenderer>().sprite = NoSpherePanels;
        else if (!isLeftPanel && isRightPanel && !isSphere)
            GetComponent<SpriteRenderer>().sprite = NoLeftPanelNoSphere;
        else if (isLeftPanel && !isRightPanel && !isSphere)
            GetComponent<SpriteRenderer>().sprite = NoRightPanelNoSphere;
        else if (!isLeftPanel && !isRightPanel && !isSphere)
            GetComponent<SpriteRenderer>().sprite = Bolvanka;
    }
}