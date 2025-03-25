using UnityEngine;



public class SCitem : ScriptableObject 
{
   public string itemName; //item adı
   public string itemDescription; //item açıklaması
   public bool canStackable;  //item stacklanabiliyor mu
   public Sprite itemIcon; // item resmi
   public GameObject itemPrefab; //itemin gameobjesi
   
}
