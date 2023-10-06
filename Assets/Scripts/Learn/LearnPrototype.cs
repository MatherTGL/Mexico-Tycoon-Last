using Sirenix.OdinInspector;
using UnityEngine;

public sealed class LearnPrototype : MonoBehaviour
{
    [Button("Button")]
    void Button()
    {
        ICar carBMW = new BMW();
        ICar carNewBMW = carBMW.Clone();
    }
}

interface IAnimal
{
    byte age { get; set; }
    IAnimal Clone();
}

class Sheep : IAnimal
{
    private string _name;
    public string name => _name;

    private byte _age;
    public byte age { get => _age; set => _age = value; }


    public Sheep()
    {
        _age = (byte)Random.Range(1, 25);
    }

    private Sheep(Sheep donor)
    {
        this._name = donor.name;
        this._age = donor.age;
    }

    IAnimal IAnimal.Clone() => new Sheep(this);
}

interface ICar
{
    double cost { get; set; }


    ICar Clone();
}

class BMW : ICar
{
    private double _cost;
    double ICar.cost { get => _cost; set => _cost = value; }


    public BMW()
    {
        _cost = Random.Range(1, 50);
    }

    private BMW(in BMW donor)
    {
        this._cost = donor._cost;
    }

    ICar ICar.Clone()
    {
        return new BMW(this);
    }
}