using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeehiveManagementSystem
{
    abstract class Bee : IWorker
    {
        public string Job { get; set; }
        public virtual float CostPerShift { get; }
        public Bee(string job)
        {
            Job = job;
        }
        public void WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift))
                DoJob();
        }
        protected abstract void DoJob();
    }
    
    class Queen : Bee
    {
        private const float EGGS_PER_SHIFT = 0.45f;                  // How many eggs to lay each shift?
        private const float HONEY_PER_UNASSSIGNED_WORKER = 0.5f;     // How many unassiged worker consume?
        private IWorker[] workers = new IWorker[0];
        private float eggs = 0;
        private float unassignedWorkers = 3;
        public override float CostPerShift { get { return 2.15f; } }
        public string StatusReport { get; private set; }
        public Queen() :base("Queen") 
        {
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
            AssignBee("Egg Care");
        }
    
        public void AssignBee(string job)
        {
            switch (job)
            {
                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufaturer());
                    break;
                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
                default:
                    break;
            }
            UpdateStatusReport();
        }

        private void  UpdateStatusReport()
        {
            StatusReport = $"Vault report:\n{HoneyVault.StatusReport}\n" +
                           $"\nEgg count: {eggs:0.0}\nUnassigned workers: {unassignedWorkers:0.0}\n" +
                           $"{WorkerStatus("Nectar Collector")}\n{WorkerStatus("Honey Manufacturer")}" +
                           $"\n{WorkerStatus("Egg Care")}\nTOTAL WORKERS: {workers.Length}";
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach (Bee worker in workers)
                if (worker.Job == job) count++;
            string s = "s";
            if (count == 1) s = "";
            return $"{count} {job} bee{s}";
        }

        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;
            foreach (IWorker worker in workers)
            {
                worker.WorkTheNextShift();
                
            }
            HoneyVault.ConsumeHoney(HONEY_PER_UNASSSIGNED_WORKER * workers.Length);
            UpdateStatusReport();
        }

        public void CareForEggs(float eggsToConvert)
        {
            if (eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }
        private void AddWorker (IWorker worker)
        {
            if (unassignedWorkers >= 1)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }
    }
    
    class NectarCollector : Bee
    {
        private const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;
        public NectarCollector() :base("Nectar Collector") { }
        public override float CostPerShift { get { return 1.95f; } }

        protected override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
        }
    }
    
    class HoneyManufaturer : Bee
    {
        private const float NECTAR_PROCESSED_PER_SHIFT = 33.15f;
        public HoneyManufaturer() :base("Honey Manufacturer") { }
        public override float CostPerShift { get { return 1.7f; } }

        protected override void DoJob()
        {
            HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }
    }

    class EggCare : Bee 
    {
        private const float CARE_PROGRESS_PER_SHIFT = 0.15f;
        private Queen queen;
        public EggCare(Queen queen) :base("Egg Care") 
        {
            this.queen = queen; // + new reference to Queen Bee
        }
        public override float CostPerShift { get { return 1.35f; } }

        protected override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
    }
}
