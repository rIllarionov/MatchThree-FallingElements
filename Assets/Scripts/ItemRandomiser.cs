using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemRandomiser : MonoBehaviour
{
    [SerializeField] private ScriptableItemsHolder _itemsHolder;
    private Tile[,] _tileMap;

    public void Initialize (Tile[,] tileMap)
    {
        _tileMap = tileMap;
    }
    public ScriptableItemSettings GetRandomElement(bool isItemGood, int rowIndex, int columnIndex)
    {
        return FindRandomElement(isItemGood,rowIndex,columnIndex);
    }
        private ScriptableItemSettings FindRandomElement (bool isItemGood, int rowIndex, int columnIndex)
    {
        //предпологаем что элемент нам сразу подходит В процессе меняем флаг на фолс если элемент не подходит
        //генерируем рандомный элемент
        var randomElement = _itemsHolder.Items[Random.Range(0, _itemsHolder.Items.Length)];

        //если слева есть более двух элементов то проверяем эти два элемента
        if (columnIndex > 1 && isItemGood)
        {
            var currentType = Convert.ToInt32(randomElement.Type);
            var firstElement = Convert.ToInt32(_tileMap[rowIndex, columnIndex - 1].Item.Type);
            var secondElement = Convert.ToInt32(_tileMap[rowIndex, columnIndex - 2].Item.Type);

            if (firstElement==secondElement)//если предыдущие два элемента равны, то сравниваем с текущим
            {
                if (currentType==firstElement)
                {
                    isItemGood = false; //если они все три равны то помечаем элемент как неподходящий
                }
            }
        }

        //далее сравниваем по столбцу (если по строкам не нашли совпадений)
        if (rowIndex > 1 && isItemGood)
        {
            var currentType = Convert.ToInt32(randomElement.Type);
            var firstElement = Convert.ToInt32(_tileMap[rowIndex - 1, columnIndex].Item.Type);
            var secondElement = Convert.ToInt32(_tileMap[rowIndex - 2, columnIndex].Item.Type);

            if (firstElement==secondElement)
            {
                if (currentType==firstElement)
                {
                    isItemGood = false;
                }
            }
        }

        //если по результатам проверки рандомный элемент не совпал с предыдущими двумя элементами
        //по горизнтали и по вертикали, то он нам подходит
        if (isItemGood)
        {
            return randomElement;
        }

        //иначе запускаем новую итерацию метода
        else
        {
            return GetRandomElement(true, rowIndex, columnIndex);
        }
        
    }
}
