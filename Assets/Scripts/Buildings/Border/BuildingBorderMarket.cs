using Resources;
using System;


namespace Building.Border
{
    public sealed class BuildingBorderMarket : IBuildingBorderMarket
    {
        private uint[] _quantityInStock;

        private uint[] _costPerKg;

        private bool[] _isSale;

        private byte _indexTypeDrug;


        public BuildingBorderMarket()
        {
            InitArrays();
        }

        bool IBuildingBorderMarket.CheckResourceInSale(in TypeProductionResources.TypeResource typeResource)
        {
            _indexTypeDrug = (byte)typeResource;

            return CheckResources(_indexTypeDrug);
        }

        private bool CheckResources(in byte indexResource)
        {
            if (_isSale[indexResource])
                return true;
            else
                return false;
        }

        private void InitArrays()
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;

            _quantityInStock = new uint[amountTypeDrugs];
            _costPerKg = new uint[amountTypeDrugs];
            _isSale = new bool[amountTypeDrugs];

            for (int i = 0; i < _isSale.Length; i++)
                _isSale[i] = true;
        }
    }
}
