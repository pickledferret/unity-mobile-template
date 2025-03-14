using UnityEngine;

public class PopUpBase : MonoBehaviour
{
    public virtual void ClosePopUp()
    {
        Destroy(gameObject);
    }
}
