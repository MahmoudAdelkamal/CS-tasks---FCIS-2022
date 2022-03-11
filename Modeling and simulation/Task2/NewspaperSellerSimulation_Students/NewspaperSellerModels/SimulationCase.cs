using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
	public class SimulationCase
	{
		public int DayNo { get; set; }
		public int RandomNewsDayType { get; set; }
		public Enums.DayType NewsDayType { get; set; }
		public int RandomDemand { get; set; }
		public int Demand { get; set; }
		public decimal DailyCost { get; set; }
		public decimal SalesProfit { get; set; }
		public decimal LostProfit { get; set; }
		public decimal ScrapProfit { get; set; }
		public decimal DailyNetProfit { get; set; }

		public void GetDailyCost(SimulationSystem sys)
		{
			DailyCost = sys.NumOfNewspapers * sys.PurchasePrice ;
		}
		public void GetScrapProfit(SimulationSystem sys)
		{
			if (sys.NumOfNewspapers >= Demand)
				ScrapProfit = ((sys.NumOfNewspapers - Demand) * sys.ScrapPrice);
			else 
				ScrapProfit = 0;
		}
		public void GetSalesProfit(SimulationSystem sys)
		{
			SalesProfit = Math.Min(sys.NumOfNewspapers,Demand) * sys.SellingPrice ;
		}
		public void GetLostProfit(SimulationSystem sys)
		{
			if (Demand >= sys.NumOfNewspapers)
				LostProfit = (Demand - sys.NumOfNewspapers) * (sys.SellingPrice - sys.PurchasePrice) ;
			else
				LostProfit = 0;
		}
		public void GetDailyNetProfit(SimulationSystem sys)
		{
			DailyNetProfit = SalesProfit - DailyCost - LostProfit + ScrapProfit;
		}


		public int GenerateRandomDemand()
		{
			Random random = new Random();
			int rnd = random.Next(1, 100);
			return rnd;
		}
		public void GetDemand(SimulationSystem sys)
		{
			GetNewsDayType(sys);
			RandomDemand = GenerateRandomDemand();
			foreach (var item in sys.DemandDistributions)
			{
				foreach (var curDay in item.DayTypeDistributions)
				{
					if (curDay.DayType == NewsDayType && RandomDemand >= curDay.MinRange && RandomDemand <= curDay.MaxRange)
					{
						Demand = item.Demand;
						return;
					}
				}
			}
		}
		public int GenerateRandomDayType()
		{
			Random random = new Random();
			int rnd = random.Next(1, 100);
			return rnd;
		}

		public void GetNewsDayType(SimulationSystem sys)
		{
			RandomNewsDayType = GenerateRandomDayType();
			foreach(var item in sys.DayTypeDistributions)
			{
				if (RandomNewsDayType >= item.MinRange && RandomNewsDayType <= item.MaxRange)
				{
					NewsDayType = item.DayType;
					return;
				}
			}
		}
	}
}
