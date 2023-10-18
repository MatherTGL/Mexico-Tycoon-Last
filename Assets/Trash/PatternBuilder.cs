using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PatternBuilder : MonoBehaviour
{
    [Button("Click")]
    void ButtonClick()
    {
        IDeveloper developer = new SamsungDeveloper();
        Director director = new Director(developer);
        Laptop laptop = developer.GetLaptop();
        director.CreateGamerLaptop();
    }
}

public interface IDeveloper
{
    void CreateLaptop();
    Laptop GetLaptop();
}

public class SamsungDeveloper : IDeveloper
{
    private Laptop _laptop;


    public SamsungDeveloper() => _laptop = new Laptop();

    void IDeveloper.CreateLaptop()
    {
        _laptop.SetMemoryRAM(Laptop.MemoryRAM.second);
    }

    Laptop IDeveloper.GetLaptop() => _laptop;
}

public class AppleDeveloper : IDeveloper
{
    private Laptop _laptop;


    public AppleDeveloper() => _laptop = new Laptop();

    void IDeveloper.CreateLaptop()
    {
        _laptop.SetMemoryRAM(Laptop.MemoryRAM.first);
    }

    Laptop IDeveloper.GetLaptop() => _laptop;
}

public class Laptop
{
    public enum MemoryRAM { first, second, third }
    private MemoryRAM _memoryRAM;


    public void SetMemoryRAM(in MemoryRAM memoryRAM)
    {
        _memoryRAM = memoryRAM;
    }
}

public class Director
{
    private IDeveloper _Ideveloper;


    public Director(IDeveloper developer)
    {
        _Ideveloper = developer;
    }

    public void CreateGamerLaptop()
    {
        _Ideveloper.CreateLaptop();
    }
}