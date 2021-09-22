using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test3
{
    public class Log
    {
        private String logDate = null;
        private String logAction = null;
        private int logX = 0;
        private int logY = 0;

        public Log(string logDate, string logAction, int logX, int logY)
        {
            this.logDate = logDate;
            this.logAction = logAction;
            this.logX = logX;
            this.logY = logY;
        }

        public Log()
        {
        }

        //setメソッド
        public void setLogDate(string logDate)
        {
            this.logDate = logDate;
        }
        public void setLogAction(string logDate)
        {
            this.logDate = logDate;
        }
        public void setLogX(int logX)
        {
            this.logX = logX;
        }
        public void setLogY(int logY)
        {
            this.logY = logY;
        }


        //getメソッド
        public String getLogDate()
        {
            return this.logDate;
        }
        public String getLogAction()
        {
            return this.logAction;
        }
        public int getLogX()
        {
            return this.logX;
        }
        public int getLogY()
        {
            return this.logY;
        }


        /*
        private String logDete { get; set; }
        private String logAction { get; set; }
        private int logX { get; set; }
        private int logY { get; set; }

        
        private String logDete { get; set; }
        private DateTime dete1 { get; set; }
        private String action { get; set; }

       
        public String getDate()
        {
            return date;
        }
        public void setDate(String n)
        {
            date = n;
        }
        */
    }
}
