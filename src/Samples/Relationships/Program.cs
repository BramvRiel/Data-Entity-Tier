﻿using DataEntityTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relationships
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new AppleTreeRepository();
            repository.Add(new AppleTree());
        }
    }
}