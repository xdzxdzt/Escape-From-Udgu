using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject[] Pages; // Массив объектов

    void Start()
    {
        EnableRandomPages();
    }

    void EnableRandomPages()
    {
        // Создаем список индексов всех объектов
        List<int> indices = new List<int>();
        for (int i = 0; i < Pages.Length; i++)
        {
            indices.Add(i);
        }

        // Перемешиваем индексы
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Включаем только первые 7 объектов из перемешанного списка
        for (int i = 0; i < Pages.Length; i++)
        {
            if (i < 7)
            {
                Pages[indices[i]].SetActive(true); // Включаем объект
            }
            else
            {
                Pages[indices[i]].gameObject.SetActive(false); // Отключаем объект
            }
        }
    }
}
