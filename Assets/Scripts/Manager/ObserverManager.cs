using System;
using System.Collections;
using System.Collections.Generic;

public class ObserverManager : SingleTon<ObserverManager>
{
    ArrayList observerList = new ArrayList();

    private Queue<Action> mainThreadActions = new Queue<Action>();
    private void OnDestroy()
    {
        observerList.Clear();
    }

    private void Update()
    {
        while (mainThreadActions.Count > 0)
        {
            var action = mainThreadActions.Dequeue();
            action.Invoke();
        }
    }

    public void AddObserver(Observer observer)
    {
        observerList.Add(observer);
    }
    public void RemoveObserver(Observer observer)
    {
        observerList.Remove(observer);
    }
    public void Notify(byte[] buffer)
    {
        // queue로 하는 이유 : 서버는 메인 스레드가 아니여서 인스턴스 하면 오류가 생김 그래서 queue에 넣고 update침
        mainThreadActions.Enqueue(() =>
       {
           foreach (Observer observer in observerList)
           {
               observer.Notify(buffer); // 이제 메인 스레드에서 실행됨
           }
       });
        // foreach (Observer observer in observerList)
        // {
        //     observer.Notify(buffer);
        // }
    }
}
