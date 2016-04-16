using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using Pharmhack.OData.Default;

namespace Pharmhack.OData
{
	public class FredClient : Container
	{
		public FredClient() 
			: base(new Uri("http://dev.frednxt.com.au/data"))
		{
			this.SendingRequest2 += FredClient_SendingRequest2;
		}

		void FredClient_SendingRequest2(object sender, SendingRequest2EventArgs e)
		{
			var credentials = string.Format("{0}:{1}",
				ConfigurationManager.AppSettings["FredUsername"],
				ConfigurationManager.AppSettings["FredPassword"]);

			var authHeaderValue = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

			e.RequestMessage.SetHeader("Authorization", authHeaderValue);
		}
	}
}
