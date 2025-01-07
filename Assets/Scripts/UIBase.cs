using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected PopUp_Name uiName;
    protected PopUp_Type uiType;
    protected PopUp_State state;
    public PopUp_State GetState()
    {
        return state;
    }
    public void SetState(PopUp_State state)
    {
        this.state = state;
    }
    public virtual void Init(PopUp_Name uiName)
    {
        this.uiName = uiName;
        state = PopUp_State.Open;
    }
    public virtual void Draw(bool active)
    {
        gameObject.SetActive(active);
        if (active)
        {
            state = PopUp_State.Open;
        }
        else
        {
            state = PopUp_State.Close;
        }
    }
    public virtual void Close()
    {
        state = PopUp_State.Close;
        gameObject.SetActive(false);
    }
}
