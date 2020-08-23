//plug these into monobehaviour controller classes to isolate work
public class HealthManager : ResourceManager
{
    public HealthManager(int _entityID, float _baseHP, float _hpPerLvl) 
        : base(_entityID, _baseHP, _hpPerLvl) { }
}
