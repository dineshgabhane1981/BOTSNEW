using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebApp.ViewModel
{
    public class DataPoint
    {
		
		public DataPoint(string label, double y)
		{
			this.Label = label;
			this.Y = y;
		}
		
		public string Label = "";		
		public Nullable<double> Y = null;
	}
}