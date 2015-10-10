using BugBusiness.Interface.BugIntelligence.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugIntelligence
{
    public interface IBugIntelligence
    {
         ClassifyResult classify(byte[] image);
    }
}
