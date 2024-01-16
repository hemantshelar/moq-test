using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeGenerator.Services;

public interface ICustomLogger
{
    int WriteLog(string strLog, int level);

}
public class CustomLogger : ICustomLogger
{
    public int WriteLog(string strLog, int level)
    {
        Console.WriteLine($"{strLog} - {level}");
        return level;
    }
}