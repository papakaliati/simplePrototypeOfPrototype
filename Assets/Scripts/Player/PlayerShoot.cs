using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    [SerializeField]
    private PlayerWeapon weapon;
    [SerializeField]
    private LayerMask mask;
    

    public Camera cam;

	// Use this for initialization
	void Start () {
        if(cam == null)
        {
            Debug.LogError("No camera detected");
            this.enabled = false;
        }
        weapon.BeamEmmitter.Stop(); 
	}

    void Update()
    {
        ChangeElement();
        if(Input.GetButtonDown("Fire1"))
        {
            if(weapon.WeaponElement != "")
            {
                Shoot();
            }            
        }
        if(Input.GetButtonUp("Fire1"))
        {
            if (weapon.BeamEmmitter.isPlaying)
            {
                weapon.BeamEmmitter.Stop();
            }
        }
    }
	
    void ChangeElement()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            weapon.WeaponElement = "Fire";
            weapon.BeamEmmitter.startColor = new Color(1f, 0.05f, 0f,0.5f);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            weapon.WeaponElement = "Water";
            weapon.BeamEmmitter.startColor = new Color(0f, 0.05f, 1f, 0.5f);

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            weapon.WeaponElement = "Electricity";
            weapon.BeamEmmitter.startColor = new Color(0f, 0.8f, 0.8f, 0.5f);
        }
    }

    private void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.Range, mask))
        {
            Debug.Log("We hit " + _hit.collider.name);
        }
        weapon.BeamEmmitter.Play();

    }
}
