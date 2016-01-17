using UnityEngine;

[System.Serializable]

public class PlayerWeapon {

    [SerializeField]
    public ParticleSystem BeamEmmitter;

	// Use this for initialization
    public string WeaponElement = null;
    public float Range = 20f;


}
