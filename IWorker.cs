using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeehiveManagementSystem
{
    interface IWorker
    {
        string Job { get; set; }

        void WorkTheNextShift();
    }
}
