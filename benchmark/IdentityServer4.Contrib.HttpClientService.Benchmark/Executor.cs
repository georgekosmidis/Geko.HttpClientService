using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Benchmark
{
    public class Executor
    {
        private volatile Task _task;

        public Executor(Task task)
        {
            _task = task;
        }

        public void Execute(int numberOfTimes)
        {
            for (var i = 0; i < numberOfTimes; i++)
            {
                _task.GetAwaiter().GetResult();
            }
        }
    }
}
