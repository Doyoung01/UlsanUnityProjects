using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    void Start()
    {
        OnDeactiveWeapon();
    }

    void Update()
    {
        
    }

    public BoxCollider weaponCol;

    public void OnActiveWeapon()
    {
        // dagger Collider Active
        weaponCol.enabled = true;
    }

    public void OnDeactiveWeapon()
    {
        // dagger Collider Deactive
        weaponCol.enabled = false;
    }
}
