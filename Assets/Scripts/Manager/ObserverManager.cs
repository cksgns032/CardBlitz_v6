using System.Collections;

public class ObserverManager : SingleTon<ObserverManager>
{
    ArrayList observerList = new ArrayList();
    private void OnDestroy()
    {
        observerList.Clear();
    }

    public void AddObserver(Observer observer)
    {
        observerList.Add(observer);
    }
    public void RemoveObserver(Observer observer)
    {
        observerList.Remove(observer);
    }
    public void Notify(SendData data)
    {
        foreach (Observer observer in observerList)
        {
            observer.Notify(data);
        }
    }
}
