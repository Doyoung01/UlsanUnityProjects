using UnityEngine;

public class SuccessTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            ResultManager.Instance.ShowUI();
            ResultManager.Instance.successUI.SetActive(true);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
