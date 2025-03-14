//using Avalonia.Media;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UP2.Models
//{
//    public partial class Product
//    {
//        public decimal OriginalPrice { get; set; }
//        public Brush ProductBackground
//        {
//            get
//            {
//                return CurrentDiscount > 15 ? new SolidColorBrush(Color.FromRgb(231, 250, 191)) : new SolidColorBrush(Color.FromRgb(255, 255, 255));
//            }
//        }
//        public string StrikethroughPrice => OriginalPrice.ToString(); 
//        public string FinalPrice => (OriginalPrice - (OriginalPrice * CurrentDiscount / 100)).ToString(); 
//        public bool IsDiscounted => CurrentDiscount > 0; 
//    }
//}

