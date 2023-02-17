using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Models.Frequency
{
    public class FrequencyModel
    {
        public string FrequencyTypeValue { get; set; } = "None";
        public bool IsCustom { get; set; }
        public short Every { get; set; }
    }
}
