using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeGenerator.Models;
public class JokesAPI
{
    public string EndPoint { get; set; }
}

public class JokeCategories
{
    public List<string> Names { get; set; }
}