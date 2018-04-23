using System;
using System.Collections.Generic;

namespace Export.Models {
    public class InMemoryModel {
        const int DataItemsCount = 100;

        public int ID { get; set; }
        public string Text { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public static List<InMemoryModel> GetTypedListModel() {
            List<InMemoryModel> typedList = new List<InMemoryModel>();

            Random randomizer = new Random();

            for(int index = 0; index < DataItemsCount; index++) {
                typedList.Add(new InMemoryModel() {
                    ID = index,
                    Text = "Text" + index.ToString(),
                    Quantity = randomizer.Next(1, 50),
                    Price = (decimal)Math.Round((randomizer.NextDouble() * 100), 2)
                });
            }

            return typedList;
        }
    }
}