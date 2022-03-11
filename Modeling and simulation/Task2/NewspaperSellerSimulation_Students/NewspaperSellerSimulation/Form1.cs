using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
   
    public partial class Form1 : Form
    {
        SimulationSystem sys;
        public Form1()
        {
            InitializeComponent();
            sys = new SimulationSystem();
        }
       
        private void Form1_Load(object sender, EventArgs e)
		{
            Input input = new Input(ref sys);
            input.ReadFromFile();
            PerformanceMeasures performanceMeasures = new PerformanceMeasures();
            for (int i = 1; i <= sys.NumOfRecords; i++)
			{
                SimulationCase simulationCase = new SimulationCase();
                simulationCase.DayNo = i;
                simulationCase.GetDemand(sys);
                simulationCase.GetDailyCost(sys);
                simulationCase.GetSalesProfit(sys);
                simulationCase.GetLostProfit(sys);
                simulationCase.GetScrapProfit(sys);
                simulationCase.GetDailyNetProfit(sys);

                sys.SimulationTable.Add(simulationCase);

                performanceMeasures.TotalSalesProfit += simulationCase.SalesProfit;
                performanceMeasures.TotalCost += simulationCase.DailyCost;
                performanceMeasures.TotalLostProfit += simulationCase.LostProfit;
                performanceMeasures.TotalScrapProfit += simulationCase.ScrapProfit;
                performanceMeasures.TotalNetProfit += simulationCase.DailyNetProfit;
                if (simulationCase.LostProfit > 0)
                    performanceMeasures.DaysWithMoreDemand++;
                if (simulationCase.ScrapProfit > 0)
                    performanceMeasures.DaysWithUnsoldPapers++;
            }
            sys.PerformanceMeasures = performanceMeasures;
            string testingResult = TestingManager.Test(sys, Constants.FileNames.TestCase1);
            MessageBox.Show(testingResult);


            var binding = new BindingList<SimulationCase>(sys.SimulationTable);
            var src = new BindingSource(binding, null);
            dataGridView1.DataSource = src;
        }
	}
}
