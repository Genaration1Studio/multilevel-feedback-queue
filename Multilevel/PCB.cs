using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multilevel
{
    class PCB
    {
        private String process_id;
        private String process_state;
        private String io_state;
        private double memory;
        private String mem_unit;
        private int burst_time;
        private int arrival_time;
        private int io_time;
        private int completion_time;
        private int turnaround_time;
        private int waiting_time;
        private int time_silde;
        private int rq;
        private int fcfcQ;
 
        public PCB()
        {

        }

        public PCB(string id, double memory, int time , string unit)
        {
            process_id = "P" + id;
            process_state = "New";
            this.memory = memory;
            mem_unit = unit;
            burst_time = 0;
            arrival_time = time;
            io_time = 0;
            waiting_time = 0;
            time_silde = 0;
            rq = 1;
            fcfcQ = 0;
        }

        public void setProcessID(string id)
        {
            process_id = "P" + id;
        }

        public void setProcessState(String state)
        {
            process_state = state;
        }

        public void setIOState(String state)
        {
            io_state = state;
        }

        public void setMemory(double memory)
        {
            memory = memory;
        }


        public void setBurstTime(int burst_time)
        {
            this.burst_time += burst_time;
        }

        public void setArrivalTime(int arrival_time)
        {
            arrival_time = arrival_time;
        }

        public void setIOTime(int io_time)
        {
            this.io_time += io_time;
        }

        public void setCompleteTime(int completiont_time)
        {
            completion_time = completiont_time;
        }

        public void setTurnaroundTime(int trun_time)
        {
            turnaround_time = trun_time;
        }

        public void setWaitingTime(int wait_time)
        {
            waiting_time += wait_time;
        }

        public void setTimeSlide(int time_slide)
        {
            this.time_silde += time_slide;
        }
        public void clearTimeSlide(int time_slide)
        {
            this.time_silde = time_slide;
        }

        public void setRQ(int num)
        {
            rq = num;
        }

        public void setFCFSQ(int num)
        {
            fcfcQ = num;
        }

        public void setMemUnit(String unit)
        {
            mem_unit = unit;
        }

        public String getProcessID()
        {
            return process_id;
        }

        public String getProcessState()
        {
            return process_state;
        }

        public String getIOState()
        {
            return io_state;
        }

        public double getMemory()
        {
            return memory;
        }

        public int getBurstTime()
        {
            return burst_time;
        }

        public int getArrivalTime()
        {
            return arrival_time;
        }

        public int getIOTime()
        {
            return io_time;
        }

        public int getCompleteTime()
        {
            return completion_time;
        }

        public int getTurnaroundTime()
        {
            return turnaround_time;
        }
        
        public int getWaitingTime()
        {
            return waiting_time;
        }

        public int getTimeSide()
        {
            return time_silde;
        }

        public int getRQ()
        {
            return rq;
        }

        public int getFCFSQ()
        {
            return fcfcQ;
        }

        public String getMemUnit()
        {
            return mem_unit;
        }
    }
}
