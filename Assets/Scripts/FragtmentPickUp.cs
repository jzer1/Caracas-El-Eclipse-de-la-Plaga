using UnityEngine;

public class FragtmentPickUp : MonoBehaviour, ICollectible
{
    public int fragmentValue = 1;

    public void Collect(PlayerNivel playerTransform)
    {
        
        playerTransform.AddFragments(fragmentValue);
        Debug.Log("Fragment collected! Total fragments: " + playerTransform.fragments);
        Destroy(gameObject); 
    }
}
