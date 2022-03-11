using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NewspaperSellerModels;
namespace NewspaperSellerSimulation
{
	class Input
	{
		private SimulationSystem sys;
		public Input(ref SimulationSystem syt)
		{
			sys = syt;
		}
		public void ReadFromFile()
		{

			FileStream fileStream = new FileStream("TestCase1.txt", FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			while (streamReader.Peek() != -1)
			{
				string next = streamReader.ReadLine();
				if (next.Length == 0)
					continue;
				if (next == "NumOfNewspapers")
				{
					sys.NumOfNewspapers = Convert.ToInt32(streamReader.ReadLine());
				}
				else if (next == "NumOfRecords")
				{
					sys.NumOfRecords = Convert.ToInt32(streamReader.ReadLine());
				}
				else if (next == "PurchasePrice")
				{
					sys.PurchasePrice = Convert.ToDecimal(streamReader.ReadLine());
				}
				else if (next == "ScrapPrice")
				{
					sys.ScrapPrice = Convert.ToDecimal(streamReader.ReadLine());
				}
				else if (next == "SellingPrice")
				{
					sys.SellingPrice = Convert.ToDecimal(streamReader.ReadLine());
				}
				else if (next == "DayTypeDistributions")
				{
					next = streamReader.ReadLine();
					string[] arr = next.Split(',');
					DayTypeDistribution op = new DayTypeDistribution();

					// Good
					op.DayType = Enums.DayType.Good;
					op.Probability = Convert.ToDecimal(arr[0]);
					var acc = op.Probability;
					op.CummProbability = acc;

					op.MinRange = 1;
					op.MaxRange = Convert.ToInt32(acc * 100);
					sys.DayTypeDistributions.Add(op);

					// Fair
					op = new DayTypeDistribution();
					op.DayType = Enums.DayType.Fair;
					op.Probability = Convert.ToDecimal(arr[1]);
					op.MinRange = Convert.ToInt32(acc*100 + 1);
					acc += op.Probability;
					op.CummProbability = acc;
					op.MaxRange = Convert.ToInt32(acc * 100);
					sys.DayTypeDistributions.Add(op);


					// Poor
					op = new DayTypeDistribution();
					op.DayType = Enums.DayType.Poor;
					op.Probability = Convert.ToDecimal(arr[2]);
					op.MinRange = Convert.ToInt32(acc * 100 + 1);
					acc += op.Probability;
					op.CummProbability = acc;
					op.MaxRange = Convert.ToInt32(acc * 100);
					sys.DayTypeDistributions.Add(op);


				}
				else if (next == "DemandDistributions")
				{
					bool isFirst = true;
					decimal GoodAcc = 0, PoorAcc = 0, FairAcc = 0;
					while (streamReader.Peek() != -1)
					{
						string curLine = streamReader.ReadLine();
						
						string[] arr = curLine.Split(',');
					
						DemandDistribution demandObj = new DemandDistribution();
						demandObj.Demand = Convert.ToInt32(arr[0]);
						DayTypeDistribution op = new DayTypeDistribution();
						

						// Good
						op.DayType = Enums.DayType.Good;
						op.Probability = Convert.ToDecimal(arr[1]);
					
					
						if (isFirst)
							op.MinRange = 1;
						else 
							op.MinRange = Convert.ToInt32(GoodAcc * 100) + 1;
						GoodAcc += op.Probability;
						op.MaxRange = Convert.ToInt32(GoodAcc * 100);
						op.CummProbability = GoodAcc;
						demandObj.DayTypeDistributions.Add(op);


						// Fair
						op = new DayTypeDistribution();
						op.DayType = Enums.DayType.Fair;
						op.Probability = Convert.ToDecimal(arr[2]);
						
					
						if (isFirst)
							op.MinRange = 1;
						else 
							op.MinRange = Convert.ToInt32(FairAcc * 100) + 1;
						FairAcc += op.Probability;
						op.MaxRange = Convert.ToInt32(FairAcc * 100);
						op.CummProbability = FairAcc;
						demandObj.DayTypeDistributions.Add(op);


						// Poor
						op = new DayTypeDistribution();
						op.DayType = Enums.DayType.Poor;
						op.Probability = Convert.ToDecimal(arr[3]);
			
					
						if (isFirst)
							op.MinRange = 1;
						else
							op.MinRange = Convert.ToInt32(PoorAcc * 100) + 1;
						PoorAcc += op.Probability;
						op.MaxRange = Convert.ToInt32(PoorAcc * 100);
						op.CummProbability = PoorAcc;
						demandObj.DayTypeDistributions.Add(op);


						sys.DemandDistributions.Add(demandObj);

						isFirst = false;
					}
				}
			}
		}
	}
}
