using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Gun2 : MonoBehaviour
{
    public float range;
    public Camera mainCamera;

    public Animation Gun2Animation;
    public AnimationClip shootAutoAnim;
    public AnimationClip reloadAnim;
    public AnimationClip drawAnim;
    public AnimationClip miraAnim;
    public AnimationClip backMiraAnim;

    public float fireRate;
    private float currentRateToFire;
    public float timeToReload;
    private float currentTimeToReload;
    public bool canShoot = true;
    public bool isDraw;

    public AudioSource Gun2Audio;
    public AudioClip shootSound;

    public ParticleSystem MuzzleFlashEffect;
    public ParticleSystem CartridgeEjectEffect;

    public GameObject ImpactTemp;

    public int bullets = 50;
    public int cartuchos = 4;
    private int startBullets;

    public float intensity;
    public float smooth;
    public bool isMine;
    private Quaternion origin_rotation;

    public bool crosshair;
    
    //public Text currentammoText;
    //public Text currentcartText;

    void Start()
    {
        startBullets = bullets;
        currentRateToFire = timeToReload;
        currentTimeToReload = fireRate;
        origin_rotation = transform.localRotation;
    }

    void Update()
    {
        //currentammoText.text = bullets.ToString();
        //currentcartText.text = cartuchos.ToString();
        currentRateToFire += Time.deltaTime;
        currentTimeToReload += Time.deltaTime;

        if (Gun2Animation.IsPlaying(drawAnim.name))
        {
            isDraw = true;
        }
        else
        {
            isDraw = false;
        }

        if (currentTimeToReload >= timeToReload)
        {
            canShoot = true;
        }

        UpdateSway();
    }

    private void UpdateSway()
    {
        float t_x_axis = Input.GetAxis("Mouse X");
        float t_y_axis = Input.GetAxis("Mouse Y");

        if (!isMine)
        {
            t_x_axis = 0;
            t_y_axis = 0;
        }

        //calcula a rotação do alvo
        Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_axis, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_axis, Vector3.right);
        Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }

    //Botão atirar
    public void ShootButton()
    {
        if (currentRateToFire >= fireRate && canShoot && isDraw == false && bullets > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        bullets --;
        currentRateToFire = 0;
        MuzzleFlashEffect.Play();
        CartridgeEjectEffect.Play();

        Gun2Audio.clip = shootSound;
        Gun2Audio.Play();
        
        Gun2Animation.clip = shootAutoAnim;
        Gun2Animation.Play();  
       
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
        {
            Instantiate(ImpactTemp, hit.point, Quaternion.LookRotation(hit.normal));
          //Debug.Log("Acertou" + hit.transform.name);
        }
    }

    //Botão recarregar
    public void ReloadButton()
    {
        if (cartuchos > 0)
        {
            Reload();
        }
    }

    void Reload()
    {
        cartuchos--;
        bullets += startBullets;
        currentTimeToReload = 0;
        canShoot = false;
        Gun2Animation.clip = reloadAnim;
        Gun2Animation.Play(PlayMode.StopAll);
    }

    //Botão de Mirar
    public void MiraButton()
    {
        crosshair =! crosshair;
        if(miraAnim == crosshair)
        {
            Mira();
        }
        else
        {
            BackMira();
        }
    }
        
    void Mira()
    {
        Gun2Animation.clip = miraAnim;
        Gun2Animation.Play();
    }

    void BackMira()
    {
        Gun2Animation.clip = backMiraAnim;
        Gun2Animation.Play();
    }
}