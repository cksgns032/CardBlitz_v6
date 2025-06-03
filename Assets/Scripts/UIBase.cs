using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected Layer_Type type;
    protected string uiName;
    protected Active_State state;
    public Active_State GetState()
    {
        return state;
    }
    public void SetState(Active_State state)
    {
        this.state = state;
    }
    public virtual void Init(Layer_Type type, string name)
    {
        this.type = type;
        this.uiName = name;
        state = Active_State.Open;
    }
    public virtual void Draw()
    {
        gameObject.SetActive(true);
        state = Active_State.Open;
    }
    public virtual void Close()
    {
        state = Active_State.Close;
        gameObject.SetActive(false);
    }
}
