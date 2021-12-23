using System.Threading.Tasks;

namespace Geko.HttpClientService.Benchmark;

public class Executor
{
    private readonly Task _task;

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
