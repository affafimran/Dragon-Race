using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DragonStat
{
    public string _name;
    public int _health;
    public int _age;
    public int _speed;
    public float _timeSpan;
    public DragonType _dragonType;


    public DragonStat(DragonStat _dragon)
    {
        _name = _dragon._name;
        _health = _dragon._health;
        _age = _dragon._age;
        _speed = _dragon._speed;
        _timeSpan = _dragon._timeSpan;
        _dragonType = _dragon._dragonType;
    }
}

public enum DragonType
{
    Water,Lightning,Earth,Fire,Wind,Holy,Shadow,Ice,Poison,Undead,Metallic,Grass
}
