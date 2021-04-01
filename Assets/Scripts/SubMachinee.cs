using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMachinee : MonoBehaviour
{
    public float range;
    public Camera mainCamera;

    public float fireRate;
    public float timeToReload;
    private float currentRateToFire;
    private float currentTimeToReload;
    public bool canShoot = true;
    public bool isDraw;

    public Animation subMachineAnimation;
    public AnimationClip shootAnim;
    public AnimationClip reloadAnim;
    public AnimationClip drawAnim;
    public AnimationClip miraAnim;
    public AnimationClip backMiraAnim;
    public bool crosshair;

    public AudioSource subMachineAudio;
    public AudioClip shootSound;

    public ParticleSystem MuzzleFlashEffect;
    public ParticleSystem CartridgeEjectEffect;

    public GameObject ImpactTemp;

    public float intensity;
    public float smooth;
    public bool isMine;
    private Quaternion origin_rotation;

    public int bullets = 20;
    public int cartuchos = 4;
    private int startBullets;

    public Text currentammoText;
    public Text currentcartText;

    void Start()
    {
        startBullets = bullets;
        currentRateToFire = timeToReload;
        currentTimeToReload = fireRate;
        origin_rotation = transform.localRotation;
    }

    void Update()
    {
        currentammoText.text = bullets.ToString();
        currentcartText.text = cartuchos.ToString();
        currentRateToFire += Time.deltaTime;
        currentTimeToReload += Time.deltaTime;

        if (subMachineAnimation.IsPlaying(drawAnim.name))
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

        subMachineAudio.clip = shootSound;
        subMachineAudio.Play();
        
        subMachineAnimation.clip = shootAnim;
        subMachineAnimation.Play();  
       
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
        subMachineAnimation.clip = reloadAnim;
        subMachineAnimation.Play(PlayMode.StopAll);
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
        subMachineAnimation.clip = miraAnim;
        subMachineAnimation.Play();
    }

    void BackMira()
    {
        subMachineAnimation.clip = backMiraAnim;
        subMachineAnimation.Play();
    }
}