using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerControl : MonoBehaviour
{
    public static TriggerControl instance = null;
    GameObject triggerObject;

    public Image reloadImage, lifeBar;
    public Text incNumber;
    public ParticleSystem cloud;

    bool meshChange = false;
    public bool isCompleted = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hide"))
        {
            meshChange = true;
            triggerObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hide"))
        {
            meshChange = false;
            triggerObject = other.gameObject;
            reloadImage.fillAmount = 0;
        }
    }
    private void Update()
    {
        Reload();
        MeshChange();
        MeshReturn();
    }
    void Reload()
    {
        reloadImage.transform.parent.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y, 0);

        if (meshChange && gameObject.GetComponent<BoxCollider>().Equals(null))
        {
            reloadImage.fillAmount += Time.deltaTime / 3;
        }
        if (reloadImage.fillAmount == 1)
        {
            isCompleted = true;
            cloud.Play();
        }
        else
        {
            isCompleted = false;
        }
    }
    void MeshChange()
    {
        if (isCompleted && meshChange)
        {
            gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            gameObject.AddComponent<MeshFilter>();
            gameObject.GetComponent<Renderer>().enabled = true;

            gameObject.GetComponent<MeshFilter>().mesh = triggerObject.GetComponentInParent<MeshFilter>().mesh;
            gameObject.GetComponent<Renderer>().material = triggerObject.GetComponentInParent<Renderer>().material;

            if (gameObject.GetComponent<BoxCollider>().Equals(null))
            {
                gameObject.AddComponent<BoxCollider>();
            }
            Destroy(gameObject.GetComponent<CapsuleCollider>());

            reloadImage.fillAmount = 0;
        }
    }
    void MeshReturn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!gameObject.GetComponent<BoxCollider>().Equals(null))
            {
                cloud.Play();
            }

            gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            gameObject.GetComponent<Renderer>().enabled = false;

            if (gameObject.GetComponent<CapsuleCollider>().Equals(null))
            {
                gameObject.AddComponent<CapsuleCollider>();

                gameObject.GetComponent<CapsuleCollider>().height = 2;
                gameObject.GetComponent<CapsuleCollider>().radius = 0.5f;
                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
            }

            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<MeshFilter>());
        }
    }
}
