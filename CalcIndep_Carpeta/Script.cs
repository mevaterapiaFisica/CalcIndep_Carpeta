using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    class Script
    {
        public Script()
        {
        }
        public void Execute(ScriptContext context)
        {
            CalcIndep_Carpeta.Form1 form1 = new CalcIndep_Carpeta.Form1(true, context.Patient, context.PlanSetup, context.CurrentUser, context.PlanSumsInScope);
            
            form1.ShowDialog();
            form1.Dispose();

        }
    }
}


