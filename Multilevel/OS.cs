using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Multilevel
{
    class OS
    {
        public List<PCB> jobQ = new List<PCB>();
        public List<PCB> terminateQ = new List<PCB>();
        public List<string> readyQ1 = new List<string>();
        public List<string> readyQ2 = new List<string>();
        public List<PCB> readyQ3 = new List<PCB>();
        public List<PCB> cd = new List<PCB>();
        public List<PCB> printer = new List<PCB>();
        public List<PCB> webcam = new List<PCB>();
        public string cpu_state = null;
        public int round = 0;
        public double Memsize = 0;
        private int contFCFSQ = 0;
        public void setCpu(string process_id)
        {
            cpu_state = process_id;
        }

        public Boolean isCpuEmty()
        {
            if (cpu_state == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addProcessJQ(PCB process)
        {
            setReadyQ1();
            jobQ.Add(process);
        }

        public void addProcessTer(PCB process)
        {
            terminateQ.Add(process);
        }

        public void setReadyQ1()
        {

            string process;
            for (int i = 0; i < jobQ.Count(); i++)
            {
                process = jobQ[i].getProcessState();
                if (process == "New")
                {
                    jobQ[i].setProcessState("Ready");
                    readyQ1.Add(jobQ[i].getProcessID());
                }
            }
        }

        public void setReadyQ2()
        {
            string process;
            for (int i = 0; i < jobQ.Count(); i++)
            {
                process = jobQ[i].getProcessState();
                if (process == "Running")
                {
                    jobQ[i].setProcessState("Ready");
                    jobQ[i].setRQ(2);
                    jobQ[i].clearTimeSlide(0);
                    readyQ2.Add(jobQ[i].getProcessID());
                    round = 1;
                }
            }
        }

        public void setReadyQ3()
        {
            string process;
            for (int i = 0; i < jobQ.Count(); i++)
            {
                process = jobQ[i].getProcessState();
                if (process == "Running")
                {
                    contFCFSQ++;
                    jobQ[i].setProcessState("Ready");
                    jobQ[i].setFCFSQ(contFCFSQ);
                    jobQ[i].setRQ(3);
                    jobQ[i].clearTimeSlide(0);
                    readyQ3.Add(jobQ[i]);                  
                    round = 1;
                }

            }
        }

        public void addCD()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessState() == "Running")
                {
                    jobQ[i].setProcessState("Waiting");
                    clearCpu();
                    cd.Add(jobQ[i]);
                }
            }
            setIOToRun();
        }

        public void ejectCD()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessID() == cd[0].getProcessID())
                {
                    jobQ[i].setProcessState("Ready");
                    if (jobQ[i].getRQ() == 1)
                    {
                        readyQ1.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 2)
                    {
                        readyQ2.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 3)
                    {
                        for (int k = 0; k < getSize(); k++)
                        {
                            if (jobQ[k].getProcessID() == getCpu())
                            {
                                jobQ[k].setProcessState("Ready");
                                readyQ3.Add(jobQ[k]);
                            }
                        }
                        readyQ3.Add(jobQ[i]);
                        readyQ3.Sort(delegate (PCB x, PCB y)
                        {
                            return x.getFCFSQ().CompareTo(y.getFCFSQ());
                        });
                        clearCpu();
                    }
                }
            }
            cd.RemoveAt(0);
        }

        public void addPrinter()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessState() == "Running")
                {
                    jobQ[i].setProcessState("Waiting");
                    clearCpu();
                    printer.Add(jobQ[i]);
                }
            }
            setIOToRun();
        }

        public void ejectPrinter()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessID() == printer[0].getProcessID())
                {
                    jobQ[i].setProcessState("Ready");
                    if (jobQ[i].getRQ() == 1)
                    {
                        readyQ1.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 2)
                    {
                        readyQ2.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 3)
                    {
                        for (int k = 0; k < getSize(); k++)
                        {
                            if (jobQ[k].getProcessID() == getCpu())
                            {
                                jobQ[k].setProcessState("Ready");
                                readyQ3.Add(jobQ[k]);
                            }
                        }
                        readyQ3.Add(jobQ[i]);
                        readyQ3.Sort(delegate (PCB x, PCB y)
                        {
                            return x.getFCFSQ().CompareTo(y.getFCFSQ());
                        });
                        clearCpu();
                    }
                }
            }
            printer.RemoveAt(0);
        }

        public void addWebcam()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessState() == "Running")
                {
                    jobQ[i].setProcessState("Waiting");
                    clearCpu();
                    webcam.Add(jobQ[i]);
                }
            }
            setIOToRun();
        }

        public void ejectWebcam()
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessID() == webcam[0].getProcessID())
                {
                    jobQ[i].setProcessState("Ready");
                    if (jobQ[i].getRQ() == 1)
                    {
                        readyQ1.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 2)
                    {
                        readyQ2.Add(jobQ[i].getProcessID());
                    }
                    else if (jobQ[i].getRQ() == 3)
                    {
                        for (int k=0;k< getSize(); k++)
                        {
                            if (jobQ[k].getProcessID() == getCpu())
                            {
                                jobQ[k].setProcessState("Ready");
                                readyQ3.Add(jobQ[k]);
                            }
                        }
                        readyQ3.Add(jobQ[i]);
                        readyQ3.Sort(delegate (PCB x, PCB y)
                        {
                            return x.getFCFSQ().CompareTo(y.getFCFSQ());
                        });
                        clearCpu();
                    }
                }
            }
            webcam.RemoveAt(0);
        }

        public void setIOToRun()
        {
            if (getSizeCD() > 0)
            {
                for (int i = 0; i < getSizeCD(); i++)
                {
                    if (i == 0)
                    {
                        cd[i].setIOState("Working");
                        ioTime();
                    }
                    else
                    {
                        cd[i].setIOState("Waiting");
                    }
                }
            }
            if (getSizePrinter() > 0)
            {
                for (int i = 0; i < getSizePrinter(); i++)
                {
                    if (i == 0)
                    {
                        printer[i].setIOState("Working");
                        ioTime();
                    }
                    else
                    {
                        printer[i].setIOState("Waiting");
                    }
                }
            }
            if (getSizeWebcam() > 0)
            {
                for (int i = 0; i < getSizeWebcam(); i++)
                {
                    if (i == 0)
                    {
                        webcam[i].setIOState("Working");
                        ioTime();
                    }
                    else
                    {
                        webcam[i].setIOState("Waiting");
                    }
                }
            }
        }

        public string getCpu()
        {
            return cpu_state;
        }


        public void clearCpu()
        {
            cpu_state = null;
        }

        public List<PCB> getProcessjQ()
        {
            return jobQ;
        }

        public List<PCB> getProcessTer()
        {
            return terminateQ;
        }

        public int getSize()
        {
            return jobQ.Count();
        }

        public List<string> getReadyQ1()
        {
            return readyQ1;
        }

        public int getSizeReadyQ1()
        {
            return readyQ1.Count();
        }

        public List<string> getReadyQ2()
        {
            return readyQ2;
        }

        public int getSizeReadyQ2()
        {
            return readyQ2.Count();
        }

        public List<PCB> getReadyQ3()
        {
            return readyQ3;
        }

        public int getSizeReadyQ3()
        {
            return readyQ3.Count();
        }

        public void checkTQ(int tq1, int tq2)
        {
            int tq = 0;
            for (int i = 0; i < jobQ.Count; i++)
            {
                if (jobQ[i].getProcessID() == getCpu())
                {
                    tq = jobQ[i].getTimeSide();
                    if (jobQ[i].getRQ() == 1)
                    {
                        if (tq >= tq1)
                        {
                            setReadyQ2();
                            clearCpu();
                        }
                    }
                    else if (jobQ[i].getRQ() == 2)
                    {
                        if (tq >= tq2)
                        {
                            setReadyQ3();
                            clearCpu();
                        }
                    }
                    else if (jobQ[i].getRQ() == 3)
                    {

                    }
                }
            }

        }

        public string running(int tq1, int tq2)
        {
            string processID = " ";

            checkTQ(tq1,tq2);
            if (isCpuEmty())
            {
                if (readyQ1.Count() > 0)
                {
                    setCpu(readyQ1[0]);
                }
                else if (readyQ2.Count() > 0)
                {
                    setCpu(readyQ2[0]);
                }
                else if (readyQ3.Count() > 0)
                {
                    setCpu(readyQ3[0].getProcessID());
                }

                for (int i = 0; i < jobQ.Count; i++)
                {
                    if (jobQ[i].getProcessID() == getCpu())
                    {
                        processID = jobQ[i].getProcessID();
                        jobQ[i].setProcessState("Running");
                        jobQ[i].setBurstTime(1);
                        jobQ[i].setTimeSlide(1);

                        if (readyQ1.Count() > 0)
                        {
                            if (readyQ1[0] == processID)
                            {
                                readyQ1.RemoveAt(0);
                            }
                        }
                        else if (readyQ2.Count() > 0)
                        {
                            if (readyQ2[0] == processID)
                            {
                                readyQ2.RemoveAt(0);
                            }
                        }
                        else if (readyQ3.Count() > 0)
                        {
                            if (readyQ3[0].getProcessID() == processID)
                            {
                                readyQ3.RemoveAt(0);
                            }
                        }                    
                        return processID;
                    }
                }
            }
            else
            {
                for (int i = 0; i < jobQ.Count; i++)
                {
                    if (jobQ[i].getProcessID() == getCpu())
                    {
                        processID = jobQ[i].getProcessID();
                        jobQ[i].setBurstTime(1);
                        jobQ[i].setTimeSlide(1);
                        return processID;
                    }
                }
            }
            return processID;
        }

        public Boolean terminate(int time)
        {
            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessState() == "Running")
                {
                    if (getSizeReadyQ1() > 0)
                    {
                        for (int j = 0; j < getSizeReadyQ1(); j++)
                        {
                            if (jobQ[i].getProcessID() == readyQ1[j])
                            {
                                readyQ1.RemoveAt(j);
                            }
                        }
                    }
                    if (getSizeReadyQ2() > 0)
                    {
                        for (int j = 0; j < getSizeReadyQ2(); j++)
                        {
                            if (jobQ[i].getProcessID() == readyQ2[j])
                            {
                                readyQ2.RemoveAt(j);
                            }
                        }
                    }
                    if (getSizeReadyQ3() > 0)
                    {
                        for (int j = 0; j < getSizeReadyQ3(); j++)
                        {
                            if (jobQ[i].getProcessID() == readyQ3[j].getProcessID())
                            {
                                readyQ3.RemoveAt(j);
                            }
                        }
                    }
                    jobQ[i].setProcessState("Terminate");
                    jobQ[i].setCompleteTime(time);
                    jobQ[i].setTurnaroundTime(time - jobQ[i].getArrivalTime());
                    clearCpu();
                    terminateQ.Add(jobQ[i]);
                    return true;
                }
            }
            return false;
        }

        public void waitingTime()
        {

            for (int i = 0; i < getSize(); i++)
            {
                if (jobQ[i].getProcessState() == "Ready")
                {
                    jobQ[i].setWaitingTime(1);
                }
            }
        }

        public void ioTime()
        {
            if (getSizeCD() > 0)
            {
                for (int i = 0; i < getSize(); i++)
                {
                    if (jobQ[i].getProcessID()==cd[0].getProcessID())
                    {
                        jobQ[i].setIOTime(1);
                    }
                }
            }
            if (getSizePrinter() > 0)
            {
                for (int i = 0; i < getSize(); i++)
                {
                    if (jobQ[i].getProcessID() == printer[0].getProcessID())
                    {
                        jobQ[i].setIOTime(1);
                    }
                }
            }
            if (getSizeWebcam() > 0)
            {
                for (int i = 0; i < getSize(); i++)
                {
                    if (jobQ[i].getProcessID() == webcam[0].getProcessID())
                    {
                        jobQ[i].setIOTime(1);
                    }
                }
            }
        }

        public int getSizeCD()
        {
            return cd.Count();
        }

        public List<PCB> getCD()
        {
            return cd;
        }

        public int getSizePrinter()
        {
            return printer.Count();
        }

        public List<PCB> getPrinter()
        {
            return printer;
        }

        public int getSizeWebcam()
        {
            return webcam.Count();
        }

        public List<PCB> getWebcam()
        {
            return webcam;
        }

        public double getMemsize()
        {
            string hwclass = "Win32_PhysicalMemory";
            string syntex = "Capacity";
            string mem;
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + hwclass);
            foreach (ManagementObject mj in mos.Get())
            {
                mem = mj[syntex].ToString();
                Memsize += double.Parse(mem);
            }
            return Memsize;
        }
    }
}
