using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ams3.Models {

	public class Category {

		public enum ForAsset { All, Vehicle, Equipment, Property }

		public int Id { get; set; }
		public string Name { get; set; }
		public ForAsset AssetType { get; set; } = Category.ForAsset.All;

		public Category() { }

	}
}