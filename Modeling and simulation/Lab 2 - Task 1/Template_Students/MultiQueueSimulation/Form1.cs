using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimulationSystem system = new SimulationSystem();
        public int GetTime(int Probability,List<TimeDistribution>L)
        {
            for (int i = 0; i < L.Count; i++)
            {
                if (Probability>= L[i].MinRange && Probability <= L[i].MaxRange)
                {
                    return L[i].Time;
                }
            }
            return 0;
        }
        
        public int MaxServerTime(List<Server>L)
        {
            int max = 0;
            for (int i = 0; i < L.Count; i++)
            {
                max = Math.Max(max, L[i].FinishTime);
            }
            return max;
        }
        
        public int GetServer(List<Server> servers, int ArrivalTime)
        {
            int ServerId = 0;
            int min = int.MaxValue;
            for (int i = 0; i < servers.Count; i++)
            {
                // if the server has finished his working before I arrive, it's ok to use it
                if (servers[i].FinishTime <= ArrivalTime)
                {
                    ServerId = i;
                    break;
                }
                // hasn't finish yet? choose the first one who finishes
                else if (servers[i].FinishTime < min)
                {
                    min = servers[i].FinishTime;
                    ServerId = i;
                }
            }
            return ServerId;
        }
        public int GetRandomServer(List<Server>Servers,int ArrivalTime)
        {
            List<Server>FinishedServers = new List<Server>();
            for (int i = 0; i < Servers.Count; i++)
            {
                // ready servers
                if (Servers[i].FinishTime <= ArrivalTime)
                {
                    FinishedServers.Add(Servers[i]);
                }
            }
            if (FinishedServers.Count > 0)
            {
                Random random = new Random();
                int ServerId = random.Next(0,FinishedServers.Count);
                return FinishedServers[ServerId].ID-1;
            }
            int serverId = 0;
            int min = int.MaxValue;
            for (int i = 0; i < Servers.Count; i++)
            {
                if (Servers[i].FinishTime < min)
                {
                    min = Servers[i].FinishTime;
                    serverId = i;
                }
            }
            return serverId;
        }
        public int GetLeastUtilizedServer(List<Server> Servers)
        {
            decimal min = int.MaxValue;
            int id = 0;
            for (int i = 0; i < Servers.Count; i++)
            {
                decimal Current = (decimal)system.Servers[i].TotalWorkingTime / MaxServerTime(system.Servers);
                if (Current < min)
                {
                    min = Current;
                    id = i;
                }
            }
            return id;
        }
        public int MaxQueueLength(List<SimulationCase>L)
        {
            int Maximum = 0;
            for (int i = 0; i < L.Count; i++)
            {
                int CurrentLength = 0;
                int Start = 0;
                if (L[i].TimeInQueue > 0)
                { 
                    for (int j = i; j < L.Count; j++)
                    {
                        if (L[j].TimeInQueue > 0 && CurrentLength==0)
                        {
                            Start = L[j].StartTime;
                            CurrentLength++;
                        }
                        else if (L[j].TimeInQueue > 0 && CurrentLength>0 && L[j].ArrivalTime < Start)
                        {
                            CurrentLength++;
                        }
                        else if (L[i].ArrivalTime > Start && CurrentLength>0)
                        {
                            break;
                        }
                    }
                    if (CurrentLength>0 && CurrentLength > Maximum)
                    {
                        Maximum = CurrentLength;
                    }
                }
            }
            return Maximum;
        }
        private void ReadFromFile()
        {
            FileStream fileStream = new FileStream("TestCase3.txt", FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            while(streamReader.Peek()!=-1)
            {
                string next = streamReader.ReadLine();
                if(next.Length == 0)
                    continue;
                if(next=="NumberOfServers")
                {
                    system.NumberOfServers = Convert.ToInt32(streamReader.ReadLine());
                }
                else if(next=="StoppingNumber")
                {
                    system.StoppingNumber = Convert.ToInt32(streamReader.ReadLine());
                }
                else if(next=="StoppingCriteria")
                {
                    int StoppingCriteria = Convert.ToInt32(streamReader.ReadLine());
                    Dictionary<int, Enums.StoppingCriteria> dict = new Dictionary<int, Enums.StoppingCriteria>();
                    dict.Add(1, Enums.StoppingCriteria.NumberOfCustomers);
                    dict.Add(2, Enums.StoppingCriteria.SimulationEndTime);
                    system.StoppingCriteria = dict[StoppingCriteria];
                }
                else if(next=="SelectionMethod")
                {
                    int SelectionMethod = Convert.ToInt32(streamReader.ReadLine());
                    Dictionary<int, Enums.SelectionMethod> dict = new Dictionary<int, Enums.SelectionMethod>();
                    dict.Add(1, Enums.SelectionMethod.HighestPriority);
                    dict.Add(2, Enums.SelectionMethod.LeastUtilization);
                    dict.Add(3, Enums.SelectionMethod.Random);
                    system.SelectionMethod = dict[SelectionMethod];
                }
                else if(next=="InterarrivalDistribution")
                {
                    int CummulativeProbability = 0;
                    while(true)
                    {
                        string Next = streamReader.ReadLine();
                        if(Next == "")
                            break;
                        string[] arr = Next.Split(',');
                        TimeDistribution timeDistribution = new TimeDistribution();
                        int Probability = Convert.ToInt32(float.Parse(arr[1]) * 100);
                        timeDistribution.Time = Convert.ToInt32(arr[0]);;
                        timeDistribution.Probability = Probability;
                        timeDistribution.CummProbability = timeDistribution.Probability;
                        timeDistribution.CummProbability+=CummulativeProbability;
                        timeDistribution.MinRange = CummulativeProbability + 1;
                        CummulativeProbability += Probability;
                        timeDistribution.MaxRange = CummulativeProbability;
                        system.InterarrivalDistribution.Add(timeDistribution);
                    }
                }
                else if(next.Contains("ServiceDistribution_Server"))
                {
                    int NumberOfServers = system.NumberOfServers;
                    for(int i = 0; i < NumberOfServers; i++)
                    {
                        int CummulativeProbability = 0;
                        Server server = new Server();
                        while (true)
                        {
                            string Next = streamReader.ReadLine();
                            if(Next == "" || Next == null)
                            {
                                streamReader.ReadLine();
                                break;
                            }
                            string[] arr = Next.Split(',');
                            TimeDistribution timeDistribution = new TimeDistribution();
                            timeDistribution.Time = Convert.ToInt32(arr[0]);
                            int Probability = Convert.ToInt32(float.Parse(arr[1])*100);
                            timeDistribution.Probability = Probability;
                            timeDistribution.CummProbability = Probability;
                            timeDistribution.CummProbability += CummulativeProbability;
                            timeDistribution.MinRange = CummulativeProbability + 1;
                            CummulativeProbability += Probability;
                            timeDistribution.MaxRange = CummulativeProbability;
                            server.TimeDistribution.Add(timeDistribution);
                        }
                        server.ID = i + 1;
                        server.FinishTime = 0;
                        system.Servers.Add(server);
                    }
                }
            }
            fileStream.Close();
        }
        private void HandleNoOfCustomersCriteria()
        {
            Random random = new Random();
            int ArrivalTime = 0;
            for (int i = 0; i < system.StoppingNumber; i++)
            {
                SimulationCase simulationCase = new SimulationCase();
                simulationCase.CustomerNumber = i + 1;
                if (i == 0)
                {
                    // 1st one
                    simulationCase.InterArrival = 0;
                    simulationCase.ArrivalTime = 0;
                    simulationCase.RandomInterArrival = 1;
                    simulationCase.RandomService = random.Next(1, 100);
                    int ServerId = 0; // highest priority
                    if (system.SelectionMethod == Enums.SelectionMethod.Random)
                        ServerId = random.Next(0, system.NumberOfServers);
                    simulationCase.ServiceTime = GetTime(simulationCase.RandomService, system.Servers[ServerId].TimeDistribution);
                    simulationCase.AssignedServer = system.Servers[ServerId];
                    system.Servers[ServerId].FinishTime = simulationCase.ServiceTime;
                    system.Servers[ServerId].TotalWorkingTime += simulationCase.ServiceTime;
                    simulationCase.StartTime = 0;
                    simulationCase.EndTime = simulationCase.ServiceTime;
                    simulationCase.TimeInQueue = 0;
                    system.SimulationTable.Add(simulationCase);
                }
                else
                {
                    simulationCase.RandomInterArrival = random.Next(1, 100);
                    simulationCase.InterArrival = GetTime(simulationCase.RandomInterArrival, system.InterarrivalDistribution);
                    ArrivalTime += simulationCase.InterArrival;
                    simulationCase.ArrivalTime = ArrivalTime;
                    simulationCase.RandomService = random.Next(1, 100);
                    int ServerId = 0;
                    if (system.SelectionMethod == Enums.SelectionMethod.HighestPriority)
                    {
                        ServerId = GetServer(system.Servers, simulationCase.ArrivalTime);
                    }
                    else if (system.SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        ServerId = GetRandomServer(system.Servers, simulationCase.ArrivalTime);
                    }
                    else if (system.SelectionMethod == Enums.SelectionMethod.LeastUtilization)
                    {
                        ServerId = GetLeastUtilizedServer(system.Servers);
                    }
                    simulationCase.AssignedServer = system.Servers[ServerId];
                    simulationCase.ServiceTime = GetTime(simulationCase.RandomService, system.Servers[ServerId].TimeDistribution);
                    simulationCase.StartTime = Math.Max(system.Servers[ServerId].FinishTime, simulationCase.ArrivalTime);
                    simulationCase.EndTime = simulationCase.StartTime + simulationCase.ServiceTime;
                    simulationCase.TimeInQueue = simulationCase.StartTime - simulationCase.ArrivalTime;
                    system.Servers[ServerId].FinishTime = simulationCase.EndTime;
                    system.Servers[ServerId].TotalWorkingTime += simulationCase.ServiceTime;
                    system.SimulationTable.Add(simulationCase);
                }
            }
        }
        private void HandleSimulationEndTimeCriteria()
        {
            Random random = new Random();
            int ArrivalTime = 0;
            int CustomerNumber = 1;
            while (ArrivalTime < system.StoppingNumber)
            {
                SimulationCase simulationCase = new SimulationCase();
                if (ArrivalTime == 0)
                {
                    simulationCase.CustomerNumber = 1;
                    simulationCase.InterArrival = 0;
                    simulationCase.ArrivalTime = 0;
                    simulationCase.RandomInterArrival = 1;
                    simulationCase.RandomService = random.Next(1, 100);
                    if (system.SelectionMethod == Enums.SelectionMethod.HighestPriority)
                    {
                        simulationCase.ServiceTime = GetTime(simulationCase.RandomService, system.Servers[0].TimeDistribution);
                        simulationCase.AssignedServer = system.Servers[0];
                        system.Servers[0].FinishTime = simulationCase.ServiceTime;
                        system.Servers[0].TotalWorkingTime += simulationCase.ServiceTime;
                    }
                    else if (system.SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        int id = random.Next(0, system.NumberOfServers);
                        simulationCase.ServiceTime = GetTime(simulationCase.RandomService, system.Servers[id].TimeDistribution);
                        simulationCase.AssignedServer = system.Servers[id];
                        system.Servers[id].FinishTime = simulationCase.ServiceTime;
                        system.Servers[id].TotalWorkingTime += simulationCase.ServiceTime;
                    }
                    simulationCase.StartTime = 0;
                    simulationCase.EndTime = simulationCase.ServiceTime;
                    simulationCase.TimeInQueue = 0;
                    system.SimulationTable.Add(simulationCase);
                }
                else
                {
                    simulationCase.CustomerNumber = CustomerNumber;
                    CustomerNumber++;
                    simulationCase.RandomInterArrival = random.Next(1, 100);
                    simulationCase.InterArrival = GetTime(simulationCase.RandomInterArrival, system.InterarrivalDistribution);
                    ArrivalTime += simulationCase.InterArrival;
                    simulationCase.ArrivalTime = ArrivalTime;
                    simulationCase.RandomService = random.Next(1, 100);
                    int ServerId = 0;
                    if (system.SelectionMethod == Enums.SelectionMethod.HighestPriority)
                    {
                        ServerId = GetServer(system.Servers, simulationCase.ArrivalTime);
                    }
                    else if (system.SelectionMethod == Enums.SelectionMethod.Random)
                    {
                        ServerId = GetRandomServer(system.Servers, simulationCase.ArrivalTime);
                    }
                    else if (system.SelectionMethod == Enums.SelectionMethod.LeastUtilization)
                    {
                        ServerId = GetLeastUtilizedServer(system.Servers);
                    }
                    simulationCase.ServiceTime = GetTime(simulationCase.RandomService, system.Servers[ServerId].TimeDistribution);
                    simulationCase.StartTime = Math.Max(system.Servers[ServerId].FinishTime, simulationCase.ArrivalTime);
                    simulationCase.EndTime = simulationCase.StartTime + simulationCase.ServiceTime;
                    simulationCase.TimeInQueue = simulationCase.StartTime - simulationCase.ArrivalTime;
                    simulationCase.AssignedServer = system.Servers[ServerId];
                    system.Servers[ServerId].FinishTime = simulationCase.EndTime;
                    system.Servers[ServerId].TotalWorkingTime += simulationCase.ServiceTime;
                    system.SimulationTable.Add(simulationCase);
                }
            }
        }
        public void HandleStoppingCriteria()
        {
            if(system.StoppingCriteria == Enums.StoppingCriteria.NumberOfCustomers)
            {
                HandleNoOfCustomersCriteria();  
            }
            else if(system.StoppingCriteria == Enums.StoppingCriteria.SimulationEndTime)
            {
                HandleSimulationEndTimeCriteria();
            }
        }
        private decimal GetAverageWaitingTime()
        {
            int WaitingTime = 0;
            for (int i = 0; i < system.SimulationTable.Count; i++)
            {
                WaitingTime += system.SimulationTable[i].TimeInQueue;
            }
            return (decimal)WaitingTime / system.SimulationTable.Count;
        }
        private decimal GetWaitingProbability()
        {
            int Waitingprobability = 0;
            for (int i = 0; i < system.SimulationTable.Count; i++)
            {
                if(system.SimulationTable[i].TimeInQueue > 0)
                    Waitingprobability++;
            }
            return (decimal)Waitingprobability / system.SimulationTable.Count;
        }
        private void CalculatePerformance()
        {
            system.PerformanceMeasures.AverageWaitingTime = GetAverageWaitingTime();
            system.PerformanceMeasures.WaitingProbability = GetWaitingProbability();
            system.PerformanceMeasures.MaxQueueLength = MaxQueueLength(system.SimulationTable);
            List<int> frequency = new List<int>();
            for (int i = 0; i < system.NumberOfServers; i++)
                frequency.Add(0);
            for(int i=0;i<system.SimulationTable.Count;i++)
            {
                frequency[system.SimulationTable[i].AssignedServer.ID - 1]++;
            }
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                if (frequency[i] == 0)
                    system.Servers[i].AverageServiceTime = 0;
                else
                    system.Servers[i].AverageServiceTime = (decimal)system.Servers[i].TotalWorkingTime / frequency[i];

                int dif = MaxServerTime(system.Servers) - system.Servers[i].TotalWorkingTime;
                system.Servers[i].Utilization = (decimal)system.Servers[i].TotalWorkingTime / MaxServerTime(system.Servers);
                system.Servers[i].IdleProbability = (decimal)dif / MaxServerTime(system.Servers);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ReadFromFile(); 
            HandleStoppingCriteria();
            CalculatePerformance();
            for (int i = 0; i < system.NumberOfServers; i++)
            { 
                comboBox1.Items.Add(i + 1);
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
            MessageBox.Show(TestingManager.Test(system, Constants.FileNames.TestCase3));
            var binding = new BindingList<SimulationCase>(system.SimulationTable);
            var src = new BindingSource(binding, null);
            dataGridView1.DataSource = src;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int SelectedServer = Convert.ToInt32(comboBox1.SelectedItem.ToString());
            List<SimulationCase> L = system.SimulationTable;
            for(int i=0;i<L.Count;i++)
            {
                if(L[i].AssignedServer.ID==SelectedServer)
                {
                    for (int j=L[i].StartTime;j<=L[i].EndTime;j++)
                    {
                        chart1.Series["Running"].Points.AddXY(j, 1);
                    }
                }
            }
        }
    }
}
