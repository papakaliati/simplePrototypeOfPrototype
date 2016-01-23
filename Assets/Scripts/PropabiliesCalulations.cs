using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PropabiliesCalulations <T> {

	/// <summary>
	/// Gets the random selection provided the the possibilies.
	/// </summary>
	/// <returns>The random selection.</returns>
	/// <param name="attributionRates">Attribution rates.</param>
	public static int GetRandomSelection (T[] attributionRates) {
		List<ItemType> items = new List<ItemType> ();
		for (var i = 0; i < attributionRates.Count (); i++)
			items.Add (new ItemType (i, attributionRates [i]));
		return SelectRandomItem (items);
	}

	private static int SelectRandomItem (List<ItemType> cards) {
		var deck = new List<ItemType>();
		cards.ForEach (c => {
			for (int i = 0; i < System.Convert.ToInt32(c.AttributionRate); i++)
				deck.Add (c);
		});
		deck = deck.OrderBy(c => System.Guid.NewGuid()).ToList();
		return deck[0].Id;
	}

	public class ItemType {
		
		public int Id { private set; get;}
		public T AttributionRate { private set; get; }

		public ItemType (int Id, T AttributionRate){
			this.Id= Id;
			this.AttributionRate = AttributionRate;
		}
	}
}
