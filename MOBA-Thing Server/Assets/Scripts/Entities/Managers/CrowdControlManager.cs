using System.Collections.Generic;

public interface ICrowdControl
{
    void Init(int _entityID);
    void Release();
}

public class CrowdControlManager
{
    private int EntityID { get; }
    private List<ICrowdControl> ccs = new List<ICrowdControl>();

    public CrowdControlManager(int _entityID)
    {
        EntityID = _entityID;
    }

    public void PurgeAll()
    {
        foreach (ICrowdControl cc in ccs)
        {
            cc.Release();
        }
        ccs.Clear();
    }

    public void PurgeOfType<T>() where T : class, ICrowdControl
    {
        for (int i = ccs.Count; i >= 0; i--)
        {
            if(ccs[i].GetType() == typeof(T))
            {
                ccs[i].Release();
                ccs.RemoveAt(i);
            }
        }
    }

    public void AddCC(ICrowdControl _cc)
    {
        ccs.Add(_cc);
        _cc.Init(EntityID);
    }
    public void RemoveCC(ICrowdControl _cc)
    {
        ccs.Find(cc => cc == _cc).Release();
        ccs.Remove(_cc);
    }
}
