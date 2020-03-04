using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Multilevel
{
    public partial class Multilevel : Form
    {
        int time_value = 0;
        int count_id = 0;
        double MemKB = 0;
        double MemMB = 0;
        double MemGB = 0;
        double Mem = 0;
        String unit = null;

        OS os = new OS();
        public Multilevel()
        {
            InitializeComponent();
            lbl_q1_timeq_value.Text = q1_timeq.Value.ToString();
            lbl_q2_timeq_value.Text = q2_timeq.Value.ToString();
            lbl_cpu.UseMnemonic = false;
            lbl_run_proid.Text = " ";
            lbl_run_burst.Text = " ";
            lbl_run_state.Text = " ";
            MemKB = os.getMemsize() / 1024;
            MemMB = MemKB / 1024;
            MemGB = MemMB / 1024;
            process_slide.MaximumValue = Convert.ToInt32(MemKB);

            /*  timer1.Enabled = true;
              timer1.Interval = 1000;
              timer1.Start();*/
        }

        public void btn_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void btn_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void q1_timeq_ValueChanged(object sender, EventArgs e)
        {
            lbl_q1_timeq_value.Text = q1_timeq.Value.ToString();
        }

        public void q2_timeq_ValueChanged(object sender, EventArgs e)
        {
            lbl_q2_timeq_value.Text = q2_timeq.Value.ToString();
        }

        public void process_slide_ValueChanged(object sender, EventArgs e)
        {
            lbl_value_process_mem.Text = process_slide.Value.ToString();
        }

        public void time()
        {
            time_value++;
            lbl_times.Text = time_value.ToString();
        }

        public void btn_newprocess_Click(object sender, EventArgs e)
        {
            os.round = 0;
            if ((os.isCpuEmty()) && (os.getSizeReadyQ1() > 0))
            {
                processRunning(os.running(q1_timeq.Value,q2_timeq.Value));
            }
            else
            {
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
            }
            time();
            os.waitingTime();
            os.ioTime();
            checkUnit();
            PCB process = new PCB(count_id.ToString(), process_slide.Value, time_value, unit);
            if (process_slide.Value != 0)
            {
                if (MemPer())
                {
                    os.addProcessJQ(process);
                    count_id++;
                    if (os.getSizeReadyQ1() >= 0)
                    {
                        update_datatableRQ1(os.getReadyQ1());
                    }
                    if (os.getSizeReadyQ2() >= 0)
                    {
                        if (os.round != 1)
                        {
                            update_datatableRQ2(os.getReadyQ2());
                        }
                    }
                    if (os.getSizeReadyQ3() >= 0)
                    {
                        if (os.round != 1)
                        {
                            update_datatableRQ3(os.getReadyQ3());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Slide Memory Bar To New Process.");
            }
            update_datatableJQ(os.getProcessjQ());
        }

        public void btn_man_Click(object sender, EventArgs e)
        {
            os.round = 0;
            if ((os.isCpuEmty()) && (os.getSizeReadyQ1() >= 0))
            {
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
            }
            else
            {
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
            }
            time();
            os.waitingTime();
            os.ioTime();
            os.setReadyQ1();    
            if (os.getSizeReadyQ1() >= 0)
            {
                update_datatableRQ1(os.getReadyQ1());
            }
            if (os.getSizeReadyQ2() >= 0)
            {
                if (os.round != 1)
                {
                    update_datatableRQ2(os.getReadyQ2());
                }              
            }
            if (os.getSizeReadyQ3() >= 0)
            {
                if (os.round != 1)
                {
                    update_datatableRQ3(os.getReadyQ3());
                }
            }
            update_datatableJQ(os.getProcessjQ());
        }

        public void btn_reset_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void update_datatableJQ(List<PCB> process)
        {
            DataTable dtTableJQ = new DataTable("JQTB");

            dtTableJQ.Columns.Add(new DataColumn("col_process_id", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_process_state", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_burst", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_arrival", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_IO", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_comple", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_turnaround", typeof(string)));
            dtTableJQ.Columns.Add(new DataColumn("col_waiting", typeof(string)));


            foreach (PCB job in process)
            {
                dtTableJQ.Rows.Add(job.getProcessID(), job.getProcessState(), job.getBurstTime(), job.getArrivalTime(), job.getIOTime(), job.getCompleteTime(), job.getTurnaroundTime(), job.getWaitingTime());
            }

            this.dtg_jobq.AutoGenerateColumns = false;
            this.dtg_jobq.AllowUserToAddRows = false;
            this.dtg_jobq.DataSource = dtTableJQ;

            dtg_jobq.DefaultCellStyle.SelectionBackColor = dtg_jobq.DefaultCellStyle.BackColor;
            dtg_jobq.DefaultCellStyle.SelectionForeColor = dtg_jobq.DefaultCellStyle.ForeColor;
        }

        private void update_datatableRQ1(List<string> process)
        {
            DataTable dtTableRQ1 = new DataTable("RQ1TB");

            dtTableRQ1.Columns.Add(new DataColumn("col_q1_process_id", typeof(string)));

            for (int i = 0; i < process.Count(); i++)
            {
                dtTableRQ1.Rows.Add(process[i]);
            }

            this.dtg_q1.AutoGenerateColumns = false;
            this.dtg_q1.AllowUserToAddRows = false;
            this.dtg_q1.DataSource = dtTableRQ1;

            dtg_q1.DefaultCellStyle.SelectionBackColor = dtg_q1.DefaultCellStyle.BackColor;
            dtg_q1.DefaultCellStyle.SelectionForeColor = dtg_q1.DefaultCellStyle.ForeColor;
        }

        private void update_datatableRQ2(List<string> process)
        {
            DataTable dtTableRQ2 = new DataTable("RQ2TB");

            dtTableRQ2.Columns.Add(new DataColumn("col_q2_process_id", typeof(string)));

            for (int i = 0; i < process.Count(); i++)
            {
 
                dtTableRQ2.Rows.Add(process[i]);
            }

            this.dtg_q2.AutoGenerateColumns = false;
            this.dtg_q2.AllowUserToAddRows = false;
            this.dtg_q2.DataSource = dtTableRQ2;

            dtg_q2.DefaultCellStyle.SelectionBackColor = dtg_q2.DefaultCellStyle.BackColor;
            dtg_q2.DefaultCellStyle.SelectionForeColor = dtg_q2.DefaultCellStyle.ForeColor;
        }

        private void update_datatableRQ3(List<PCB> process)
        {
            DataTable dtTableRQ3 = new DataTable("RQ3TB");

            dtTableRQ3.Columns.Add(new DataColumn("col_q3_process_id", typeof(string)));

            for (int i = 0; i < process.Count(); i++)
            {
                dtTableRQ3.Rows.Add(process[i].getProcessID());
            }

            this.dtg_q3.AutoGenerateColumns = false;
            this.dtg_q3.AllowUserToAddRows = false;
            this.dtg_q3.DataSource = dtTableRQ3;

            dtg_q3.DefaultCellStyle.SelectionBackColor = dtg_q3.DefaultCellStyle.BackColor;
            dtg_q3.DefaultCellStyle.SelectionForeColor = dtg_q3.DefaultCellStyle.ForeColor;
        }

        private void update_datatableTerminate(List<PCB> process)
        {
            DataTable dtTableTer = new DataTable("JQTB");

            dtTableTer.Columns.Add(new DataColumn("col_terminate_process_id", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_burst", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_arrival", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_IO", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_comple", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_turnaround", typeof(string)));
            dtTableTer.Columns.Add(new DataColumn("col_terminate_waiting", typeof(string)));


            foreach (PCB job in process)
            {
                dtTableTer.Rows.Add(job.getProcessID(), job.getBurstTime(), job.getArrivalTime(), job.getIOTime(), job.getCompleteTime(), job.getTurnaroundTime(), job.getWaitingTime());
            }

            this.dtg_terminat.AutoGenerateColumns = false;
            this.dtg_terminat.AllowUserToAddRows = false;
            this.dtg_terminat.DataSource = dtTableTer;

            dtg_terminat.DefaultCellStyle.SelectionBackColor = dtg_terminat.DefaultCellStyle.BackColor;
            dtg_terminat.DefaultCellStyle.SelectionForeColor = dtg_terminat.DefaultCellStyle.ForeColor;
        }

        private void update_datatableIOCD(List<PCB> process)
        {
            DataTable dtTableIOCD = new DataTable("CDTB");

            dtTableIOCD.Columns.Add(new DataColumn("col_cd_pid", typeof(string)));
            dtTableIOCD.Columns.Add(new DataColumn("col_cd_state2", typeof(string)));

            foreach (PCB job in process)
            {
                dtTableIOCD.Rows.Add(job.getProcessID(), job.getIOState());
            }

            this.dtg_cd.AutoGenerateColumns = false;
            this.dtg_cd.AllowUserToAddRows = false;
            this.dtg_cd.DataSource = dtTableIOCD;

            dtg_cd.DefaultCellStyle.SelectionBackColor = dtg_cd.DefaultCellStyle.BackColor;
            dtg_cd.DefaultCellStyle.SelectionForeColor = dtg_cd.DefaultCellStyle.ForeColor;
        }

        private void update_datatableIOPrinter(List<PCB> process)
        {
            DataTable dtTableIOPrinter = new DataTable("PrinterTB");

            dtTableIOPrinter.Columns.Add(new DataColumn("col_print_process_id", typeof(string)));
            dtTableIOPrinter.Columns.Add(new DataColumn("col_print_state", typeof(string)));

            foreach (PCB job in process)
            {
                dtTableIOPrinter.Rows.Add(job.getProcessID(), job.getIOState());
            }

            this.dtg_print.AutoGenerateColumns = false;
            this.dtg_print.AllowUserToAddRows = false;
            this.dtg_print.DataSource = dtTableIOPrinter;

            dtg_print.DefaultCellStyle.SelectionBackColor = dtg_print.DefaultCellStyle.BackColor;
            dtg_print.DefaultCellStyle.SelectionForeColor = dtg_print.DefaultCellStyle.ForeColor;
        }

        private void update_datatableIOWebcam(List<PCB> process)
        {
            DataTable dtTableIOWebcam = new DataTable("WebcamTB");

            dtTableIOWebcam.Columns.Add(new DataColumn("col_web_process_id", typeof(string)));
            dtTableIOWebcam.Columns.Add(new DataColumn("col_web_state", typeof(string)));

            foreach (PCB job in process)
            {
                dtTableIOWebcam.Rows.Add(job.getProcessID(), job.getIOState());
            }

            this.dtg_webcam.AutoGenerateColumns = false;
            this.dtg_webcam.AllowUserToAddRows = false;
            this.dtg_webcam.DataSource = dtTableIOWebcam;

            dtg_webcam.DefaultCellStyle.SelectionBackColor = dtg_webcam.DefaultCellStyle.BackColor;
            dtg_webcam.DefaultCellStyle.SelectionForeColor = dtg_webcam.DefaultCellStyle.ForeColor;
        }

        private void processRunning(string process)
        {
            List<PCB> x = os.getProcessjQ();
            for (int i = 0; i < os.getSize(); i++)
            {
                if (os.getSizeReadyQ1() == 0 && os.getSizeReadyQ2() == 0 && os.getSizeReadyQ3() == 0 && os.isCpuEmty())
                {
                    lbl_run_proid.Text = " ";
                    lbl_run_burst.Text = " ";
                    lbl_run_state.Text = " ";
                    circle_process.Value = 0;
                    circle_memory.Value = 0;
                    circle_process.Update();
                    circle_memory.Update();
                }
                if (x[i].getProcessID() == process)
                {
                        lbl_run_proid.Text = process;
                        lbl_run_burst.Text = x[i].getBurstTime().ToString();
                        lbl_run_state.Text = x[i].getProcessState();
                        processPer();
                }
            }
        }

        private void btn_terminate_Click(object sender, EventArgs e)
        {
            time();
            os.waitingTime();
            os.ioTime();
            if (os.terminate(time_value))
            {
                os.setReadyQ1();
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                update_datatableTerminate(os.terminateQ);
                update_datatableJQ(os.getProcessjQ());
                update_datatableRQ1(os.getReadyQ1());
                update_datatableRQ2(os.getReadyQ2());
                update_datatableRQ3(os.getReadyQ3());
                avgTime();
                downMemPer();
            }
            else if (os.jobQ.Count() < 1)
            {
                MessageBox.Show("Please Add Process !!!");
            }
            else if (os.jobQ.Count()==os.terminateQ.Count())
            {
                MessageBox.Show("All process is  Terminate.");
            }
            else
            {

            }
        }

        private void avgTime()
        {
            float avgWait = 0;
            float avgTurn = 0;
            for (int i=0;i<os.getSize();i++)
            {
                avgWait += os.jobQ[i].getWaitingTime();
            }
            for (int i = 0; i < os.getSize(); i++)
            {
                avgTurn += os.jobQ[i].getTurnaroundTime();
            }
            avgWait = avgWait / os.getSize();
            avgTurn = avgTurn / os.getSize();
            lbl_avg_wait.Text = "Waiting Time = " + avgWait.ToString() + " s";
            lbl_avg_Turn.Text = "Turnaround Time = " + avgTurn.ToString() + " s";
        }

        private void btn_add_cd_Click(object sender, EventArgs e)
        {          
            time();
            os.round = 0;
            os.waitingTime();
            if ((os.getSize() > 0))
            {
                if (!os.isCpuEmty())
                {
                    os.addCD();
                    update_datatableIOCD(os.getCD());
                    processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                }
                else if (os.jobQ.Count() == os.terminateQ.Count())
                {
                    MessageBox.Show("All process is  Terminate, \nPlease Add Process.");
                }
                else if(os.jobQ.Count()==(os.cd.Count()+os.webcam.Count()+os.printer.Count()))
                {
                    MessageBox.Show("All process is waiting !!");
                }
                else
                {
                    MessageBox.Show("Please Next Step 1 Step.");
                }
                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Please Add Process.");
            }
        }

        private void btn_eject_cd_Click(object sender, EventArgs e)
        {
            time();
            os.round = 0;
            os.waitingTime();
            if (os.getSizeCD() > 0)
            {
                os.ejectCD();
                os.setIOToRun();
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                update_datatableIOCD(os.getCD());
                
                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Not process in I/O Queue !");
            }
        }

        private void btn_add_print_Click(object sender, EventArgs e)
        {
            time();
            os.round = 0;
            os.waitingTime();
            if ((os.getSize() > 0))
            {
                if (!os.isCpuEmty())
                {
                    os.addPrinter();
                    update_datatableIOPrinter(os.getPrinter());
                    processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                }
                else if (os.jobQ.Count() == os.terminateQ.Count())
                {
                    MessageBox.Show("All process is  Terminate, \nPlease Add Process.");
                }
                else if (os.jobQ.Count() == (os.cd.Count() + os.webcam.Count() + os.printer.Count()))
                {
                    MessageBox.Show("All process is waiting !!");
                }
                else
                {
                    MessageBox.Show("Please Next Step 1 Step.");
                }

                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Please Add Process.");
            }
        }

        private void btn_eject_print_Click(object sender, EventArgs e)
        {
            time();
            os.round = 0;
            os.waitingTime();
            if (os.getSizePrinter() > 0)
            {
                os.ejectPrinter();;
                os.setIOToRun();
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                update_datatableIOPrinter(os.getPrinter());

                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Not process in I/O Queue !");
            }
        }

        private void btn_add_web_Click(object sender, EventArgs e)
        {
            time();
            os.round = 0;
            os.waitingTime();
            if ((os.getSize() > 0))
            {
                if (!os.isCpuEmty())
                {
                    os.addWebcam();
                    update_datatableIOWebcam(os.getWebcam());
                    processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                }
                else if (os.jobQ.Count() == os.terminateQ.Count())
                {
                    MessageBox.Show("All process is  Terminate, \nPlease Add Process.");
                }
                else if (os.jobQ.Count() == (os.cd.Count() + os.webcam.Count() + os.printer.Count()))
                {
                    MessageBox.Show("All process is waiting !!");
                }
                else
                {
                    MessageBox.Show("Please Next Step 1 Step.");
                }

                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Please Add Process.");
            }
        }

        private void btn_eject_web_Click(object sender, EventArgs e)
        {
            time();
            os.round = 0;
            os.waitingTime();
            if (os.getSizeWebcam() > 0)
            {
                os.ejectWebcam();
                os.setIOToRun();
                processRunning(os.running(q1_timeq.Value, q2_timeq.Value));
                update_datatableIOWebcam(os.getWebcam());
                if (os.getSizeReadyQ1() >= 0)
                {
                    update_datatableRQ1(os.getReadyQ1());
                }
                if (os.getSizeReadyQ2() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ2(os.getReadyQ2());
                    }
                }
                if (os.getSizeReadyQ3() >= 0)
                {
                    if (os.round != 1)
                    {
                        update_datatableRQ3(os.getReadyQ3());
                    }
                }
                os.setReadyQ1();
                update_datatableJQ(os.getProcessjQ());
            }
            else
            {
                MessageBox.Show("Not process in I/O Queue !");
            }
        }

        private void ddl_unit_process_mem_onItemSelected(object sender, EventArgs e)
        {
            if (ddl_unit_process_mem.selectedIndex == 0)
            {             
                process_slide.MaximumValue = Convert.ToInt32(MemKB);
                process_slide.Value = 0;
                lbl_value_process_mem.Text = "0";
            }
            if (ddl_unit_process_mem.selectedIndex == 1)
            {
                process_slide.MaximumValue = Convert.ToInt32(MemMB);
                process_slide.Value = 0;
                lbl_value_process_mem.Text = "0";
            }
            if (ddl_unit_process_mem.selectedIndex == 2)
            {
                process_slide.MaximumValue = Convert.ToInt32(MemGB);
                process_slide.Value = 0;
                lbl_value_process_mem.Text = "0";
            }
        }

        public void processPer()
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(0);
                circle_process.Value = i;
                circle_process.Update();
            }
        }

        public Boolean MemPer()
        {
            Boolean sate = false;
            if (ddl_unit_process_mem.selectedIndex == 0)
            {
                Mem += (Convert.ToDouble(process_slide.Value) / MemKB) * 100;
                if (Mem <= circle_memory.MaxValue)
                {
                    circle_memory.Value = Convert.ToInt32(Mem);
                    circle_memory.Update();
                    sate = true;
                }
                else
                {
                    circle_memory.Value = 100;
                    circle_memory.Update();
                    MessageBox.Show("Memmory Full !!!");
                }
            }
            if (ddl_unit_process_mem.selectedIndex == 1)
            {
                Mem += (Convert.ToDouble(process_slide.Value) / MemMB) * 100;
                if (Mem <= circle_memory.MaxValue)
                {
                    circle_memory.Value = Convert.ToInt32(Mem);
                    circle_memory.Update();
                    sate = true;
                }
                else
                {
                    circle_memory.Value = 100;
                    circle_memory.Update();
                    MessageBox.Show("Memmory Full !!!");
                }
            }
            if (ddl_unit_process_mem.selectedIndex == 2)
            {
                Mem += (Convert.ToDouble(process_slide.Value) / MemGB) * 100;
                if (Mem <= circle_memory.MaxValue)
                {
                    circle_memory.Value = Convert.ToInt32(Mem);
                    circle_memory.Update();
                    sate = true;
                }
                else
                {
                    circle_memory.Value = 100;
                    circle_memory.Update();
                    MessageBox.Show("Memmory Full !!!");
                }     
            }
            return sate;
        }

        public void downMemPer()
        {
            PCB process = new PCB();
            process = os.terminateQ[os.terminateQ.Count()-1];
            if (process.getMemUnit() == "KB")
            {
                Mem -= (process.getMemory() / MemKB) * 100;
                circle_memory.Value = Convert.ToInt32(Mem);
                circle_memory.Update();
            }
            if (process.getMemUnit() == "MB")
            {
                Mem -= (process.getMemory() / MemMB) * 100;
                circle_memory.Value = Convert.ToInt32(Mem);
                circle_memory.Update();
            }
            if (process.getMemUnit() == "GB")
            {
                Mem -= (process.getMemory() / MemGB) * 100;
                circle_memory.Value = Convert.ToInt32(Mem);
                circle_memory.Update();
            }
            if (os.jobQ.Count() == os.terminateQ.Count())
            {
                Mem = 0;
                circle_memory.Value = 0;
                circle_memory.Update();
            }
        }

        public void checkUnit()
        {
            if (ddl_unit_process_mem.selectedIndex == 0)
            {
                unit = "KB";
            }
            if (ddl_unit_process_mem.selectedIndex == 1)
            {
                unit = "MB";
            }
            if (ddl_unit_process_mem.selectedIndex == 2)
            {
                unit = "GB";
            }
        }

    }
}
